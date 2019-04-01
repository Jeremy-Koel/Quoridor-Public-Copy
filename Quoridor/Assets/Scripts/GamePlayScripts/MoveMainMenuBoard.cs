using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMainMenuBoard : MonoBehaviour
{
    // Start is called before the first frame update
    public bool moveBoard;
    public Vector3 target;
    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        moveBoard = false;
        target = new Vector3(-5.3f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveBoard)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target, step);

            if (transform.position == target)
            {
                moveBoard = false;

            }
        }
    }
}
