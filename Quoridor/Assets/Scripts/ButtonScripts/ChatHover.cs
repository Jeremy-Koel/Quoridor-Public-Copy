using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChatHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private FriendsChatPanel chatPanel;

    private void Awake()
    {
        chatPanel = GameObject.Find("FriendsChatPanel").GetComponent<FriendsChatPanel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        chatPanel.HoverUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        chatPanel.HoverDown();
    }
}
