using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArms : MonoBehaviour
{
    public bool moveArm;
    public Vector3 target;
    public float speed = 12.0f;
    private Vector3 origPos;
    private GameObject parent;

   // public bool armReachedTarget = false;
    
    // Start is called before the first frame update
    void Start()
    {
        origPos = transform.position;
        moveArm = false;
        target = transform.position;
        //parent = this.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveArm)
        {
            //armReachedTarget = false;
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target, step);

            if (transform.position == target && target != origPos)
            {
                //armReachedTarget = true;
                //moveArm = false;
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
