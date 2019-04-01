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

    private void Awake()
    {
        chatSelectionContentObject = GameObject.Find("ChatSelectionContent");
        chatSelectionContentRectTransform = chatSelectionContentObject.GetComponent<RectTransform>();

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

    public void MoveContentPane(float value)
    {
        // Adjust dimensions of rightMostBoundaries
        widthOfAllChatButtons = (chatSelectionContentObject.GetComponentsInChildren<Button>().Length * widthOfChatButton);
        rightMostBoundaries.localPosition = new Vector2((leftMostBoundaries.localPosition.x - (widthOfAllChatButtons / 2)),
                                        leftMostBoundaries.localPosition.y);
        //chatSelectionContentObject.GetComponent<LayoutElement>().minWidth = widthOfAllChatButtons;

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
