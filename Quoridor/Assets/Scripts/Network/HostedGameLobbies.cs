using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class HostedGameLobbies : MonoBehaviour
{
    RectTransform hostedGameLobbiesRectTransform;
    Button hostGameButton;
    Button joinGameButton;
    public Button matchmakingButton;
    Button refreshGamesButton;
    public GameObject hostedGamePrefab;
    public List<GameObject> hostedGames;
    public GameObject noHostedGamesPanel;
    public GameSparksUserID gameSparksUserID;

    public TMPro.TextMeshProUGUI findingMatchText;

    public int heightOfHostedGame = 80;

    public ChallengeManager challengeManager;

    [SerializeField]
    public RectTransform upperBoundary;
    [SerializeField]
    public RectTransform lowerBoundary;
    [SerializeField]
    public Button scrollUpButton;
    [SerializeField]
    public Button scrollDownButton;
    public float heightOfAllGames;

    private bool hosting = false;
    private bool refreshingLock = false;
    private Timer refreshTimer;

    private void Awake()
    {
        hostedGameLobbiesRectTransform = GameObject.Find("HostedGameLobbies").GetComponent<RectTransform>();
        hostGameButton = GameObject.Find("HostGameButton").GetComponent<Button>();
        hostGameButton.onClick.AddListener(OnHostGameButtonClick);
        joinGameButton = GameObject.Find("JoinGameButton").GetComponent<Button>();
        joinGameButton.onClick.AddListener(OnJoinGameButtonClick);
        refreshGamesButton = GameObject.Find("RefreshGamesButton").GetComponent<Button>();
        refreshGamesButton.onClick.AddListener(onRefreshGamesButtonClick);
        hostedGames = new List<GameObject>();
        challengeManager = GameObject.Find("ChallengeManager").GetComponent<ChallengeManager>();

        GameObject upMostBoundsObject = new GameObject("upMostBoundariesObject", typeof(RectTransform));
        upperBoundary = upMostBoundsObject.GetComponent<RectTransform>();
        upperBoundary.localPosition = new Vector2((hostedGameLobbiesRectTransform.localPosition.x),
                                               (hostedGameLobbiesRectTransform.localPosition.y));

        GameObject lowMostBoundsObject = new GameObject("lowMostBoundariesObject", typeof(RectTransform));
        lowerBoundary = lowMostBoundsObject.GetComponent<RectTransform>();
        lowerBoundary.localPosition = new Vector2((hostedGameLobbiesRectTransform.localPosition.x),
                                               (hostedGameLobbiesRectTransform.localPosition.y + heightOfHostedGame));
        
        refreshTimer = gameObject.AddComponent<Timer>();
        refreshTimer.SetTimeDefault(10f);
        refreshTimer.ResetTimer();
        refreshTimer.timeUp.AddListener(RefreshTime);
        refreshTimer.StartCountdown();
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

    private void OnDestroy()
    {
        ScriptMessage.Listener -= RefreshHostedGames;
        gameSparksUserID = null;
        challengeManager = null;
    }

    public void RemoveRefreshHostedGamesListener()
    {
        ScriptMessage.Listener -= RefreshHostedGames;
    }

    private void RefreshTime()
    {
        refreshTimer.ResetTimer();
        refreshTimer.StartCountdown();
        FindGameLobbies();
    }

    private void RefreshHostedGames(ScriptMessage message)
    {
        onRefreshGamesButtonClick();
    }


    void FindGameLobbies()
    {
        if (!refreshingLock)
        {
            refreshingLock = true;
            RemoveAllHostedGames();
            FindPendingMatchesRequest findPendingMatchesRequest = new FindPendingMatchesRequest();
            findPendingMatchesRequest.SetMatchShortCode("HostedMatch");
            findPendingMatchesRequest.SetMaxMatchesToFind(100);
            findPendingMatchesRequest.Send(OnFindPendingMatchesRequestSuccess, OnFindPendingMatchesRequestError);
        }
        
    }

    public void OnFindPendingMatchesRequestSuccess(FindPendingMatchesResponse response)
    {
        //Debug.Log("Matches found: " + response.ScriptData.BaseData["matchesFound"]);
        if (hosting)
        {
            string hostDisplayName = gameSparksUserID.myDisplayName;
            string matchID = "";
            string matchShortCode = "";
            AddHostedGameToLobbies(hostDisplayName, matchID, matchShortCode);
        }
        var pendingMatches = response.PendingMatches.GetEnumerator();
        bool endOfMatches = !pendingMatches.MoveNext();
        while (!endOfMatches)
        {
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
        UnblockRefreshInput();

        if (hostedGames.Count == 0)
        {
            // No pending matches found
            Debug.Log("No Pending Matches found");
            noHostedGamesPanel.SetActive(true);
        }
        else
        {
            noHostedGamesPanel.SetActive(false);
        }
        refreshingLock = false;
    }

    public void OnFindPendingMatchesRequestError(FindPendingMatchesResponse response)
    {
        Debug.Log("Find Matches Error: " + response.Errors.JSON);
        UnblockRefreshInput();
        refreshingLock = false;
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

    private void BlockMatchmakingButton()
    {
        matchmakingButton.interactable = false;
        findingMatchText.color = new Color(0, 0, 0, 0);
        Destroy(matchmakingButton.GetComponentInChildren<ButtonFontTMP>());
    }
    private void UnblockMatchmakingButton()
    {
        matchmakingButton.interactable = true;
        matchmakingButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().gameObject.AddComponent<ButtonFontTMP>();
    }

    private void OnJoinGameButtonClick()
    {
        //hostGameButton.interactable = false;
        //BlockMatchmakingButton();
    }

    public void OnHostGameButtonClick()
    {
        BlockMatchmakingButton();
        joinGameButton.interactable = false;
        EventSystem.current.SetSelectedGameObject(null);

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
            var buttonText = hostGameButton.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            buttonText[0].text = "Cancel Host";
        }
        else
        {
            // Cancel host
            hosting = false;
            matchmakingRequest.SetAction("cancel");
            matchmakingRequest.Send(OnMatchmakingSuccess, OnMatchmakingError);
            // Change button text to represent hosting a game
            var buttonText = hostGameButton.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            buttonText[0].text = "Host Game";
            joinGameButton.interactable = true;
            onRefreshGamesButtonClick();
            UnblockMatchmakingButton();
        }

        matchmakingRequest.Send(OnMatchmakingSuccess, OnMatchmakingError);
    }

    public void CancelHosting()
    {
        if (hosting)
        {
            MatchmakingRequest matchmakingRequest = new MatchmakingRequest();
            matchmakingRequest.SetMatchShortCode("HostedMatch");

            GameSparks.Core.GSRequestData participantData = new GameSparks.Core.GSRequestData();
            participantData.AddString("displayName", gameSparksUserID.myDisplayName);
            participantData.AddBoolean("hosting", true);
            matchmakingRequest.SetParticipantData(participantData);
            matchmakingRequest.SetMatchData(participantData);
            matchmakingRequest.SetSkill(0);
            // Cancel host
            hosting = false;
            matchmakingRequest.SetAction("cancel");
            matchmakingRequest.Send(OnMatchmakingSuccess, OnMatchmakingError);
            // Change button text to represent hosting a game
            var buttonText = hostGameButton.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            buttonText[0].text = "Host Game";
            joinGameButton.interactable = true;
            onRefreshGamesButtonClick();
            UnblockMatchmakingButton();
        }
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
        var hostedGameLobbyText = hostedGameLobby.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        TMPro.TextMeshProUGUI joinText = hostedGameLobbyText[0];
        TMPro.TextMeshProUGUI playerText = hostedGameLobbyText[1];
        //Text messageText = hostedGameLobbyText[1];
        if (hostName == gameSparksUserID.myDisplayName)
        {
            if (hostName.Length >= 10)
            {
                playerText.text = (hostName.Substring(0, 10) + ("(You)"));
            }
            else
            {
                playerText.text = (hostName) + ("(You)");
            }
            Destroy(hostedGameLobby.GetComponent<WholeButtonFontTMP>());
            Destroy(hostedGameLobby.GetComponent<HostedGameLobbyButton>());
        }
        else
        {
            if (hostName.Length >= 10)
            {
                playerText.text = (hostName.Substring(0, 10));
            }
            else
            {
                playerText.text = (hostName);
            }
        }
        //playerText.text = ("<b>" + messageWho + ":</b>");
        //messageText.text = (messageMessage);

        hostedGameLobby.transform.SetParent(hostedGameLobbiesRectTransform);
        hostedGameLobby.transform.localScale = new Vector3(1, 1, 1);

        hostedGames.Add(hostedGameLobby);

        hostedGameLobbiesRectTransform.sizeDelta = new Vector2(hostedGameLobbiesRectTransform.sizeDelta.x, hostedGames.Count * heightOfHostedGame);

        LayoutRebuilder.ForceRebuildLayoutImmediate(hostedGameLobbiesRectTransform);
    }

    public void MoveContentPane(float value)
    {
        // Adjust dimensions of rightMostBoundaries
        heightOfAllGames = (hostedGameLobbiesRectTransform.GetComponentsInChildren<Button>().Length * heightOfHostedGame);
        if (heightOfAllGames > 300)
        {
            lowerBoundary.localPosition = new Vector2((upperBoundary.localPosition.x),
                                upperBoundary.localPosition.y + (heightOfAllGames - 1200));

            hostedGameLobbiesRectTransform.localPosition = new Vector2(hostedGameLobbiesRectTransform.localPosition.x,
                                                            hostedGameLobbiesRectTransform.localPosition.y + value);

            if (hostedGameLobbiesRectTransform.localPosition.y <= upperBoundary.localPosition.y)
            {
                hostedGameLobbiesRectTransform.localPosition = upperBoundary.localPosition;

                scrollUpButton.gameObject.SetActive(false);
                scrollDownButton.gameObject.SetActive(true);
            }
            else if (hostedGameLobbiesRectTransform.localPosition.y >= lowerBoundary.localPosition.y)
            {
                hostedGameLobbiesRectTransform.localPosition = lowerBoundary.localPosition;
                scrollDownButton.gameObject.SetActive(false);
                scrollUpButton.gameObject.SetActive(true);
            }
            else
            {
                scrollDownButton.gameObject.SetActive(true);
                scrollUpButton.gameObject.SetActive(true);
            }
        }
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
