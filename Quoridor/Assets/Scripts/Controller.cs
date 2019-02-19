using UnityEngine;
using GameCore;
using System.Collections.Generic;

public class Controller : MonoBehaviour
{
    private GameBoard gameBoard;
    private Dictionary<string, PlayerCoordinate> spaceCoordMap;
    private Dictionary<string, WallCoordinate> wallCoordMap;
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
        spaceCoordMap = new Dictionary<string, PlayerCoordinate>();
        wallCoordMap = new Dictionary<string, WallCoordinate>();
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

            // Decide if the opponent placed a wall or piece, and call appropriate method 
            if (moveString.Length == 2 && IsValidMove(GameBoard.PlayerEnum.TWO, moveString))
            {
                MoveOpponentPieceInGUI(moveString);
            }
            else if (moveString.Length == 3 && IsValidWallPlacement(GameBoard.PlayerEnum.TWO, moveString))
            {
                MoveOpponentWallInGUI(moveString);
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
        string mirroredMove = "";
        // Temporary solution if move is not there
        if (challengeManagerScript.LastOpponentMove != null)
        {
            mirroredMove = BoardUtil.MirrorMove(challengeManagerScript.LastOpponentMove);
        }
        return mirroredMove;
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

    public void AddToSpaceMap(GameObject obj)
    {
        spaceCoordMap.Add(obj.name, new PlayerCoordinate(obj.name));
    }

    public void AddToWallMap(GameObject collider)
    {
        wallCoordMap.Add(collider.name, new WallCoordinate(collider.name));   
    }

    // TODO - represent whose turn it is in the GUI, so it can be used here 
    public bool IsValidMove(GameBoard.PlayerEnum player, string spaceName)
    {
        PlayerCoordinate pc = spaceCoordMap[spaceName];
        bool validMove = gameBoard.MovePiece(player, pc);

        // If validMove send move across network
        if (validMove && isMultiplayerGame)
        {
            // Send move via ChallengeManager
            challengeManagerScript.Move(spaceName);
        }
        return validMove;
    }


    public bool IsValidWallPlacement(GameBoard.PlayerEnum player, string spaceName)
    {
        return gameBoard.PlaceWall(player, new WallCoordinate(spaceName));
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
    
    private void MoveOpponentPieceInGUI(string guiSpaceName)
    {
        GameObject opponentMouse = GameObject.Find("opponentMouse");
        GameObject targetSquare = GameObject.Find(guiSpaceName);
        ClickSquare clickSquare = targetSquare.GetComponent<ClickSquare>();
        opponentMouse.transform.position = new Vector3(clickSquare.transform.position.x, clickSquare.transform.position.y, -0.5f);

    }

    private void MoveOpponentWallInGUI(string colliderName)
    {
        GameObject wall = GetUnusedOpponentWall();
        Collider collider = GetCollider(colliderName);
        if (wall != null && collider != null)
        {
            wall.transform.localScale = collider.transform.localScale;
            wall.transform.position = new Vector3(collider.transform.position.x, collider.transform.position.y, wall.transform.position.z);
            wall.GetComponent<MoveWalls>().SetLockPlace(true);
            //Debug.Log("Opponent tried to place a wall"); // TODO - move wall 
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

    private Collider GetCollider(string colliderName)
    {
        Collider collider = GameObject.Find(colliderName).GetComponent<Collider>();

        return collider;
    }
}
