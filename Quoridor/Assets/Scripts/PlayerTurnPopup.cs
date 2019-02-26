using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnPopup : MonoBehaviour
{

    public EventManager eventManager;
    public Controller controller; 

    private float popupTime = 5f;
    //how long should we turn for.

    public bool isPoppedUp = false;
    //are we currently popped up?
    //we use this to prevent multiple instance of popping up starting.

    private GameObject playerTurnPopup;
    private GameObject playerTurnPopupText;
    //private bool initialInactive;

    private void Awake()
    {
        playerTurnPopup = GameObject.Find("PlayerTurnBox");
        playerTurnPopupText = GameObject.Find("PlayerTurnBoxText");
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            eventManager.ListenToChallengeTurnTaken(UpdateTurnPopup);
            controller = GameObject.Find("GameController").GetComponent<Controller>();
        }
    }

    private void Start()
    {
        playerTurnPopup.SetActive(false);
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            UpdateTurnPopup();
        }
    }

    void Update()
    {
        //if (initialInactive)
        //{
        //    playerTurnPopup.SetActive(false);
        //    initialInactive = false;
        //}
        //if (isPoppedUp)
        //    StartCoroutine(PopUp());
    }

    public void UpdateTurnPopup()
    {
        if (!isPoppedUp)
        {
            isPoppedUp = true;
            playerTurnPopup.SetActive(true);
            // Get info from ChallengeManager/Controller
            Text turnText = playerTurnPopupText.GetComponent<Text>();
            turnText.text = controller.GetPlayerNameForTurn() + "'s Turn!";
            Debug.Log("turnText value: " + turnText.text);
            StartCoroutine(PopUp());
        }

    }

    //void PopUp()
    IEnumerator PopUp()
    {
        //playerTurnPopup.SetActive(true);            
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
