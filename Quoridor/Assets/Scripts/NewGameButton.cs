using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameButton : MonoBehaviour
{
    private Button newGameWinButton;
    private Button newGameMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("NewGameButton") != null)
        {
            newGameMenuButton = GameObject.Find("NewGameButton").GetComponent<Button>();
            newGameMenuButton.interactable = false;
            if (GameModeStatus.GameMode == GameModeEnum.SINGLE_PLAYER)
            {
                newGameMenuButton.interactable = true;
            }
        }
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            if (GameObject.Find("WinScreenNewGameButton") != null)
            {
                newGameWinButton = GameObject.Find("WinScreenNewGameButton").GetComponent<Button>();      
                ChallengeStartedMessage.Listener += OnChallengeStarted;
                ChallengeIssuedMessage.Listener += OnChallengeIssued;
            }
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            ChallengeStartedMessage.Listener -= OnChallengeStarted;
        }        
    }

    public void onNewGameButtonClick()
    {
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            // For now just do a matchmaking request again (and the two players will match if they are the only two searching)
            onMatchMakingButtonClick();
        }
        else
        {
            // Get ChallengeManager/EventManager/MessageQueue DontDestroyOnLoad Objects and reset them
            ChallengeManager challengeManager = GameObject.Find("ChallengeManager").GetComponent<ChallengeManager>();
            challengeManager = new ChallengeManager();
            
            EventManager eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
            eventManager = new EventManager();

            MessageQueue messageQueue = GameObject.Find("MessageQueue").GetComponent<MessageQueue>();
            messageQueue = new MessageQueue();
            
            SceneManager.LoadScene("GameBoard");
        }
    }

    public void onMatchMakingButtonClick()
    {
        BlockInput();
        Debug.Log("Making/sending matchmaking request");
        MatchmakingRequest request = new MatchmakingRequest();
        request.SetMatchShortCode("DefaultMatch");
        request.SetSkill(0);
        request.Send(OnMatchmakingSuccess, OnMatchmakingError);
    }


    public void OnMatchmakingSuccess(MatchmakingResponse response)
    {
        //UnblockInput();
        Debug.Log("Matchmaking Success");
    }

    public void OnMatchmakingError(MatchmakingResponse response)
    {
        //UnblockInput();
        Debug.Log("Matchmaking Error");
    }

    private void OnChallengeIssued(ChallengeIssuedMessage message)
    {
        Debug.Log("On Challenge Issued");
        var challengeInstaceId = message.Challenge.ChallengeId;
        Debug.Log("This challenge ID: " + challengeInstaceId);
        if (challengeInstaceId != null)
        {
            new AcceptChallengeRequest()
                .SetChallengeInstanceId(challengeInstaceId)
                //.SetMessage(message)
                .Send((response) => {
                    //string challengeInstanceId = response.ChallengeInstanceId;
                    //GSData scriptData = response.ScriptData;
                });
        }
    }

    private void OnChallengeStarted(ChallengeStartedMessage message)
    {
        UnblockInput();
        Debug.Log("Challenge Started");
        // Switch to GameBoard Scene connected to opponent
        SceneManager.LoadScene("GameBoard");

    }

    private void BlockInput()
    {
        newGameWinButton.interactable = false;
    }

    private void UnblockInput()
    {
        newGameWinButton.interactable = true;
    }
}
