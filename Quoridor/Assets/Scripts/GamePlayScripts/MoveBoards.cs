using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoards : MonoBehaviour
{
    // Start is called before the first frame update
    public bool moveBoard = false;
    public Vector3 target;
    public Vector3 origPos;
    public Vector3 activePos;
    private float speed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        //moveBoard = false;
        origPos = transform.position;
        target = new Vector3(12, 13, -3);
        activePos = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveBoard)
        {
            if (transform.position == origPos)
            {
                target = activePos;
            }
            else if (transform.position == activePos)
            {
                target = origPos;
            }

            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target, step);

            if (transform.position == target)
            {
                moveBoard = false;
                if (target == origPos)
                {
                    transform.parent.gameObject.SetActive(false);
                }

            }
        }
    }
}
