using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

public class HostedGameLobbies : MonoBehaviour
{
    RectTransform hostedGameLobbiesRectTransform;
    Button hostGameButton;
    public GameObject hostedGamePrefab;
    public List<GameObject> hostedGames;

    public ChallengeManager challengeManager;

    private void Awake()
    {
        hostedGameLobbiesRectTransform = GameObject.Find("HostedGameLobbies").GetComponent<RectTransform>();
        hostGameButton = GameObject.Find("HostGameButton").GetComponent<Button>();
        hostGameButton.onClick.AddListener(onHostGameButtonClick);
        hostedGames = new List<GameObject>();
        challengeManager = GameObject.Find("ChallengeManager").GetComponent<ChallengeManager>();

        //FindGameLobbies();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindGameLobbies()
    {
        FindPendingMatchesRequest findPendingMatchesRequest = new FindPendingMatchesRequest();
        findPendingMatchesRequest.SetMatchShortCode("HostedMatch");
        findPendingMatchesRequest.SetMaxMatchesToFind(100);
        findPendingMatchesRequest.Send(OnFindPendingMatchesRequestSuccess, OnFindPendingMatchesRequestError);
    }

    public void OnFindPendingMatchesRequestSuccess(FindPendingMatchesResponse response)
    {
        //UnblockInput();

        if ((bool)response.ScriptData.BaseData["matchesFound"])
        {
            Debug.Log(response.ScriptData.BaseData["matchesFound"]);
            var pendingMatches = response.PendingMatches.GetEnumerator();
            Debug.Log(pendingMatches.ToString());
            bool endOfMatches = !pendingMatches.MoveNext();
            while (!endOfMatches)
            {
                // Get match data
                var matchedPlayers = pendingMatches.Current.MatchedPlayers.GetEnumerator();
                matchedPlayers.MoveNext();
                string hostDisplayName = matchedPlayers.Current.ParticipantData.BaseData["displayName"].ToString();
                string matchID = pendingMatches.Current.Id;
                string matchShortCode = pendingMatches.Current.MatchShortCode;
                AddHostedGameToLobbies(hostDisplayName, matchID, matchShortCode);
                endOfMatches = !pendingMatches.MoveNext();
            }
        }        
        else
        {
            // No pending matches found
            Debug.Log("No Pending Matches found");
        }
    }

    public void OnFindPendingMatchesRequestError(FindPendingMatchesResponse response)
    {
        //UnblockInput();
        Debug.Log("Find Matches Error");
    }

    public void onHostGameButtonClick()
    {
        GameSparksUserID gameSparksUserIDScript = GameObject.Find("GameSparksUserID").GetComponent<GameSparksUserID>();

        // Do a matchmaking request
        MatchmakingRequest matchmakingRequest = new MatchmakingRequest();
        matchmakingRequest.SetMatchShortCode("HostedMatch");
        
        GameSparks.Core.GSRequestData participantData = new GameSparks.Core.GSRequestData();
        participantData.AddString("displayName", gameSparksUserIDScript.myDisplayName);
        matchmakingRequest.SetParticipantData(participantData);

        matchmakingRequest.SetSkill(0);

        matchmakingRequest.Send(OnMatchmakingSuccess, OnMatchmakingError);
    }

    public void OnMatchmakingSuccess(MatchmakingResponse response)
    {
        //UnblockInput();
        Debug.Log("Matchmaking Success");

        FindGameLobbies();
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
}
