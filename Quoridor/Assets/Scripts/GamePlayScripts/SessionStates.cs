using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  The purpose of this class is to keep track of all session-level preferences.
 *  Use of this class is preferred over PlayerPrefs.
 */

public static class GameSession
{
    public static GameModeEnum GameMode { get; set; }
    public static DifficultyEnum Difficulty { get; set; }
    public static PlayerTurnEnum PlayerTurnPref { get; set; }
    public static float BackgroundVolumePref { get; set; }
    public static float EffectVolumePref { get; set; }
    public static int WinCount { get; set; }
    public static bool ForceAiMove { get; set; }

    static GameSession()
    {
        GameMode = GameModeEnum.SINGLE_PLAYER;
        Difficulty = DifficultyEnum.EASY;
        PlayerTurnPref = PlayerTurnEnum.FIRST;
        BackgroundVolumePref = 1.0f;
        EffectVolumePref = 1.0f;
        WinCount = 0;
        ForceAiMove = false;
    }


}
