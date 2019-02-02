using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSquare : MonoBehaviour
{
    public GameObject mouse;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("ADDED");
        mouse = GameObject.Find("playerMouse");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseUp()
    {
        Debug.Log(this.name);
        mouse.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -0.5f);
    }
}
