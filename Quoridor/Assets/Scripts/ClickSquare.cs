using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;

public class ClickSquare : MonoBehaviour
{
    public GameObject mouse;
    public GameObject mouse2;
  
    private ClickMouse clickMouseScript;
    private ClickMouse clickMouseScript2;

    private Controller controller;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("ADDED");
        mouse = GameObject.Find("playerMouse");
        mouse2 = GameObject.Find("playerMouse2");

        clickMouseScript = mouse.GetComponent<ClickMouse>();
        clickMouseScript2 = mouse2.GetComponent<ClickMouse>();

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
        if (clickMouseScript.mouseSelected)
        {
            if ((mouse.transform.position.x != this.transform.position.x || mouse.transform.position.y != this.transform.position.y))
            {
                if (controller.IsValidMove(GameBoard.PlayerEnum.ONE, gameObject.name))
                {
                    mouse.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -0.5f);
                }

                //OnClickToggleMouse();
            }

            
        }
        
        if (clickMouseScript2.mouseSelected)
        {
            if ((mouse2.transform.position.x != this.transform.position.x || mouse2.transform.position.y != this.transform.position.y))
            {
                if (controller.IsValidMove(GameBoard.PlayerEnum.TWO, gameObject.name))
                {
                    mouse2.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -0.5f);
                }

               // OnClickToggleMouse2();
            }

            
        }
        
    }

    public void OnClickToggleMouse()
    {

        clickMouseScript.mouseSelected = !clickMouseScript.mouseSelected;
    }

    public void OnClickToggleMouse2()
    {
        clickMouseScript2.mouseSelected = !clickMouseScript2.mouseSelected;
        
    }
}
