using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveBoards : MonoBehaviour
{
    // Start is called before the first frame update
    public bool moveBoard = false;
    public Vector3 target;
    public Vector3 origPos;
    public Vector3 activePos;
    private float speed = 80f;
    // Start is called before the first frame update
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        //moveBoard = false;
        origPos = transform.position;

        if (sceneName == "MainMenu")
        {
            target = new Vector3(0, 0, -3);
            speed = 10f;
        }
        else
        {
            target = new Vector3(13, 13, -3);
        }

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

            //float step = speed * Time.deltaTime; // calculate distance to move
            float step = FastAnimations.CalculateSpeed(speed);
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
