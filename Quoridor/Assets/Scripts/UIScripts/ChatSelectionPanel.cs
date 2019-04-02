using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatSelectionPanel : MonoBehaviour
{
    public Button chatSelectionLeftButton;
    public Button chatSelectionRightButton;
    public GameObject chatSelectionContentObject;
    public RectTransform chatSelectionContentRectTransform;
    public RectTransform leftMostBoundaries;
    public RectTransform rightMostBoundaries;
    public float widthOfChatButton;
    private float widthOfAllChatButtons;
    public GameObject chatSelectionButtonPrefab;

    public GameObject chatWindowPanel;

    private List<GameObject> chatSelectionButtons;

    public void SetSelectionButtonsInteractive(int targetIndex)
    {
        int index = 0;
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

    private void Awake()
    {
        chatWindowPanel = GameObject.Find("ChatWindowPanel");
        chatSelectionContentObject = GameObject.Find("ChatSelectionContent");
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
            }
            if (chatSelectionContentRectTransform.localPosition.x >= leftMostBoundaries.localPosition.x)
            {
                chatSelectionContentRectTransform.localPosition = leftMostBoundaries.localPosition;
            }
        }
    }
}
