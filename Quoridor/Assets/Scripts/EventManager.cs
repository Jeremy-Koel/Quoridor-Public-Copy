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
    
    public UnityEvent ChallengeStarted = new UnityEvent();
    public UnityEvent ChallengeTurnTaken = new UnityEvent();
    public UnityEvent ChallengeStartingPlayerSet = new UnityEvent();

    public UnityEvent GameBoardReady = new UnityEvent();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        
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
