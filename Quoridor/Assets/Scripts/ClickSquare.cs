using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;

public class ClickSquare : MonoBehaviour
{
    public GameObject playerMouse;
    public GameObject opponentMouse;
  
    private ClickMouse playerClickMouseScript;
    private ClickMouse opponentClickMouseScript;

    private Controller controller;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("ADDED");
        playerMouse = GameObject.Find("playerMouse");
        opponentMouse = GameObject.Find("opponentMouse");

        playerClickMouseScript = playerMouse.GetComponent<ClickMouse>();
        opponentClickMouseScript = opponentMouse.GetComponent<ClickMouse>();

        controller = gameObject.GetComponentInParent<Controller>();
        // mouseSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnMouseEnter()
    {
        //TODO
        //controller.IsValidMove...
        transform.localScale = new Vector3(transform.localScale.x + .05f, transform.localScale.y + .05f, transform.localScale.z);
        GameObject highlight = transform.GetChild(0).gameObject;
        highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        transform.localScale = new Vector3(transform.localScale.x - .05f, transform.localScale.y - .05f, transform.localScale.z);
        GameObject highlight = transform.GetChild(0).gameObject;
        highlight.SetActive(false);
    }

    private void OnMouseUp()
    {
        //Debug.Log(this.name);
        if (playerClickMouseScript.mouseSelected)
        {
            if ((playerMouse.transform.position.x != this.transform.position.x || playerMouse.transform.position.y != this.transform.position.y))
            {
                if (controller.IsValidMove(GameBoard.PlayerEnum.ONE, gameObject.name))
                {
                    playerMouse.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -0.5f);
                    controller.MarkPlayerMoved();
                }
                
                //OnClickToggleMouse();
            }
        }

        if (opponentClickMouseScript.mouseSelected)
        {
            if ((opponentMouse.transform.position.x != this.transform.position.x || opponentMouse.transform.position.y != this.transform.position.y))
            {
                if (controller.IsValidMove(GameBoard.PlayerEnum.TWO, gameObject.name))
                {
                    opponentMouse.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -0.5f);
                }

                // OnClickToggleMouse2();
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
