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

    private MatchmakingRequest lastMatchmakingRequest;
    private AcceptChallengeRequest lastAcceptChallengeRequest;

    // Start is called before the first frame update
    void Start()
    {
        //if (GameObject.Find("NewGameButton") != null)
        //{
        //    newGameMenuButton = GameObject.Find("NewGameButton").GetComponent<Button>();
        //    newGameMenuButton.interactable = false;
        //    //newGameMenuButton.gameObject.SetActive(false);
        //    if (GameSession.GameMode == GameModeEnum.SINGLE_PLAYER)
        //    {
        //        //newGameMenuButton.gameObject.SetActive(true);
        //        newGameMenuButton.interactable = true;
        //    }
        //}
        if (GameSession.GameMode == GameModeEnum.MULTIPLAYER)
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
        ChallengeStartedMessage.Listener -= OnChallengeStarted;
        ChallengeIssuedMessage.Listener -= OnChallengeIssued;
    }

    public void onNewGameButtonClick()
    {
        if (GameSession.GameMode == GameModeEnum.MULTIPLAYER)
        {

            EventManager eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
            eventManager.InvokePlayAgain();

            StartCoroutine(onMatchMakingButtonClick());
        }
        else
        {
            //SceneManager.LoadScene("GameBoard");
            GameObject.Find("LevelChanger").GetComponent<LevelChanger>().FadeToLevel("GameBoard");
        }
    }

    public IEnumerator onMatchMakingButtonClick()
    {
        BlockInput();
        Debug.Log("Making/sending matchmaking request");

        string matchGroupNumber = "";
        yield return GetMatchmakingGroupNumber(matchGroupNumber);

        MatchmakingRequest request = new MatchmakingRequest();
        request.SetMatchShortCode("DefaultMatch");
        request.SetSkill(0);
        request.SetMatchGroup(matchGroupNumber);
        // Store last request incase we get a throttled response
        lastMatchmakingRequest = request;

        request.Send(OnMatchmakingSuccess, OnMatchmakingError);
        
    }

    private IEnumerator GetMatchmakingGroupNumber(string matchmakingGroupNumber)
    {
        MessageQueue messageQueue = GameObject.Find("MessageQueue").GetComponent<MessageQueue>();
        while (messageQueue.IsQueueEmpty(MessageQueue.QueueNameEnum.MATCHMAKINGGROUPNUMBER))
        {
            yield return messageQueue.CheckQueueNotEmpty(MessageQueue.QueueNameEnum.MATCHMAKINGGROUPNUMBER);
        }
        ScriptMessage message = messageQueue.DequeueMatchmakingGroupNumber();
        IDictionary<string, object> messageData = message.Data.BaseData;
        Debug.Log("Matchmaking Group Number: " + messageData["matchGroupNumber"].ToString());
        var matchGroupNumber = messageData["matchGroupNumber"];
        matchmakingGroupNumber = matchGroupNumber.ToString();
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
        if (ThrottleHandler.IsRequestThrottled(response.Errors.JSON))
        {
            var request = lastMatchmakingRequest;
            request.Send(OnMatchmakingSuccess, OnMatchmakingError);
        }
    }

    private void OnChallengeIssued(ChallengeIssuedMessage message)
    {
        Debug.Log("On Challenge Issued");
        var challengeInstaceId = message.Challenge.ChallengeId;
        Debug.Log("This challenge ID: " + challengeInstaceId);
        if (challengeInstaceId != null)
        {
            var acc = new AcceptChallengeRequest();
            acc.SetChallengeInstanceId(challengeInstaceId);
            acc.Send(OnChallengeIssuedSuccess);
        }
    }

    private void OnChallengeIssuedSuccess(AcceptChallengeResponse response)
    {
        if (ThrottleHandler.IsRequestThrottled(response.Errors.JSON))
        {
            var request = lastAcceptChallengeRequest;
            request.Send(OnChallengeIssuedSuccess);
        }
    }

    private void OnChallengeStarted(ChallengeStartedMessage message)
    {
        UnblockInput();
        Debug.Log("Challenge Started");
        // Switch to GameBoard Scene connected to opponent
        //SceneManager.LoadScene("GameBoard");
        GameObject.Find("LevelChanger").GetComponent<LevelChanger>().FadeToLevel("GameBoard");
        EventManager eventManager = GameObject.Find("EventManger").GetComponent<EventManager>();
        eventManager.InvokeNewGame();
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
