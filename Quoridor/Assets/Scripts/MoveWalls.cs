using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using UnityEngine.EventSystems;

public class MoveWalls : MonoBehaviour
{

    public GameObject playerOne;
    public bool mouseHeldDownOnWall = false;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 startPos;
    public InterfaceController interfaceController;
    private string wallTag;
    private BoxCollider2D wallCollider;
    private bool lockPlace;
    private InvalidMovePopup invalidPopup;
   
    // Start is called before the first frame update
    void Start()
    {
        playerOne = GameObject.Find("playerMouse");
        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
        startPos = transform.position;
        wallTag = this.tag;
        wallCollider = GetComponent<BoxCollider2D>();
        lockPlace = false;
        invalidPopup = interfaceController.GetComponent<InvalidMovePopup>();
        
    }

    // Update is called once per frame
    void Update()
    {
        GameBoard.PlayerEnum player = interfaceController.GetWhoseTurn();
        if (player == GameBoard.PlayerEnum.ONE && wallTag == "PlayerOneWall" && !lockPlace) 
        {
            wallCollider.enabled = true;
        }
        // This was commented out on 2/24 to prevent local player from being able to move opponent's walls 
        //else if (player == GameBoard.PlayerEnum.TWO && wallTag == "PlayerTwoWall" && !lockPlace)
        //{
        //    wallCollider.enabled = true;
        //}
        else
        {
            wallCollider.enabled = false;
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(1) && mouseHeldDownOnWall)
            {
                transform.localScale = new Vector3(transform.localScale.y, transform.localScale.x, transform.localScale.z);
            }
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
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(1) && !mouseHeldDownOnWall)
            {
                transform.localScale = new Vector3(transform.localScale.y, transform.localScale.x, transform.localScale.z);
            }
        }

        //Debug.Log("MoveWalls.OnMouseOver");
    }

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            mouseHeldDownOnWall = true;
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }
        //Debug.Log("MoveWalls.OnMouseDown");
    }

    private void OnMouseUp()
    {
        mouseHeldDownOnWall = false;
        //Debug.Log("MoveWalls.OnMouseUp");
    }
    void OnMouseDrag()
    {
        if (!EventSystem.current.IsPointerOverGameObject() || mouseHeldDownOnWall)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
        }
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

        GameBoard.PlayerEnum player = interfaceController.GetWhoseTurn();

        if (closest != null && interfaceController.RecordLocalPlayerMove(closest.name))
        {
            transform.position = new Vector3(closest.transform.position.x, closest.transform.position.y, -.7f);
            lockPlace = true;
        }
        else
        {
            if (transform.localScale.x == 6)
            {
                transform.localScale = new Vector3(transform.localScale.y, transform.localScale.x, transform.localScale.z);
            }

            transform.position = startPos;
            invalidPopup.isPoppedUp = true;
        }
    }

}
