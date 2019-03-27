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

    public Button refreshLeaderboardButton;

    private void Awake()
    {
        leaderboardView = GameObject.Find("LeaderboardListViewport");
        leaderboardContent = GameObject.Find("LeaderboardListContent").GetComponent<RectTransform>();
        playersListLayoutGroup = GameObject.Find("LeaderboardListContent").GetComponent<VerticalLayoutGroup>();
        playersList = new List<GameObject>();
        challengeManager = GameObject.Find("ChallengeManager").GetComponent<ChallengeManager>();
        refreshLeaderboardButton = GameObject.Find("RefreshLeaderboardList").GetComponent<Button>();
        refreshLeaderboardButton.onClick.AddListener(onRefreshLeaderboardButtonClick);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetLeaderboardData()
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
            UnityEngine.UI.Text[] playerObjectTexts = playerObject.GetComponentsInChildren<Text>();
            Text playerText = playerObjectTexts[0];
            Text winsText = playerObjectTexts[1];

            // MOVE PLAYER NAME AND WINS LABELING TO ABOVE THE LEADERBOARD INSTEAD OF INSIDE OF EVERY OBJECT - NK

            if (playerName.Length >= 10)
            {
                playerText.text = ("<b>Player Name: </b>" + playerName.Substring(0, 10));
            }
            else
            {
                playerText.text = ("<b>Player Name: </b>" + playerName);
            }
            // Adding spacing for now to avoid making changes to unity scene - NK
            winsText.text = ("                              <b>Wins: </b>" + playerWins);

            playerObject.transform.SetParent(leaderboardContent);
            playerObject.transform.localScale = new Vector3(1, 1, 1);

            playersList.Add(playerObject);
        }

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
