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
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        soundEffectController = GameObject.Find("GameController").GetComponent<SoundEffectController>();
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            //messageQueue = GameObject.Find("MessageQueue").GetComponent<MessageQueue>();
            //GameObject challengeManagerObject = GameObject.Find("ChallengeManager");
            //challengeManagerScript = challengeManagerObject.GetComponent<ChallengeManager>();

            //eventManager.ListenToChallengeStartingPlayerSet(SetupMultiplayerGame);
            //eventManager.ListenToMoveReceived(MakeOpponentMove);
            //eventManager.ListenToNewGame(RestartGame);
            //eventManager.InvokeGameBoardReady();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        winPanel = GameObject.Find("WinScreen");
        winPanel.SetActive(false);
        menuPanel = GameObject.Find("MenuOptions");
        helpScreen = GameObject.Find("HelpMenu");
        playerOneText = GameObject.Find("PlayerOneText").GetComponent<Text>();
        playerTwoText = GameObject.Find("PlayerTwoText").GetComponent<Text>();
        soundEffectController = GameObject.Find("GameController").GetComponent<SoundEffectController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MoveOpponentPieceInGUI(string guiSpaceName)
    {
        GameObject opponentMouse = GameObject.Find("opponentMouse");
        GameObject targetSquare = GameObject.Find(guiSpaceName);
        ClickSquare clickSquare = targetSquare.GetComponent<ClickSquare>();
        MoveMouse moveMouseScript = opponentMouse.GetComponent<MoveMouse>();
        moveMouseScript.target = new Vector3(clickSquare.transform.position.x, clickSquare.transform.position.y, -0.5f);
        moveMouseScript.moveMouse = true;
        soundEffectController.PlaySqueakSound();
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

    //public void SetupMultiplayerGame()
    //{
    //    PlayerInfo playerInfo = GetPlayerInfo(challengeManagerScript.CurrentPlayerInfo.PlayerNumber);

    //    // set player in gameboard 
    //    gameCoreController.SetPlayerTurn(playerInfo.PlayerNumber);
        
    //    // set gui text 
    //    playerOneText = GameObject.Find("PlayerOneText").GetComponent<Text>();
    //    playerTwoText = GameObject.Find("PlayerTwoText").GetComponent<Text>();
    //    if (playerOneText != null)
    //    {
    //        playerOneText.text = networkGameController.GetPlayerOneDisplayName();
    //    }
    //    if (playerTwoText != null)
    //    {
    //        playerTwoText.text = networkGameController.GetPlayerTwoDisplayName();
    //    }
    //}

    //private PlayerInfo GetPlayerInfo(int playerNumber = 0)
    //{
    //    PlayerInfo playerInfo = new PlayerInfo();
    //    if (challengeManagerScript)
    //    {
    //        playerInfo = challengeManagerScript.GetPlayerInfo(playerNumber);
    //    }
    //    else
    //    {
    //        Debug.Log("challengeManager not active (not a multiplayer game)");
    //    }
    //    return playerInfo;
    //}

    //public void RestartGame()
    //{
    //    gameCoreController.ResetGameBoard();

    //    if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
    //    {
    //        eventManager.InvokeGameBoardReady();
    //    }
    //}

}
