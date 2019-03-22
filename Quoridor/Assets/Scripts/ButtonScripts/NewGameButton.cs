﻿using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameButton : MonoBehaviour
{
    private Button newGameWinButton;
    private Button newGameMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("NewGameButton") != null)
        {
            newGameMenuButton = GameObject.Find("NewGameButton").GetComponent<Button>();
            newGameMenuButton.interactable = false;
            if (GameModeStatus.GameMode == GameModeEnum.SINGLE_PLAYER)
            {
                newGameMenuButton.interactable = true;
            }
        }
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            if (GameObject.Find("WinScreenNewGameButton") != null)
            {
                newGameWinButton = GameObject.Find("WinScreenNewGameButton").GetComponent<Button>();      
                ChallengeStartedMessage.Listener += OnChallengeStarted;
                ChallengeIssuedMessage.Listener += OnChallengeIssued;
            }
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {

    }

    public void onNewGameButtonClick()
    {
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {

            EventManager eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
            eventManager.InvokePlayAgain();

            StartCoroutine(onMatchMakingButtonClick());
        }
        else
        {
            SceneManager.LoadScene("GameBoard");
        }
    }

    public IEnumerator onMatchMakingButtonClick()
    {
        BlockInput();
        Debug.Log("Making/sending matchmaking request");

        string matchGroupNumber = "";
        yield return GetMatchmakingGroupNumber(matchGroupNumber);

        MatchmakingRequest request = new MatchmakingRequest();
        request.SetMatchShortCode("DefaultMatch");
        request.SetSkill(0);
        request.SetMatchGroup(matchGroupNumber);
        request.Send(OnMatchmakingSuccess, OnMatchmakingError);
        
    }

    private IEnumerator GetMatchmakingGroupNumber(string matchmakingGroupNumber)
    {
        MessageQueue messageQueue = GameObject.Find("MessageQueue").GetComponent<MessageQueue>();
        while (messageQueue.IsQueueEmpty(MessageQueue.QueueNameEnum.MATCHMAKINGGROUPNUMBER))
        {
            yield return messageQueue.CheckQueueNotEmpty(MessageQueue.QueueNameEnum.MATCHMAKINGGROUPNUMBER);
        }
        ScriptMessage message = messageQueue.DequeueMatchmakingGroupNumber();
        IDictionary<string, object> messageData = message.Data.BaseData;
        Debug.Log("Matchmaking Group Number: " + messageData["matchGroupNumber"].ToString());
        var matchGroupNumber = messageData["matchGroupNumber"];
        matchmakingGroupNumber = matchGroupNumber.ToString();
    }

    public void OnMatchmakingSuccess(MatchmakingResponse response)
    {
        //UnblockInput();
        Debug.Log("Matchmaking Success");
    }

    public void OnMatchmakingError(MatchmakingResponse response)
    {
        //UnblockInput();
        Debug.Log("Matchmaking Error");
    }

    private void OnChallengeIssued(ChallengeIssuedMessage message)
    {
        Debug.Log("On Challenge Issued");
        var challengeInstaceId = message.Challenge.ChallengeId;
        Debug.Log("This challenge ID: " + challengeInstaceId);
        if (challengeInstaceId != null)
        {
            new AcceptChallengeRequest()
                .SetChallengeInstanceId(challengeInstaceId)
                //.SetMessage(message)
                .Send((response) => {
                    //string challengeInstanceId = response.ChallengeInstanceId;
                    //GSData scriptData = response.ScriptData;
                });
        }
    }

    private void OnChallengeStarted(ChallengeStartedMessage message)
    {
        UnblockInput();
        Debug.Log("Challenge Started");
        // Switch to GameBoard Scene connected to opponent
        SceneManager.LoadScene("GameBoard");
        EventManager eventManager = GameObject.Find("EventManger").GetComponent<EventManager>();
        eventManager.InvokeNewGame();
    }

    private void BlockInput()
    {
        newGameWinButton.interactable = false;
    }

    private void UnblockInput()
    {
        newGameWinButton.interactable = true;
    }
}