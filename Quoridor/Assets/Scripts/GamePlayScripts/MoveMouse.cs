using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;

public class MoveMouse : MonoBehaviour
{
    public bool moveMouse;
    public Vector3 target;
    public float speed = 10f;
    //public MoveMouse playerMoveMouseScript;
    //public MoveArms playerMoveArmScript;
   // private InterfaceController interfaceController;
    // Start is called before the first frame update
    void Start()
    {
        moveMouse = false;
        target = transform.position;
        //playerMoveMouseScript = GameObject.Find("playerMouse").GetComponent<MoveMouse>();
        //playerMoveArmScript = GameObject.Find("ScientistArmOne").GetComponent<MoveArms>();
      //  interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveMouse)
        {
            // if (interfaceController.GetWhoseTurn() == GameBoard.PlayerEnum.ONE || (interfaceController.GetWhoseTurn() == GameBoard.PlayerEnum.TWO && playerMoveMouseScript.moveMouse == false) )
            //if((name == "playerMouse") || (name == "opponentMouse" && playerMoveMouseScript.moveMouse == false && playerMoveArmScript.moveArm == false))
            //{

            //float step = speed * Time.deltaTime; // calculate distance to move
            float step = FastAnimations.CalculateSpeed(speed);
            transform.position = Vector3.MoveTowards(transform.position, target, step);

            if (transform.position == target)
            {
                moveMouse = false;
                transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
                transform.localScale = new Vector3(.05f, .05f, transform.localScale.z);
            }
            //}
        }
    }
}
