﻿using System;
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
    public UnityEvent ChallengeWon = new UnityEvent();
    public UnityEvent ChallengeLost = new UnityEvent();

    public UnityEvent GameBoardReady = new UnityEvent();

    public UnityEvent MoveSent = new UnityEvent();
    public UnityEvent MoveReceived = new UnityEvent();
    public UnityEvent GameOver = new UnityEvent();

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

    public void ListenToChallengeWon(UnityAction action)
    {
        Debug.Log("Challenge Won Listener Added");
        ChallengeWon.AddListener(action);
    }

    public void InvokeChallengeWon()
    {
        Debug.Log("Challenge Won Invoked");
        ChallengeWon.Invoke();
    }

    public void ListenToChallengeLost(UnityAction action)
    {
        Debug.Log("Challenge Lost Listener Added");
        ChallengeLost.AddListener(action);
    }

    public void InvokeChallengeLost()
    {
        Debug.Log("Challenge Lost Invoked");
        ChallengeLost.Invoke();
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

    public void ListenToMoveSent(UnityAction action)
    {
        Debug.Log("Move Sent Listener Added");
        MoveSent.AddListener(action);
    }

    public void InvokeMoveSent()
    {
        Debug.Log("Move Sent Invoked");
        MoveSent.Invoke();
    }

    public void ListToMoveReceived(UnityAction action)
    {
        Debug.Log("Move Received Listener Added");
        MoveReceived.AddListener(action);
    }

    public void InvokeMoveReceived()
    {
        Debug.Log("Move Received Invoked");
        MoveReceived.Invoke();
    }

    public void ListenToGameOver(UnityAction action)
    {
        Debug.Log("Game Over Listener Added");
        GameOver.AddListener(action);
    }

    public void InvokeGameOver()
    {
        Debug.Log("Game Over Invoked");
        GameOver.Invoke();
    }

}
