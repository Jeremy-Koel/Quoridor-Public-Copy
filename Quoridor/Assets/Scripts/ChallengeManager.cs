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

    public string FirstPlayerName { get; private set; }

    public string FirstPlayerID { get; private set; }

    public string SecondPlayerName { get; private set; }

    public string SecondPlayerID { get; private set; }

    public string CurrentPlayerName { get; private set; }

    public string ChallengeID { get; private set; }

    public string LastOpponentMove { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);        
    }

    // Start is called before the first frame update
    void Start()
    {

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
    }

    void OnChallengeStarted(ChallengeStartedMessage message)
    {
        Debug.Log("ChallengeStarted");
        IsChallengeActive = true;
        ChallengeID = message.Challenge.ChallengeId;
        FirstPlayerName = message.Challenge.Challenger.Name;
        FirstPlayerID = message.Challenge.Challenger.Id;
        SecondPlayerName = message.Challenge.Challenged.First().Name;
        SecondPlayerID = message.Challenge.Challenged.First().Id;
        CurrentPlayerName = message.Challenge.NextPlayer == FirstPlayerID ? FirstPlayerName : SecondPlayerName;

        Debug.Log("ChallengeID: " + ChallengeID);
        Debug.Log("HostsPlayerName: " + FirstPlayerName);
        Debug.Log("ChallengersPlayerName: " + SecondPlayerName);

        ChallengeStarted.Invoke();
    }

    void OnChallengeTurnTaken(ChallengeTurnTakenMessage message)
    {
        Debug.Log("Challenge Turn Taken");
        // Get current player's ID
        GameObject gameSparksUserIDObject = GameObject.Find("GameSparksUserID");
        GameSparksUserID gameSparksUserIDScript = gameSparksUserIDObject.GetComponent<GameSparksUserID>();
        string gameSparksUserID = gameSparksUserIDScript.myUserID;
        Debug.Log("My Player ID: " + gameSparksUserID);

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


}
