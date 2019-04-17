using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMultiplayerScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public bool moveBoard = false;
    public Vector3 target;
    public float speed = 28f;
    private Vector3 onScreenTarget;
    private Vector3 offScreenTarget;
    private GameObject mainMenuPanel;
    private GameObject dummyMenuPanel;
    private MainMenu mainMenuScript;
    // Start is called before the first frame update
    void Start()
    {
        onScreenTarget = new Vector3(0, 0, 0);
        offScreenTarget = transform.position;
        // moveBoard = false;
        target = onScreenTarget;
        mainMenuPanel = GameObject.Find("MainButtonPanel");
        dummyMenuPanel = GameObject.Find("DummyMenuPanel");
        mainMenuScript = GameObject.Find("Main Camera").GetComponent<MainMenu>();
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
                if (transform.position == onScreenTarget)
                {
                    target = offScreenTarget;
                }
                else if(transform.position == offScreenTarget)
                {
                    target = onScreenTarget;
                    mainMenuScript.SetMainMenuButtonsActive();
                    dummyMenuPanel.SetActive(false);
                    mainMenuPanel.SetActive(true);
                }

                moveBoard = false;

            }
        }
    }


}
