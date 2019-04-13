using GameSparks.Api;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChatWindowPanel : MonoBehaviour
{
    private GSEnumerable<GetMyTeamsResponse._Team> teams = null;
    [SerializeField]
    public ChatSelectionPanel chatSelectionPanel;
    [SerializeField]
    public GameObject chatInput;
    [SerializeField]
    public GameObject globalChatButtonObject;
    [SerializeField]
    private GameObject chatMessagesView;
    [SerializeField]
    public RectTransform chatMessagesViewContent;
    [SerializeField]
    public GameObject chatMessagesBox;
    [SerializeField]
    public VerticalLayoutGroup chatMessagesLayoutGroup;
    [SerializeField]
    public List<GameObject> chatMessages;
    [SerializeField]
    public GameObject lobbyMessagePrefab;
    [SerializeField]
    public GameObject inGameMessagePrefab;
    [SerializeField]
    public GameObject friendChatMessagesBoxPrefab;
    [SerializeField]
    public ChallengeManager challengeManager;
    [SerializeField]
    public GameObject pdaFlash;
    // A list of each list of a friend's chat messages
    [SerializeField]
    private List<List<GameObject>> listOfChatMessages;
    // List of teamIDs for reference (we only need one instance of each teamID)
    [SerializeField]
    private List<string> teamIDs;
    // List of friendsChatMessagesRectTransforms
    [SerializeField]
    public List<RectTransform> listOfFriendsMessagesContents;

    public GameSparksUserID gameSparksUserIDScript;

    private string currentTeamID = "0";
    
    private Timer flashTimer;

    private void Awake()
    {
        //pdaFlash = GameObject.Find("PDA Flash");
        
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            globalChatButtonObject = GameObject.Find("GlobalChatButton");
            globalChatButtonObject.GetComponent<Button>().onClick.AddListener(SwitchToGlobalChat);
            chatInput = GameObject.Find("ChatInput");
            chatMessagesView = GameObject.Find("ChatMessagesView");
            var globalMessages = chatMessagesView.GetComponentsInChildren<GlobalMessagesScript>(true);
            var globalMessagesEnum = globalMessages.GetEnumerator();
            globalMessagesEnum.MoveNext();
            chatMessagesBox = ((GlobalMessagesScript)globalMessagesEnum.Current).gameObject;
            chatMessagesViewContent = GameObject.Find("GlobalMessages").GetComponent<RectTransform>();
            chatMessagesLayoutGroup = GameObject.Find("GlobalMessages").GetComponent<VerticalLayoutGroup>();
            TeamChatMessage.Listener += TeamChatMessageRouter;
            ScriptMessage_JoinFriendTeam.Listener += JoinFriendTeam;
            Debug.Log("Name Of ChatMessagesViewContent: " + chatMessagesViewContent.name);
            listOfChatMessages = new List<List<GameObject>>();
            teamIDs = new List<string>();
            listOfFriendsMessagesContents = new List<RectTransform>();
            chatSelectionPanel = GameObject.Find("ChatSelectionPanel").GetComponent<ChatSelectionPanel>();
            gameSparksUserIDScript = GameObject.Find("GameSparksUserID").GetComponent<GameSparksUserID>();
        }
        flashTimer = gameObject.AddComponent<Timer>();
        flashTimer.SetTimeDefault(0.375f);
        flashTimer.ResetTimer();
        flashTimer.timeUp.AddListener(pdaFlashOff);
        pdaFlash.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        listOfChatMessages = new List<List<GameObject>>();
        teamIDs = new List<string>();
        listOfFriendsMessagesContents = new List<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the chat is focused
        //if (chatInput.GetComponent<InputField>().isFocused)
        //{
        // Notice if the CTRL key is pressed
        if (Input.GetAxisRaw("Fire1") == 1)
        {
            // block input
            //int caretPosition = chatInput.GetComponent<InputField>().caretPosition;
            chatInput.GetComponent<TMPro.TMP_InputField>().DeactivateInputField();

            // Notice if V is also pressed
            if (Input.GetAxisRaw("CtrlV") == 1)
            {
                //chatInput.GetComponent<InputField>().caretPosition = caretPosition;
                // get clipboard
                string copiedText = ClipboardHelper.clipBoard;
                List<string> splitText = new List<string>();

                // split clipboard into multiple strings?
                if (copiedText.Length >= 1000)
                {
                    chatInput.GetComponent<TMPro.TMP_InputField>().ActivateInputField();
                    chatInput.GetComponent<TMPro.TMP_InputField>().text = copiedText.Substring(0, 1000);
                    chatInput.GetComponent<TMPro.TMP_InputField>().Select();
                    chatInput.GetComponent<TMPro.TMP_InputField>().MoveTextEnd(false);
                }
                else
                {
                    chatInput.GetComponent<TMPro.TMP_InputField>().ActivateInputField();
                    chatInput.GetComponent<TMPro.TMP_InputField>().text = chatInput.GetComponent<TMPro.TMP_InputField>().text + copiedText;
                    if (chatInput.GetComponent<TMPro.TMP_InputField>().text.Length > 1000)
                    {
                        chatInput.GetComponent<TMPro.TMP_InputField>().text = chatInput.GetComponent<TMPro.TMP_InputField>().text.Substring(0, 1000);
                    }
                    chatInput.GetComponent<TMPro.TMP_InputField>().Select();
                    chatInput.GetComponent<TMPro.TMP_InputField>().MoveTextEnd(false);
                }
                // unblock input

                //chatInput.SetActive(true);
            }
            // Handle CTRL + C
            if (Input.GetAxisRaw("CtrlC") == 1)
            {
                ClipboardHelper.clipBoard = chatInput.GetComponent<TMPro.TMP_InputField>().text;
            }
        }
        //}
 
    }


    public void SwitchToGlobalChat()
    {
        SwitchAllFriendChatOff();
        currentTeamID = "0";
        var globalMessagesContent = chatMessagesBox.GetComponentInChildren<GlobalMessagesScript>(true);
        globalMessagesContent.gameObject.SetActive(true);
        chatSelectionPanel.SetSelectionButtonsInteractive(-1);
    }

    public void SwitchActiveChat(string teamID, string playerName)
    {
        chatMessagesViewContent.gameObject.SetActive(false);
        int teamIDIndex = LookupTeam(teamID, playerName);
        //Predicate<string> predicate = delegate (string toCompare) { return toCompare == teamID; };
        //int teamIDIndex = teamIDs.FindIndex(predicate);
        //if (teamIDIndex == -1)
        //{
        //    // Create stuff
        //    teamIDs.Add(teamID);
        //    teamIDIndex = teamIDs.Count - 1;
        //    ChatMessagesViewContentCreator();
        //    chatSelectionPanel.AddChatSelectionButton(playerName, teamID);
        //}
        int index = 0;
        // Set all friends chats to inactive (aside from the one we want)
        var listOfFriendsMessagesContentsEnum = listOfFriendsMessagesContents.GetEnumerator(); // The list has a null item the second time this function is called after changing scenes
        
        
        while (listOfFriendsMessagesContentsEnum.MoveNext())
        {
            // Let's try a wrecking ball
            if (listOfFriendsMessagesContentsEnum.Current == null)
            {
                FindChatMessagesContent();
            }
            if (index == teamIDIndex)
            {
                listOfFriendsMessagesContentsEnum.Current.parent.gameObject.SetActive(true);
            }
            else
            {
                listOfFriendsMessagesContentsEnum.Current.parent.gameObject.SetActive(false);
            }
            index++;
        }
        chatSelectionPanel.SetSelectionButtonsInteractive(teamIDIndex);
        currentTeamID = teamID;
    }

    public void SwitchAllFriendChatOff()
    {
        var listOfFriendsMessagesContentsEnum = listOfFriendsMessagesContents.GetEnumerator();
        while (listOfFriendsMessagesContentsEnum.MoveNext())
        {
            listOfFriendsMessagesContentsEnum.Current.parent.gameObject.SetActive(false);
        }
    }

    private void ChatMessagesViewContentCreator()
    {
        // Add new list of chat messages to list
        List<GameObject> friendChatMessages = new List<GameObject>();
        listOfChatMessages.Add(friendChatMessages);
        GameObject friendChatMessagesObject = Instantiate(friendChatMessagesBoxPrefab);
        RectTransform friendChatMessagesRectTransform = friendChatMessagesObject.GetComponent<RectTransform>();
        friendChatMessagesRectTransform.SetParent(chatMessagesViewContent.parent.parent);
        friendChatMessagesObject.transform.localScale = new Vector3(1, 1, 1);
        friendChatMessagesObject.transform.localPosition = chatMessagesViewContent.parent.parent.localPosition;
        var friendChatMessagesRectTransformChildrenEnum = friendChatMessagesObject.GetComponentsInChildren<RectTransform>().GetEnumerator();
        friendChatMessagesRectTransformChildrenEnum.MoveNext();
        friendChatMessagesRectTransformChildrenEnum.MoveNext();
        var friendChatMessagesRectTransformChild = (RectTransform)friendChatMessagesRectTransformChildrenEnum.Current;
        listOfFriendsMessagesContents.Add(friendChatMessagesRectTransformChild);
        //listOfFriendsMessagesContents.Add(friendChatMessagesRectTransform);
    }

    private void FindChatMessagesContent()
    {
        // We are gonna clear that stuff out
        listOfFriendsMessagesContents.Clear();
        chatMessagesView = GameObject.Find("ChatMessagesView");
        // Every messages object is going to have a messages script
        var friendsChatMessages = chatMessagesView.GetComponentsInChildren<MessagesScript>(true);
        // We want the rect transform of the component's gameObject
        var friendsChatMessagesEnum = friendsChatMessages.GetEnumerator();
        while (friendsChatMessagesEnum.MoveNext())
        {
            var currentFriendsChatMessages = (MessagesScript)friendsChatMessagesEnum.Current;
            var currentRectTransform = currentFriendsChatMessages.gameObject.GetComponent<RectTransform>();
            listOfFriendsMessagesContents.Add(currentRectTransform);
        }
    }

    private void TeamChatMessageRouter(TeamChatMessage message)
    {
        string messageWho = message.Who.ToString();
        string messageMessage = message.Message.ToString();
        Debug.Log("Team chat message recieved: " + messageMessage);
        Debug.Log("Message sent by: " + messageWho);

        string teamID = "0";
        if (chatMessagesView == null)
        {
            chatMessagesView = GameObject.Find("ChatMessagesView");
        }
        if (chatMessagesBox == null)
        {

            var globalMessages = chatMessagesView.GetComponentsInChildren<GlobalMessagesScript>(true);
            var globalMessagesEnum = globalMessages.GetEnumerator();
            globalMessagesEnum.MoveNext();
            chatMessagesBox = ((GlobalMessagesScript)globalMessagesEnum.Current).gameObject;
        }
        if (chatMessagesViewContent == null)
        {
            //chatMessagesViewContent = GameObject.Find("GlobalMessages").GetComponent<RectTransform>();
            chatMessagesViewContent = (chatMessagesBox.GetComponentInChildren<GlobalMessagesScript>(true)).gameObject.GetComponent<RectTransform>();
        }
        List<GameObject> friendChatMessages = chatMessages;
        RectTransform friendChatMessagesContent = chatMessagesViewContent;
        if (message.TeamType == "GlobalTeam")
        {

        }
        else
        {
            teamID = message.TeamId;
            //Predicate<string> predicate = delegate (string toCompare) { return toCompare == teamID; };
            //int teamIDIndex = teamIDs.FindIndex(predicate);

            //// If the team ID does not exist in the list
            //if (teamIDIndex == -1)
            //{
            //    teamIDs.Add(teamID);
            //    teamIDIndex = teamIDs.Count - 1;
            //    ChatMessagesViewContentCreator();
            //    if (chatSelectionPanel == null)
            //    {
            //        chatSelectionPanel = GameObject.Find("ChatSelectionPanel").GetComponent<ChatSelectionPanel>();
            //    }
            //    // Doesn't work if message was sent by myself ( should be added in a send first )
            //    chatSelectionPanel.AddChatSelectionButton(message.Who.ToString(), teamID);
            //}
            int teamIDIndex = LookupTeam(teamID, messageWho);
            SwitchActiveChat(teamID, messageWho.ToString());
            //int teamIDIndex = LookupTeam(teamID, messageWho);
            friendChatMessages = listOfChatMessages[teamIDIndex];
            //FindChatMessagesContent();
            friendChatMessagesContent = listOfFriendsMessagesContents[teamIDIndex];            
        }
        currentTeamID = teamID;
        BuildChatMessageUI(messageWho, messageMessage, lobbyMessagePrefab, friendChatMessagesContent, friendChatMessages);
    }

    private int LookupTeam(string teamID, string messageWho)
    {
        Predicate<string> predicate = delegate (string toCompare) { return toCompare == teamID; };
        int teamIDIndex = teamIDs.FindIndex(predicate);

        // If the team ID does not exist in the list
        if (teamIDIndex == -1)
        {
            teamIDs.Add(teamID);
            teamIDIndex = teamIDs.Count - 1;
            ChatMessagesViewContentCreator();
            if (chatSelectionPanel == null)
            {
                chatSelectionPanel = GameObject.Find("ChatSelectionPanel").GetComponent<ChatSelectionPanel>();
            }
            // Doesn't work if message was sent by myself ( should be added in a send first )
            chatSelectionPanel.AddChatSelectionButton(messageWho, teamID);
        }
        return teamIDIndex;
    }

    //Called when Input changes
    private void inputSubmitCallBack()
    {
        if (chatInput.GetComponent<TMPro.TMP_InputField>().text != "" && Input.GetKey(KeyCode.Return))
        {
            SendChatMessage(chatInput.GetComponent<TMPro.TMP_InputField>().text);
            chatInput.GetComponent<TMPro.TMP_InputField>().text = "";
        }
        Debug.Log("Input Submitted");
        //chatInput.GetComponent<InputField>().ActivateInputField(); //Re-focus on the input field
        chatInput.GetComponent<TMPro.TMP_InputField>().Select();//Re-focus on the input field
    }
    
    public void submitChatButton()
    {
        if (chatInput.GetComponent<TMPro.TMP_InputField>().text != "")
        {
            SendChatMessage(chatInput.GetComponent<TMPro.TMP_InputField>().text);
            chatInput.GetComponent<TMPro.TMP_InputField>().text = "";
        }
        Debug.Log("Input Submitted");
        //chatInput.GetComponent<InputField>().ActivateInputField(); //Re-focus on the input field
        chatInput.GetComponent<TMPro.TMP_InputField>().Select();//Re-focus on the input field
    }

    //Called when Input is submitted
    private void inputChangedCallBack()
    {
        Debug.Log("Input Changed");
        GameObject.Find("ChatCharLimitText").GetComponent<TMPro.TMP_Text>().text = chatInput.GetComponent<TMPro.TMP_InputField>().text.Length.ToString() + "/1000";
    }

    void OnEnable()
    {
        //Register InputField Events
        chatInput.GetComponent<TMPro.TMP_InputField>().onEndEdit.AddListener(delegate { inputSubmitCallBack(); });
        chatInput.GetComponent<TMPro.TMP_InputField>().onValueChanged.AddListener(delegate { inputChangedCallBack(); });
    }

    void OnDisable()
    {
        //Un-Register InputField Events
        //chatInput.GetComponent<InputField>().onEndEdit.RemoveAllListeners();
        //chatInput.GetComponent<InputField>().onValueChanged.RemoveAllListeners();
    }
    void JoinGlobalTeam()
    {
        bool teamMatch = false;
        foreach (var team in teams)
        {
            if (team.TeamId == "0")
            {
                teamMatch = true;
            }
        }
        // If I am not part of GlobalTeam
        if (!teamMatch)
        {
            // join GlobalTeam
            Debug.Log("Joining Global Team");
            JoinTeamRequest joinTeamRequest = new JoinTeamRequest();
            joinTeamRequest.SetTeamId("0");
            joinTeamRequest.SetTeamType("GlobalTeam");
            joinTeamRequest.Send(JoinedGlobalTeam);
        }
        else
        {
            Debug.Log("Already part of team");
        }
    }

    public void CheckTeams()
    {
        Debug.Log("Checking Teams");
        List<string> teamTypes = new List<string>();
        teamTypes.Add("GlobalTeam");
        // Check my teams
        GetMyTeamsRequest teamsRequest = new GetMyTeamsRequest();
        teamsRequest.SetOwnedOnly(false);
        teamsRequest.SetTeamTypes(teamTypes);
        teamsRequest.Send(CheckedTeams);
    }

    void CheckedTeams(GetMyTeamsResponse response)
    {
        teams = response.Teams;
        Debug.Log(teams);
        JoinGlobalTeam();
    }

    void JoinedGlobalTeam(JoinTeamResponse response)
    {
        if (response.HasErrors)
        {
            Debug.Log("Did not join Global Team");
        }
        else
        {
            Debug.Log("Joined Global Team");
        }
    }


    private void JoinFriendTeam(ScriptMessage_JoinFriendTeam message)
    {
        var joinFriendTeamData = message.Data.BaseData.Values.GetEnumerator();
        joinFriendTeamData.MoveNext();
        string teamID = joinFriendTeamData.Current.ToString();
        // Create join team request
        JoinTeamRequest joinTeamRequest = new JoinTeamRequest();
        joinTeamRequest.SetTeamId(teamID);
        //joinTeamRequest.SetTeamType("FriendsTeam");
        joinTeamRequest.Send(JoinedFriendTeam);
    }

    void JoinedFriendTeam(JoinTeamResponse response)
    {
        if (response.HasErrors)
        {
            Debug.Log("Did not join Friend Team");
        }
        else
        {
            Debug.Log("Joined Friend Team");
        }
    }

    public void OnChatInputSend()
    {
        TMPro.TMP_InputField chatInputField = chatInput.GetComponent<TMPro.TMP_InputField>();
        string message = chatInputField.text;
        chatInput.GetComponent<TMPro.TMP_InputField>().text = "";
        SendChatMessage(message);
    }

    void SendChatMessage(string message)
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Debug.Log("Sending message: " + message);
            SendTeamChatMessageRequest teamChatMessageRequest = new SendTeamChatMessageRequest();
            teamChatMessageRequest.SetMessage(message);
            //teamChatMessageRequest.SetTeamId("0");
            teamChatMessageRequest.SetTeamId(currentTeamID);
            teamChatMessageRequest.Send(ChatMessageResponse);
        }
    }

    void ChatMessageResponse(SendTeamChatMessageResponse response)
    {
        if (response.HasErrors)
        {
            Debug.Log("Chat message not sent");
        }
        else
        {
            Debug.Log("Chat message sent");
        }
    }

    private void BuildChatMessageUI(string messageWho, string messageMessage, GameObject messageTextObjectPrefab, RectTransform chatMessagesViewContent, List<GameObject> chatMessages)
    {
        GameObject messageTextObject = Instantiate(messageTextObjectPrefab) as GameObject;
        var messageTextObjectChildrenText = messageTextObject.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        TMPro.TextMeshProUGUI playerText = messageTextObjectChildrenText[0];
        TMPro.TextMeshProUGUI messageText = messageTextObjectChildrenText[1];

        if (messageWho.Length >= 20)
        {
            playerText.text = ("<b>" + messageWho.Substring(0, 17) + "..." + ":</b>");
        }
        else
        {
            playerText.text = ("<b>" + messageWho + ":</b>");
        }
        messageText.text = messageMessage;

        if (messageWho != gameSparksUserIDScript.myDisplayName)
        {
            pdaFlashOn();
            flashTimer.StartCountdown();
        } 
            

        Debug.Log("Name Of ChatMessagesViewContent: " + chatMessagesViewContent.name);
        messageTextObject.transform.SetParent(chatMessagesViewContent);
        messageTextObject.transform.localScale = new Vector3(1, 1, 1);

        chatMessages.Add(messageTextObject);

        LayoutRebuilder.ForceRebuildLayoutImmediate(chatMessagesViewContent);

        StartCoroutine(ScrollToBottom());

        LayoutRebuilder.ForceRebuildLayoutImmediate(chatMessagesViewContent);
    }

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        //scrollRect.gameObject.SetActive(true);
        chatMessagesViewContent.parent.gameObject.GetComponentInChildren<Scrollbar>().value = 0;
        chatMessagesViewContent.parent.gameObject.GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 0f;
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatMessagesViewContent);
    }

    private void AddSpacingMessage(RectTransform chatMessagesViewContent, List<GameObject> chatMessages, GameObject chatMessageObjectPrefab) 
    {
        GameObject messageTextObject = Instantiate(chatMessageObjectPrefab) as GameObject;
        var messageTextObjectChildrenText = messageTextObject.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        TMPro.TextMeshProUGUI playerText = messageTextObjectChildrenText[0];
        TMPro.TextMeshProUGUI messageText = messageTextObjectChildrenText[1];
        playerText.text = "____________________";
        messageText.text = "";

        messageTextObject.transform.SetParent(chatMessagesViewContent);
        messageTextObject.transform.localScale = new Vector3(1, 1, 1);

        chatMessages.Add(messageTextObject);
    }
    

    private void ChatMessagesFull()
    {
        if (chatMessages.Count == 5)
        {
            Debug.Log("Deleting earliest chat message");
            chatMessages.RemoveAt(0);
        }
    }

    public static void SetRect(RectTransform trs, float left, float top, float right, float bottom)
    {
        trs.offsetMin = new Vector2(left, bottom);
        trs.offsetMax = new Vector2(-right, -top) + trs.offsetMax;
    }

    //public void ClearAllFriendsChats()
    //{
    //    teamIDs.Clear();

    //    foreach (var chatMessages in listOfChatMessages)
    //    {
    //        foreach (var chatMessage in chatMessages)
    //        {
    //            //chatMessage.
    //            GameObject.Destroy(chatMessage);
    //        }
    //        chatMessages.Clear();
    //    }
    //    listOfChatMessages.Clear();

    //    chatSelectionPanel.ClearChatSelections();

    //    foreach (var friendsMessages in listOfFriendsMessagesContents)
    //    {
    //        GameObject.Destroy(friendsMessages);
    //    }
    //    listOfFriendsMessagesContents.Clear();
    //}

    IEnumerator BuyTime()
    {
        pdaFlash.SetActive(true);
        yield return new WaitForSeconds(0.375f);
        pdaFlash.SetActive(false);
    }

    public void pdaFlashOn()
    {
        pdaFlash.SetActive(true);
    }

    public void pdaFlashOff()
    {
        pdaFlash.SetActive(false);
        flashTimer.CancelCountdown();        
    }

    private void OnDestroy()
    {
        //SetAllActive();
        TeamChatMessage.Listener -= TeamChatMessageRouter;
        ScriptMessage_JoinFriendTeam.Listener -= JoinFriendTeam;
        gameSparksUserIDScript = new GameSparksUserID();
    }

    public void SetAllActive()
    {
        var childrenComponents = new List<RectTransform>(gameObject.GetComponentsInChildren<RectTransform>());
        var childrenObjects = new List<GameObject>();
        foreach (var childComponent in childrenComponents)
        {
            childrenObjects.Add(childComponent.gameObject);
        }
        foreach (var childObject in childrenObjects)
        {
            if (childObject != null)
            {
                childObject.SetActive(true);
            }
        }
    }
}
