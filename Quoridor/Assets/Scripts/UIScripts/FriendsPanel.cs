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

    }

    private void SwitchFriendsListToOffline()
    {

    }

    private void SwitchFriendsListToRequests()
    {

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
        var friendsListData = response.ScriptData.GetGSDataList("friendsList");
        UpdateFriendsListUI(friendsListData);
    }

    private void UpdateFriendsListUI(List<GSData> friendsListData)
    {
        var friendsListEnumerator = friendsListData.GetEnumerator();
        while (friendsListEnumerator.MoveNext())
        {
            var friendBaseData = friendsListEnumerator.Current.BaseData;
            string playerName = friendBaseData["displayName"].ToString();
            // Create new (gamelobby) Button to add to children of HostedGameLobbies
            GameObject playerObject = Instantiate(friendResultButtonPrefab) as GameObject;

            // Get text component of button
            UnityEngine.UI.Text[] playerObjectTexts = playerObject.GetComponentsInChildren<Text>();
            Text playerText = playerObjectTexts[0];
            Text winsText = playerObjectTexts[1];

            if (playerName.Length >= 20)
            {
                playerText.text = (playerName.Substring(0, 20));
            }
            else
            {
                playerText.text = (playerName);
            }

            playerObject.transform.SetParent(friendsListContent);
            playerObject.transform.localScale = new Vector3(1, 1, 1);

            friendsList.Add(playerObject);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(friendsListContent);
    }
}
