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
using UnityEngine.SceneManagement;

public class ChatWindowPanel : MonoBehaviour
{
    private GSEnumerable<GetMyTeamsResponse._Team> teams = null;

    GameObject chatInput;
    private GameObject chatMessagesView;
    public RectTransform chatMessagesViewContent;
    public VerticalLayoutGroup chatMessagesLayoutGroup;
    public List<GameObject> chatMessages;
    public GameObject textMessagePrefab;
    public ChallengeManager challengeManager;


    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            chatInput = GameObject.Find("ChatInput");
            chatMessagesView = GameObject.Find("ChatMessagesView");
            chatMessagesViewContent = GameObject.Find("Messages").GetComponent<RectTransform>();
            chatMessagesLayoutGroup = GameObject.Find("Messages").GetComponent<VerticalLayoutGroup>();
            TeamChatMessage.Listener += ChatMessageReceived;
            Debug.Log("Name Of ChatMessagesViewContent: " + chatMessagesViewContent.name);
        }
        else
        {
            challengeManager = GameObject.Find("ChallengeManager").GetComponent<ChallengeManager>();
            chatInput = GameObject.Find("InGameChatInput");
            chatMessagesView = GameObject.Find("InGameChatMessagesView");
            chatMessagesViewContent = GameObject.Find("InGameMessages").GetComponent<RectTransform>();
            chatMessagesLayoutGroup = GameObject.Find("InGameMessages").GetComponent<VerticalLayoutGroup>();
            //ChallengeChatMessage.Listener += ChallengeChatMessageReceived;
            TeamChatMessage.Listener += ChatMessageReceived;
            Debug.Log("Name Of ChatMessagesViewContent: " + chatMessagesViewContent.name);
        }
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
       // if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Debug.Log("Sending message: " + message);
            SendTeamChatMessageRequest teamChatMessageRequest = new SendTeamChatMessageRequest();
            teamChatMessageRequest.SetMessage(message);
            teamChatMessageRequest.SetTeamId("0");
            teamChatMessageRequest.Send(ChatMessageResponse);
        }
        //else
        {

            //Debug.Log("Sending message: " + message);
            //ChatOnChallengeRequest challengeChatMessageRequest = new ChatOnChallengeRequest();
            //challengeChatMessageRequest.SetMessage(message);
            //challengeChatMessageRequest.SetChallengeInstanceId("0");
            //challengeChatMessageRequest.Send(ChallengeChatMessageResponse);
        }
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

    //void ChallengeChatMessageResponse(ChatOnChallengeResponse response)
    //{
    //    if (response.HasErrors)
    //    {
    //        Debug.Log("Chat message not sent");
    //    }
    //    else
    //    {
    //        Debug.Log("Chat message sent");
    //    }
    //}

    private void ChatMessageReceived(TeamChatMessage message)
    {
        string messageWho = message.Who.ToString();
        string messageMessage = message.Message.ToString();
        Debug.Log("Team chat message recieved: " + messageMessage);
        Debug.Log("Message sent by: " + messageWho);
 
        GameObject messageTextObject = Instantiate(textMessagePrefab) as GameObject;

        UnityEngine.UI.Text[] messageTextObjectChildrenText = messageTextObject.GetComponentsInChildren<Text>();
        Text playerText = messageTextObjectChildrenText[0];
        Text messageText = messageTextObjectChildrenText[1];
        if (messageWho.Length >= 10)
        {
            playerText.text = ("<b>" + messageWho.Substring(0, 10) + ":</b>");
        }
        else
        {
            playerText.text = ("<b>" + messageWho + ":</b>");
        }
        
        //playerText.text = ("<b>" + messageWho + ":</b>");
        messageText.text = (messageMessage);

        Debug.Log("Name Of ChatMessagesViewContent: " + chatMessagesViewContent.name);
        messageTextObject.transform.SetParent(chatMessagesViewContent);
        messageTextObject.transform.localScale = new Vector3(1, 1, 1);

        chatMessages.Add(messageTextObject);
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatMessagesViewContent);

        AddSpacingMessage();

        LayoutRebuilder.ForceRebuildLayoutImmediate(chatMessagesViewContent);
    }

    //private void ChallengeChatMessageReceived(ChallengeChatMessage message)
    //{
    //    string messageWho = message.Who.ToString();
    //    string messageMessage = message.Message.ToString();
    //    Debug.Log("Challenge chat message received: " + messageMessage);
    //    Debug.Log("Message sent by: " + messageWho);

    //    GameObject messageTextObject = Instantiate(textMessagePrefab) as GameObject;

    //    UnityEngine.UI.Text[] messageTextObjectChildrenText = messageTextObject.GetComponentsInChildren<Text>();
    //    Text playerText = messageTextObjectChildrenText[0];
    //    Text messageText = messageTextObjectChildrenText[1];
    //    if(messageWho.Length >= 10)
    //    {
    //        playerText.text = ("<b>" + messageWho.Substring(0, 10) + ":</b>");
    //    }
    //    else
    //    {
    //        playerText.text = ("<b>" + messageWho + ":</b>");
    //    }
    //    messageText.text = messageMessage;

    //    Debug.Log("Name of chatMessagesViewContent: " + chatMessagesViewContent.name);
    //    messageTextObject.transform.SetParent(chatMessagesViewContent);
    //    messageTextObject.transform.localScale = new Vector3(1, 1, 1);

    //    chatMessages.Add(messageTextObject);

    //    LayoutRebuilder.ForceRebuildLayoutImmediate(chatMessagesViewContent);

    //    AddSpacingMessage();

    //    LayoutRebuilder.ForceRebuildLayoutImmediate(chatMessagesViewContent);
    //}

    private void AddSpacingMessage() 
    {
        GameObject messageTextObject = Instantiate(textMessagePrefab) as GameObject;
        UnityEngine.UI.Text[] messageTextObjectChildrenText = messageTextObject.GetComponentsInChildren<Text>();
        Text playerText = messageTextObjectChildrenText[0];
        Text messageText = messageTextObjectChildrenText[1];
        playerText.text = "________________________________________";
        messageText.text = "";

        messageTextObject.transform.SetParent(chatMessagesViewContent);
        messageTextObject.transform.localScale = new Vector3(1, 1, 1);

        chatMessages.Add(messageTextObject);
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