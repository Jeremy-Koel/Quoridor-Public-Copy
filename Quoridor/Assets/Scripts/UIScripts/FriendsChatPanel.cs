using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsChatPanel : MonoBehaviour
{
    Button friendsChatPanelButton;
    private GameObject scientistArmOne;
    private GameObject friendsChatPanel;
    private RectTransform friendsChatPanelRectTransform;
    private MoveArms moveArmsOne;
    bool chatActive = false;
    bool movePDA = false;
    bool moveHand = false;
    public float speed = 2000.0f;
    public Vector2 target;
    public Vector2 activeAnchorForChatPosition;
    public Vector2 inactiveAnchorForChatPosition;
    public Vector2 activeAnchorForArmPosition;
    public Vector2 originalAnchorForArmPosition;

    private void Awake()
    {
        friendsChatPanelButton = GameObject.Find("FriendsAndChatButton").GetComponent<Button>();
        scientistArmOne = GameObject.Find("ScientistArmOne");
        moveArmsOne = scientistArmOne.GetComponent<MoveArms>();
        //activeAnchorForChatPosition = GameObject.Find("ActiveAnchorForChatPosition").GetComponent<RectTransform>().position;
        //inactiveAnchorForChatPosition = GameObject.Find("InactiveAnchorForChatPosition").GetComponent<RectTransform>().position;
        friendsChatPanel = GameObject.Find("FriendsChatPanel");
        friendsChatPanelRectTransform = friendsChatPanel.GetComponent<RectTransform>();

        //friendsChatPanelButton.onClick.AddListener(OnChatClick);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (movePDA)
        {
            if (moveHand)
            {
                if ((scientistArmOne.GetComponent<RectTransform>().localPosition.x == scientistArmOne.GetComponent<MoveArms>().chatTarget.x &&
                        scientistArmOne.GetComponent<RectTransform>().localPosition.y == scientistArmOne.GetComponent<MoveArms>().chatTarget.y))
                {
                    moveHand = false;
                }
            }
            else
            {
                float step = speed * Time.deltaTime; // calculate distance to move
                Vector2 transformPosition = new Vector2(friendsChatPanelRectTransform.localPosition.x, friendsChatPanelRectTransform.localPosition.y);
                friendsChatPanelRectTransform.localPosition = Vector2.MoveTowards(transformPosition, target, step);
                SetTarget(friendsChatPanelRectTransform.localPosition);

                if (friendsChatPanelRectTransform.localPosition.x == target.x &&
                        friendsChatPanelRectTransform.localPosition.y == target.y)
                {
                    movePDA = false;
                    SetTarget(inactiveAnchorForChatPosition);
                }
            }
        }
    }

    public void OnChatClick()
    {
        if (chatActive)
        {
            moveHand = true;
            SetTarget(activeAnchorForChatPosition);
            target = inactiveAnchorForChatPosition;
            chatActive = false;
        }
        else
        {
            target = activeAnchorForChatPosition;
            chatActive = true;
            SetTarget(target);
        }
        
        moveArmsOne.moveArm = true;
        movePDA = true;
    }

    public void SetTarget(Vector2 pos)
    {
        pos.x = pos.x - 120;
        pos.y = pos.y - 1508;

        moveArmsOne.SetTargetChat(pos);
    }
}
