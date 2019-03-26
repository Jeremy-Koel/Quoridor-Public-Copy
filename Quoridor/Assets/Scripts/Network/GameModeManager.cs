﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetGameModeSinglePlayer()
    {
        SessionStates.GameMode = GameModeEnum.SINGLE_PLAYER;
    }

    public void SetGameModeMultiplayer()
    {
        SessionStates.GameMode = GameModeEnum.MULTIPLAYER;
    }
}