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
        friendsChatPanel = GameObject.Find("FriendsChatPanel");
        friendsChatPanelRectTransform = friendsChatPanel.GetComponent<RectTransform>();
        minimizeChat = GameObject.Find("MinimizeChatButton");
        friendsPanel = GameObject.Find("FriendsPanel");
        chatSelectionPanel = GameObject.Find("ChatSelectionPanel");
        chatWindowPanel = GameObject.Find("ChatWindowPanel");
        activePosition = new Vector3(814f, 35f, 0f);
        inactivePosition = new Vector3(814f, -525f, 0f);
        hoverOverPosition = new Vector3(814f, -450f, 0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        ToggleChatElements();
        minimizeChat.SetActive(false);
        step = FastAnimations.CalculateSpeed(speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (movePDA)
        {

            if (chatActive) // Showing PDA
            {
                friendsChatPanelRectTransform.localPosition = Vector3.MoveTowards(friendsChatPanelRectTransform.localPosition, activePosition, step);
                
                // Did we reach our destination?
                if (friendsChatPanelRectTransform.localPosition.Equals(activePosition))
                {
                    movePDA = false;
                    minimizeChat.SetActive(true);
                }
            }
            else // Hiding PDA 
            {
                friendsChatPanelRectTransform.localPosition = Vector3.MoveTowards(friendsChatPanelRectTransform.localPosition, inactivePosition, step);

                // Did we reach our destination?
                if (friendsChatPanelRectTransform.localPosition.Equals(inactivePosition))
                {
                    movePDA = false;
                    openChatGameObject.SetActive(true);
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
        if (chatActive)
        {
            openChatGameObject.SetActive(false);
        }
        else
        {
            minimizeChat.SetActive(false);
        }

        ToggleChatElements();
    }

    private void ToggleChatElements()
    {
        // For these elements, we want them to show if the chat is active (open and showing)
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

}
