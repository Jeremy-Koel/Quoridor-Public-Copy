﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMouse : MonoBehaviour
{
    public bool moveMouse;
    public Vector3 target;
    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        moveMouse = false;
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveMouse)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            
            if (transform.position == target)
            {
                moveMouse = false;
                transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
                transform.localScale = new Vector3(.04f, .04f, transform.localScale.z);
            }
        }
    }
}
