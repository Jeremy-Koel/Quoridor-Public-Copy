﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnPopup : MonoBehaviour
{

    public EventManager eventManager;
    public Controller controller; 

    private float popupTime = 2f;

    public bool isPoppedUp = false;

    private GameObject playerTurnPopup;
    private GameObject playerTurnPopupText;
    private Text turnText;

    private string PLAYERNAME = "Player";
    private string COMPUTERNAME = "Computer";
    private string YOURTURNTEXT = "Player's turn";
    private string OTHERPLAYERTEXT = "Computer's turn";

    private void Awake()
    {
        playerTurnPopup = GameObject.Find("PlayerTurnBox");
        playerTurnPopupText = GameObject.Find("PlayerTurnBoxText");
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        controller = GameObject.Find("GameController").GetComponent<Controller>();
        turnText = playerTurnPopupText.GetComponent<Text>();

        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            eventManager.ListenToChallengeTurnTaken(UpdateTurnPopup);            
        }
        else
        {
            eventManager.ListenToTurnTaken(UpdateTurnPopup);
        }

    }

    private void Start()
    {
        playerTurnPopup.SetActive(false);
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            UpdateTurnPopup();
        }
        else
        {
            Text playerOneText = GameObject.Find("PlayerOneText").GetComponent<Text>();
            Debug.Log("(PlayerTurnPopup) PlayerOneText value is: " + playerOneText.text);
            if (playerOneText.text == PLAYERNAME)
            {
                turnText.text = YOURTURNTEXT;
            }
            else
            {
                turnText.text = OTHERPLAYERTEXT;
            }
            playerTurnPopup.SetActive(true);
            StartCoroutine(PopUp());
        }        
    }

    void Update()
    {

    }

    public void UpdateTurnPopup()
    {
        isPoppedUp = true;
        playerTurnPopup.SetActive(true);
            
            
        // Get info from ChallengeManager/Controller
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            turnText.text = controller.GetPlayerNameForTurn() + "'s Turn!";
        }
        else
        {
            turnText.text = turnText.text == YOURTURNTEXT ?
                OTHERPLAYERTEXT : YOURTURNTEXT;
        }
            
        Debug.Log("turnText value: " + turnText.text);
        StartCoroutine(PopUp());
    }

    IEnumerator PopUp()
    {        
        float time = 0;

        while (time < popupTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        isPoppedUp = false;
        playerTurnPopup.SetActive(false);
    }
}
