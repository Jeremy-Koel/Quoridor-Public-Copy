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

    public string PlayerNameForTurn { get; private set; }

    public bool GameBoardReady { get; set; }


    private void Awake()
    {
        int numberOfActiveThises = FindObjectsOfType<ChallengeManager>().Length;
        if (numberOfActiveThises != 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);

    }

    // Start is called before the first frame update
    void Start()
    {
        FirstPlayerInfo = new PlayerInfo();
        SecondPlayerInfo = new PlayerInfo();
        CurrentPlayerInfo = new PlayerInfo();

        GameObject gameSparksUserIDObject = GameObject.Find("GameSparksUserID");
        gameSparksUserIDScript = gameSparksUserIDObject.GetComponent<GameSparksUserID>();
        if (gameSparksUserIDScript.myUserID != null && gameSparksUserIDScript.myUserID.Length > 0)
        {
            CurrentPlayerInfo.PlayerID = gameSparksUserIDScript.myUserID;
        }
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        messageQueue = GameObject.Find("MessageQueue").GetComponent<MessageQueue>();
        eventManager.ListenToPlayAgain(PlayAgain);
        eventManager.ListenToGameOver(SetupChallengeListeners);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetChallengeManager()
    {
        //IsChallengeActive = false;
        //FirstPlayerInfo = new PlayerInfo();
        //SecondPlayerInfo = new PlayerInfo();
        //CurrentPlayerInfo = new PlayerInfo();
        //ChallengeID = "";
        //LastOpponentMove = "";
        //LastMoveUserID = "";
        //PlayerNameForTurn = "";
        //GameBoardReady = false;
    }

    public void SetupChallengeListeners()
    {
        ChallengeStartedMessage.Listener += OnChallengeStarted;
        Debug.Log("ChallengeStartedMessage Listener set");
        //ChallengeTurnTakenMessage.Listener += OnChallengeTurnTaken;
        //Debug.Log("ChallengeTurnTakenMessage Listener set");
        ScriptMessage.Listener += GeneralChallengeMessageRouter;
        eventManager.ListenToGameBoardReady(SetupPlayerInfo);
        eventManager.ListenToChallengeTurnTaken(SetPlayerNameForTurn);
        eventManager.ListenToChallengeMove(OnMoveReceived);
        eventManager.ListenToChallengeWon(ChallengeGameWon);
        eventManager.ListenToChallengeLost(ChallengeGameLost);
        //eventManager.ListenToGameOver(ResetChallengeManager);        
    }

    public void RemoveAllChallengeListeners()
    {
        Debug.Log("Removing all challenge listeners");
        ChallengeStartedMessage.Listener -= OnChallengeStarted;
        //ChallengeTurnTakenMessage.Listener -= OnChallengeTurnTaken;
        ScriptMessage.Listener -= GeneralChallengeMessageRouter;
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

    void OnMoveReceived()
    {
        Debug.Log("Challenge Move Received");
        messageQueue.WaitForQueueNotEmpty(MessageQueue.QueueNameEnum.CHALLENGEMOVE);
        ScriptMessage message = messageQueue.DequeueChallengeMove();
        IDictionary<string, object> messageData = message.Data.BaseData;

        string gameSparksUserID = gameSparksUserIDScript.myUserID;
        Debug.Log("My Player ID: " + gameSparksUserID);

        //var scriptData = message.ScriptData.BaseData;
        Debug.Log("Player ID Used for move: " + messageData["PlayerIDUsed"].ToString());
        LastMoveUserID = messageData["PlayerIDUsed"].ToString();
        if (gameSparksUserID != messageData["PlayerIDUsed"].ToString())
        {
            string scriptDataAction = messageData["Action"].ToString();
            Debug.Log("Action Received: " + scriptDataAction);

            // Notify controller that a move was received
            LastOpponentMove = scriptDataAction;
            messageQueue.EnqueueOpponentMoveQueue(scriptDataAction);
            eventManager.InvokeMoveReceived();
        }
        eventManager.InvokeChallengeTurnTaken();
    }

    public void GeneralChallengeMessageRouter(ScriptMessage message)
    {
        Debug.Log("ScriptMessage recieved: " + message.ExtCode);
        if (message.ExtCode == "ChallengeStartingPlayerMessage")
        {
            messageQueue.EnqueueStartingPlayerSetQueue(message);
        }
        else if (message.ExtCode == "MatchmakingGroupNumber")
        {
            messageQueue.EnqueueMatchmakingGroupNumber(message);
        }
        else if (message.ExtCode == "ChallengeMove")
        {
            messageQueue.EnqueueChallengeMove(message);
            eventManager.InvokeChallengeMove();
        }
        else if (message.ExtCode == "LostConnection")
        {
            eventManager.InvokeLostConnection();
        }
    }

    public void SetupPlayerInfo()
    {
        StartCoroutine(SetupPlayers());
    }

    public IEnumerator SetupPlayers()
    {
        Debug.Log("Challenge Starting Player");
        while (messageQueue.IsQueueEmpty(MessageQueue.QueueNameEnum.STARTINGPLAYER))
        {
            yield return messageQueue.CheckQueueNotEmpty(MessageQueue.QueueNameEnum.STARTINGPLAYER);
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
        PlayerNameForTurn = FirstPlayerInfo.PlayerDisplayName;

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

    public void ChallengeGameWon()
    {
        Debug.Log("Challenge Game Won");
        LogChallengeEventRequest request = new LogChallengeEventRequest();
        request.SetChallengeInstanceId(ChallengeID);
        request.SetEventKey("GameWon");
        request.SetEventAttribute("playerID", gameSparksUserIDScript.myUserID);
        request.Send(OnChallengeGameWonSuccess, OnChallengeGameWonError);
    }

    private void OnChallengeGameWonSuccess(LogChallengeEventResponse response)
    {
        Debug.Log("Challenge Game Won Success");
    }

    private void OnChallengeGameWonError(LogChallengeEventResponse response)
    {
        Debug.Log("Challenge Game Won Error: " + response.Errors.JSON.ToString());
    }

    public void ChallengeGameLost()
    {
        Debug.Log("Challenge Game Lost");
        LogChallengeEventRequest request = new LogChallengeEventRequest();
        request.SetChallengeInstanceId(ChallengeID);
        request.SetEventKey("GameLost");
        request.SetEventAttribute("playerID", gameSparksUserIDScript.myUserID);
        request.Send(OnChallengeGameLostSuccess, OnChallengeGameLostError);
    }

    private void OnChallengeGameLostSuccess(LogChallengeEventResponse response)
    {
        Debug.Log("Challenge Game Lost Success");
    }

    private void OnChallengeGameLostError(LogChallengeEventResponse response)
    {
        Debug.Log("Challenge Game Lost Error: " + response.Errors.JSON.ToString());
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

    public void PlayAgain()
    {
        if (messageQueue.IsQueueEmpty(MessageQueue.QueueNameEnum.MATCHMAKINGGROUPNUMBER))
        {
            Debug.Log("Sending playAgain event to GS");
            LogChallengeEventRequest request = new LogChallengeEventRequest();
            request.SetChallengeInstanceId(ChallengeID);
            request.SetEventKey("PlayAgain");
            request.Send(OnPlayAgainSuccess, OnPlayAgainError);
        }
    }

    private void OnPlayAgainSuccess(LogChallengeEventResponse response)
    {
        print(response.JSONString);
    }

    private void OnPlayAgainError(LogChallengeEventResponse response)
    {
        print(response.Errors.JSON.ToString());
    }


    public int WhichPlayerNumberAmI()
    {
        int playerNumber = 0;
        string playerDisplayName = "";
        GameCore.GameBoard.PlayerEnum playerEnum = GameCore.GameBoard.PlayerEnum.ONE;
        if (CurrentPlayerInfo.PlayerID == FirstPlayerInfo.PlayerID)
        {
            playerNumber = 1;
            playerDisplayName = FirstPlayerInfo.PlayerDisplayName;
        }
        else if (CurrentPlayerInfo.PlayerID == SecondPlayerInfo.PlayerID)
        {
            playerNumber = 2;
            playerDisplayName = SecondPlayerInfo.PlayerDisplayName;
            playerEnum = GameCore.GameBoard.PlayerEnum.TWO;
        }
        CurrentPlayerInfo.PlayerNumber = playerNumber;
        CurrentPlayerInfo.PlayerEnum = playerEnum;
        CurrentPlayerInfo.PlayerDisplayName = playerDisplayName;
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
            //Debug.Log("It Is My Turn");
            myTurn = true;
        }
        return myTurn;
    }

    public void SetPlayerNameForTurn()
    {
        //if (PlayerNameForTurn == "")
        //{
        //    PlayerNameForTurn = FirstPlayerInfo.PlayerDisplayName;
        //}
        //else
        //{
            PlayerNameForTurn = PlayerNameForTurn == FirstPlayerInfo.PlayerDisplayName ? 
                SecondPlayerInfo.PlayerDisplayName : FirstPlayerInfo.PlayerDisplayName;
        //}
        Debug.Log("Player Name For Turn: " + PlayerNameForTurn);
    }

}
