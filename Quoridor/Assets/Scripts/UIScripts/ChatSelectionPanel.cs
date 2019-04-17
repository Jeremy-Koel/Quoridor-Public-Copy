using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatSelectionPanel : MonoBehaviour
{
    [SerializeField]
    public Button chatSelectionLeftButton;
    [SerializeField]
    public Button chatSelectionRightButton;
    [SerializeField]
    public GameObject chatSelectionContentObject;
    [SerializeField]
    public RectTransform chatSelectionContentRectTransform;
    [SerializeField]
    public RectTransform leftMostBoundaries;
    [SerializeField]
    public RectTransform rightMostBoundaries;
    [SerializeField]
    public float widthOfChatButton;
    [SerializeField]
    private float widthOfAllChatButtons;
    [SerializeField]
    public GameObject chatSelectionButtonPrefab;
    [SerializeField]
    public GameObject chatSelectionPanel;
    [SerializeField]
    public GameObject chatWindowPanel;
    [SerializeField]
    private List<GameObject> chatSelectionButtons;

    public void SetSelectionButtonsInteractive(int targetIndex)
    {
        int index = 0;
        //FindChatSelectionButtons();
        var chatSelectionButtonsEnum = chatSelectionButtons.GetEnumerator();
        while (chatSelectionButtonsEnum.MoveNext())
        {
            if (index == targetIndex)
            {
                chatSelectionButtonsEnum.Current.GetComponent<Button>().interactable = false;
            }
            else
            {
                chatSelectionButtonsEnum.Current.GetComponent<Button>().interactable = true;
            }            
            index++;
        }
    }

    //public void FindChatSelectionButtons()
    //{
    //    // CLEAR THIS STUFFF
    //    chatSelectionButtons.Clear();
    //    chatSelectionPanel = GameObject.Find("ChatSelectionPanel");
    //    var chatSelectionButtonScripts = chatSelectionPanel.GetComponentsInChildren<ChatSelectionButton>();
    //    foreach (var chatSelectionButton in chatSelectionButtonScripts)
    //    {
    //        chatSelectionButtons.Add(chatSelectionButton.gameObject);
    //    }
    //}

    private void Awake()
    {
        //chatWindowPanel = GameObject.Find("ChatWindowPanel");
        //chatSelectionContentObject = GameObject.Find("ChatSelectionContent");
        chatSelectionContentRectTransform = chatSelectionContentObject.GetComponent<RectTransform>();
        chatSelectionButtons = new List<GameObject>();
        //chatSelectionContentObject.GetComponent<LayoutElement>().minWidth = widthOfChatButton;

        GameObject leftMostBoundsObject = new GameObject("leftMostBoundariesObject", typeof(RectTransform));
        leftMostBoundaries = leftMostBoundsObject.GetComponent<RectTransform>();
        leftMostBoundaries.localPosition = new Vector2(chatSelectionContentRectTransform.localPosition.x,
                                                chatSelectionContentRectTransform.localPosition.y);

        GameObject rightMostBoundsObject = new GameObject("rightMostBoundariesObject", typeof(RectTransform));
        rightMostBoundaries = rightMostBoundsObject.GetComponent<RectTransform>();
        rightMostBoundaries.localPosition = new Vector2((chatSelectionContentRectTransform.localPosition.x - widthOfChatButton),
                                                chatSelectionContentRectTransform.localPosition.y);

        chatSelectionRightButton.interactable = false;
        chatSelectionLeftButton.interactable = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddChatSelectionButton(string name, string teamID)
    {
        chatSelectionContentObject = GameObject.Find("ChatSelectionContent");
        chatSelectionContentRectTransform = chatSelectionContentObject.GetComponent<RectTransform>();
        GameObject chatSelectionButtonObject = Instantiate(chatSelectionButtonPrefab);
        chatSelectionButtonObject.transform.SetParent(chatSelectionContentRectTransform);
        chatSelectionButtonObject.transform.localScale = new Vector3(1, 1, 1);
        Button chatSelectionButton = chatSelectionButtonObject.GetComponent<Button>();
        var chatSelectionButtonScript = chatSelectionButton.GetComponent<ChatSelectionButton>();
        chatSelectionButtonScript.playerName = name;
        chatSelectionButtonScript.teamID = teamID;
        chatSelectionButtonScript.chatWindowPanel = chatWindowPanel.GetComponent<ChatWindowPanel>();
        chatSelectionButton.onClick.AddListener(chatSelectionButtonScript.OnClickOpenTeamChat);
        Text chatButtonName = chatSelectionButtonObject.GetComponentInChildren<Text>();
        chatButtonName.text = name;
        chatSelectionButtons.Add(chatSelectionButtonObject);

        if (chatSelectionContentObject.GetComponentsInChildren<Button>().Length > 3)
        {
            chatSelectionRightButton.interactable = true;
            chatSelectionLeftButton.interactable = true;
        }
    }

    public void MoveContentPane(float value)
    {
        // Adjust dimensions of rightMostBoundaries
        widthOfAllChatButtons = (chatSelectionContentObject.GetComponentsInChildren<Button>().Length * widthOfChatButton);
        if (widthOfAllChatButtons > 300)
        {
            rightMostBoundaries.localPosition = new Vector2((leftMostBoundaries.localPosition.x - (widthOfAllChatButtons / 2)),
                                leftMostBoundaries.localPosition.y);

            chatSelectionContentRectTransform.localPosition = new Vector2(chatSelectionContentRectTransform.localPosition.x + value,
                                                            chatSelectionContentRectTransform.localPosition.y);
            

            if (chatSelectionContentRectTransform.localPosition.x <= rightMostBoundaries.localPosition.x)
            {
                chatSelectionContentRectTransform.localPosition = rightMostBoundaries.localPosition;
                chatSelectionRightButton.interactable = false;
                chatSelectionLeftButton.interactable = true;
            }
            else if (chatSelectionContentRectTransform.localPosition.x >= leftMostBoundaries.localPosition.x)
            {
                chatSelectionContentRectTransform.localPosition = leftMostBoundaries.localPosition;
                chatSelectionLeftButton.interactable = false;
                chatSelectionRightButton.interactable = true;
            }
            else
            {
                chatSelectionRightButton.interactable = true;
                chatSelectionLeftButton.interactable = true;
            }

        }
    }

    public void ClearChatSelections()
    {
        foreach (var chatSelection in chatSelectionButtons)
        {
            GameObject.Destroy(chatSelection);
        }
        chatSelectionButtons.Clear();
    }

    private void OnDestroy()
    {
        //var chatSelectionButtonScripts = new List<ChatSelectionButton>(chatSelectionPanel.GetComponentsInChildren<ChatSelectionButton>());
        //foreach (var chatSelectionButton in chatSelectionButtonScripts)
        //{
        //    chatSelectionButton.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        //}
    }
}
