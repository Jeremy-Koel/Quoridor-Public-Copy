using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWallsProgramatically : MonoBehaviour
{
    public bool moveWall;
    public Vector3 target;
    public float speed = 20.0f;
    private bool isOnBoard = false;
    private GameObject scientistArmOne;
    private GameObject scientistArmTwo;
    private MoveArms moveArmsOne;
    private MoveArms moveArmsTwo;
    private Material verticalWallMat;
    private Material horizontalWallMat;
    private Renderer matRenderer;
   
    // Start is called before the first frame update
    void Start()
    {
        scientistArmOne = GameObject.Find("ScientistArmOne");
        scientistArmTwo = GameObject.Find("ScientistArmTwo");
        moveArmsOne = scientistArmOne.GetComponent<MoveArms>();
        moveArmsTwo = scientistArmTwo.GetComponent<MoveArms>();
        moveWall = false;
        target = transform.position;
        verticalWallMat = Resources.Load("wallColor", typeof(Material)) as Material;
        horizontalWallMat = Resources.Load("horizontalWallColor", typeof(Material)) as Material;
        matRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveWall)
        {
            if (tag == "PlayerOneWall")
            {
                moveArmsOne.moveArm = true;

            }
            else
            {
                moveArmsTwo.moveArm = true;
            }

            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target, step);

            if (transform.position == target)
            {
                moveWall = false;
            }
        }

    }

    public bool IsOnBoard()
    {
        return isOnBoard;
    }

    public void SetIsOnBoard(bool setValue)
    {
        isOnBoard = setValue;
    }

    public Vector3 GetWallSize(Vector3 colliderSize)
    {
        Vector3 newSize = colliderSize;
        if (colliderSize.x == .7f)
        {
            newSize = new Vector3(colliderSize.x, (colliderSize.y * 2) + .5f, colliderSize.z);
            matRenderer.material = verticalWallMat;
        }
        else if (colliderSize.x == 4f)
        {
            newSize = new Vector3((colliderSize.x * 2) +.5f, colliderSize.y, colliderSize.z);
            matRenderer.material = horizontalWallMat;
        }

        return newSize;
    }

    public void SetTarget(Vector3 pos, Vector3 colliderSize)
    {
        if (colliderSize.x == .7f)
        {
            pos = new Vector3(pos.x, pos.y - (colliderSize.y/2) - .5f, -0.7f);
        }
        else if (colliderSize.x == 4f)
        {
            pos = new Vector3(pos.x + (colliderSize.x/2) + .5f, pos.y, -0.7f);
        }

        target = pos;

        if(tag == "PlayerOneWall")
        {
            moveArmsOne.SetTarget(pos);
        }
        else
        {
            moveArmsTwo.SetTarget(pos);
        }
    }
}
