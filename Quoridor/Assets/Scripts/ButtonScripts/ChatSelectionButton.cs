using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatSelectionButton : MonoBehaviour
{
    public string playerID;
    public string playerName;
    public string teamID;
    public ChatWindowPanel chatWindowPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickOpenTeamChat()
    {
        chatWindowPanel.SwitchActiveChat(teamID, playerName);
    }
}
