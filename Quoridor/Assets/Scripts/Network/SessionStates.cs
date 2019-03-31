using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SessionStates
{
    public static GameModeEnum GameMode { get; set; }
    public static DifficultyEnum Difficulty { get; set; }
    public static PlayerTurnEnum PlayerTurnPref { get; set; }
    public static float MasterVolumePref { get; set; }
    public static float BackgroundVolumePref { get; set; }
    public static float EffectVolumePref { get; set; }

    static SessionStates()
    {
        GameMode = GameModeEnum.SINGLE_PLAYER;
        Difficulty = DifficultyEnum.EASY;
        PlayerTurnPref = PlayerTurnEnum.FIRST;
        MasterVolumePref = 1.0f;
        BackgroundVolumePref = 1.0f;
        EffectVolumePref = 1.0f;
    }


}
