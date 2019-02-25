using UnityEngine;
using GameCore;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Controller : MonoBehaviour
{
    private GameBoard gameBoard;
    private Dictionary<string, PlayerCoordinate> spaceCoordMap;
    private Dictionary<string, WallCoordinate> wallCoordMap;
    private ChallengeManager challengeManagerScript;
    private EventManager eventManager;
    private MessageQueue messageQueue;
    private GameObject winPanel;
    private GameObject menuPanel;
    private GameObject helpScreen;
    private Text playerOneText;
    private Text playerTwoText;
    private bool opponentTurn;

    private void Awake()
    {
        Debug.Log("Awake Controller");

        gameBoard = new GameBoard(GameBoard.PlayerEnum.ONE, "e1", "e9");

        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
            messageQueue = GameObject.Find("MessageQueue").GetComponent<MessageQueue>();
            GameObject challengeManagerObject = GameObject.Find("ChallengeManager");
            challengeManagerScript = challengeManagerObject.GetComponent<ChallengeManager>();

            eventManager.ListenToChallengeStartingPlayerSet(SetupMultiplayerGame);
            eventManager.ListenToMoveReceived(MakeOpponentMove);
            eventManager.InvokeGameBoardReady();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        spaceCoordMap = new Dictionary<string, PlayerCoordinate>();
        wallCoordMap = new Dictionary<string, WallCoordinate>();
        winPanel = GameObject.Find("WinScreen");
        winPanel.SetActive(false);
        menuPanel = GameObject.Find("MenuOptions");
        helpScreen = GameObject.Find("HelpMenu");
        
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {

        }
        else
        {
            // randomize first player 
            opponentTurn = new System.Random().NextDouble() >= .5;
            if (opponentTurn)
            {
                gameBoard = new GameBoard(GameBoard.PlayerEnum.TWO, "e1", "e9");
            }
            
            // start watching for moves 
            InvokeRepeating("WatchForMoves", 0.1f, 0.1f);
        }
        
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
            opponentTurn = false; // changing this here so MakeOpponentMove() is not called again while it is running in background 
            MakeOpponentMove();
        }
    }

    public void MarkLocalPlayerMove()
    {
        opponentTurn = true;
    }

    // Get move from AI or network 
    public async void MakeOpponentMove()
    {
        Debug.Log("Making move");

        // Decide if the opponent placed a wall or piece, and call appropriate method 
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            string moveString = GetOpponentMoveString();
            Debug.Log("MakeOpponentMove, moveString: " + moveString);
            if (moveString.Length == 2)
            {
                moveString = BoardUtil.MirrorMove(moveString);
                ReceiveMove(GameBoard.PlayerEnum.TWO, moveString);
                MoveOpponentPieceInGUI(moveString);
            }
            else if (moveString.Length == 3)
            {
                moveString = BoardUtil.MirrorWall(moveString);
                ReceiveWall(GameBoard.PlayerEnum.TWO, moveString);
                MoveOpponentWallInGUI(moveString);
            }
        }
        else
        {
            string moveString = await Task.Run(() => GetOpponentMoveString());
            Debug.Log("MakeOpponentMove, moveString: " + moveString);
            if (moveString.Length == 2 && IsValidMove(GameBoard.PlayerEnum.TWO, moveString))
            {
                MoveOpponentPieceInGUI(moveString);
            }
            else if (moveString.Length == 3 && IsValidWallPlacement(GameBoard.PlayerEnum.TWO, moveString))
            {
                MoveOpponentWallInGUI(moveString);
            }
        }
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
        while (messageQueue.IsQueueEmpty("opponentMoveQueue"))
        {
            
        }
        if (!messageQueue.IsQueueEmpty("opponentMoveQueue"))
        {
            mirroredMove = messageQueue.DequeueOpponentMoveQueue();
        }
        else
        {

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
            Debug.Log("Valid Multiplayer Move");
            if (challengeManagerScript.IsItMyTurn())
            {
                Debug.Log("Making the move");
                MarkLocalPlayerMove();
                // Send move via ChallengeManager
                challengeManagerScript.Move(spaceName);
            }
        }
        return validMove;
    }

    public void ReceiveMove(GameBoard.PlayerEnum player, string spaceName)
    {
        PlayerCoordinate pc = spaceCoordMap[spaceName];
        bool validMove = gameBoard.MovePiece(player, pc);
    }

    public void ReceiveWall(GameBoard.PlayerEnum player, string spaceName)
    {
        Debug.Log("ReceiveWall, spaceName given: " + spaceName);
        bool validWall = gameBoard.PlaceWall(player, new WallCoordinate(spaceName));
    }
    
    public bool IsValidWallPlacement(GameBoard.PlayerEnum player, string spaceName)
    {
        bool validWallPlacement = false;
        validWallPlacement = gameBoard.PlaceWall(player, new WallCoordinate(spaceName));
        // If validMove send move across network
        if (validWallPlacement && GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            Debug.Log("Valid Multiplayer Move");
            if (challengeManagerScript.IsItMyTurn())
            {
                Debug.Log("Making the move");
                MarkLocalPlayerMove();
                // Send move via ChallengeManager
                challengeManagerScript.Move(spaceName);
            }
        }
        
        return validWallPlacement;
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
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            if (gameBoard.PlayerTwoWin())
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
            if (gameBoard.PlayerOneWin())
            {
                return "Player One Wins!";
            }
            else
            {
                return "Player Two Wins!";
            }
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
            playerInfo = challengeManagerScript.GetPlayerInfo(playerNumber);
        }
        else
        {
            Debug.Log("challengeManager not active (not a multiplayer game)");
        }
        return playerInfo;
    }

    public void SetupMultiplayerGame()
    {
        Debug.Log("Setting up multiplayer game in controller");
        PlayerInfo playerInfo = GetPlayerInfo(challengeManagerScript.CurrentPlayerInfo.PlayerNumber);
        if (playerInfo.PlayerNumber == 1)
        {
            opponentTurn = false;
            // Set player's turn in GameBoard
            gameBoard.SetPlayerTurnRandom();
            while (gameBoard.GetWhoseTurn() != 1)
            {
                gameBoard.SetPlayerTurnRandom();
            }
        }
        else if (playerInfo.PlayerNumber == 2)
        {
            opponentTurn = true;
            // Set player's turn in GameBoard
            gameBoard.SetPlayerTurnRandom();
            while (gameBoard.GetWhoseTurn() != 2)
            {
                gameBoard.SetPlayerTurnRandom();
            }
        }

        playerOneText = GameObject.Find("PlayerOneText").GetComponent<Text>();
        playerTwoText = GameObject.Find("PlayerTwoText").GetComponent<Text>();

        if (playerOneText != null)
        {
            playerOneText.text = challengeManagerScript.FirstPlayerInfo.PlayerDisplayName;
        }
        if (playerTwoText != null)
        {
            playerTwoText.text = challengeManagerScript.SecondPlayerInfo.PlayerDisplayName;
        }
        
    }

}
