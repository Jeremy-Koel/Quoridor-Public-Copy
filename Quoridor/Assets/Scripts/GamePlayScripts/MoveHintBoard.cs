using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveHintBoard : MonoBehaviour
{
    // Start is called before the first frame update
    public bool moveBoard = false;
    public bool moveOntoScreen = false;
    public bool moveOffScreen = false;
    public Vector3 onScreenTarget;
    public Vector3 origPos;
    public Vector3 activePos;
    private EventManager eventManager;
    private float speed = 40f;

    private void OnDestroy()
    {
        eventManager = null;
    }

    private void Awake()
    {
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        eventManager.ListenToHintCalcStart(SetMoveOnScreen);
        eventManager.ListenToHintCalcEnd(SetMoveOffScreen);
    }

    // Start is called before the first frame update
    void Start()
    {
        origPos = transform.position;
        onScreenTarget = new Vector3(45, -5, 0);
        activePos = onScreenTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveOntoScreen)
        {
            // calculate distance to move
            float step = FastAnimations.CalculateSpeed(speed);
            transform.position = Vector3.MoveTowards(transform.position, onScreenTarget, step);

            if (transform.position == onScreenTarget)
            {
                moveOntoScreen = false;
            }
        }
        else if (moveOffScreen)
        {
            float step = FastAnimations.CalculateSpeed(speed);
            transform.position = Vector3.MoveTowards(transform.position, origPos, step);

            if (transform.position == origPos)
            {
                moveOffScreen = false;
            }
        }
    }

    private void SetMoveOnScreen()
    {
        moveOntoScreen = true;
    }

    private void SetMoveOffScreen()
    {
        moveOffScreen = true;
    }
}
