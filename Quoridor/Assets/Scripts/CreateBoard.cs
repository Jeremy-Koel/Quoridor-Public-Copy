﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CreateBoard : MonoBehaviour
{
    public InterfaceController interfaceController;

    public GameObject[,] gameBoard = new GameObject[9, 9];
    public GameObject squarePrefab;

    public GameObject highlightSquarePrefab;

    public GameObject mousePrefab;

    public GameObject mousePrefab2;

    public GameObject wallColliderVerticalPrefab;

    public GameObject wallColliderHorizontalPrefab;

    public GameObject gameBoardWrapper;

    public GameObject wallColliderWrapper;

    // Start is called before the first frame update
    void Start()
    {
        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
        generateCubes();
        generatePlayer();
        addScriptToWalls();
    }

    void generateCubes()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                GameObject piece = Instantiate(squarePrefab) as GameObject;

                piece.AddComponent<ClickSquare>();


                gameBoard[row, col] = piece;

                piece.transform.position = new Vector3((col * 3.45f) + 11.25f, (row * -3.45f) + 27f, -0.2f);

                piece.name = GetSpaceStringName(row, col);

                PlaceWallColliders(piece);

                piece.transform.SetParent(gameBoardWrapper.transform);

                interfaceController.AddToSpaceMap(piece);

                GameObject highlightPiece = Instantiate(highlightSquarePrefab) as GameObject;


                highlightPiece.transform.position = new Vector3(piece.transform.position.x, piece.transform.position.y, -.19f);
                highlightPiece.transform.SetParent(piece.transform);
                highlightPiece.SetActive(false);
            }


        }
    }

    void PlaceWallColliders(GameObject piece)
    {
        if (piece.name[0] != 'i' && piece.name[1] != '9')
        {
            Vector3 newPos = new Vector3(piece.transform.position.x + 1.8f, piece.transform.position.y + 1.8f, -0.6f);
            GameObject wallCollHorizontal = Instantiate(wallColliderHorizontalPrefab) as GameObject;
            GameObject wallCollVertical = Instantiate(wallColliderVerticalPrefab) as GameObject;

            wallCollHorizontal.transform.position = newPos;
            wallCollHorizontal.name = piece.name + "h";

            wallCollVertical.transform.position = newPos;
            wallCollVertical.name = piece.name + "v";

            wallCollVertical.transform.SetParent(wallColliderWrapper.transform);
            wallCollHorizontal.transform.SetParent(wallColliderWrapper.transform);

            interfaceController.AddToWallMap(wallCollHorizontal);
            interfaceController.AddToWallMap(wallCollVertical);
        }
    }

    void generatePlayer()
    {
        GameObject player = Instantiate(mousePrefab) as GameObject;
        player.name = "playerMouse";
        player.AddComponent<MoveMouse>();
        GameObject startPlace = GameObject.Find("e1");

        player.transform.position = new Vector3(startPlace.transform.position.x, startPlace.transform.position.y, -.3f);

        GameObject opponent = Instantiate(mousePrefab2) as GameObject;
        opponent.name = "opponentMouse";
        opponent.AddComponent<MoveMouse>();
        GameObject startPlace2 = GameObject.Find("e9");

        opponent.transform.position = new Vector3(startPlace2.transform.position.x, startPlace2.transform.position.y, -.3f);
    }

    void addScriptToWalls()
    {
        GameObject[] p2walls = GameObject.FindGameObjectsWithTag("PlayerTwoWall");

        foreach (GameObject wall in p2walls)
        {
            wall.AddComponent<MoveWalls>();
            wall.AddComponent<MoveWallsProgramatically>();
        }

        GameObject[] p1walls = GameObject.FindGameObjectsWithTag("PlayerOneWall");

        foreach (GameObject wall in p1walls)
        {
            wall.AddComponent<MoveWalls>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private string GetSpaceStringName(int row, int col)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(System.Convert.ToChar('a' + col));
        for (int i = 0; i < 9; ++i)
        {
            if (i == row)
            {
                sb.Append(9 - row);
            }
        }
        return sb.ToString();
    }
}
