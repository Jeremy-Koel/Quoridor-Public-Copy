using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSquare : MonoBehaviour
{
    public GameObject mouse;
    private bool mouseSelected;
    private ClickMouse test;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("ADDED");
        mouse = GameObject.Find("playerMouse");

        test = mouse.GetComponent<ClickMouse>();

        
        mouseSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(test.mouseSelected)
        {
            Debug.Log("Clicked Mouse");
        }

    }

    private void OnMouseUp()
    {
        Debug.Log(this.name);
        mouse.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -0.5f);
    }

    public void OnClickToggleMouse()
    {
        mouseSelected = !mouseSelected;
    }
}
