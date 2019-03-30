﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour
{
    private ChallengeManager challengeManagerScript;
    private EventManager eventManager;
    private MessageQueue messageQueue;
    private GameObject winPanel;
    private GameObject menuPanel;
    private GameObject helpScreen;
    private Text playerWallLabel;
    private Text opponentWallLabel;
    private SoundEffectController soundEffectController;
    private NetworkGameController networkGameController;
    private GameCoreController gameCoreController;
    private Queue<Animator> playerWallIndicators;
    private Queue<Animator> opponentWallIndicators;
    private Animator playerTurnBoxAnimator;

    private void Awake()
    {
        challengeManagerScript = GameObject.Find("ChallengeManager").GetComponent<ChallengeManager>();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        playerWallLabel = GameObject.Find("PlayerWallLabel").GetComponent<Text>();
        opponentWallLabel = GameObject.Find("OpponentWallLabel").GetComponent<Text>();
        playerTurnBoxAnimator = GameObject.Find("PlayerTurnBox").GetComponent<Animator>();
        initAnimatorQueues();

        eventManager.ListenToInvalidMove(SwitchTurnIndicatorToInvalidMove);

        if (SessionStates.GameMode == GameModeEnum.SINGLE_PLAYER)
        {
            eventManager.ListenToLocalPlayerMoved(GenerateMoveForAI);
        }
        else if (SessionStates.GameMode == GameModeEnum.MULTIPLAYER)
        {
            //eventManager.ListenToLocalPlayerMoved();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        winPanel = GameObject.Find("WinScreen");
        winPanel.SetActive(false);
        menuPanel = GameObject.Find("MenuOptions");
        helpScreen = GameObject.Find("HelpMenu");

        // Grab other controllers 
        soundEffectController = GameObject.Find("GameController").GetComponent<SoundEffectController>();
        gameCoreController = GameObject.Find("GameController").GetComponent<GameCoreController>();
        networkGameController = GameObject.Find("GameController").GetComponent<NetworkGameController>();
    }

    private void Update()
    {
        //if (IsGameOver() && !menuPanel.activeSelf)
        //{
        //    winPanel.SetActive(true);
        //}
    }

    private void initAnimatorQueues()
    {
        playerWallIndicators = new Queue<Animator>();
        playerWallIndicators.Enqueue(GameObject.Find("PlayerWallIndicator0").GetComponent<Animator>());
        playerWallIndicators.Enqueue(GameObject.Find("PlayerWallIndicator1").GetComponent<Animator>());
        playerWallIndicators.Enqueue(GameObject.Find("PlayerWallIndicator2").GetComponent<Animator>());
        playerWallIndicators.Enqueue(GameObject.Find("PlayerWallIndicator3").GetComponent<Animator>());
        playerWallIndicators.Enqueue(GameObject.Find("PlayerWallIndicator4").GetComponent<Animator>());
        playerWallIndicators.Enqueue(GameObject.Find("PlayerWallIndicator5").GetComponent<Animator>());
        playerWallIndicators.Enqueue(GameObject.Find("PlayerWallIndicator6").GetComponent<Animator>());
        playerWallIndicators.Enqueue(GameObject.Find("PlayerWallIndicator7").GetComponent<Animator>());
        playerWallIndicators.Enqueue(GameObject.Find("PlayerWallIndicator8").GetComponent<Animator>());
        playerWallIndicators.Enqueue(GameObject.Find("PlayerWallIndicator9").GetComponent<Animator>());

        opponentWallIndicators = new Queue<Animator>();
        opponentWallIndicators.Enqueue(GameObject.Find("OpponentWallIndicator0").GetComponent<Animator>());
        opponentWallIndicators.Enqueue(GameObject.Find("OpponentWallIndicator1").GetComponent<Animator>());
        opponentWallIndicators.Enqueue(GameObject.Find("OpponentWallIndicator2").GetComponent<Animator>());
        opponentWallIndicators.Enqueue(GameObject.Find("OpponentWallIndicator3").GetComponent<Animator>());
        opponentWallIndicators.Enqueue(GameObject.Find("OpponentWallIndicator4").GetComponent<Animator>());
        opponentWallIndicators.Enqueue(GameObject.Find("OpponentWallIndicator5").GetComponent<Animator>());
        opponentWallIndicators.Enqueue(GameObject.Find("OpponentWallIndicator6").GetComponent<Animator>());
        opponentWallIndicators.Enqueue(GameObject.Find("OpponentWallIndicator7").GetComponent<Animator>());
        opponentWallIndicators.Enqueue(GameObject.Find("OpponentWallIndicator8").GetComponent<Animator>());
        opponentWallIndicators.Enqueue(GameObject.Find("OpponentWallIndicator9").GetComponent<Animator>());
    }

    public void MoveOpponentPieceInGUI(string guiSpaceName)
    {
        GameObject opponentMouse = GameObject.Find("opponentMouse");
        GameObject targetSquare = GameObject.Find(guiSpaceName);
        ClickSquare clickSquare = targetSquare.GetComponent<ClickSquare>();
        MoveMouse moveMouseScript = opponentMouse.GetComponent<MoveMouse>();
        moveMouseScript.target = new Vector3(clickSquare.transform.position.x, clickSquare.transform.position.y, -1f);
        opponentMouse.transform.localScale = new Vector3(opponentMouse.transform.localScale.x + .001f, opponentMouse.transform.localScale.y + .001f, opponentMouse.transform.localScale.z);
        moveMouseScript.moveMouse = true;
        soundEffectController.PlaySqueakSound();
        SwitchTurnIndicatorToLocal();
    }

    public void MoveOpponentWallInGUI(string colliderName)
    {
        GameObject wall = GetUnusedOpponentWall();
        Collider collider = GetCollider(colliderName);
        if (wall != null && collider != null)
        {
            TriggerOpponentWallIndicatorAnimation();

            //wall.transform.localScale = collider.transform.localScale;
            MoveWallsProgramatically moveWallsProgramatically = wall.GetComponent<MoveWallsProgramatically>();
            wall.transform.localScale = moveWallsProgramatically.GetWallSize(collider.transform.localScale);
            moveWallsProgramatically.SetTarget(collider.transform.position, collider.transform.localScale);
            moveWallsProgramatically.moveWall = true;
            moveWallsProgramatically.SetIsOnBoard(true);
            collider.GetComponent<WallPlacement>().SetWallPlacedHere(true);
            
            SwitchTurnIndicatorToLocal();
        }
    }

    private GameObject GetUnusedOpponentWall()
    {
        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("PlayerTwoWall"))
        {
            if (!wall.GetComponent<MoveWallsProgramatically>().IsOnBoard())
            {
                return wall;
            }
        }
        return null;
    }

    private Collider GetCollider(string colliderName)
    {
        Collider collider = GameObject.Find(colliderName).GetComponent<Collider>();

        return collider;
    }

    public void SetPlayerWallLabelText(string str)
    {
        if (playerWallLabel != null)
        {
            playerWallLabel.text = str;
        }
    }

    public void SetOpponentWallLabelText(string str)
    {
        if (opponentWallLabel != null)
        {
            opponentWallLabel.text = str;
        }
    }

    private async void GenerateMoveForAI()
    {
        string str = await gameCoreController.GetMoveFromAI();
        bool recorded = gameCoreController.RecordOpponentMove(str);

        // Check if the AI's move won the game - NK
        if (recorded)
        {
            CheckIsGameOver();
        }
    }

    public void PlayMouseMoveSound()
    {
        soundEffectController.PlaySqueakSound();
    }

    public void PlayErrorSound()
    {
        soundEffectController.PlayErrorSound();
    }

    public bool RecordLocalPlayerMove(string move)
    {
        bool movedSuccessfully = gameCoreController.RecordLocalPlayerMove(move);
        if (movedSuccessfully)
        {
            if (move.Length == 3)
            {
                TriggerPlayerWallIndicatorAnimation();
            }

            if (SessionStates.GameMode == GameModeEnum.SINGLE_PLAYER)
            {
                eventManager.InvokeLocalPlayerMoved();
            }
            else
            {
                challengeManagerScript.Move(move);
            }

            // Switch turn indicator 
            SwitchTurnIndicatorToOpponent();

            // Check if the local player won - NK
            CheckIsGameOver();
        }
        else
        {
            eventManager.InvokeInvalidMove();
        }
        return movedSuccessfully;
    }

    private void TriggerPlayerWallIndicatorAnimation()
    {
        Debug.Log("trigger wall tick animation");
        Animator anim = playerWallIndicators.Dequeue();
        anim.SetTrigger("PlaceWall");
    }

    private void TriggerOpponentWallIndicatorAnimation()
    {
        Animator anim = opponentWallIndicators.Dequeue();
        anim.SetTrigger("PlaceWall");
    }

    private void SwitchTurnIndicatorToInvalidMove()
    {
        playerTurnBoxAnimator.SetTrigger("InvalidMove");
        soundEffectController.PlayErrorSound();
    }

    private void SwitchTurnIndicatorToOpponent()
    {
        playerTurnBoxAnimator.SetTrigger("LocalTurnTaken");
    }

    private void SwitchTurnIndicatorToLocal()
    {
        playerTurnBoxAnimator.SetTrigger("OpponentTurnTaken");
    }
    
    public string GetPlayerNameForTurn()
    {
        string playerDisplayName = "";
        playerDisplayName = challengeManagerScript.PlayerNameForTurn;
        return playerDisplayName;
    }

    public string GetLocalPlayerName()
    {
        if (SessionStates.GameMode == GameModeEnum.MULTIPLAYER)
        {
            return challengeManagerScript.CurrentPlayerInfo.PlayerDisplayName;
        }
        else
        {
            return "";
        }
    }

    public int GetPlayerOneWallCount()
    {
        return gameCoreController.GetPlayerOneWallCount();
    }

    public int GetPlayerTwoWallCount()
    {
        return gameCoreController.GetPlayerTwoWallCount();
    }

    public void AddToSpaceMap(GameObject obj)
    {
        gameCoreController.AddToSpaceMap(obj.name);
    }

    public void AddToWallMap(GameObject collider)
    {
        gameCoreController.AddToWallMap(collider.name);
    }

    public GameCore.GameBoard.PlayerEnum GetWhoseTurn()
    {
        return gameCoreController.GetWhoseTurn();
    }

    public string WhoWon()
    {
        if (SessionStates.GameMode == GameModeEnum.MULTIPLAYER)
        {
            if (!gameCoreController.DidPlayerOneWin()) 
            {
                if (challengeManagerScript.CurrentPlayerInfo.PlayerID == challengeManagerScript.SecondPlayerInfo.PlayerID)
                {
                    return challengeManagerScript.FirstPlayerInfo.PlayerDisplayName + " Wins!";
                }
                else
                {
                    return challengeManagerScript.SecondPlayerInfo.PlayerDisplayName + " Wins!";
                }
            }
            else
            {
                return challengeManagerScript.CurrentPlayerInfo.PlayerDisplayName + " Wins!";
            }
        }
        else
        {
            if (gameCoreController.DidPlayerOneWin())
            {
                return "You Win!";
            }
            else
            {
                return "AI Wins!";
            }
        }
    }

    public bool IsGameOver()
    {
        return gameCoreController.IsGameOver();
    }

    public void CheckIsGameOver()
    {
        if (IsGameOver() && !menuPanel.activeSelf)
        {
            winPanel.SetActive(true);
        }
    }

    public List<string> GetPossibleMoves()
    {
        return gameCoreController.GetPossibleMoves();
    }
}
