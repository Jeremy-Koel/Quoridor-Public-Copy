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

public class InGameChatWindowPanel : MonoBehaviour
{
    [SerializeField]
    public GameObject chatInput;
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
    public GameObject inGameMessagePrefab;
    [SerializeField]
    public ChallengeManager challengeManager;
    [SerializeField]
    public GameObject pdaFlash;

    public GameSparksUserID gameSparksUserIDScript;

    // Start is called before the first frame update
    void Start()
    {
        pdaFlash.SetActive(false);
        if (GameSession.GameMode == GameModeEnum.MULTIPLAYER)
        {
            challengeManager = GameObject.Find("ChallengeManager").GetComponent<ChallengeManager>();
            gameSparksUserIDScript = GameObject.Find("GameSparksUserID").GetComponent<GameSparksUserID>();
        }
        
        chatInput = GameObject.Find("InGameChatInput");
        chatMessagesView = GameObject.Find("InGameChatMessagesView");
        chatMessagesViewContent = GameObject.Find("InGameMessages").GetComponent<RectTransform>();
        chatMessagesLayoutGroup = GameObject.Find("InGameMessages").GetComponent<VerticalLayoutGroup>();
        ChallengeChatMessage.Listener += ChallengeChatMessageReceived;
        Debug.Log("Name Of ChatMessagesViewContent: " + chatMessagesViewContent.name);
    }

    // Update is called once per frame
    void Update()
    {
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
    public void OnChatInputSend()
    {
        TMPro.TMP_InputField chatInputField = chatInput.GetComponent<TMPro.TMP_InputField>();
        string message = chatInputField.text;
        chatInput.GetComponent<TMPro.TMP_InputField>().text = "";
        SendChatMessage(message);
    }

    void SendChatMessage(string message)
    {
        if (GameSession.GameMode == GameModeEnum.MULTIPLAYER)
        {
            Debug.Log("Sending message: " + message);
            ChatOnChallengeRequest challengeChatMessageRequest = new ChatOnChallengeRequest();
            challengeChatMessageRequest.SetMessage(message);
            challengeChatMessageRequest.SetChallengeInstanceId(challengeManager.ChallengeID);
            challengeChatMessageRequest.Send(ChallengeChatMessageResponse);
        }
        else
        {
            // Send player's message
            BuildChatMessageUI("Player", message, inGameMessagePrefab, chatMessagesViewContent, chatMessages);
            // Use AI chat
            string aiMessage;
            if (GameSession.Difficulty == DifficultyEnum.EASY)
            {
                aiMessage = AIChat.GetEasyAIMessage();
            }
            else
            {
                aiMessage = AIChat.GetHardAIMessage();
            }
            //make time
            //Invoke("WaitForAI", 0f);
            StartCoroutine("WaitForAI");
            //pdaFlash.SetActive(true); 
        }
    }

    void ChallengeChatMessageResponse(ChatOnChallengeResponse response)
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

    private void ChallengeChatMessageReceived(ChallengeChatMessage message)
    {
        string messageWho = message.Who.ToString();
        string messageMessage = message.Message.ToString();
        if (messageMessage.Length > 1000)
        {
            messageMessage = messageMessage.Substring(0, 1000);
        }
        Debug.Log("Team chat message recieved: " + messageMessage);
        Debug.Log("Message sent by: " + messageWho);
        //GameSparksManager gsm = GameObject.Find("GameSparksManager").GetComponent<GameSparksManager>()
        if (gameSparksUserIDScript.myDisplayName != messageWho)
        {
            //pdaFlash.SetActive(true);
            //    // make time
            //    StartCoroutine(BuyTime(counter));
        }
        //else
        //{

        //}
        BuildChatMessageUI(messageWho, messageMessage, inGameMessagePrefab, chatMessagesViewContent, chatMessages);
    }

    private void BuildChatMessageUI(string messageWho, string messageMessage, GameObject messageTextObjectPrefab, RectTransform chatMessagesViewContent, List<GameObject> chatMessages)
    {
        //if (gameObject.activeSelf)
        //{
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

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (messageWho != gameSparksUserIDScript.myDisplayName)
            {
                StartCoroutine(nameof(BuyTime));
            }
        }


        Debug.Log("Name Of ChatMessagesViewContent: " + chatMessagesViewContent.name);
        messageTextObject.transform.SetParent(chatMessagesViewContent);
        messageTextObject.transform.localScale = new Vector3(1, 1, 1);

        chatMessages.Add(messageTextObject);

        LayoutRebuilder.ForceRebuildLayoutImmediate(chatMessagesViewContent);

        //AddSpacingMessage(chatMessagesViewContent, chatMessages, messageTextObjectPrefab);
        //chatMessagesViewContent.parent.gameObject.GetComponentInChildren<Scrollbar>().value = 0;
        StartCoroutine(ScrollToBottom());

        LayoutRebuilder.ForceRebuildLayoutImmediate(chatMessagesViewContent);
        //}        
    }

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        //scrollRect.gameObject.SetActive(true);
        chatMessagesViewContent.parent.gameObject.GetComponentInChildren<Scrollbar>().value = 0;
        chatMessagesViewContent.parent.gameObject.GetComponentInChildren<ScrollRect>().verticalNormalizedPosition = 0f;
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatMessagesViewContent);
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

    IEnumerator BuyTime()
    {
        pdaFlash.SetActive(true);
        yield return new WaitForSeconds(0.375f);
        pdaFlash.SetActive(false);
    }

    IEnumerator WaitForAI()
    {
        yield return new WaitForSeconds(2f);
        string aiMessage;
        if (GameSession.Difficulty == DifficultyEnum.EASY)
        {
            aiMessage = AIChat.GetEasyAIMessage();
        }
        else
        {
            aiMessage = AIChat.GetHardAIMessage();
        }
        BuildChatMessageUI("Computer", aiMessage, inGameMessagePrefab, chatMessagesViewContent, chatMessages);
        StartCoroutine(BuyTime());
    }

    private void OnDestroy()
    {
        ChallengeChatMessage.Listener -= ChallengeChatMessageReceived;
        gameSparksUserIDScript = null;
    }
}
