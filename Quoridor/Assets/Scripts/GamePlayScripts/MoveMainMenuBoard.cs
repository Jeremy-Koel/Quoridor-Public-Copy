using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMainMenuBoard : MonoBehaviour
{
    // Start is called before the first frame update
    public bool moveBoard =false;
    public Vector3 target;
    public float speed = 20f;
    // Start is called before the first frame update
    void Start()
    {
       // moveBoard = false;
        target = new Vector3(-5.3f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveBoard)
        {
            //float step = speed * Time.deltaTime; // calculate distance to move
            float step = FastAnimations.CalculateSpeed(speed);
            transform.position = Vector3.MoveTowards(transform.position, target, step);

            if (transform.position == target)
            {
                moveBoard = false;

            }
        }
    }
}
