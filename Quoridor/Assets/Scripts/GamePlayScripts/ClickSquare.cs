﻿using System.Collections;
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
    private InvalidMovePopup invalidPopup;
    private InterfaceController interfaceController;

    // Start is called before the first frame update
    void Start()
    {
        playerMouse = GameObject.Find("playerMouse");
        opponentMouse = GameObject.Find("opponentMouse");

        playerClickMouseScript = playerMouse.GetComponent<ClickMouse>();
        opponentClickMouseScript = opponentMouse.GetComponent<ClickMouse>();

        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();

        invalidPopup = interfaceController.GetComponent<InvalidMovePopup>();        
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //TODO
            //controller.IsValidMove...
            // transform.localScale = new Vector3(transform.localScale.x + .05f, transform.localScale.y + .05f, transform.localScale.z);
            if (interfaceController.GetWhoseTurn() == GameBoard.PlayerEnum.ONE)
            {
                GameObject highlight = transform.GetChild(0).gameObject;
                highlight.SetActive(true);
            }
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
                    if (interfaceController.RecordLocalPlayerMove(gameObject.name))
                    {
                        //playerMouse.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -0.5f);
                        moveMouseScript.target = new Vector3(transform.position.x, transform.position.y, -0.5f);
                        moveMouseScript.moveMouse = true;
                        interfaceController.PlayMouseMoveSound();

                    }
                    else
                    {
                        invalidPopup.isPoppedUp = true;
                    }
                }
            }
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