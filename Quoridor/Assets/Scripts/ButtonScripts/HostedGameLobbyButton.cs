﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

public class HostedGameLobbyButton : MonoBehaviour
{
    public string hostName;
    public string gameID;
    public string matchShortCode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {

        HostedGameLobbies hostedGameLobbiesScript = GetComponentInParent<HostedGameLobbies>();
        hostedGameLobbiesScript.RemoveRefreshHostedGamesListener();

        //ChallengeManager challengeManager = GameObject.Find("ChallengeManager").GetComponent<ChallengeManager>();
        JoinPendingMatchRequest joinPendingMatch = new JoinPendingMatchRequest();
        joinPendingMatch.SetMatchShortCode(matchShortCode);
        joinPendingMatch.SetPendingMatchId(gameID);
        joinPendingMatch.Send(OnJoinPendingMatchSuccess, OnJoinPendingMatchError);

        RemoveAllHostedGames();
    }

    public void OnMatchmakingSuccess(MatchmakingResponse response)
    {
        //UnblockInput();
        Debug.Log("Matchmaking Success");
    }

    public void OnMatchmakingError(MatchmakingResponse response)
    {
        //UnblockInput();
        Debug.Log("Matchmaking Error");
    }

    void OnJoinPendingMatchSuccess(JoinPendingMatchResponse response)
    {
        Debug.Log("Joined pending match");
    }

    void OnJoinPendingMatchError(JoinPendingMatchResponse response)
    {
        Debug.Log("Failed to join pending match");
    }

    void RemoveAllHostedGames()
    {
        RectTransform hostedGameLobbiesRectTransform = GetComponentInParent<RectTransform>();
        foreach (RectTransform child in hostedGameLobbiesRectTransform)
        {
            GameObject.Destroy(child.gameObject);
        }
        HostedGameLobbies hostedGameLobbiesScript = GetComponentInParent<HostedGameLobbies>();
        hostedGameLobbiesScript.hostedGames.Clear();
    }
}