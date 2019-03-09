using System.Collections;
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
    private Text playerOneText;
    private Text playerTwoText;
    private SoundEffectController soundEffectController;
    private NetworkGameController networkGameController;
    private GameCoreController gameCoreController;

    private void Awake()
    {
        challengeManagerScript = GameObject.Find("ChallengeManager").GetComponent<ChallengeManager>();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        playerOneText = GameObject.Find("PlayerOneText").GetComponent<Text>();
        playerTwoText = GameObject.Find("PlayerTwoText").GetComponent<Text>();

        if (GameModeStatus.GameMode == GameModeEnum.SINGLE_PLAYER)
        {
            eventManager.ListenToLocalPlayerMoved(GenerateMoveForAI);
        }
        else if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
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
        if (IsGameOver() && !menuPanel.activeSelf)
        {
            winPanel.SetActive(true);
        }
    }

    public void MoveOpponentPieceInGUI(string guiSpaceName)
    {
        GameObject opponentMouse = GameObject.Find("opponentMouse");
        GameObject targetSquare = GameObject.Find(guiSpaceName);
        ClickSquare clickSquare = targetSquare.GetComponent<ClickSquare>();
        MoveMouse moveMouseScript = opponentMouse.GetComponent<MoveMouse>();
        moveMouseScript.target = new Vector3(clickSquare.transform.position.x, clickSquare.transform.position.y, -0.5f);
        moveMouseScript.moveMouse = true;
        soundEffectController.PlaySqueakSound();
    }

    public void MoveOpponentWallInGUI(string colliderName)
    {
        GameObject wall = GetUnusedOpponentWall();
        Collider collider = GetCollider(colliderName);
        if (wall != null && collider != null)
        {
            wall.transform.localScale = collider.transform.localScale;
            MoveWallsProgramatically moveWallScript = wall.GetComponent<MoveWallsProgramatically>();
            moveWallScript.target = new Vector3(collider.transform.position.x, collider.transform.position.y, wall.transform.position.z);
            moveWallScript.moveWall = true;

            moveWallScript.SetIsOnBoard(true);
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

    public void SetPlayerOneText(string str)
    {
        if (playerOneText != null)
        {
            playerOneText.text = str;
        }
    }

    public void SetPlayerTwoText(string str)
    {
        if (playerTwoText != null)
        { 
            playerTwoText.text = str;
        }
    }

    private async void GenerateMoveForAI()
    {
        string str = await gameCoreController.GetMoveFromAI();
        gameCoreController.RecordOpponentMove(str);
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
            if (GameModeStatus.GameMode == GameModeEnum.SINGLE_PLAYER)
            {
                eventManager.InvokeLocalPlayerMoved();
            }
            else
            {
                challengeManagerScript.Move(move);
            }
        }
        return movedSuccessfully;
    }

    public string GetPlayerNameForTurn()
    {
        string playerDisplayName = "";
        playerDisplayName = challengeManagerScript.PlayerNameForTurn;
        return playerDisplayName;
    }

    public string GetLocalPlayerName()
    {
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
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
        return gameCoreController.GetPlayerOneWallCount();
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
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
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

}
