using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWallsProgramatically : MonoBehaviour
{
    public bool moveWall;
    public Vector3 target;
    public float speed = 12.0f;
    private bool isOnBoard = false;
    // Start is called before the first frame update
    void Start()
    {

        moveWall = false;
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(moveWall)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target, step);

            if(transform.position == target)
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
            newSize = new Vector3(colliderSize.x, colliderSize.y * 2, colliderSize.z);
        }
        else if (colliderSize.x == 3f)
        {
            newSize = new Vector3(colliderSize.x * 2, colliderSize.y, colliderSize.z);
        }

        return newSize;
    }

    public void SetTarget(Vector3 pos, Vector3 colliderSize)
    {
        if (colliderSize.x == .7f)
        {
            pos = new Vector3(pos.x, pos.y - (colliderSize.y/2) - .25f, pos.z);
        }
        else if (colliderSize.x == 3f)
        {
            pos = new Vector3(pos.x + (colliderSize.x/2) + .25f, pos.y, pos.z);
        }

        target = pos;
    }
}
