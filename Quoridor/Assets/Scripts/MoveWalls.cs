using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;

public class MoveWalls : MonoBehaviour
{

    public GameObject playerOne;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 startPos;
    private Controller controller;
    private string wallTag;
    private BoxCollider2D wallCollider;
    private bool lockPlace;

    // Start is called before the first frame update
    void Start()
    {
        playerOne = GameObject.Find("playerMouse");
        controller = GameObject.Find("GameController").GetComponent<Controller>();
        startPos = transform.position;
        wallTag = this.tag;
        wallCollider = GetComponent<BoxCollider2D>();
        lockPlace = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameBoard.PlayerEnum player = controller.GetWhoseTurn();
        if (player == GameBoard.PlayerEnum.ONE && wallTag == "PlayerOneWall" && !lockPlace) 
        {
            wallCollider.enabled = true;
        }
        else if (player == GameBoard.PlayerEnum.TWO && wallTag == "PlayerTwoWall" && !lockPlace)
        {
            wallCollider.enabled = true;
        }
        else
        {
            wallCollider.enabled = false;
        }

    }

    public bool IsOnBoard()
    {
        return lockPlace;
    }

    public void SetLockPlace(bool setValue)
    {
        lockPlace = setValue;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            transform.localScale = new Vector3(transform.localScale.y, transform.localScale.x, transform.localScale.z);
        }
    }


    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }

    void SnapWallToPlace(Collider[] hits)
    {
        Collider closest = null;

        if (hits.Length == 1)
        {
            if(transform.localScale.x == hits[0].transform.localScale.x && transform.localScale.y == hits[0].transform.localScale.y)
            {
                closest = hits[0];
            }
            else
            {
                transform.position = startPos;
            }
        }
        else if (hits.Length > 1)
        {
           
            float lowestDiffX = 0;
            float lowestDiffY = 0;
            float diffx = 0;
            float diffy = 0;

            foreach (Collider hit in hits)
            {
                if (transform.localScale.x == hit.transform.localScale.x && transform.localScale.y == hit.transform.localScale.y)
                {
                    diffx = System.Math.Abs(transform.position.x - hit.transform.position.x);
                    diffy = System.Math.Abs(transform.position.y - hit.transform.position.y);

                    if (lowestDiffX == 0 && lowestDiffY == 0)
                    {
                        lowestDiffX = diffx;
                        lowestDiffY = diffy;
                    }

                    if (closest == null)
                    {
                        closest = hit;
                    }

                    if (transform.localScale.x == 0.7f)
                    {
                        if (diffy < lowestDiffY)
                        {
                            lowestDiffX = diffx;
                            lowestDiffY = diffy;
                            closest = hit;
                        }
                    }
                    else if(transform.localScale.x == 6f)
                    {
                        if(diffx < lowestDiffX)
                        {
                            lowestDiffX = diffx;
                            lowestDiffY = diffy;
                            closest = hit;
                        }
                    }
                }
            }
        }

        GameBoard.PlayerEnum player = controller.GetWhoseTurn();

        if (closest != null && controller.IsValidWallPlacement(player, closest.name))
        {
            transform.position = new Vector3(closest.transform.position.x, closest.transform.position.y, -.7f);
            lockPlace = true;
            controller.MarkPlayerMoved();
        }
        else
        {
            if (transform.localScale.x == 6)
            {
                transform.localScale = new Vector3(.7f, 6, -.7f);
            }

            transform.position = startPos;
        }
    }

}
