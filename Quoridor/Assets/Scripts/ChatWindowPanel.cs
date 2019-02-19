﻿using GameSparks.Api;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ChatWindowPanel : MonoBehaviour
{
    private GSEnumerable<GetMyTeamsResponse._Team> teams = null;

    GameObject chatInput;
    GameObject chatMessagesView;
    //public Transform chatMessagesViewContent;
    public RectTransform chatMessagesViewContent;
    public List<GameObject> chatMessages;
    public GameObject textMessagePrefab;


    private void Awake()
    {
        chatInput = GameObject.Find("ChatInput");
        chatMessagesView = GameObject.Find("ChatMessagesView");
        //  chatMessagesViewContent = chatMessagesView.GetComponent<>();
        TeamChatMessage.Listener += ChatMessageReceived;
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void JoinGlobalTeam()
    {
        bool teamMatch = false;
        foreach (var team in teams)
        {
            if (team.TeamId == "0")
            {
                teamMatch = true;
            }
        }
        // If I am not part of GlobalTeam
        if (!teamMatch)
        {
            // join GlobalTeam
            Debug.Log("Joining Global Team");
            JoinTeamRequest joinTeamRequest = new JoinTeamRequest();
            joinTeamRequest.SetTeamId("0");
            joinTeamRequest.SetTeamType("GlobalTeam");
            joinTeamRequest.Send(JoinedGlobalTeam);
        }
        else
        {
            Debug.Log("Already part of team");
        }
    }

    public void CheckTeams()
    {
        Debug.Log("Checking Teams");
        List<string> teamTypes = new List<string>();
        teamTypes.Add("GlobalTeam");
        // Check my teams
        GetMyTeamsRequest teamsRequest = new GetMyTeamsRequest();
        teamsRequest.SetOwnedOnly(false);
        teamsRequest.SetTeamTypes(teamTypes);
        teamsRequest.Send(CheckedTeams);
    }

    void CheckedTeams(GetMyTeamsResponse response)
    {
        teams = response.Teams;
        Debug.Log(teams);
        JoinGlobalTeam();
    }

    void JoinedGlobalTeam(JoinTeamResponse response)
    {
        if (response.HasErrors)
        {
            Debug.Log("Did not join Global Team");
        }
        else
        {
            Debug.Log("Joined Global Team");
        }
    }

    public void OnChatInputSend()
    {
        InputField chatInputField = chatInput.GetComponent<InputField>();
        string message = chatInputField.text;
        SendChatMessage(message);
    }

    void SendChatMessage(string message)
    {
        Debug.Log("Sending message: " + message);
        SendTeamChatMessageRequest teamChatMessageRequest = new SendTeamChatMessageRequest();
        teamChatMessageRequest.SetMessage(message);
        teamChatMessageRequest.SetTeamId("0");
        teamChatMessageRequest.Send(ChatMessageResponse);
    }

    void ChatMessageResponse(SendTeamChatMessageResponse response)
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

    private void ChatMessageReceived(TeamChatMessage message)
    {
        Debug.Log("Team chat message recieved: " + message.Message);
        Debug.Log("Message sent by: " + message.Who);

        //ChatMessagesFull();

        GameObject messageTextObject = Instantiate(textMessagePrefab) as GameObject;
        //GameObject messageTextObjectBuffer = Instantiate(textMessagePrefab) as GameObject;
        UnityEngine.UI.Text[] messageTextObjectChildrenText = messageTextObject.GetComponentsInChildren<Text>();
        Text playerText = messageTextObjectChildrenText[0];
        Text messageText = messageTextObjectChildrenText[1];
        //GameObject messageTextObject = GameObject.Find("Label");

        //Text messageText = messageTextObject.GetComponent<Text>();
        //messageText.text = ("<b>" + message.Who + ":</b> " + message.Message);
        playerText.text = ("<b>" + message.Who + ":</b>");
        messageText.text = (message.Message);

        //messageTextObject.SetActive(false);

        messageTextObject.transform.SetParent(chatMessagesViewContent);
        messageTextObject.transform.localScale = new Vector3(1, 1, 1);
        

        //messageTextObjectBuffer.transform.SetParent(chatMessagesViewContent);
        //Vector2 values = messageTextObject.GetComponent<RectTransform>().anchoredPosition;

        //SetRect(chatMessagesViewContent, 0f, -(values.y / 2), 0f, 0f);

        chatMessages.Add(messageTextObject);
        //messageTextObject.SetActive(true);

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
}