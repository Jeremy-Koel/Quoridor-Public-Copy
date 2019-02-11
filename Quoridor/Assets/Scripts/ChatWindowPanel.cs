using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatWindowPanel : MonoBehaviour
{
    private GSEnumerable<GetMyTeamsResponse._Team> teams = null;

    private void Awake()
    {

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
}
