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
    public UnityEvent ChallengeStarted;
    public UnityEvent ChallengeTurnTaken;
    public UnityEvent ChallengeStartingPlayerSet;
    //public UnityEvent ChallengeWon;
    //public UnityEvent ChallengeLost;

    public Queue<ScriptMessage> scriptMessageQueue;

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
            CurrentPlayerID = gameSparksUserIDScript.myUserID;
        }
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        scriptMessageQueue = new Queue<ScriptMessage>();

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
        //FirstPlayerName = message.Challenge.Challenger.Name;
        //FirstPlayerID = message.Challenge.Challenger.Id;
        //SecondPlayerName = message.Challenge.Challenged.First().Name;
        //SecondPlayerID = message.Challenge.Challenged.First().Id;
        //CurrentPlayerName = message.Challenge.NextPlayer == FirstPlayerID ? FirstPlayerName : SecondPlayerName;

        Debug.Log("ChallengeID: " + ChallengeID);
        //Debug.Log("HostsPlayerName: " + FirstPlayerName);
        //Debug.Log("ChallengersPlayerName: " + SecondPlayerName);

        eventManager.InvokeChallengeStarted();
        //ChallengeStarted.Invoke();
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

    public void GeneralChallengeMessage(ScriptMessage message)
    {
        Debug.Log("ScriptMessage recieved: " + message.ExtCode);
        if (message.ExtCode == "ChallengeStartingPlayerMessage")
        {
            scriptMessageQueue.Enqueue(message);
            //InvokeRepeating("SetupPlayerInfoCoroutine", 0.0f, 0.5f);
            //Debug.Log("SetupPlayerInfoCoroutine Done");
            //SetupPlayerInfo(message);
        }
    }

    //IEnumerator GeneralChallengeMessage(ScriptMessage message)
    //{
    //    Debug.Log("ScriptMessage recieved: " + message.ExtCode);
    //    if (message.ExtCode == "ChallengeStartingPlayerMessage")
    //    {
    //        yield return StartCoroutine(SetupPlayerInfoCoroutine());
    //        Debug.Log("SetupPlayerInfoCoroutine Done");
    //        SetupPlayerInfo(message);
    //    }
    //}

    //public void SetupPlayerInfoCoroutine(ScriptMessage message)
    //{
    //    Debug.Log("Checking if GameBoard is ready");
        
    //    if (GameBoardReady)
    //    {
    //        CancelInvoke("SetupPlayerInfoCoroutine");
    //        Debug.Log("GameBoard ready");
    //        SetupPlayerInfo(message);            
    //    }
    //    else
    //    {
    //        Debug.Log("GameBoard not ready, waiting one half second...");
    //        //yield return new WaitForSeconds(5);
    //        //SetupPlayerInfoCoroutine();
    //    }        
    //}

    public void SetupPlayerInfo()
    {
        Debug.Log("Challenge Starting Player");
        ScriptMessage message = scriptMessageQueue.Dequeue();
        IDictionary<string, object> messageData = message.Data.BaseData;
        Debug.Log("Starting Player ID: " + messageData["startingPlayer"].ToString());
        string startingPlayerID = messageData["startingPlayer"].ToString();
        string gameSparksUserID = gameSparksUserIDScript.myUserID;

        
        FirstPlayerInfo.PlayerID = startingPlayerID;
        Debug.Log("Starting Player Name: " + messageData["startingPlayerName"].ToString());
        FirstPlayerInfo.PlayerDisplayName = messageData["startingPlayerName"].ToString();
        FirstPlayerInfo.PlayerNumber = 1;
        FirstPlayerInfo.PlayerEnum = GameCore.GameBoard.PlayerEnum.ONE;
        FirstPlayerID = startingPlayerID;
        FirstPlayerName = messageData["startingPlayerName"].ToString();

        Debug.Log(FirstPlayerInfo.ToString());
        
        Debug.Log("Second Player ID: " + messageData["secondPlayer"].ToString());
        SecondPlayerInfo.PlayerID = messageData["secondPlayer"].ToString();
        Debug.Log("Second Player Name: " + messageData["secondPlayerName"].ToString());
        SecondPlayerInfo.PlayerDisplayName = messageData["secondPlayerName"].ToString();
        SecondPlayerInfo.PlayerNumber = 2;
        SecondPlayerInfo.PlayerEnum = GameCore.GameBoard.PlayerEnum.TWO;
        SecondPlayerID = messageData["secondPlayer"].ToString();
        SecondPlayerName = messageData["secondPlayerName"].ToString();

        Debug.Log(SecondPlayerInfo.ToString());

        CurrentPlayerInfo.PlayerID = gameSparksUserIDScript.myUserID;
        CurrentPlayerID = gameSparksUserIDScript.myUserID;

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
        //ChallengeStartingPlayerSet.Invoke();
        //eventManager.InvokeChallengeStartingPlayerSet();
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

}
