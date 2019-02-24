using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;
using GameSparks.Api.Messages;

[System.Serializable]
public class EventManager : MonoBehaviour
{
    
    public UnityEvent ChallengeStarted;
    public UnityEvent ChallengeTurnTaken;
    public UnityEvent ChallengeStartingPlayerSet;

    public UnityEvent GameBoardReady;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        // Refactor into a list of events
        if (ChallengeStarted == null)
        {
            ChallengeStarted = new UnityEvent();
        }
        if (ChallengeTurnTaken == null)
        {
            ChallengeTurnTaken = new UnityEvent();
        }
        if (ChallengeStartingPlayerSet == null)
        {
            ChallengeStartingPlayerSet = new UnityEvent();
            DebugListenerSet("ChallengeStartingPlayerSet");
        }
        if (GameBoardReady == null)
        {
            GameBoardReady = new UnityEvent();
        }
        
    }

    public void DebugListenerSet(string eventType)
    {
        Debug.Log("Logging Listener activated for " + eventType);
    }

    public void ListenToChallengeStarted(UnityAction action)
    {
        Debug.Log("Challenge Started Listener Added");
        ChallengeStarted.AddListener(action);
    }

    public void InvokeChallengeStarted()
    {
        Debug.Log("Challenge Started Invoked");
        ChallengeStarted.Invoke();
    }

    public void ListenToChallengeTurnTaken(UnityAction action)
    {
        Debug.Log("Challenge Turn Taken Listener Added");
        ChallengeTurnTaken.AddListener(action);
    }
    public void InvokeChallengeTurnTaken()
    {
        Debug.Log("ChallengeTurnTaken Invoked");
        ChallengeTurnTaken.Invoke();
    }

    public void ListenToChallengeStartingPlayerSet(UnityAction action)
    {
        Debug.Log("Challenge Starting Player Set Listener Added");
        ChallengeStartingPlayerSet.AddListener(action);
    }

    public void InvokeChallengeStartingPlayerSet()
    {
        Debug.Log("Challenge Starting Player Set Invoked");
        ChallengeStartingPlayerSet.Invoke();
    }

    public void ListenToGameBoardReady(UnityAction action)
    {
        Debug.Log("GameBoard Ready Listener Added");
        GameBoardReady.AddListener(action);
    }

    public void InvokeGameBoardReady()
    {
        Debug.Log("GameBoard Ready Invoked");
        GameBoardReady.Invoke();
    }
}
