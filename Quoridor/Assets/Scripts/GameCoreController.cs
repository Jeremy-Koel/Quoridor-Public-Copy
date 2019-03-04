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
    private bool opponentTurn;

    // Start is called before the first frame update
    void Start()
    {
        spaceCoordMap = new Dictionary<string, PlayerCoordinate>();
        wallCoordMap = new Dictionary<string, WallCoordinate>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToSpaceMap(GameObject obj)
    {
        spaceCoordMap.Add(obj.name, new PlayerCoordinate(obj.name));
    }

    public void AddToWallMap(GameObject collider)
    {
        wallCoordMap.Add(collider.name, new WallCoordinate(collider.name));
    }

    public void ResetGameBoard()
    {
        gameBoard = new GameBoard(GameBoard.PlayerEnum.ONE, "e1", "e9");
    }

    public void SetPlayerTurn(int n)
    {
        if (n == 1)
        {
            opponentTurn = false;
            // Set player's turn in GameBoard
            gameBoard.SetPlayerTurnRandom();
            while (gameBoard.GetWhoseTurn() != 1)
            {
                gameBoard.SetPlayerTurnRandom();
            }
        }
        else if (n == 2)
        {
            opponentTurn = true;
            // Set player's turn in GameBoard
            gameBoard.SetPlayerTurnRandom();
            while (gameBoard.GetWhoseTurn() != 2)
            {
                gameBoard.SetPlayerTurnRandom();
            }
        }
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
}
