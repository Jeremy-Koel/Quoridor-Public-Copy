using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWallsProgramatically : MonoBehaviour
{
    public bool moveWall;
    public Vector3 target;
    public float speed = 12.0f;

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
}
