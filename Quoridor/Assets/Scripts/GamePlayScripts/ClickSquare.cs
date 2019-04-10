using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using UnityEngine.EventSystems;

public class ClickSquare : MonoBehaviour
{
    public GameObject playerMouse;
    public GameObject opponentMouse;
    //private ClickMouse playerClickMouseScript;
    // private ClickMouse opponentClickMouseScript;
    private InterfaceController interfaceController;
    private MoveArms armTwoScript;
    private MoveMouse opponentMoveMouseScript;

    // Start is called before the first frame update
    void Start()
    {
        playerMouse = GameObject.Find("playerMouse");
        opponentMouse = GameObject.Find("opponentMouse");

        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
        armTwoScript = GameObject.Find("ScientistArmTwo").GetComponent<MoveArms>();
        opponentMoveMouseScript = GameObject.Find("opponentMouse").GetComponent<MoveMouse>();
    }

    void Update()
    {

    }
    //private void OnMouseEnter()
    //{
    //    if (!EventSystem.current.IsPointerOverGameObject())
    //    {
    //        //TODO
    //        //controller.IsValidMove...
    //        // transform.localScale = new Vector3(transform.localScale.x + .05f, transform.localScale.y + .05f, transform.localScale.z);
    //        if (interfaceController.GetWhoseTurn() == GameBoard.PlayerEnum.ONE)
    //        {
    //            GameObject highlight = transform.GetChild(0).gameObject;
    //            highlight.SetActive(true);
    //        }
    //    }
    //}

    //private void OnMouseExit()
    //{
    //    if (!EventSystem.current.IsPointerOverGameObject())
    //    {
    //        // transform.localScale = new Vector3(transform.localScale.x - .05f, transform.localScale.y - .05f, transform.localScale.z);
    //        GameObject highlight = transform.GetChild(0).gameObject;
    //        highlight.SetActive(false);
    //    }
    //}

    private void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (interfaceController.GetWhoseTurn() == GameBoard.PlayerEnum.ONE && armTwoScript.moveArm == false && opponentMoveMouseScript.moveMouse == false)
            {
                MoveMouse moveMouseScript = playerMouse.GetComponent<MoveMouse>();
                if ((playerMouse.transform.position.x != this.transform.position.x || playerMouse.transform.position.y != this.transform.position.y) && !moveMouseScript.moveMouse)
                {
                    if (interfaceController.RecordLocalPlayerMove(gameObject.name))
                    {
                        //playerMouse.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -0.5f);
                        moveMouseScript.target = new Vector3(transform.position.x, transform.position.y, -1f);
                        playerMouse.transform.localScale = new Vector3(playerMouse.transform.localScale.x + .001f, playerMouse.transform.localScale.y + .001f, playerMouse.transform.localScale.z);
                        moveMouseScript.moveMouse = true;
                        interfaceController.PlayMouseMoveSound();

                    }
                    else
                    {
                        // Invalid move
                    }
                }
            }
           // }
        }
    }

    //public void OnClickToggleMouse()
    //{

    //    playerClickMouseScript.mouseSelected = !playerClickMouseScript.mouseSelected;
    //}

    //public void OnClickToggleMouse2()
    //{
    //    opponentClickMouseScript.mouseSelected = !opponentClickMouseScript.mouseSelected;

    //}
}