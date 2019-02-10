using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMouse : MonoBehaviour
{

    public bool mouseSelected;
    private bool cursorOnMouse;

    // Start is called before the first frame update
    void Start()
    {
        mouseSelected = false;
        cursorOnMouse = false;
    }


    //https://answers.unity.com/questions/587637/replacing-onmouseenterexitdownetc-with-raycasting.html
    // Update is called once per frame
    public GameObject hoveredGO;
    public enum HoverState { HOVER, NONE };
    public HoverState hover_state = HoverState.NONE;

    void Update()
    {
        RaycastHit hitInfo = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hover_state == HoverState.NONE)
            {
                hitInfo.collider.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
                hoveredGO = hitInfo.collider.gameObject;
            }
            hover_state = HoverState.HOVER;
        }
        else
        {
            if (hover_state == HoverState.HOVER)
            {
                hoveredGO.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
            }
            hover_state = HoverState.NONE;
        }

        if (hover_state == HoverState.HOVER)
        {
            hitInfo.collider.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver); //Mouse is hovering
            if (Input.GetMouseButtonDown(0))
            {
                hitInfo.collider.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver); //Mouse down
            }
            if (Input.GetMouseButtonUp(0))
            {
                hitInfo.collider.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver); //Mouse up
            }

        }

        if(Input.GetMouseButtonUp(0) && !cursorOnMouse)
        {
            mouseSelected = false;
            Debug.Log("Update Mouse");
        }
    }

    private void OnMouseEnter()
    {
        cursorOnMouse = true;
    }

    private void OnMouseExit()
    {
        cursorOnMouse = false;
    }
    private void OnMouseUp()
    {
        mouseSelected = true;
        Debug.Log("ButtonMouse");
        //gameObject.SendMessage("OnClickToggleMouse", SendMessageOptions.DontRequireReceiver);
    }
}
