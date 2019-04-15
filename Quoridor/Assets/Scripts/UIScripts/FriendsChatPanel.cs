using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FriendsChatPanel : MonoBehaviour
{
    Button openChatButton;
    private GameObject scientistArmOne;
    private GameObject friendsChatPanel;
    private GameObject openChatGameObject;
    private GameObject minimizeChat;
    private List<GameObject> minimizeChatButtons = new List<GameObject>();
    private GameObject friendsPanel;
    private GameObject chatSelectionPanel;
    private GameObject chatWindowPanel;
    private RectTransform friendsChatPanelRectTransform;
    private MoveArms moveArmsOne;
    bool chatActive = false;
    bool movePDA = false;
    bool moveHand = false;
    bool pointerIn = false;
    bool pointerOut = false;
    public float speed = 2000.0f;
    private float step;
    public Vector2 target;
    public Vector2 activeAnchorForChatPosition;
    public Vector2 inactiveAnchorForChatPosition;
    public Vector2 activeAnchorForArmPosition;
    public Vector2 originalAnchorForArmPosition;

    private Vector3 activePosition;
    private Vector3 inactivePosition;
    private Vector3 hoverOverPosition;

    private void Awake()
    {
        openChatGameObject = GameObject.Find("OpenChatButton");
        openChatButton = openChatGameObject.GetComponent<Button>();
        //scientistArmOne = GameObject.Find("ScientistArmOne");
        //moveArmsOne = scientistArmOne.GetComponent<MoveArms>();
        friendsChatPanel = GameObject.Find("FriendsChatPanel");
        friendsChatPanelRectTransform = friendsChatPanel.GetComponent<RectTransform>();
        minimizeChat = GameObject.Find("MinimizeChatButton");
        minimizeChatButtons.Add(GameObject.Find("MinimizeChatButton"));
        minimizeChatButtons.Add(GameObject.Find("MinimizeChatButtonTopLeft"));
        minimizeChatButtons.Add(GameObject.Find("MinimizeChatButtonTopRight"));
        minimizeChatButtons.Add(GameObject.Find("MinimizeChatButtonTopBottom"));
        minimizeChatButtons.Add(GameObject.Find("MinimizeChatButtonTopTop"));
        //minimizeChatButtons.Add(GameObject.Find("MinimizeChatButtonBottom"));
        friendsPanel = GameObject.Find("FriendsPanel");
        chatSelectionPanel = GameObject.Find("ChatSelectionPanel");
        chatWindowPanel = GameObject.Find("ChatWindowPanel");
        activePosition = new Vector3(814f, 35f, 0f);
        inactivePosition = new Vector3(814f, -499f, 0f);
        hoverOverPosition = new Vector3(814f, -450f, 0f);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        ToggleChatElements();
        step = FastAnimations.CalculateSpeed(speed);
    }

    // Update is called once per frame
    void Update()
    {
        //if (movePDA)
        //{
        //    if (moveHand)
        //    {
        //        if ((scientistArmOne.GetComponent<RectTransform>().localPosition.x == scientistArmOne.GetComponent<MoveArms>().chatTarget.x &&
        //                scientistArmOne.GetComponent<RectTransform>().localPosition.y == scientistArmOne.GetComponent<MoveArms>().chatTarget.y))
        //        {
        //            moveHand = false;
        //        }
        //    }
        //    else
        //    {
        //        //float step = speed * Time.deltaTime; // calculate distance to move
        //        float step = FastAnimations.CalculateSpeed(speed);
        //        Vector2 transformPosition = new Vector2(friendsChatPanelRectTransform.localPosition.x, friendsChatPanelRectTransform.localPosition.y);
        //        friendsChatPanelRectTransform.localPosition = Vector2.MoveTowards(transformPosition, target, step);
        //        SetTarget(friendsChatPanelRectTransform.localPosition);

        //        if (friendsChatPanelRectTransform.localPosition.x == target.x &&
        //                friendsChatPanelRectTransform.localPosition.y == target.y)
        //        {
        //            movePDA = false;
        //            SetTarget(inactiveAnchorForChatPosition);
        //        }
        //    }
        //}

        if (movePDA)
        {

            if (chatActive) // Showing PDA
            {
                friendsChatPanelRectTransform.localPosition = Vector3.MoveTowards(friendsChatPanelRectTransform.localPosition, activePosition, step);
                
                // Did we reach our destination?
                if (friendsChatPanelRectTransform.localPosition.Equals(activePosition))
                {
                    movePDA = false;
                }
            }
            else // Hiding PDA 
            {
                friendsChatPanelRectTransform.localPosition = Vector3.MoveTowards(friendsChatPanelRectTransform.localPosition, inactivePosition, step);

                // Did we reach our destination?
                if (friendsChatPanelRectTransform.localPosition.Equals(inactivePosition))
                {
                    movePDA = false;
                }
            }
        }
        else if (pointerIn)
        {
            friendsChatPanelRectTransform.localPosition = Vector3.MoveTowards(friendsChatPanelRectTransform.localPosition, hoverOverPosition, step);

            // Did we reach our destination?
            if (friendsChatPanelRectTransform.localPosition.Equals(hoverOverPosition))
            {
                pointerIn = false;
            }
        }
        else if (pointerOut)
        {
            friendsChatPanelRectTransform.localPosition = Vector3.MoveTowards(friendsChatPanelRectTransform.localPosition, inactivePosition, step);

            // Did we reach our destination?
            if (friendsChatPanelRectTransform.localPosition.Equals(inactivePosition))
            {
                pointerOut = false;
            }
        }
    }

    public void OnChatClick()
    {
        // Force chat panel to redraw itself so new chats are added 
        LayoutRebuilder.ForceRebuildLayoutImmediate(friendsChatPanelRectTransform);
        LayoutRebuilder.MarkLayoutForRebuild(friendsChatPanelRectTransform);
        friendsChatPanelRectTransform.gameObject.SetActive(false);
        friendsChatPanelRectTransform.gameObject.SetActive(true);

        movePDA = true;
        chatActive = !chatActive;

        // If the chat is active, then we don't want the open button to be interactable 
        openChatGameObject.SetActive(!chatActive);

        ToggleChatElements();
    }

    private void ToggleChatElements()
    {
        // For these elements, we want them to show if the chat is active (open and showing)
        //minimizeChat.SetActive(chatActive);
        foreach (var minButton in minimizeChatButtons)
        {
            minButton.SetActive(chatActive);
        }
        friendsPanel.SetActive(chatActive);
        chatSelectionPanel.SetActive(chatActive);
        chatWindowPanel.SetActive(chatActive);
    }

    public void HoverUp()
    {
        pointerIn = true;
    }

    public void HoverDown()
    {
        pointerOut = true;
    }

    public void SetTarget(Vector2 pos)
    {
        //pos.x = pos.x - 120;
        //pos.y = pos.y - 1508;

        //moveArmsOne.SetTargetChat(pos);
    }

}
