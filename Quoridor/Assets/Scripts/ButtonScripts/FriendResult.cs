using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using UnityEngine.UI;


public class FriendResult : MonoBehaviour
{
    public string playerID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        LogEventRequest_FriendRequest friendRequest = new LogEventRequest_FriendRequest();
        friendRequest.Set_playerID(playerID);
        friendRequest.Send(OnFriendRequestResponse);
    }

    private void OnFriendRequestResponse(LogEventResponse response)
    {

        UnityEngine.UI.Text[] friendResultObjectTexts = GameObject.Find(playerID).GetComponentsInChildren<Text>();
        Text friendResultText = friendResultObjectTexts[0];
        if (!response.HasErrors)
        {
            // Change text of button
            friendResultText.text = "Request Sent!";
        }
        else
        {
            friendResultText.text = "Request Already Sent!";
        }
    }
}
