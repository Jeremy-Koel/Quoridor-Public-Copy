using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameModeStatus
{
    public static GameModeEnum GameMode { get; set; }

    static GameModeStatus()
    {
        GameMode = GameModeEnum.SINGLE_PLAYER;
    }
}
