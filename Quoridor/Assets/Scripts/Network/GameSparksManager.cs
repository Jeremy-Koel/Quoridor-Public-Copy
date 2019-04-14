﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using System;

public class GameSparksManager : MonoBehaviour
{
    private ChallengeManager challengeManager;
    private EventManager eventManager;
    public bool connected;
    private float checkConnectionSpeed = 5.0f;
    public bool oppDisconnected = false;

    private void Awake()
    {
        int numberOfActiveThises = FindObjectsOfType<GameSparksManager>().Length;
        if (numberOfActiveThises != 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
        ScriptMessage_GameDisconnected.Listener += GameDisconnectedHandler;
        ScriptMessage_ConnectionLost.Listener += LostConnectionMessageHandler;
        GS.GameSparksAvailable += HandleGameSparksAvailable;
        CheckConnectionRepeating();
        // Listen to events for checking connection
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        eventManager.ListenToLostConnection(LostConnection);
    }

    private void GameDisconnectedHandler(ScriptMessage_GameDisconnected message)
    {
        Debug.Log("Lost connection message received");
        string playerId = message.BaseData.GetString("playerWhoDisconnected");
        var gameSparksUserID = GameObject.Find("GameSparksUserID").GetComponent<GameSparksUserID>();
        if (playerId != gameSparksUserID.myUserID)
        {
            oppDisconnected = true;
        }
        else
        {
            oppDisconnected = false;
        }
        eventManager.InvokeLostConnection();
    }

    private void CheckConnectionRepeating()
    {
        InvokeRepeating("CheckConnection", checkConnectionSpeed, checkConnectionSpeed);
    }

    // Disconnect handling
    private void LostConnectionMessageHandler(ScriptMessage_ConnectionLost message)
    {
        Debug.Log("Lost connection message received");
        LostConnection();
    }

    private void LostConnection()
    {
        Debug.Log("Lost connection");
        //connected = false;
        //if (oppDisconnected)
        //{
        //    connected = true;
        //    oppDisconnected = false;
        //}
    }

    private void CheckConnection()
    {
        challengeManager = GameObject.Find("ChallengeManager").GetComponent<ChallengeManager>();
        LogChallengeEventRequest_CheckConnection checkConnectionRequest = new LogChallengeEventRequest_CheckConnection();
        checkConnectionRequest.SetChallengeInstanceId(challengeManager.ChallengeID);
        checkConnectionRequest.Send(CheckConnectionResponse);
    }

    private void CheckConnectionResponse(LogChallengeEventResponse message)
    {
        Debug.Log("Check Connection Response: " + message);
    }

    // Triggered when gamesparks available value is modified
    private void HandleGameSparksAvailable(bool value)
    {
        Debug.Log("HandleGameSparksAvailable value: " + value.ToString());
        if (!value)
        {
            //LostConnection();
            eventManager.InvokeLostConnection();
            connected = false;
        }
        else
        {
            connected = true;
        }
    }
}
