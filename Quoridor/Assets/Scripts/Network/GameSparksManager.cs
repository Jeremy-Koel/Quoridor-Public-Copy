using System.Collections;
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
    private void Awake()
    {
        int numberOfActiveThises = FindObjectsOfType<GameSparksManager>().Length;
        if (numberOfActiveThises != 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
        ScriptMessage_ConnectionLost.Listener += LostConnectionMessageHandler;
        GS.GameSparksAvailable += HandleGameSparksAvailable;
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
    }

    private void CheckConnection()
    {
        
    }

    // Triggered when gamesparks available value is modified
    private void HandleGameSparksAvailable(bool value)
    {
        Debug.Log("HandleGameSparksAvailable value: " + value.ToString());
        if (!value)
        {
            LostConnection();
        }
    }
}
