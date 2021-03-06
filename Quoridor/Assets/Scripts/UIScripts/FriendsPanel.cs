﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Api;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using System;

public class FriendsPanel : MonoBehaviour
{
    TMPro.TMP_InputField searchFriendsInput;
    Button onlineFriendsButton;
    Button offlineFriendsButton;
    Button friendRequestsButton;
    Button addFriendsButton;
    Button searchFriendsButton;
    GameObject addFriendsPanel;
    public GameObject chatWindowPanel;
    GameObject chatSelectionPanel;
    RectTransform friendsListContent;
    public VerticalLayoutGroup friendsListLayoutGroup;
    private Image friendsPanelUIObjectImage;
    private GameObject friendsListView;
    private GameObject friendsListViewPort;
    public GameObject friendResultButtonPrefab;
    public List<GameObject> friendsList;
    public List<GameObject> friendsSearchResultList;
    private RectTransform friendsSearchResultListContent;
    private VerticalLayoutGroup friendsSearchResultLayoutGroup;
    private GameObject friendsSearchResultListView;
    //private GameObject friendsSearchResultListViewPort;

    private bool pending;
    private bool shakeInput = false;
    private Timer shakeTimer;
    private float shakeDuration = 0.04f;
    private float shakeDefault = 0.04f;
    private int heightOfFriendsResult = 75;

    private string currentActiveFriendsPanel = "";
    private const string friendRequestsPanelString = "friendRequestsPanel";
    private const string onlineFriendsPanelString = "onlineFriendsPanel";
    private const string addFriendsPanelString = "addFriendsPanel";

    private bool searchLock = false;
    private Timer searchLockTimer;

    private void Awake()
    {
        searchFriendsInput = GameObject.Find("SearchFriendsInput").GetComponent<TMPro.TMP_InputField>();
        onlineFriendsButton = GameObject.Find("OnlineFriendsButton").GetComponent<Button>();
        //offlineFriendsButton = GameObject.Find("OfflineFriendsButton").GetComponent<Button>();
        friendRequestsButton = GameObject.Find("FriendRequestsButton").GetComponent<Button>();
        addFriendsButton = GameObject.Find("AddFriendsButton").GetComponent<Button>();
        searchFriendsButton = GameObject.Find("SearchFriendsButton").GetComponent<Button>();
        addFriendsPanel = GameObject.Find("AddFriendsPanel");
        chatWindowPanel = GameObject.Find("ChatWindowPanel");
        chatSelectionPanel = GameObject.Find("ChatSelectionPanel");
        friendsPanelUIObjectImage = GameObject.Find("FriendsPanel").GetComponent<Image>();
        friendsListView = GameObject.Find("FriendsListView");
        friendsListViewPort = GameObject.Find("FriendsListViewport");
        friendsListContent = GameObject.Find("FriendsListContent").GetComponent<RectTransform>();
        friendsListLayoutGroup = friendsListContent.GetComponent<VerticalLayoutGroup>();
        friendsList = new List<GameObject>();
        friendsSearchResultList = new List<GameObject>();
        friendsSearchResultListContent = GameObject.Find("AddFriendsContent").GetComponent<RectTransform>();
        friendsSearchResultLayoutGroup = GameObject.Find("AddFriendsContent").GetComponent<VerticalLayoutGroup>();
        friendsSearchResultListView = GameObject.Find("AddFriendsView");
        //friendsSearchResultListViewPort = GameObject.Find("");
        pending = false;

        searchLockTimer = gameObject.AddComponent<Timer>();
        searchLockTimer.SetTimeDefault(0.5f);
        searchLockTimer.ResetTimer();
        searchLockTimer.timeUp.AddListener(ClearSearchLock);
        searchLockTimer.StartCountdown();

        // We don't want the addFriendsPanel active at the start
        addFriendsPanel.SetActive(false);

        ScriptMessage_FriendRequest.Listener += GotAFriendRequest;
        ScriptMessage_AcceptedFriendRequest.Listener += GotANewFriend;
    }

    private void GotANewFriend(ScriptMessage_AcceptedFriendRequest message)
    {
        // If we're looking at our current friends list
        if (currentActiveFriendsPanel == onlineFriendsPanelString)
        {
            // Refresh friend requests list
            GetFriendsList("all");
        }
    }

    private void GotAFriendRequest(ScriptMessage_FriendRequest message)
    {
        // If we're looking at our current friend requests
        if (currentActiveFriendsPanel == friendRequestsPanelString)
        {
            // Refresh friend requests list
            GetPendingFriendsList();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Add on clicks
        onlineFriendsButton.onClick.AddListener(SwitchFriendsListToOnline);
        //offlineFriendsButton.onClick.AddListener(SwitchFriendsListToOffline);
        friendRequestsButton.onClick.AddListener(SwitchFriendsListToRequests);
        addFriendsButton.onClick.AddListener(SwitchToAddfriends);
        searchFriendsButton.onClick.AddListener(SearchForFriendsToAdd);
        // Call starting point function
        SwitchFriendsListToOnline();
    }

    private void OnDestroy()
    {
        onlineFriendsButton.onClick.RemoveAllListeners();
        //offlineFriendsButton.onClick.AddListener(SwitchFriendsListToOffline);
        friendRequestsButton.onClick.RemoveAllListeners();
        addFriendsButton.onClick.RemoveAllListeners();
        searchFriendsButton.onClick.RemoveAllListeners();
        // Call starting point function
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Submit") == 1)
        {
            if (searchFriendsInput.gameObject.activeSelf)
            {
                SearchForFriendsToAdd();
            }
        }
    }

    private void ClearSearchLock()
    {
        searchLock = false;
        searchLockTimer.ResetTimer();
        searchLockTimer.StartCountdown();
    }


    private void SwitchActiveAddFriendsPanel()
    {
        if (addFriendsPanel.activeSelf)
        {
            currentActiveFriendsPanel = addFriendsPanelString;

            // Make friends list panel invisible
            friendsPanelUIObjectImage.enabled = false;
            friendsListView.SetActive(false);

            // Make chat invisible
            chatWindowPanel.SetActive(false);
            chatSelectionPanel.SetActive(false);
        }
        else
        {
            // Make friends list panel visible
            friendsPanelUIObjectImage.enabled = true;
            friendsListView.SetActive(true);

            // Make chat visible
            chatWindowPanel.SetActive(true);
            chatSelectionPanel.SetActive(true);
        }
        if (shakeTimer == null)
        {
            shakeTimer = gameObject.AddComponent<Timer>();
            shakeTimer.timeUp.AddListener(TimeLimitReached);
            shakeTimer.SetTimeDefault(shakeDefault);
        } 
        searchFriendsInput.gameObject.GetComponent<Animator>().SetBool("Shaking", true);
        shakeTimer.StartCountdown();
    }

    void TimeLimitReached()
    {
        //shakeInput = false;
        searchFriendsInput.gameObject.GetComponent<Animator>().SetBool("Shaking", false);
        shakeTimer.CancelCountdown();
    }

    private void SwitchFriendsListToOnline()
    {
        // Switch all other buttons to interactable
        SwitchFriendsPanelButtons();
        currentActiveFriendsPanel = onlineFriendsPanelString;

        onlineFriendsButton.interactable = false;

        addFriendsPanel.SetActive(false);
        SwitchActiveAddFriendsPanel();

        // Get my friends list
        GetFriendsList("online");
    }

    private void SwitchFriendsListToOffline()
    {
        // Switch all other buttons to interactable
        SwitchFriendsPanelButtons();

        //offlineFriendsButton.interactable = false;

        addFriendsPanel.SetActive(false);
        SwitchActiveAddFriendsPanel();

        // Get my friends list
        GetFriendsList("offline");
    }

    private void SwitchFriendsListToRequests()
    {
        // Switch all other buttons to interactable
        SwitchFriendsPanelButtons();
        currentActiveFriendsPanel = friendRequestsPanelString;

        friendRequestsButton.interactable = false;

        addFriendsPanel.SetActive(false);
        SwitchActiveAddFriendsPanel();

        // Get my pending friend requests
        GetPendingFriendsList();
    }

    private void SwitchToAddfriends()
    {
        // Switch all other buttons to interactable
        SwitchFriendsPanelButtons();
        addFriendsButton.interactable = false;

        addFriendsPanel.SetActive(true);

        // Get component and set name
        string name = GameObject.Find("GameSparksUserID").GetComponent<GameSparksUserID>().myDisplayName;
        GameObject.Find("AddFriendsYourNameText").GetComponent<TMPro.TextMeshProUGUI>().text = name;

        SwitchActiveAddFriendsPanel();
    }

    private void SwitchFriendsPanelButtons()
    {
        addFriendsButton.interactable = true;        
        onlineFriendsButton.interactable = true;
        //offlineFriendsButton.interactable = true;
        friendRequestsButton.interactable = true;
    }

    private void SearchForFriendsToAdd()
    {
        if (!searchLock)
        {
            searchLock = true;
            LogEventRequest_FindPlayers findPlayersRequest = new LogEventRequest_FindPlayers();
            findPlayersRequest.Set_displayName(searchFriendsInput.text);
            findPlayersRequest.Send(SearchForFriendsResponse);
        }
    }

    private void SearchForFriendsResponse(LogEventResponse response)
    {
        ClearSearchForFriendsList();
        var friendsSearchListData = response.ScriptData.BaseData;
        UpdateSearchForFriendsUI(friendsSearchListData);
    }

    private void UpdateSearchForFriendsUI(IDictionary<string, object> friendsSearchListData)
    {
        var friendsSearchListDataEnumerator = friendsSearchListData.GetEnumerator();
        friendsSearchListDataEnumerator.MoveNext();
        var playerListData = (List<object>)friendsSearchListDataEnumerator.Current.Value;
        var playerListDataEnumerator = playerListData.GetEnumerator();
        while (playerListDataEnumerator.MoveNext())
        {
            var playerData = playerListDataEnumerator.Current;
            var playerDataDictionary = (IDictionary<string, object>)playerData;
            var playerDataEnumerator = playerDataDictionary.GetEnumerator();

            playerDataEnumerator.MoveNext();
            string playerName = playerDataEnumerator.Current.Value.ToString();

            if (playerName.Length >= 20)
            {
                playerName = (playerName.Substring(0, 17) + "...");
            }

            playerDataEnumerator.MoveNext();
            playerDataEnumerator.MoveNext();
            string playerID = playerDataEnumerator.Current.Value.ToString();
            //playerName = playerName + "+" + playerID.Substring(20, 4);

            GameObject friendResultButton = Instantiate(friendResultButtonPrefab) as GameObject;
            FriendResult friendResultScript = friendResultButton.GetComponent<FriendResult>();
            friendResultScript.playerID = playerID;
            friendResultButton.GetComponent<Button>().onClick.AddListener(friendResultScript.OnClickSendFriendRequest);
            friendResultButton.name = playerID + "search";
            // Get text component of button
            UnityEngine.UI.Text[] playerObjectTexts = friendResultButton.GetComponentsInChildren<Text>();
            Text playerText = playerObjectTexts[0];

            playerText.text = playerName;

            friendResultButton.transform.SetParent(friendsSearchResultListContent);
            friendResultButton.transform.localScale = new Vector3(1, 1, 1);

            friendsSearchResultList.Add(friendResultButton);

        }
        friendsSearchResultListContent.sizeDelta = new Vector2(friendsSearchResultListContent.sizeDelta.x, friendsSearchResultList.Count * heightOfFriendsResult);
        LayoutRebuilder.ForceRebuildLayoutImmediate(friendsSearchResultListContent);
        searchLock = false;
    }

    // Get player's pending friend requests
    public void GetPendingFriendsList()
    {
        pending = true;
        LogEventRequest_GetFriendRequests getFriendsListRequest = new LogEventRequest_GetFriendRequests();
        getFriendsListRequest.Send(GetFriendsListResponse);
    }

    // Friends group determines which group of friends to search for (online/offline)
    private void GetFriendsList(string friendsGroup)
    {
        // For now ignore the group choice
        friendsGroup = "all";
        pending = false;
        LogEventRequest_GetFriendsList getFriendsListRequest = new LogEventRequest_GetFriendsList();
        getFriendsListRequest.Set_group(friendsGroup);
        getFriendsListRequest.Send(GetFriendsListResponse);
    }

    private void GetFriendsListResponse(LogEventResponse response)
    {
        ClearFriendsList();
        var friendsListData = response.ScriptData.BaseData;
        UpdateFriendsListUI(friendsListData);
    }

    private void UpdateFriendsListUI(IDictionary<string, object> friendsListData)
    {
        // The player name is burried deep in key/value pair-like structures
        var friendsListEnumerator = friendsListData.GetEnumerator();
        friendsListEnumerator.MoveNext();
        var friendBaseData = (KeyValuePair<string, object>)friendsListEnumerator.Current;
        var friendActualBaseData = (GameSparks.Core.GSData)friendBaseData.Value;
        var friendPlayerIDBaseData = friendActualBaseData.BaseData.GetEnumerator();
        bool toDestroy = false;
        while (friendPlayerIDBaseData.MoveNext())
        {
            string playerID = friendPlayerIDBaseData.Current.Key;
            var friendPlayerIDActualBaseData = (GameSparks.Core.GSData)friendPlayerIDBaseData.Current.Value;
            var friendPlayerNameBaseData = friendPlayerIDActualBaseData.BaseData.GetEnumerator();
            friendPlayerNameBaseData.MoveNext();
            string playerName = friendPlayerNameBaseData.Current.Value.ToString();

            GameObject friendObject = Instantiate(friendResultButtonPrefab) as GameObject;
            FriendResult friendResultScript = friendObject.GetComponent<FriendResult>();

            friendResultScript.playerName = playerName;

            if (pending)
            {
                friendResultScript.playerID = playerID;
                friendObject.GetComponent<Button>().onClick.AddListener(friendResultScript.OnClickAcceptFriendRequest);
                friendObject.name = playerID + "pending";
            }
            else
            {
                friendObject.GetComponent<Button>().onClick.AddListener(friendResultScript.OnClickOpenTeamChat);
                friendResultScript.chatWindowPanel = chatWindowPanel.GetComponent<ChatWindowPanel>();
                if (friendPlayerNameBaseData.MoveNext())
                {
                    string teamID = friendPlayerNameBaseData.Current.Value.ToString();
                    friendResultScript.teamID = teamID;
                }
                else
                {
                    toDestroy = true;
                }
            }
                    
            // Get text component of button
            UnityEngine.UI.Text[] friendObjectTexts = friendObject.GetComponentsInChildren<Text>();
            Text friendNameText = friendObjectTexts[0];

            if (playerName.Length >= 20)
            {
                friendNameText.text = (playerName.Substring(0, 17) + "...");
            }
            else
            {
                friendNameText.text = (playerName);
            }

            friendObject.transform.SetParent(friendsListContent);
            friendObject.transform.localScale = new Vector3(1, 1, 1);
            friendsListContent.sizeDelta = new Vector2(friendsListContent.sizeDelta.x, (friendsListContent.sizeDelta.y + 30));

            if (toDestroy)
            {
                toDestroy = false;
                Destroy(friendObject);
            }
            friendsList.Add(friendObject);
        }
        pending = false;

        LayoutRebuilder.ForceRebuildLayoutImmediate(friendsListContent);
    }

    private void ClearFriendsList()
    {
        foreach (RectTransform child in friendsListContent)
        {
            GameObject.Destroy(child.gameObject);
        }
        friendsList.Clear();
    }

    private void ClearSearchForFriendsList()
    {
        foreach (RectTransform child in friendsSearchResultListContent)
        {
            GameObject.Destroy(child.gameObject);
        }
        friendsSearchResultList.Clear();
    }
}
