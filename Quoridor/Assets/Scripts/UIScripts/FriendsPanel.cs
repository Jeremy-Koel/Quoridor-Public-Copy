using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Api;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;

public class FriendsPanel : MonoBehaviour
{
    Button onlineFriendsButton;
    Button offlineFriendsButton;
    Button friendRequestsButton;
    Button addFriendsButton;
    GameObject addFriendsPanel;
    GameObject chatWindowPanel;
    GameObject chatSelectionPanel;
    RectTransform friendsListContent;
    public VerticalLayoutGroup friendsListLayoutGroup;
    private GameObject friendsListView;
    public GameObject friendResultButtonPrefab;
    public List<GameObject> friendsList;

    private void Awake()
    {
        onlineFriendsButton = GameObject.Find("OnlineFriendsButton").GetComponent<Button>();
        offlineFriendsButton = GameObject.Find("OfflineFriendsButton").GetComponent<Button>();
        friendRequestsButton = GameObject.Find("FriendRequestsButton").GetComponent<Button>();
        addFriendsButton = GameObject.Find("AddFriendsButton").GetComponent<Button>();
        addFriendsPanel = GameObject.Find("AddFriendsPanel");
        chatWindowPanel = GameObject.Find("ChatWindowPanel");
        chatSelectionPanel = GameObject.Find("ChatSelectionPanel");
        friendsListView = GameObject.Find("FriendsListViewport");
        friendsListContent = GameObject.Find("FriendsListContent").GetComponent<RectTransform>();
        friendsListLayoutGroup = friendsListContent.GetComponent<VerticalLayoutGroup>();
        friendsList = new List<GameObject>();
        // We don't want the addFriendsPanel active at the start
        SwitchActiveAddFriendsPanel();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Add on clicks
        onlineFriendsButton.onClick.AddListener(SwitchFriendsListToOnline);
        offlineFriendsButton.onClick.AddListener(SwitchFriendsListToOffline);
        friendRequestsButton.onClick.AddListener(SwitchFriendsListToRequests);
        addFriendsButton.onClick.AddListener(SwitchToAddfriends);
        // Call starting point function
        SwitchFriendsListToOnline();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SwitchActiveAddFriendsPanel()
    {
        addFriendsPanel.SetActive(!addFriendsPanel.activeSelf);
    }

    private void SwitchFriendsListToOnline()
    {
        onlineFriendsButton.interactable = false;
        // Switch all other buttons to interactable
        offlineFriendsButton.interactable = true;
        friendRequestsButton.interactable = true;
        addFriendsButton.interactable = true;
        // Get my friends list
        GetFriendsList("online");
    }

    private void SwitchFriendsListToOffline()
    {
        offlineFriendsButton.interactable = false;
        // Switch all other buttons to interactable
        onlineFriendsButton.interactable = true;
        friendRequestsButton.interactable = true;
        addFriendsButton.interactable = true;
        // Get my friends list
        GetFriendsList("offline");
    }

    private void SwitchFriendsListToRequests()
    {
        friendRequestsButton.interactable = false;
        // Switch all other buttons to interactable
        onlineFriendsButton.interactable = true;
        offlineFriendsButton.interactable = true;
        addFriendsButton.interactable = true;
    }

    private void SwitchToAddfriends()
    {
        addFriendsButton.interactable = false;
        // Switch all other buttons to interactable
        onlineFriendsButton.interactable = true;
        offlineFriendsButton.interactable = true;
        friendRequestsButton.interactable = true;
    }

    // Friends group determines which group of friends to search for (online/offline)
    private void GetFriendsList(string friendsGroup)
    {
        // For now ignore the group choice
        friendsGroup = "all";

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
        var friendsListEnumerator = friendsListData.GetEnumerator();
        while (friendsListEnumerator.MoveNext())
        {
            // The player name is burried deep in key/value pair-like structures
            var friendBaseData = (KeyValuePair<string, object>)friendsListEnumerator.Current;
            var friendActualBaseData = (GameSparks.Core.GSData)friendBaseData.Value;
            var friendPlayerIDBaseData = friendActualBaseData.BaseData.GetEnumerator();
            friendPlayerIDBaseData.MoveNext();
            var friendPlayerIDActualBaseData = (GameSparks.Core.GSData)friendPlayerIDBaseData.Current.Value;
            var friendPlayerNameBaseData = friendPlayerIDActualBaseData.BaseData.GetEnumerator();
            friendPlayerNameBaseData.MoveNext();
            string playerName = friendPlayerNameBaseData.Current.Value.ToString();
            
            GameObject friendObject = Instantiate(friendResultButtonPrefab) as GameObject;

            // Get text component of button
            UnityEngine.UI.Text[] friendObjectTexts = friendObject.GetComponentsInChildren<Text>();
            Text friendNameText = friendObjectTexts[0];

            if (playerName.Length >= 20)
            {
                friendNameText.text = (playerName.Substring(0, 20));
            }
            else
            {
                friendNameText.text = (playerName);
            }

            friendObject.transform.SetParent(friendsListContent);
            friendObject.transform.localScale = new Vector3(1, 1, 1);

            friendsList.Add(friendObject);
        }

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
}
