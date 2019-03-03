using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArms : MonoBehaviour
{
    public bool moveArm;
    public Vector3 target;
    public float speed = 12.0f;
    public bool armReachedTarget = false;
    
    // Start is called before the first frame update
    void Start()
    {

        moveArm = false;
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveArm)
        {
            armReachedTarget = false;
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target, step);

            if (transform.position == target)
            {
                armReachedTarget = true;
                moveArm = false;
            }
        }
    }
}
