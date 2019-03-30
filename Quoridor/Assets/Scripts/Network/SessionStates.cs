﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SessionStates
{
    public static GameModeEnum GameMode { get; set; }
    public static DifficultyEnum Difficulty { get; set; }

    public static PlayerTurnEnum PlayerTurnPref { get; set; }

    static SessionStates()
    {
        GameMode = GameModeEnum.SINGLE_PLAYER;
        Difficulty = DifficultyEnum.EASY;
        PlayerTurnPref = PlayerTurnEnum.FIRST;
    }


}
