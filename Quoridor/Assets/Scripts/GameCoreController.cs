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
        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
        gameBoard = new GameBoard(GameBoard.PlayerEnum.ONE, "e1", "e9");
        spaceCoordMap = new Dictionary<string, PlayerCoordinate>();
        wallCoordMap = new Dictionary<string, WallCoordinate>();

        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            eventManager.ListenToNewGame(ResetGameBoard);
            eventManager.InvokeGameBoardReady();
        }
        else
        {
            // Randomize player turn 
            //bool opponentTurn = new System.Random().NextDouble() >= .5;
            bool opponentTurn = false;
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

    // Start is called before the first frame update
    void Start()
    {

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
        if (RecordMove(GameBoard.PlayerEnum.TWO, moveString))
        {
            if (moveString.Length == 2)
            {
                interfaceController.MoveOpponentPieceInGUI(moveString);
            }
            else
            {
                interfaceController.MoveOpponentWallInGUI(moveString);
            }
            return true;
        }
        return false;
    }

    private bool RecordMove(GameBoard.PlayerEnum player, string moveString)
    {
        bool returnValue = false;
        if (moveString.Length == 2)
        {
            returnValue = gameBoard.MovePiece(player, new PlayerCoordinate(moveString));
        }
        else if (moveString.Length == 3)
        {
            returnValue = gameBoard.PlaceWall(player, new WallCoordinate(moveString));
        }

        if (returnValue && GameModeStatus.GameMode == GameModeEnum.SINGLE_PLAYER)
        {
            eventManager.InvokeTurnTaken();
        }

        return returnValue;
    }

    public Task<string> GetMoveFromAI()
    {
        MonteCarlo tree = new MonteCarlo(gameBoard);
        return Task.Run(() => tree.MonteCarloTreeSearch());
    }

    public void SetupMultiplayerGame(int playerNumber)
    {
        // Set player's turn in GameBoard
        if (playerNumber == 1)
        {
            gameBoard.SetPlayerTurn(GameBoard.PlayerEnum.ONE);
        }
        else if (playerNumber == 2)
        {
            gameBoard.SetPlayerTurn(GameBoard.PlayerEnum.TWO);
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
