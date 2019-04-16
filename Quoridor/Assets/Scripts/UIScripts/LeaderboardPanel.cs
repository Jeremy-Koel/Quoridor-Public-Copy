using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Api;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using System;

public class LeaderboardPanel : MonoBehaviour
{
    private GameObject leaderboardView;
    public RectTransform leaderboardContent;
    public VerticalLayoutGroup playersListLayoutGroup;
    public List<GameObject> playersList;
    public GameObject playerPrefab;
    public ChallengeManager challengeManager;
    public int heightOfLeaderboardPlayer = 75;
    [SerializeField]
    public RectTransform upperBoundary;
    [SerializeField]
    public RectTransform lowerBoundary;
    [SerializeField]
    public Button scrollUpButton;
    [SerializeField]
    public Button scrollDownButton;
    public float heightOfAllPlayers;
    public ScrollRect scrollbar;
    public Button refreshLeaderboardButton;

    private void OnDestroy()
    {
        challengeManager = null;
    }

    private void Awake()
    {

        leaderboardView = GameObject.Find("LeaderboardListViewport");
        leaderboardContent = GameObject.Find("LeaderboardListContent").GetComponent<RectTransform>();
        playersListLayoutGroup = GameObject.Find("LeaderboardListContent").GetComponent<VerticalLayoutGroup>();
        playersList = new List<GameObject>();
        challengeManager = GameObject.Find("ChallengeManager").GetComponent<ChallengeManager>();
        refreshLeaderboardButton = GameObject.Find("RefreshLeaderboardListButton").GetComponent<Button>();
        //refreshLeaderboardButton.onClick.AddListener(onRefreshLeaderboardButtonClick);
        scrollUpButton = GameObject.Find("LeaderboardScrollUp").GetComponent<Button>();
        //scrollUpButton.onClick.AddListener(MoveContentPane);
        scrollDownButton = GameObject.Find("LeaderboardScrollDown").GetComponent<Button>();
        //scrollDownButton.onClick.AddListener(MoveContentPane);
        scrollbar = GameObject.Find("LeaderboardList").GetComponent<ScrollRect>();
        scrollbar.onValueChanged.AddListener(OnValueChange);

        GameObject upMostBoundsObject = new GameObject("upMostBoundariesObject", typeof(RectTransform));
        upperBoundary = upMostBoundsObject.GetComponent<RectTransform>();
        upperBoundary.localPosition = new Vector2((leaderboardContent.localPosition.x),
                                               (leaderboardContent.localPosition.y));

        GameObject lowMostBoundsObject = new GameObject("lowMostBoundariesObject", typeof(RectTransform));
        lowerBoundary = lowMostBoundsObject.GetComponent<RectTransform>();
        lowerBoundary.localPosition = new Vector2((leaderboardContent.localPosition.x),
                                               ( leaderboardContent.localPosition.y + heightOfLeaderboardPlayer));
    }

    // Start is called before the first frame update
    void Start()
    {
        GetLeaderboardData();
        scrollUpButton.gameObject.SetActive(false);
        scrollDownButton.gameObject.SetActive(false);
    }

    void OnValueChange(Vector2 position)
    {
        MoveContentPane(0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetLeaderboardData()
    {
        RemoveLeaderboardData();
        LeaderboardDataRequest_HighScoreLB leaderboardDataRequest = new LeaderboardDataRequest_HighScoreLB();
        //LeaderboardDataRequest leaderboardDataRequest = new LeaderboardDataRequest();
        //leaderboardDataRequest.SetLeaderboardShortCode("HighScoreLB");
        leaderboardDataRequest.SetEntryCount(100);
        leaderboardDataRequest.Send(OnleaderboardDataRequestSuccess, OnleaderboardDataRequestError);
    }

    private void OnleaderboardDataRequestSuccess(LeaderboardDataResponse_HighScoreLB response)
    {
        AddLeaderboardData(response);
    }

    private void OnleaderboardDataRequestError(LeaderboardDataResponse_HighScoreLB response)
    {
        
    }

    public void onRefreshLeaderboardButtonClick()
    {
        BlockRefreshInput();
        GetLeaderboardData();
    }

    private void BlockRefreshInput()
    {
        if (refreshLeaderboardButton != null)
        {
            refreshLeaderboardButton.enabled = false;
        }
    }

    private void UnblockRefreshInput()
    {
        if (refreshLeaderboardButton != null)
        {
            refreshLeaderboardButton.enabled = true;
        }
    }

    public void MoveContentPane(float value)
    {
        // Adjust dimensions of rightMostBoundaries
        heightOfAllPlayers = (leaderboardContent.GetComponentsInChildren<Button>().Length * heightOfLeaderboardPlayer);
        if (heightOfAllPlayers > 480)
        {
            lowerBoundary.localPosition = new Vector2((upperBoundary.localPosition.x),
                                upperBoundary.localPosition.y + (heightOfAllPlayers - 1200));

            leaderboardContent.localPosition = new Vector2(leaderboardContent.localPosition.x,
                                                            leaderboardContent.localPosition.y + value);

            if (leaderboardContent.localPosition.y <= upperBoundary.localPosition.y + 10)
            {
                leaderboardContent.localPosition = upperBoundary.localPosition;
                
                scrollUpButton.gameObject.SetActive(false);
                scrollDownButton.gameObject.SetActive(true);
            }
            else if (leaderboardContent.localPosition.y >= lowerBoundary.localPosition.y - 10)
            {
                leaderboardContent.localPosition = lowerBoundary.localPosition;
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

    // Add leaderboard data to leaderboard list in UI
    void AddLeaderboardData(LeaderboardDataResponse_HighScoreLB response)
    {
        var leaderboardData = response.Data.GetEnumerator();
        while (leaderboardData.MoveNext())
        {
            var leaderboardBaseData = leaderboardData.Current.BaseData;
            string playerName = leaderboardBaseData.GetString("SUPPLEMENTAL-playerName");
            string playerWins = leaderboardBaseData.GetNumber("MAX-playerAttr").ToString();
            // Create new (gamelobby) Button to add to children of HostedGameLobbies
            GameObject playerObject = Instantiate(playerPrefab) as GameObject;

            // Get text component of button
            var playerObjectTexts = playerObject.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI playerText = playerObjectTexts[0];
            TMPro.TextMeshProUGUI winsText = playerObjectTexts[1];
            
            if (playerName.Length >= 20)
            {
                playerText.text = (playerName.Substring(0, 17) + "...");
            }
            else
            {
                playerText.text = (playerName);
            }

            winsText.text = (playerWins);

            playerObject.transform.SetParent(leaderboardContent);
            playerObject.transform.localScale = new Vector3(1, 1, 1);

            playersList.Add(playerObject);
        }
        var leaderBoardListContent = GameObject.Find("LeaderboardListContent").GetComponent<RectTransform>();
        leaderBoardListContent.sizeDelta = new Vector2 (leaderBoardListContent.sizeDelta.x, playersList.Count * heightOfLeaderboardPlayer);
        LayoutRebuilder.ForceRebuildLayoutImmediate(leaderboardContent);
    }

    void RemoveLeaderboardData()
    {
        foreach (RectTransform child in leaderboardContent)
        {
            GameObject.Destroy(child.gameObject);
        }
        playersList.Clear();
    }
}
