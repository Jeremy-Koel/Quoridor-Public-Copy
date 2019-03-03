using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public string PlayerDisplayName { get; set; }
    public string PlayerID { get; set; }
    public int PlayerNumber { get; set; }
    public GameCore.GameBoard.PlayerEnum PlayerEnum { get; set; }

    override public string ToString()
    {
        string playerInfoString = "";
        playerInfoString = playerInfoString + "PlayerDisplayName: " + PlayerDisplayName + ". ";
        playerInfoString = playerInfoString + "PlayerID: " + PlayerID + ". ";
        playerInfoString = playerInfoString + "PlayerNumber: " + PlayerNumber + ". ";
        playerInfoString = playerInfoString + "PlayerEnum: " + PlayerEnum.ToString() + ". ";
        return playerInfoString;
    }

}
