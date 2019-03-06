using GameCore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameCoreController : MonoBehaviour
{
    private GameBoard gameBoard;
    private Dictionary<string, PlayerCoordinate> spaceCoordMap;
    private Dictionary<string, WallCoordinate> wallCoordMap;
    private EventManager eventManager;
    private InterfaceController interfaceController;

    private void Awake()
    {
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            eventManager.ListenToNewGame(ResetGameBoard);
            eventManager.InvokeGameBoardReady();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
        gameBoard = new GameBoard(GameBoard.PlayerEnum.TWO, "e1", "e9");
        spaceCoordMap = new Dictionary<string, PlayerCoordinate>();
        wallCoordMap = new Dictionary<string, WallCoordinate>();

        if (GameModeStatus.GameMode == GameModeEnum.SINGLE_PLAYER)
        {
            // Randomize player turn 
            bool opponentTurn = new System.Random().NextDouble() >= .5;
            if (opponentTurn)
            {
                interfaceController.SetPlayerOneText("Computer");
                interfaceController.SetPlayerTwoText("Player");
                gameBoard = new GameBoard(GameBoard.PlayerEnum.TWO, "e1", "e9");
                eventManager.InvokeLocalPlayerMoved();
            }
            else
            {
                interfaceController.SetPlayerOneText("Player");
                interfaceController.SetPlayerTwoText("Computer");
            }
        }

    }

    public void AddToSpaceMap(string name)
    {
        spaceCoordMap.Add(name, new PlayerCoordinate(name));
    }

    public void AddToWallMap(string name)
    {
        wallCoordMap.Add(name, new WallCoordinate(name));
    }

    public void ResetGameBoard()
    {
        gameBoard = new GameBoard(GameBoard.PlayerEnum.ONE, "e1", "e9");
        
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            eventManager.InvokeGameBoardReady();
        }
    }

    public bool IsGameOver()
    {
        return gameBoard.IsGameOver();
    }

    public bool DidPlayerOneWin()
    {
        return gameBoard.PlayerOneWin();
    }

    public bool RecordLocalPlayerMove(string moveString)
    {
        return RecordMove(GameBoard.PlayerEnum.ONE, moveString);
    }

    public bool RecordOpponentMove(string moveString)
    {
        return RecordMove(GameBoard.PlayerEnum.TWO, moveString);
    }

    private bool RecordMove(GameBoard.PlayerEnum player, string moveString)
    {
        if (moveString.Length == 2)
        {
            return gameBoard.MovePiece(player, new PlayerCoordinate(moveString));
        }
        else if (moveString.Length == 3)
        {
            return gameBoard.PlaceWall(player, new WallCoordinate(moveString));
        }
        return false;
    }

    public Task<string> GetMoveFromAI()
    {
        MonteCarlo tree = new MonteCarlo(gameBoard);
        return Task.Run(() => tree.MonteCarloTreeSearch());
    }

    public void SetupMultiplayerGame(int playerNumber)
    {
        if (playerNumber == 1)
        {
            // Set player's turn in GameBoard
            gameBoard.SetPlayerTurnRandom();
            while (gameBoard.GetWhoseTurn() != 1)
            {
                gameBoard.SetPlayerTurnRandom();
            }
        }
        else if (playerNumber == 2)
        {
            // Set player's turn in GameBoard
            gameBoard.SetPlayerTurnRandom();
            while (gameBoard.GetWhoseTurn() != 2)
            {
                gameBoard.SetPlayerTurnRandom();
            }
        }
    }

    public int GetPlayerOneWallCount()
    {
        return gameBoard.GetPlayerWallCount(GameBoard.PlayerEnum.ONE);
    }

    public int GetPlayerTwoWallCount()
    {
        return gameBoard.GetPlayerWallCount(GameBoard.PlayerEnum.TWO);
    }

    public GameBoard.PlayerEnum GetWhoseTurn()
    {
        if (gameBoard.GetWhoseTurn() == 1)
        {
            return GameBoard.PlayerEnum.ONE;
        }
        else
        {
            return GameBoard.PlayerEnum.TWO;
        }
    }
}
