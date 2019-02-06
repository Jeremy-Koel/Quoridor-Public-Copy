﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWalls : MonoBehaviour
{

    public GameObject playerOne;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        playerOne = GameObject.Find("playerMouse");
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnMouseOver()
    {
       // Debug.Log("TEST");
        if (Input.GetMouseButtonDown(1))
        {
            // Debug.Log("Right CLick");
            // this.transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + 89.5f);
            //transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z + 89.5f);


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
        if(hits.Length == 1)
        {
            if (System.Math.Abs(hits[0].transform.localEulerAngles.z - 180) <= System.Math.Abs(transform.localEulerAngles.z + 10) && System.Math.Abs(hits[0].transform.localEulerAngles.z - 180) >= transform.localEulerAngles.z - 10)
            {
                transform.position = new Vector3(hits[0].transform.position.x, hits[0].transform.position.y, -.7f);
            }
            else
            {
                transform.position = startPos;
            }
        }
        else if (hits.Length > 1)
        {
            Collider closest = null;
            float lowestDiffX = 0;
            float lowestDiffY = 0;
            float diffx = 0;
            float diffy = 0;

            foreach (Collider hit in hits)
            {
                float colliderRotation = System.Math.Abs(hit.transform.localEulerAngles.z - 180);
                float wallRotation = System.Math.Abs(transform.localEulerAngles.z);


                if (System.Math.Abs(hit.transform.localEulerAngles.z - 180) <= System.Math.Abs(transform.localEulerAngles.z + 10) && System.Math.Abs(hit.transform.localEulerAngles.z -180) >= transform.localEulerAngles.z - 10)
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

                    if (diffy < lowestDiffY)
                    {
                        lowestDiffX = diffx;
                        lowestDiffY = diffy;
                        closest = hit;
                    }
                }
            }

            if(closest == null)
            {
                transform.position = startPos;
            }
            else
            {
                transform.position = new Vector3(closest.transform.position.x, closest.transform.position.y, -.7f);
            }

           

            // transform.position = new Vector3(hits[0].transform.position.x, hits[0].transform.position.y, -.7f);
        }
        else if(hits.Length == 0)
        {
            transform.position = startPos;
        }
    }

}
