using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ChallengeManager : MonoBehaviour
{

    public UnityEvent ChallengeStarted;
    public UnityEvent ChallengeTurnTaken;
    //public UnityEvent ChallengeWon;
    //public UnityEvent ChallengeLost;

    public bool IsChallengeActive { get; private set; }

    public string HostsPlayerName { get; private set; }

    public string HostsPlayerID { get; private set; }

    public string ChallengersPlayerName { get; private set; }

    public string ChallengersPlayerID { get; private set; }

    public string CurrentPlayerName { get; private set; }

    public string challengeID { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        ChallengeStartedMessage.Listener += OnChallengeStarted;
        Debug.Log("ChallengeStartedMessage Listener set");
        ChallengeTurnTakenMessage.Listener += OnChallengeTurnTaken;
        Debug.Log("ChallengeTurnTakenMessage Listener set");
    }

    // Start is called before the first frame update
    void Start()
    {
        //ChallengeStartedMessage.Listener += OnChallengeStarted;
        //ChallengeTurnTakenMessage.Listener += OnChallengeTurnTaken;
        //ChallengeWonMessage.Listener += OnChallengeWon;
        //ChallengeLostMessage.Listener += OnChallengeLost;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnChallengeStarted(ChallengeStartedMessage message)
    {
        IsChallengeActive = true;
        challengeID = message.Challenge.ChallengeId;
        HostsPlayerName = message.Challenge.Challenger.Name;
        HostsPlayerID = message.Challenge.Challenger.Id;
        ChallengersPlayerName = message.Challenge.Challenged.First().Name;
        ChallengersPlayerID = message.Challenge.Challenged.First().Id;
        CurrentPlayerName = message.Challenge.NextPlayer == HostsPlayerID ? HostsPlayerName : ChallengersPlayerName;

        ChallengeStarted.Invoke();
    }

    void OnChallengeTurnTaken(ChallengeTurnTakenMessage message)
    {
        // Get current player ID
        var playerIDObject = GameObject.FindWithTag("PlayerID");
        GameSparksUserID playerIDComponent = playerIDObject.GetComponent<GameSparksUserID>();
        var myCurrentPlayerID = playerIDComponent.myUserID;

        var scriptData = message.Challenge.ScriptData.BaseData;
        Debug.Log("My Player ID: " + myCurrentPlayerID);
        Debug.Log("Player ID Used: " + scriptData["PlayerIDUsed"].ToString());
        if (myCurrentPlayerID != scriptData["PlayerIDUsed"].ToString())
        {
            string scriptDataAction = scriptData["ActionUsed"].ToString();
            Debug.Log("Action Received: " + scriptDataAction);


            ////GamePanel.Instance.ReceiveMove(scriptDataX);
            //// Get GamePanel
            //var gameBoardObject = GameObject.Find("GameBoard");
            //GamePanel gamePanelComponent = gamePanelObject.GetComponent<GamePanel>();
            //gamePanelComponent.ReceiveMove(scriptDataAction);

        }
        //ChallengeTurnTaken.Invoke(); UnityEvent
    }

    public void GetLastValidMove()
    {
        // Might be bad performance looking for the GameController repeatedly
        GameObject gameControllerObject = GameObject.Find("GameController");
        Controller gameControllerScript = gameControllerObject.GetComponent<Controller>();
        string move = gameControllerScript.lastValidMove;
        Move(move);
    }

    public void Move(string action)
    {        
        LogChallengeEventRequest request = new LogChallengeEventRequest();
        request.SetChallengeInstanceId(challengeID);
        request.SetEventKey("Move");
        request.SetEventAttribute("Action", action);
        request.Send(OnMoveSuccess, OnMoveError);
    }

    private void OnMoveSuccess(LogChallengeEventResponse response)
    {
        print(response.JSONString);
    }

    private void OnMoveError(LogChallengeEventResponse response)
    {
        print(response.Errors.JSON.ToString());
    }


}
