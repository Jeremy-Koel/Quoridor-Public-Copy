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
    private UnityEvent CheckConnectionPeriodic = new UnityEvent();
    private UnityEvent LostConnection = new UnityEvent();
    private UnityEvent ChallengeStarted = new UnityEvent();
    private UnityEvent ChallengeTurnTaken = new UnityEvent();
    private UnityEvent ChallengeStartingPlayerSet = new UnityEvent();
    private UnityEvent ChallengeWon = new UnityEvent();
    private UnityEvent ChallengeLost = new UnityEvent();
    private UnityEvent ChallengeMove = new UnityEvent();

    private UnityEvent GameBoardReady = new UnityEvent();

    private UnityEvent LocalPlayerMoved = new UnityEvent();
    private UnityEvent MoveReceived = new UnityEvent();
    private UnityEvent GameOver = new UnityEvent();
    private UnityEvent NewGame = new UnityEvent();
    private UnityEvent PlayAgain = new UnityEvent();

    private UnityEvent InvalidMove = new UnityEvent();


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
        CheckConnectionPeriodic.RemoveAllListeners();
        ChallengeStarted.RemoveAllListeners();
        ChallengeTurnTaken.RemoveAllListeners();
        ChallengeStartingPlayerSet.RemoveAllListeners();
        ChallengeWon.RemoveAllListeners();
        ChallengeLost.RemoveAllListeners();
        GameBoardReady.RemoveAllListeners();
        LocalPlayerMoved.RemoveAllListeners();
        MoveReceived.RemoveAllListeners();
        GameOver.RemoveAllListeners();
        TurnTaken.RemoveAllListeners();
    }

    public void ResetEventManager()
    {
        LostConnection = new UnityEvent();
        CheckConnectionPeriodic = new UnityEvent();
        ChallengeStarted = new UnityEvent();
        ChallengeTurnTaken = new UnityEvent();
        ChallengeStartingPlayerSet = new UnityEvent();
        ChallengeWon = new UnityEvent();
        ChallengeLost = new UnityEvent();
        ChallengeMove = new UnityEvent();

        GameBoardReady = new UnityEvent();

        LocalPlayerMoved = new UnityEvent();
        MoveReceived = new UnityEvent();
        GameOver = new UnityEvent();
        NewGame = new UnityEvent();
        PlayAgain = new UnityEvent();
        // For AI Game
        TurnTaken = new UnityEvent();
    }

    public void ListenToCheckConnectionPeriodic(UnityAction action)
    {
        Debug.Log("Check Connection Periodic Listener Added");
        CheckConnectionPeriodic.AddListener(action);
    }

    public void InvokeCheckConnectionPeriodic()
    {
        Debug.Log("Check Connection Periodic Invoked");
        CheckConnectionPeriodic.Invoke();
    }

    public void ListenToLostConnection(UnityAction action)
    {
        Debug.Log("LostConnection Listener Added");
        LostConnection.AddListener(action);
    }

    public void InvokeLostConnection()
    {
        Debug.Log("LostConnection Invoked");
        LostConnection.Invoke();
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
    
    public void ListenToChallengeMove(UnityAction action)
    {
        Debug.Log("Challenge Move Listener Added");
        ChallengeMove.AddListener(action);
    }

    public void InvokeChallengeMove()
    {
        Debug.Log("Challenge Move Invoked");
        ChallengeMove.Invoke();
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

    public void ListenToLocalPlayerMoved(UnityAction action)
    {
        Debug.Log("Move Sent Listener Added");
        LocalPlayerMoved.AddListener(action);
    }

    public void InvokeLocalPlayerMoved()
    {
        Debug.Log("Move Sent Invoked");
        LocalPlayerMoved.Invoke();
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

    public void ListenToNewGame(UnityAction action)
    {
        Debug.Log("New Game Listener Added");
        NewGame.AddListener(action);
    }

    public void InvokeNewGame()
    {
        Debug.Log("New Game Invoked");
        NewGame.Invoke();
    }

    public void ListenToPlayAgain(UnityAction action)
    {
        Debug.Log("Play Again Listener Added");
        PlayAgain.AddListener(action);
    }

    public void InvokePlayAgain()
    {
        Debug.Log("Play Again Invoked");
        PlayAgain.Invoke();
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

    public void ListenToInvalidMove(UnityAction action)
    {
        Debug.Log("Invalid Move Listener Added");
        InvalidMove.AddListener(action);
    }

    public void InvokeInvalidMove()
    {
        Debug.Log("Invalid Move Invoked");
        InvalidMove.Invoke();
    }

}
