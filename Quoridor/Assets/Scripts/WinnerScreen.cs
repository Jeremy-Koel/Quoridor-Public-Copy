using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerScreen : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject winPanel;
    private Text winText;
    private Controller controller;
    void Start()
    {
        winPanel = GameObject.Find("WinScreen");
        winText = GameObject.Find("WinnerText").GetComponent<Text>();
        winPanel.transform.position = new Vector3(winPanel.transform.position.x, winPanel.transform.position.y, 2);
        controller = winPanel.transform.parent.GetComponentInParent<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if(controller.IsGameOver())
        {
            winPanel.transform.position = new Vector3(winPanel.transform.position.x, winPanel.transform.position.y, -2);
            winText.text = "Game Over!";
           
        }
    }
}
