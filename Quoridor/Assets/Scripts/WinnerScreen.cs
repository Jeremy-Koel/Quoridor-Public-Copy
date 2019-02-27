using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerScreen : MonoBehaviour
{
    // Start is called before the first frame update
    //private GameObject winPanel;
    private Text winText;
    private Controller controller;
    private Text gameOverText;

    void Start()
    {
        controller = GameObject.Find("GameController").GetComponent<Controller>();
        winText = GameObject.Find("WinnerText").GetComponent<Text>();
        winText.text = getWhoWon();
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        gameOverText.text = "Game Over!";

        SoundEffectController soundEffectController = GameObject.Find("GameController").GetComponent<SoundEffectController>();
        if (winText.text == "You Win!" || winText.text == controller.GetLocalPlayerName()+" Wins!")
        {
            Debug.Log("playing win sound");
            soundEffectController.PlayWinSound();
        }
        else
        {
            Debug.Log("playing lose sound");
            soundEffectController.PlayLoseSound();
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        //if(controller.IsGameOver())
        //{
        //    //winPanel.transform.position = new Vector3(winPanel.transform.position.x, winPanel.transform.position.y, -2);
        //    winText.text = "Game Over!";
           
        //}
    }

    private string getWhoWon()
    {
        return controller.WhoWon();
    }
}
