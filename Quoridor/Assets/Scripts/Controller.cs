using UnityEngine;
using GameCore;
using UnityEngine.UI;
using System.Collections.Generic;

public class Controller : MonoBehaviour
{
    private GameBoard gameBoard;
    private Dictionary<string, PlayerCoordinate> spaceCoordMap;
    private Dictionary<string, WallCoordinate> wallCoordMap;
    private ChallengeManager challengeManagerScript;
    private GameObject winPanel;
    private GameObject menuPanel;
    private GameObject helpScreen;
    private Text playerOneText;
    private Text playerTwoText;
    private bool opponentTurn;

    // Start is called before the first frame update
    void Start()
    {
        opponentTurn = false;
        gameBoard = new GameBoard(GameBoard.PlayerEnum.ONE, "e1", "e9");
        spaceCoordMap = new Dictionary<string, PlayerCoordinate>();
        wallCoordMap = new Dictionary<string, WallCoordinate>();
        winPanel = GameObject.Find("WinScreen");
        winPanel.SetActive(false);
        menuPanel = GameObject.Find("MenuOptions");
        helpScreen = GameObject.Find("HelpMenu");
        playerOneText = GameObject.Find("PlayerOneText").GetComponent<Text>();
        playerTwoText = GameObject.Find("PlayerTwoText").GetComponent<Text>();
        
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            // clean up this mess later
            GameObject challengeManagerObject = GameObject.Find("ChallengeManager");
            challengeManagerScript = challengeManagerObject.GetComponent<ChallengeManager>();
            PlayerInfo playerInfo = GetPlayerInfo(challengeManagerScript.CurrentPlayerNumber);
            string startingPos = "";
            string opposingPos = "";
            if (playerInfo.PlayerNumber == 1)
            {
                startingPos = "e1";
                opposingPos = "e9";
            }
            else if (playerInfo.PlayerNumber == 2)
            {
                startingPos = "e9";
                opposingPos = "e1";
            }
            gameBoard = new GameBoard(playerInfo.PlayerEnum, startingPos, opposingPos);
            playerOneText.text = challengeManagerScript.FirstPlayerInfo.PlayerDisplayName;
            playerTwoText.text = challengeManagerScript.SecondPlayerInfo.PlayerDisplayName;
        }
        else
        {
            opponentTurn = false;
        }

        // start watching for moves 
        InvokeRepeating("WatchForMoves", 1.0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(IsGameOver() && !menuPanel.activeSelf)
        {
            winPanel.SetActive(true);
        }
    }

    private void WatchForMoves()
    {
        if (opponentTurn)
        {
            MakeOpponentMove();
        }
    }

    public void MarkLocalPlayerMove()
    {
        opponentTurn = true;
    }

    public void MakeOpponentMove()
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

        opponentTurn = false;
    }

    private string GetOpponentMoveString()
    {
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
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

    public void AddToSpaceMap(GameObject obj)
    {
        spaceCoordMap.Add(obj.name, new PlayerCoordinate(obj.name));
    }

    public void AddToWallMap(GameObject collider)
    {
        wallCoordMap.Add(collider.name, new WallCoordinate(collider.name));   
    }
    
    public bool IsValidMove(GameBoard.PlayerEnum player, string spaceName)
    {
        PlayerCoordinate pc = spaceCoordMap[spaceName];
        bool validMove = gameBoard.MovePiece(player, pc);

        // If validMove send move across network
        if (validMove && GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
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
    
    public string WhoWon()
    {
        if(gameBoard.PlayerOneWin())
        {
            return "Player One Wins!";
        }
        else
        {
            return "Player Two Wins!";
        }
    }

    public int GetPlayerWallCount(GameBoard.PlayerEnum player)
    {
        return gameBoard.GetPlayerWallCount(player);
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
            MoveWallsProgramatically moveWallScript = wall.GetComponent<MoveWallsProgramatically>();
            moveWallScript.target = new Vector3(collider.transform.position.x, collider.transform.position.y, wall.transform.position.z);
            moveWallScript.moveWall = true;

            wall.GetComponent<MoveWalls>().SetLockPlace(true);
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

    private PlayerInfo GetPlayerInfo(int playerNumber = 0)
    {
        PlayerInfo playerInfo = new PlayerInfo();
        if (challengeManagerScript)
        {
            string playerDisplayName = "";
            string playerID = "";
            
            playerDisplayName = GetPlayerName(playerNumber);
            playerID = GetPlayerID(playerNumber);

            playerInfo.PlayerDisplayName = playerDisplayName;
            playerInfo.PlayerID = playerID;
            
        }
        else
        {
            Debug.Log("challengeManager not active (not a multiplayer game)");
        }
        return playerInfo;
    }

    private string GetPlayerName(int playerNumber = 0)
    {
        string playerDisplayName = "";
        
        if (playerNumber == 1)
        {
            playerDisplayName = challengeManagerScript.FirstPlayerName;
        }
        else if (playerNumber == 2)
        {
            playerDisplayName = challengeManagerScript.SecondPlayerName;
        }
        else
        {
            Debug.Log("Invalid playerNumber, use 1 or 2");
        }
        
        return playerDisplayName;
    }

    private string GetPlayerID(int playerNumber = 0)
    {
        string playerID = "";

        if (playerNumber == 1)
        {
            playerID = challengeManagerScript.FirstPlayerID;
        }
        else if (playerNumber == 2)
        {
            playerID = challengeManagerScript.SecondPlayerID;
        }
        else
        {
            Debug.Log("Invalid playerNumber, use 1 or 2");
        }

        return playerID;
    }
    
}
