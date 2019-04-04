using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using System;
using UnityEngine.SceneManagement;

public class HostedGameLobbies : MonoBehaviour
{
    RectTransform hostedGameLobbiesRectTransform;
    Button hostGameButton;
    Button refreshGamesButton;
    public GameObject hostedGamePrefab;
    public List<GameObject> hostedGames;

    public ChallengeManager challengeManager;
    
    private bool hosting = false;

    private void Awake()
    {
        hostedGameLobbiesRectTransform = GameObject.Find("HostedGameLobbies").GetComponent<RectTransform>();
        hostGameButton = GameObject.Find("HostGameButton").GetComponent<Button>();
        hostGameButton.onClick.AddListener(onHostGameButtonClick);
        refreshGamesButton = GameObject.Find("RefreshGamesButton").GetComponent<Button>();
        refreshGamesButton.onClick.AddListener(onRefreshGamesButtonClick);
        hostedGames = new List<GameObject>();
        challengeManager = GameObject.Find("ChallengeManager").GetComponent<ChallengeManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ScriptMessage.Listener += RefreshHostedGames;
        onRefreshGamesButtonClick();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveRefreshHostedGamesListener()
    {
        ScriptMessage.Listener -= RefreshHostedGames;
    }

    private void RefreshHostedGames(ScriptMessage message)
    {
        onRefreshGamesButtonClick();
    }


    void FindGameLobbies()
    {
        RemoveAllHostedGames();
        FindPendingMatchesRequest findPendingMatchesRequest = new FindPendingMatchesRequest();
        findPendingMatchesRequest.SetMatchShortCode("HostedMatch");
        findPendingMatchesRequest.SetMaxMatchesToFind(100);
        findPendingMatchesRequest.Send(OnFindPendingMatchesRequestSuccess, OnFindPendingMatchesRequestError);
    }

    public void OnFindPendingMatchesRequestSuccess(FindPendingMatchesResponse response)
    {
        //Debug.Log("Matches found: " + response.ScriptData.BaseData["matchesFound"]);
        var pendingMatches = response.PendingMatches.GetEnumerator();
        bool matchesFound = false;
        bool endOfMatches = !pendingMatches.MoveNext();
        while (!endOfMatches)
        {
            GameObject.Find("NoHostedGamesPanel").SetActive(false);
            matchesFound = true;
            // Get match data
            var matchedPlayers = pendingMatches.Current.MatchedPlayers.GetEnumerator();
            matchedPlayers.MoveNext();
            if ((bool)matchedPlayers.Current.ParticipantData.BaseData["hosting"])
            {
                string hostDisplayName = matchedPlayers.Current.ParticipantData.BaseData["displayName"].ToString();
                string matchID = pendingMatches.Current.Id;
                string matchShortCode = pendingMatches.Current.MatchShortCode;
                AddHostedGameToLobbies(hostDisplayName, matchID, matchShortCode);
            }
            endOfMatches = !pendingMatches.MoveNext();
        }
        if (!matchesFound)
        {
            // No pending matches found
            Debug.Log("No Pending Matches found");
            GameObject.Find("NoHostedGamesPanel").SetActive(true);
        }
        UnblockRefreshInput();
    }

    public void OnFindPendingMatchesRequestError(FindPendingMatchesResponse response)
    {
        Debug.Log("Find Matches Error: " + response.Errors.JSON);
        UnblockRefreshInput();
    }


    private void onRefreshGamesButtonClick()
    {
        BlockRefreshInput();
        // Make sure the user isn't currently hosting that way the refresh doesn't remove their hosted lobby/game
        if (!hosting)
        {
            GameSparksUserID gameSparksUserIDScript = GameObject.Find("GameSparksUserID").GetComponent<GameSparksUserID>();
            // Do a matchmaking request
            MatchmakingRequest matchmakingRequest = new MatchmakingRequest();
            matchmakingRequest.SetMatchShortCode("HostedMatch");

            GameSparks.Core.GSRequestData participantData = new GameSparks.Core.GSRequestData();
            participantData.AddString("displayName", gameSparksUserIDScript.myDisplayName);
            participantData.AddBoolean("hosting", false);
            matchmakingRequest.SetParticipantData(participantData);
            matchmakingRequest.SetMatchData(participantData);
            matchmakingRequest.SetSkill(0);

            matchmakingRequest.Send(OnMatchmakingSuccess, OnMatchmakingError);
            FindGameLobbies();
        }        
        else
        {
            FindGameLobbies();
        }        
    }

    private void BlockRefreshInput()
    {
        if (refreshGamesButton != null)
        {
            refreshGamesButton.enabled = false;
        }        
    }

    private void UnblockRefreshInput()
    {
        if (refreshGamesButton != null)
        {
            refreshGamesButton.enabled = true;
        }        
    }

    public void onHostGameButtonClick()
    {
        GameSparksUserID gameSparksUserIDScript = GameObject.Find("GameSparksUserID").GetComponent<GameSparksUserID>();

        MatchmakingRequest matchmakingRequest = new MatchmakingRequest();
        matchmakingRequest.SetMatchShortCode("HostedMatch");

        GameSparks.Core.GSRequestData participantData = new GameSparks.Core.GSRequestData();
        participantData.AddString("displayName", gameSparksUserIDScript.myDisplayName);
        participantData.AddBoolean("hosting", true);
        matchmakingRequest.SetParticipantData(participantData);
        matchmakingRequest.SetMatchData(participantData);
        matchmakingRequest.SetSkill(0);

        
        if (!hosting)
        {
            hosting = true;
            // Change button text to represent canceling host
            UnityEngine.UI.Text[] buttonText = hostGameButton.GetComponentsInChildren<Text>();
            buttonText[0].text = "Cancel Host";
        }
        else
        {
            // Cancel host
            hosting = false;
            matchmakingRequest.SetAction("cancel");
            matchmakingRequest.Send(OnMatchmakingSuccess, OnMatchmakingError);
            // Change button text to represent hosting a game
            UnityEngine.UI.Text[] buttonText = hostGameButton.GetComponentsInChildren<Text>();
            buttonText[0].text = "Host Game";
        }

        matchmakingRequest.Send(OnMatchmakingSuccess, OnMatchmakingError);
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

    // Add game data to lobbies in UI
    void AddHostedGameToLobbies(string hostName, string gameID, string matchShortCode)
    {
        // Create new (gamelobby) Button to add to children of HostedGameLobbies
        GameObject hostedGameLobby = Instantiate(hostedGamePrefab) as GameObject;
        HostedGameLobbyButton hostedGameLobbyScript = hostedGameLobby.GetComponent<HostedGameLobbyButton>();
        hostedGameLobbyScript.hostName = hostName;
        hostedGameLobbyScript.gameID = gameID;
        hostedGameLobbyScript.matchShortCode = matchShortCode;
        hostedGameLobby.GetComponent<Button>().onClick.AddListener(hostedGameLobbyScript.onClick);
        
        // Get text component of button
        UnityEngine.UI.Text[] hostedGameLobbyText = hostedGameLobby.GetComponentsInChildren<Text>();
        Text playerText = hostedGameLobbyText[0];
        //Text messageText = hostedGameLobbyText[1];

        if (hostName.Length >= 10)
        {
            playerText.text = ("<b>" + hostName.Substring(0, 10) + ":</b>");
        }
        else
        {
            playerText.text = ("<b>" + hostName + ":</b>");
        }

        //playerText.text = ("<b>" + messageWho + ":</b>");
        //messageText.text = (messageMessage);

        hostedGameLobby.transform.SetParent(hostedGameLobbiesRectTransform);
        hostedGameLobby.transform.localScale = new Vector3(1, 1, 1);

        hostedGames.Add(hostedGameLobby);

        LayoutRebuilder.ForceRebuildLayoutImmediate(hostedGameLobbiesRectTransform);
    }

    void RemoveAllHostedGames()
    {
        foreach (RectTransform child in hostedGameLobbiesRectTransform)
        {
            GameObject.Destroy(child.gameObject);
        }
        hostedGames.Clear();
    }
}
