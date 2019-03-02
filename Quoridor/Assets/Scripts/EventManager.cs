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
    
    private UnityEvent ChallengeStarted = new UnityEvent();
    private UnityEvent ChallengeTurnTaken = new UnityEvent();
    private UnityEvent ChallengeStartingPlayerSet = new UnityEvent();
    private UnityEvent ChallengeWon = new UnityEvent();
    private UnityEvent ChallengeLost = new UnityEvent();

    private UnityEvent GameBoardReady = new UnityEvent();

    private UnityEvent MoveSent = new UnityEvent();
    private UnityEvent MoveReceived = new UnityEvent();
    private UnityEvent GameOver = new UnityEvent();
    // For AI Game
    private UnityEvent TurnTaken = new UnityEvent();

    private void Awake()
    {

        int numberOfActiveThises = FindObjectsOfType(this.GetType()).Length;
        if (numberOfActiveThises != 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
        ListenToGameOver(ResetEventManager);
    }

    void Start()
    {
        
    }

    public void RemoveAllListeners()
    {
        Debug.Log("Removing all unityEvent listeners");
        ChallengeStarted.RemoveAllListeners();
        ChallengeTurnTaken.RemoveAllListeners();
        ChallengeStartingPlayerSet.RemoveAllListeners();
        ChallengeWon.RemoveAllListeners();
        ChallengeLost.RemoveAllListeners();
        GameBoardReady.RemoveAllListeners();
        MoveSent.RemoveAllListeners();
        MoveReceived.RemoveAllListeners();
        //GameOver.RemoveAllListeners();
        TurnTaken.RemoveAllListeners();
    }

    public void ResetEventManager()
    {
        ChallengeStarted = new UnityEvent();
        ChallengeTurnTaken = new UnityEvent();
        ChallengeStartingPlayerSet = new UnityEvent();
        ChallengeWon = new UnityEvent();
        ChallengeLost = new UnityEvent();

        GameBoardReady = new UnityEvent();

        MoveSent = new UnityEvent();
        MoveReceived = new UnityEvent();
        GameOver = new UnityEvent();
        // For AI Game
        TurnTaken = new UnityEvent();
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

    public void ListenToMoveReceived(UnityAction action)
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

    public void ListenToTurnTaken(UnityAction action)
    {
        Debug.Log("Turn Taken Listener Added");
        TurnTaken.AddListener(action);
    }

    public void InvokeTurnTaken()
    {
        Debug.Log("Turn Taken Invoked");
        TurnTaken.Invoke();
    }

}
