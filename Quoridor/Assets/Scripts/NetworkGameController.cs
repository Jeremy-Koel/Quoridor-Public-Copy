using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGameController : MonoBehaviour
{
    private ChallengeManager challengeManagerScript;
    private MessageQueue messageQueue;

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

    public string GetPlayerOneDisplayName()
    {
        return challengeManagerScript.FirstPlayerInfo.PlayerDisplayName;
    }

    public string GetPlayerTwoDisplayName()
    {
        return challengeManagerScript.SecondPlayerInfo.PlayerDisplayName;
    }

}
