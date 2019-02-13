﻿using UnityEngine;
using GameCore;
using System.Collections.Generic;

public class Controller : MonoBehaviour
{
    private GameBoard gameBoard;
    private Dictionary<string, PlayerCoordinate> coordMap;
    private bool localPlayerTurn;
    // Temporary solution for networking, save last validMove
    public string lastValidMove;

    // Start is called before the first frame update
    void Start()
    {
        gameBoard = new GameBoard(GameBoard.PlayerEnum.ONE, "e1", "e9");
        coordMap = new Dictionary<string, PlayerCoordinate>();
        localPlayerTurn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!localPlayerTurn)
        {
            // Get move from AI or network 
            GetOpponentMove();
            
            // Decide if the opponent placed a wall or piece, and call appropriate method 

            FlipTurn();
        }
    }

    private void GetOpponentMove()
    {
        Debug.Log("Getting oppoent move!");
    }

    public void FlipTurn()
    {
        localPlayerTurn = !localPlayerTurn;
    }

    public void AddSpace(GameObject obj)
    {
        string[] strs = obj.name.Split(',');
        int x = int.Parse(strs[0]) * 2;
        int y = int.Parse(strs[1]) * 2;
        coordMap.Add(obj.name, new PlayerCoordinate(x,y));
    }

    // TODO - represent whose turn it is in the GUI, so it can be used here 
    public bool IsValidMove(GameBoard.PlayerEnum player, string spaceName)
    {
        PlayerCoordinate pc = coordMap[spaceName];
        // Get return value for network
        bool validMove = gameBoard.MovePiece(player, pc);
        // If validMove send "move" across network
        if (validMove)
        {
            Debug.Log("Is a valid move");
            lastValidMove = spaceName;
            // Get ChallengeManager to send move
            Debug.Log("Finding ChallengeManager");
            GameObject challengeManagerObject = GameObject.Find("ChallengeManager");
            if (challengeManagerObject != null)
            {
                ChallengeManager challengeManagerScript = challengeManagerObject.GetComponent<ChallengeManager>();
                challengeManagerScript.GetLastValidMove(this);
            }
        }

        return validMove;
    }


    public bool IsValidWallPlacement(GameBoard.PlayerEnum player, string spaceName)
    {
        string[] strs = spaceName.Split(',');
        int x = int.Parse(strs[0]) * 2;
        int y = int.Parse(strs[1][0].ToString()) * 2;
        char c = strs[1][1];
        return gameBoard.PlaceWall(player, new WallCoordinate(x, y, c));
    }
    

    public GameBoard.PlayerEnum GetWhoseTurn()
    {
        if (gameBoard.GetWhoseTurn() == 1)
        {
            return GameBoard.PlayerEnum.ONE;
        }
        else
        {
            return GameBoard.PlayerEnum.TWO;
        }
    }

    public bool IsGameOver()
    {
        return gameBoard.IsGameOver();
    }
    
    private void MoveOpponentPiece(int guiRow, int guiCol)
    {
        GameObject opponentMouse = GameObject.Find("opponentMouse");
        GameObject targetSquare = GameObject.Find(guiRow + "," + guiCol);
        opponentMouse.transform.position = new Vector3(targetSquare.transform.position.x, targetSquare.transform.position.y, -0.5f);
    }

    private void MoveOpponentWall()
    {
        GameObject wall = GetUnusedOpponentWall();
        if (wall != null)
        {
            Debug.Log("Opponent tried to place a wall");
        }
        else
        {
            throw new System.Exception("Opponent is out of walls!");
        }
    }

    private GameObject GetUnusedOpponentWall()
    {
        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("PlayerTwoWall"))
        {
            if (!wall.GetComponent<MoveWalls>().IsOnBoard())
            {
                return wall;
            }
        }
        return null;
    }
}
