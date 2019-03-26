using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnPopup : MonoBehaviour
{

    public EventManager eventManager;
    public InterfaceController interfaceController;
    public GameCoreController gameCoreController;

    private float popupTime = 2f;

    public bool isPoppedUp = false;

    private GameObject playerTurnPopup;
    private GameObject playerTurnPopupText;
    private Text turnText;

    private string PLAYERNAME = "Player";
    private string COMPUTERNAME = "Computer";
    private string YOURTURNTEXT = "Player's turn";
    private string OTHERPLAYERTEXT = "Computer's turn";

    private void Awake()
    {
        playerTurnPopup = GameObject.Find("PlayerTurnBox");
        playerTurnPopupText = GameObject.Find("PlayerTurnBoxText");
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
        gameCoreController = GameObject.Find("GameController").GetComponent<GameCoreController>();
        turnText = playerTurnPopupText.GetComponent<Text>();

        if (SessionStates.GameMode == GameModeEnum.MULTIPLAYER)
        {
            eventManager.ListenToChallengeTurnTaken(UpdateTurnPopup);            
        }
        else
        {
            eventManager.ListenToTurnTaken(UpdateTurnPopup);
        }

    }

    private void Start()
    {
        playerTurnPopup.SetActive(false);
        if (SessionStates.GameMode == GameModeEnum.MULTIPLAYER)
        {
            UpdateTurnPopup();
        }
        else
        {
            if (gameCoreController.GetWhoseTurn() == GameCore.GameBoard.PlayerEnum.ONE)
            {
                turnText.text = YOURTURNTEXT;
            }
            else
            {
                turnText.text = OTHERPLAYERTEXT;
            }
            playerTurnPopup.SetActive(true);
            StartCoroutine(PopUp());
        }        
    }

    void Update()
    {

    }

    public void UpdateTurnPopup()
    {
        isPoppedUp = true;
        playerTurnPopup.SetActive(true);


        // Get info from ChallengeManager/Controller
        if (SessionStates.GameMode == GameModeEnum.MULTIPLAYER)
        {
            turnText.text = interfaceController.GetPlayerNameForTurn() + "'s Turn!";
        }
        else
        {
            turnText.text = turnText.text == YOURTURNTEXT ?
                OTHERPLAYERTEXT : YOURTURNTEXT;
        }

        Debug.Log("turnText value: " + turnText.text);
        StartCoroutine(PopUp());
    }

    IEnumerator PopUp()
    {        
        float time = 0;

        while (time < popupTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        isPoppedUp = false;
        playerTurnPopup.SetActive(false);
    }
}
