using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject singlePlayerButton = GameObject.Find("SinglePlayerButton");
        GameObject multiplayerButton = GameObject.Find("MultiPlayerButton");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetGameModeSinglePlayer()
    {
        GameModeStatus.GameMode = GameModeEnum.SINGLE_PLAYER;
    }

    private void SetGameModeMultiplayer()
    {
        GameModeStatus.GameMode = GameModeEnum.MULTIPLAYER;
    }
}
