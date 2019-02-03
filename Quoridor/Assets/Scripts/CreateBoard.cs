using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBoard : MonoBehaviour
{
    public GameObject[,] gameBoard = new GameObject[9,9];
    public GameObject squarePrefab;

    public GameObject mousePrefab;

    public GameObject mousePrefab2;

    public GameObject wallColliderPrefab;

    public GameObject gameBoardWrapper;

    public GameObject wallColliderWrapper;

    // Start is called before the first frame update
    void Start()
    {
        generateCubes();
        generatePlayer();
        addScriptToWalls();
    }

    void generateCubes()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++) 
            {
                GameObject piece = Instantiate(squarePrefab) as GameObject;

                piece.AddComponent<ClickSquare>();

                
                gameBoard[y,x] = piece;

                piece.transform.position = new Vector3((x *3.45f) + 11.25f, (y *-3.45f) + 27f, -0.2f);

                piece.name = y + "," + x;

                PlaceWallColliders(piece);

                piece.transform.SetParent(gameBoardWrapper.transform);

            }

            
        }
    }

    void PlaceWallColliders(GameObject piece)
    {
        if (piece.name[0] != '0' && piece.name[2] != '8')
        {
            Vector3 newPos = new Vector3(piece.transform.position.x + 1.8f, piece.transform.position.y + 1.8f, -0.6f);
            GameObject wallCollHorizontal = Instantiate(wallColliderPrefab) as GameObject;
            GameObject wallCollVertical = Instantiate(wallColliderPrefab) as GameObject;
            wallCollHorizontal.transform.Rotate(new Vector3(0, 0, 90));
            wallCollHorizontal.transform.position = newPos;
            wallCollHorizontal.name = piece.name + "v";
            wallCollVertical.transform.position = newPos;
            //wallCollVertical.transform.Rotate(new Vector3(0, 0, 0));
            wallCollVertical.name = piece.name + "h";

            wallCollVertical.transform.SetParent(wallColliderWrapper.transform);
            wallCollHorizontal.transform.SetParent(wallColliderWrapper.transform);
        }
    }

    void generatePlayer()
    {
        GameObject player = Instantiate(mousePrefab) as GameObject;
        player.name = "playerMouse";
        GameObject startPlace = GameObject.Find("8,4");

        player.transform.position = new Vector3(startPlace.transform.position.x, startPlace.transform.position.y, -.3f);

        GameObject player2 = Instantiate(mousePrefab2) as GameObject;
        player2.name = "playerMouse2";
        GameObject startPlace2 = GameObject.Find("0,4");

        player2.transform.position = new Vector3(startPlace2.transform.position.x, startPlace2.transform.position.y, -.3f);
    }

    void addScriptToWalls()
    {
        GameObject[] p2walls = GameObject.FindGameObjectsWithTag("PlayerTwoWall");

        foreach(GameObject wall in p2walls)
        {
            wall.AddComponent<MoveWalls>();
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
}
