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
    public UnityEvent ChallengeStartingPlayerSet;
    //public UnityEvent ChallengeWon;
    //public UnityEvent ChallengeLost;

    GameSparksUserID gameSparksUserIDScript;

    public bool IsChallengeActive { get; private set; }

    public PlayerInfo FirstPlayerInfo { get; private set; }

    public string FirstPlayerName { get; private set; }

    public string FirstPlayerID { get; private set; }

    public PlayerInfo SecondPlayerInfo { get; private set; }
    public string SecondPlayerName { get; private set; }

    public string SecondPlayerID { get; private set; }
    public PlayerInfo CurrentPlayerInfo { get; private set; }

    public string CurrentPlayerName { get; private set; }
    
    public string CurrentPlayerID { get; private set; }

    public int CurrentPlayerNumber { get; private set; }

    public string ChallengeID { get; private set; }

    public string LastOpponentMove { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameSparksUserIDObject = GameObject.Find("GameSparksUserID");
        GameSparksUserID gameSparksUserIDScript = gameSparksUserIDObject.GetComponent<GameSparksUserID>();
        CurrentPlayerInfo.PlayerID = gameSparksUserIDScript.myUserID;
        CurrentPlayerID = gameSparksUserIDScript.myUserID;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetupChallengeListeners()
    {
        ChallengeStartedMessage.Listener += OnChallengeStarted;
        Debug.Log("ChallengeStartedMessage Listener set");
        ChallengeTurnTakenMessage.Listener += OnChallengeTurnTaken;
        Debug.Log("ChallengeTurnTakenMessage Listener set");
        ScriptMessage_ChallengeStartingPlayerMessage.Listener += OnChallengeStartingPlayer;
    }

    void OnChallengeStarted(ChallengeStartedMessage message)
    {
        Debug.Log("ChallengeStarted");
        IsChallengeActive = true;
        ChallengeID = message.Challenge.ChallengeId;
        //FirstPlayerName = message.Challenge.Challenger.Name;
        //FirstPlayerID = message.Challenge.Challenger.Id;
        //SecondPlayerName = message.Challenge.Challenged.First().Name;
        //SecondPlayerID = message.Challenge.Challenged.First().Id;
        //CurrentPlayerName = message.Challenge.NextPlayer == FirstPlayerID ? FirstPlayerName : SecondPlayerName;

        Debug.Log("ChallengeID: " + ChallengeID);
        //Debug.Log("HostsPlayerName: " + FirstPlayerName);
        //Debug.Log("ChallengersPlayerName: " + SecondPlayerName);

        ChallengeStarted.Invoke();
    }

    void OnChallengeTurnTaken(ChallengeTurnTakenMessage message)
    {
        Debug.Log("Challenge Turn Taken");
        
        string gameSparksUserID = gameSparksUserIDScript.myUserID;
        Debug.Log("My Player ID: " + gameSparksUserID);

        CurrentPlayerName = message.Challenge.NextPlayer == FirstPlayerID ? FirstPlayerName : SecondPlayerName;

        var scriptData = message.Challenge.ScriptData.BaseData;
        Debug.Log("Player ID Used for move: " + scriptData["PlayerIDUsed"].ToString());
        if (gameSparksUserID != scriptData["PlayerIDUsed"].ToString())
        {
            string scriptDataAction = scriptData["ActionUsed"].ToString();
            Debug.Log("Action Received: " + scriptDataAction);

            // Notify controller that a move was received
            LastOpponentMove = scriptDataAction;

        }
        ChallengeTurnTaken.Invoke();
    }

    void OnChallengeStartingPlayer(ScriptMessage_ChallengeStartingPlayerMessage message)
    {
        IDictionary<string, object> messageData = message.Data.BaseData;
        string startingPlayerID = messageData["startingPlayer"].ToString();
        string gameSparksUserID = gameSparksUserIDScript.myUserID;

        FirstPlayerInfo.PlayerID = startingPlayerID;
        FirstPlayerInfo.PlayerDisplayName = messageData["startingPlayerName"].ToString();
        FirstPlayerInfo.PlayerNumber = 1;
        FirstPlayerInfo.PlayerEnum = GameCore.GameBoard.PlayerEnum.ONE;
        FirstPlayerID = startingPlayerID;
        FirstPlayerName = messageData["startingPlayerName"].ToString();

        SecondPlayerInfo.PlayerID = messageData["secondPlayer"].ToString();
        SecondPlayerInfo.PlayerDisplayName = messageData["secondPlayerName"].ToString();
        SecondPlayerInfo.PlayerNumber = 2;
        SecondPlayerInfo.PlayerEnum = GameCore.GameBoard.PlayerEnum.TWO;
        SecondPlayerID = messageData["secondPlayer"].ToString();
        SecondPlayerName = messageData["secondPlayerName"].ToString();

        WhichPlayerNumberAmI();

        // Tell the server to set the starting player
        SetStartingPlayer(startingPlayerID);        
    }

    public void SetStartingPlayer(string startingPlayerID)
    {
        LogChallengeEventRequest request = new LogChallengeEventRequest();
        request.SetChallengeInstanceId(ChallengeID);
        request.SetEventKey("SetStartingPlayer");
        request.SetEventAttribute("StartingPlayer", startingPlayerID);
        request.Send(OnSetStartingPlayerSuccess, OnSetStartingPlayerError);
    }

    private void OnSetStartingPlayerSuccess(LogChallengeEventResponse response)
    {
        ChallengeStartingPlayerSet.Invoke();
    }

    private void OnSetStartingPlayerError(LogChallengeEventResponse response)
    {

    }

    public void Move(string action)
    {
        Debug.Log("Sending move to GS");
        LogChallengeEventRequest request = new LogChallengeEventRequest();
        request.SetChallengeInstanceId(ChallengeID);
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


    public int WhichPlayerNumberAmI()
    {
        int playerNumber = 0;
        GameCore.GameBoard.PlayerEnum playerEnum = GameCore.GameBoard.PlayerEnum.ONE;
        if (CurrentPlayerID == FirstPlayerID)
        {
            playerNumber = 1;
            //playerEnum = GameCore.GameBoard.PlayerEnum.ONE;
        }
        else if (CurrentPlayerID == SecondPlayerID)
        {
            playerNumber = 2;
            playerEnum = GameCore.GameBoard.PlayerEnum.TWO;
        }
        CurrentPlayerInfo.PlayerNumber = playerNumber;
        CurrentPlayerNumber = playerNumber;
        CurrentPlayerInfo.PlayerEnum = playerEnum;
        return playerNumber; 
    }

}
