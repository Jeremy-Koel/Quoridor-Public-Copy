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
    public EventManager eventManager;

    public MessageQueue messageQueue;

    GameSparksUserID gameSparksUserIDScript;

    public bool IsChallengeActive { get; private set; }

    public PlayerInfo FirstPlayerInfo { get; private set; }
    
    public PlayerInfo SecondPlayerInfo { get; private set; }
    
    public PlayerInfo CurrentPlayerInfo { get; private set; }
    
    public string ChallengeID { get; private set; }

    public string LastOpponentMove { get; private set; }

    public string LastMoveUserID { get; private set; }

    public bool GameBoardReady { get; set; }


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameSparksUserIDObject = GameObject.Find("GameSparksUserID");
        gameSparksUserIDScript = gameSparksUserIDObject.GetComponent<GameSparksUserID>();
        if (gameSparksUserIDScript.myUserID != null && gameSparksUserIDScript.myUserID.Length > 0)
        {
            CurrentPlayerInfo.PlayerID = gameSparksUserIDScript.myUserID;
        }
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        messageQueue = GameObject.Find("MessageQueue").GetComponent<MessageQueue>();

        FirstPlayerInfo = new PlayerInfo();
        SecondPlayerInfo = new PlayerInfo();
        CurrentPlayerInfo = new PlayerInfo();
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
        ScriptMessage.Listener += GeneralChallengeMessage;
        eventManager.ListenToGameBoardReady(SetupPlayerInfo);
    }

    void OnChallengeStarted(ChallengeStartedMessage message)
    {
        Debug.Log("ChallengeStarted");
        IsChallengeActive = true;
        ChallengeID = message.Challenge.ChallengeId;

        Debug.Log("ChallengeID: " + ChallengeID);

        eventManager.InvokeChallengeStarted();
    }

    void OnChallengeTurnTaken(ChallengeTurnTakenMessage message)
    {
        Debug.Log("Challenge Turn Taken");
        if (message.Challenge.ScriptData.BaseData.ContainsKey("PlayerIDUsed"))
        {
            string gameSparksUserID = gameSparksUserIDScript.myUserID;
            Debug.Log("My Player ID: " + gameSparksUserID);

            var scriptData = message.Challenge.ScriptData.BaseData;
            Debug.Log("Player ID Used for move: " + scriptData["PlayerIDUsed"].ToString());
            LastMoveUserID = scriptData["PlayerIDUsed"].ToString();
            if (gameSparksUserID != scriptData["PlayerIDUsed"].ToString())
            {
                string scriptDataAction = scriptData["ActionUsed"].ToString();
                Debug.Log("Action Received: " + scriptDataAction);

                // Notify controller that a move was received
                LastOpponentMove = scriptDataAction;
                messageQueue.EnqueueOpponentMoveQueue(scriptDataAction);
                eventManager.InvokeMoveReceived();
            }
            eventManager.InvokeChallengeTurnTaken();
        }       
    }

    public void GeneralChallengeMessage(ScriptMessage message)
    {
        Debug.Log("ScriptMessage recieved: " + message.ExtCode);
        if (message.ExtCode == "ChallengeStartingPlayerMessage")
        {
            messageQueue.EnqueueStartingPlayerSetQueue(message);
        }
    }

    public void SetupPlayerInfo()
    {
        Debug.Log("Challenge Starting Player");
        while (messageQueue.IsQueueEmpty("startingPlayerSetQueue"))
        {

        }
        ScriptMessage message = messageQueue.DequeueStartingPlayerSetQueue();
        IDictionary<string, object> messageData = message.Data.BaseData;
        Debug.Log("Starting Player ID: " + messageData["startingPlayer"].ToString());
        string startingPlayerID = messageData["startingPlayer"].ToString();
        string gameSparksUserID = gameSparksUserIDScript.myUserID;

        
        FirstPlayerInfo.PlayerID = startingPlayerID;
        Debug.Log("Starting Player Name: " + messageData["startingPlayerName"].ToString());
        FirstPlayerInfo.PlayerDisplayName = messageData["startingPlayerName"].ToString();
        FirstPlayerInfo.PlayerNumber = 1;
        FirstPlayerInfo.PlayerEnum = GameCore.GameBoard.PlayerEnum.ONE;

        Debug.Log(FirstPlayerInfo.ToString());
        
        Debug.Log("Second Player ID: " + messageData["secondPlayer"].ToString());
        SecondPlayerInfo.PlayerID = messageData["secondPlayer"].ToString();
        Debug.Log("Second Player Name: " + messageData["secondPlayerName"].ToString());
        SecondPlayerInfo.PlayerDisplayName = messageData["secondPlayerName"].ToString();
        SecondPlayerInfo.PlayerNumber = 2;
        SecondPlayerInfo.PlayerEnum = GameCore.GameBoard.PlayerEnum.TWO;

        Debug.Log(SecondPlayerInfo.ToString());

        CurrentPlayerInfo.PlayerID = gameSparksUserIDScript.myUserID;

        WhichPlayerNumberAmI();

        // Tell the server to set the starting player
        eventManager.InvokeChallengeStartingPlayerSet();
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
        if (CurrentPlayerInfo.PlayerID == FirstPlayerInfo.PlayerID)
        {
            playerNumber = 1;
        }
        else if (CurrentPlayerInfo.PlayerID == SecondPlayerInfo.PlayerID)
        {
            playerNumber = 2;
            playerEnum = GameCore.GameBoard.PlayerEnum.TWO;
        }
        CurrentPlayerInfo.PlayerNumber = playerNumber;
        CurrentPlayerInfo.PlayerEnum = playerEnum;
        Debug.Log(CurrentPlayerInfo.ToString());
        return playerNumber; 
    }

    public PlayerInfo GetPlayerInfo(int playerNumber = 0)
    {
        PlayerInfo playerInfo = CurrentPlayerInfo;
        if (playerNumber == 1)
        {
            playerInfo = FirstPlayerInfo;
        }
        else if (playerNumber == 2)
        {
            playerInfo = SecondPlayerInfo;
        }
        else
        {
            Debug.Log("Invalid player number, use 1 or 2");
        }
        return playerInfo;
    }

    public bool IsItMyTurn()
    {
        bool myTurn = false;
        Debug.Log("Checking if it is my turn. LastMoveID: " + LastMoveUserID + ". CurrentPlayerID: " + CurrentPlayerInfo.PlayerID);
        if (LastMoveUserID != CurrentPlayerInfo.PlayerID)
        {
            myTurn = true;
        }
        return myTurn;
    }

}
