using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using UnityEngine.EventSystems;

public class ClickSquare : MonoBehaviour
{
    public GameObject playerMouse;
    public GameObject opponentMouse;

    private ClickMouse playerClickMouseScript;
    private ClickMouse opponentClickMouseScript;

    private Controller controller;

    private InvalidMovePopup invalidPopup;

    // script for playing sound effects 
    private SoundEffectController soundEffectController;

    // Start is called before the first frame update
    void Start()
    {
        playerMouse = GameObject.Find("playerMouse");
        opponentMouse = GameObject.Find("opponentMouse");

        playerClickMouseScript = playerMouse.GetComponent<ClickMouse>();
        opponentClickMouseScript = opponentMouse.GetComponent<ClickMouse>();

        controller = gameObject.GetComponentInParent<Controller>();

        invalidPopup = controller.GetComponent<InvalidMovePopup>();

        soundEffectController = GameObject.Find("GameController").GetComponent<SoundEffectController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //TODO
            //controller.IsValidMove...
            // transform.localScale = new Vector3(transform.localScale.x + .05f, transform.localScale.y + .05f, transform.localScale.z);
            GameObject highlight = transform.GetChild(0).gameObject;
            highlight.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            // transform.localScale = new Vector3(transform.localScale.x - .05f, transform.localScale.y - .05f, transform.localScale.z);
            GameObject highlight = transform.GetChild(0).gameObject;
            highlight.SetActive(false);
        }
    }

    private void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (playerClickMouseScript.mouseSelected)
            {
                MoveMouse moveMouseScript = playerMouse.GetComponent<MoveMouse>();
                if ((playerMouse.transform.position.x != this.transform.position.x || playerMouse.transform.position.y != this.transform.position.y) && !moveMouseScript.moveMouse)
                {
                    if (controller.IsValidMove(GameBoard.PlayerEnum.ONE, gameObject.name))
                    {
                        
                        moveMouseScript.target = new Vector3(transform.position.x, transform.position.y, -0.5f);
                        moveMouseScript.moveMouse = true;
                        //playerMouse.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -0.5f);
                        controller.MarkLocalPlayerMove();
                        soundEffectController.PlaySqueakSound();
                    }
                    else
                    {
                        invalidPopup.isPoppedUp = true;
                    }
                }
            }

            // This was commented out on 2/24 to prevent local player from being able to move opponent's mouse 
            //if (opponentClickMouseScript.mouseSelected)
            //{
            //    if ((opponentMouse.transform.position.x != this.transform.position.x || opponentMouse.transform.position.y != this.transform.position.y))
            //    {
            //        if (controller.IsValidMove(GameBoard.PlayerEnum.TWO, gameObject.name))
            //        {
            //            opponentMouse.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -0.5f);
            //        }
            //         else
            //        {
            //            invalidPopup.isPoppedUp = true;
            //        }
            //    }
            //}
        }
    }

    public void OnClickToggleMouse()
    {

        playerClickMouseScript.mouseSelected = !playerClickMouseScript.mouseSelected;
    }

    public void OnClickToggleMouse2()
    {
        opponentClickMouseScript.mouseSelected = !opponentClickMouseScript.mouseSelected;

    }
}