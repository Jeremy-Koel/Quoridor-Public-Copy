using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGameController : MonoBehaviour
{
    private ChallengeManager challengeManagerScript;
    private EventManager eventManager;
    private MessageQueue messageQueue;
    private bool opponentTurn;
    private InterfaceController interfaceController;
    private GameCoreController gameCoreController;
    private SoundEffectController soundEffectController;

    private void OnDestroy()
    {
        challengeManagerScript = null;
        eventManager = null;
        messageQueue = null;
    }

    private void Awake()
    {
        Debug.Log("Awake network controller");
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
        soundEffectController = GameObject.Find("GameController").GetComponent<SoundEffectController>();
        gameCoreController = GameObject.Find("GameController").GetComponent<GameCoreController>();
        if (GameSession.GameMode == GameModeEnum.MULTIPLAYER)
        {
            messageQueue = GameObject.Find("MessageQueue").GetComponent<MessageQueue>();
            GameObject challengeManagerObject = GameObject.Find("ChallengeManager");
            challengeManagerScript = challengeManagerObject.GetComponent<ChallengeManager>();

            eventManager.ListenToChallengeStartingPlayerSet(SetupMultiplayerGame);
            eventManager.ListenToMoveReceived(MakeNetworkOpponentMove);
            eventManager.ListenToGameOver(DetermineWinner);
            //eventManager.InvokeGameBoardReady();
        }        
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public string GetPlayerOneDisplayName()
    {
        return challengeManagerScript.FirstPlayerInfo.PlayerDisplayName;
    }

    public string GetPlayerTwoDisplayName()
    {
        return challengeManagerScript.SecondPlayerInfo.PlayerDisplayName;
    }

    public void SetupMultiplayerGame()
    {
        Debug.Log("Setting up multiplayer game in network controller");
        PlayerInfo playerInfo = GetPlayerInfo(challengeManagerScript.CurrentPlayerInfo.PlayerNumber);
        
        // Set appropriate turn in game core 
        gameCoreController.SetupMultiplayerGame(playerInfo.PlayerNumber);

        // Set player names in GUI
        //interfaceController.SetPlayerOneText(challengeManagerScript.FirstPlayerInfo.PlayerDisplayName);
        //interfaceController.SetPlayerTwoText(challengeManagerScript.SecondPlayerInfo.PlayerDisplayName);
    }

    private PlayerInfo GetPlayerInfo(int playerNumber = 0)
    {
        PlayerInfo playerInfo = new PlayerInfo();
        if (challengeManagerScript)
        {
            playerInfo = challengeManagerScript.GetPlayerInfo(playerNumber);
        }
        else
        {
            Debug.Log("challengeManager not active (not a multiplayer game)");
        }
        return playerInfo;
    }

    public void MakeNetworkOpponentMove()
    {
        Debug.Log("Getting move from network");

        // Decide if the opponent placed a wall or piece, and call appropriate method 
        if (GameSession.GameMode == GameModeEnum.MULTIPLAYER)
        {
            string moveString = GetMoveFromNetwork();
            Debug.Log("MakeOpponentMove, moveString: " + moveString);
            if (moveString.Length == 2)
            {
                moveString = GameCore.BoardUtil.MirrorMove(moveString);
                
                //if (gameCoreController.RecordOpponentMove(moveString))
                //{
                //    interfaceController.MoveOpponentPieceInGUI(moveString);
                //}
            }
            else if (moveString.Length == 3)
            {
                moveString = GameCore.BoardUtil.MirrorWall(moveString);

                //if (gameCoreController.RecordOpponentMove(moveString))
                //{
                //    interfaceController.MoveOpponentWallInGUI(moveString);
                //}
            }
            bool recorded = gameCoreController.RecordOpponentMove(moveString);
            // Check if the opponent's move won the game - NK
            if (recorded)
            {
                // Call interface method
                interfaceController.CheckIsGameOver();
            }
        }
    }

    private string GetMoveFromNetwork()
    {
        string mirroredMove = "";
        while (messageQueue.IsQueueEmpty(MessageQueue.QueueNameEnum.OPPONENTMOVE))
        {

        }
        if (!messageQueue.IsQueueEmpty(MessageQueue.QueueNameEnum.OPPONENTMOVE))
        {
            mirroredMove = messageQueue.DequeueOpponentMoveQueue();
        }
        return mirroredMove;
    }

    public void DetermineWinner()
    {
        bool playerOneWin = gameCoreController.DidPlayerOneWin();
        if (playerOneWin)
        {
            // Call GameWon GS event
            eventManager.InvokeChallengeWon();
        }
        else
        {
            // Call GameLost GS event
            eventManager.InvokeChallengeLost();
        }
    }

}
