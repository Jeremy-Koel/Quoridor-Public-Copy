using UnityEngine;
using GameCore;
using System.Collections.Generic;

public class Controller : MonoBehaviour
{
    private GameBoard gameBoard;
    private Dictionary<string, PlayerCoordinate> coordMap;
    private ChallengeManager challengeManagerScript;
    private bool localPlayerTurn;
    private bool isMultiplayerGame;
    private State state;

    private enum State
    {
        PlayerMoving,
        PlayerMoved,
        OpponentMoving,
        OpponentMoved
    }

    // Start is called before the first frame update
    void Start()
    {
        gameBoard = new GameBoard(GameBoard.PlayerEnum.ONE, "e1", "e9");
        coordMap = new Dictionary<string, PlayerCoordinate>();
        localPlayerTurn = true;

        GameObject challengeManagerObject = GameObject.Find("ChallengeManager");
        if (challengeManagerObject != null)
        {
            challengeManagerScript = challengeManagerObject.GetComponent<ChallengeManager>();
            isMultiplayerGame = challengeManagerScript.IsChallengeActive;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.PlayerMoving:
                break;
            case State.PlayerMoved:
                state = State.OpponentMoving;
                break;
            case State.OpponentMoving:
                OpponentMove();
                state = State.OpponentMoved;
                break;
            case State.OpponentMoved:
                state = State.PlayerMoving;
                break;
        }
    }

    public void OpponentMove()
    {
        if (!localPlayerTurn)
        {
            // Get move from AI or network 
            string moveString = GetOpponentMoveString();
            
            int col = BoardUtil.GetInternalPlayerCol(moveString[0]) / 2;
            int row = BoardUtil.GetInteralPlayerRow(moveString[1]) / 2;
            
            // Decide if the opponent placed a wall or piece, and call appropriate method 
            if (moveString.Length == 2)
            {
                string spaceName = row + "," + col;
                if (IsValidMove(GameBoard.PlayerEnum.TWO, spaceName))
                {
                    MoveOpponentPiece(row, col);
                }
            }
            else if (moveString.Length == 3)
            {
                WallCoordinate wc = new WallCoordinate(moveString);
                MoveOpponentWall();
            }

            FlipTurn();
        }
    }

    private string GetOpponentMoveString()
    {
        if (isMultiplayerGame)
        {
            return GetMoveFromNetwork();
        }
        else
        {
            return GetMoveFromAI();
        }
    }

    private string GetMoveFromNetwork()
    {
        return "";
    }

    private string GetMoveFromAI()
    {
        MonteCarlo tree = new MonteCarlo(gameBoard);
        return tree.MonteCarloTreeSearch();
    }

    public void MarkPlayerMoved()
    {
        state = State.PlayerMoved;
        FlipTurn();
    }

    private void FlipTurn()
    {
        localPlayerTurn = !localPlayerTurn;
    }

    public void AddSpace(GameObject obj)
    {
        string[] strs = obj.name.Split(',');
        int x = int.Parse(strs[0]) * 2;
        int y = int.Parse(strs[1]) * 2;
        coordMap.Add(obj.name, new PlayerCoordinate(x,y));
    }

    // TODO - represent whose turn it is in the GUI, so it can be used here 
    public bool IsValidMove(GameBoard.PlayerEnum player, string spaceName)
    {
        PlayerCoordinate pc = coordMap[spaceName];
        bool validMove = gameBoard.MovePiece(player, pc);

        // If validMove send move across network
        if (validMove)
        {
            // Send move via ChallengeManager
            if (isMultiplayerGame)
            {
                challengeManagerScript.Move(spaceName);
            }
        }
        return validMove;
    }


    public bool IsValidWallPlacement(GameBoard.PlayerEnum player, string spaceName)
    {
        string[] strs = spaceName.Split(',');
        int row = int.Parse(strs[0]) * 2;
        int col = int.Parse(strs[1][0].ToString()) * 2;
        char c = strs[1][1];
        return gameBoard.PlaceWall(player, new WallCoordinate(row, col, c));
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
    
    private void MoveOpponentPiece(int guiRow, int guiCol)
    {
        GameObject opponentMouse = GameObject.Find("opponentMouse");
        GameObject targetSquare = GameObject.Find(guiRow + "," + guiCol);
        ClickSquare clickSquare = targetSquare.GetComponent<ClickSquare>();
        opponentMouse.transform.position = new Vector3(clickSquare.transform.position.x, clickSquare.transform.position.y, -0.5f);

    }

    private void MoveOpponentWall()
    {
        GameObject wall = GetUnusedOpponentWall();
        if (wall != null)
        {
            Debug.Log("Opponent tried to place a wall"); // TODO - move wall 
        }
    }

    private GameObject GetUnusedOpponentWall()
    {
        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("PlayerTwoWall"))
        {
            if (!wall.GetComponent<MoveWalls>().IsOnBoard())
            {
                return wall;
            }
        }
        return null;
    }
}
