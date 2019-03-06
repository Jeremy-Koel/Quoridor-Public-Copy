﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArms : MonoBehaviour
{
    public bool moveArm;
    public Vector3 target;
    private float speed = 20.0f;
    private Vector3 origPos;
    
    // Start is called before the first frame update
    void Start()
    {
        origPos = transform.position;
        moveArm = false;
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveArm)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target, step);

            if (transform.position == target && target != origPos)
            {
                target = origPos;
            }
            else if(transform.position == target)
            {
                moveArm = false;
            }


        }
    }

    public void SetTarget(Vector3 pos)
    {
        if (name == "ScientistArmOne")
        {
            Vector3 newTarget = new Vector3(pos.x, pos.y - 23, -2);
            target = newTarget;
        }
        else if(name == "ScientistArmTwo")
        {
            Vector3 newTarget = new Vector3(pos.x, pos.y + 23, -2);
            target = newTarget;
        }
    }
}
