using System.Collections;
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

    public GameObject wallPlacementOutlineVerticalPrefab;

    public GameObject wallPlacementOutlineHorizontalPrefab;

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

                piece.transform.position = new Vector3((col * 4.85f) + 5.1f, (row * -4.85f) + 32.5f, -0.2f);

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
            Vector3 newVerticalPos = new Vector3(piece.transform.position.x + 2.43f, piece.transform.position.y + 4.9f, -0.6f);
            Vector3 newHorizontalPos = new Vector3(piece.transform.position.x, piece.transform.position.y + 2.43f, -0.6f);
            GameObject wallCollHorizontal = Instantiate(wallColliderHorizontalPrefab) as GameObject;
            GameObject wallCollVertical = Instantiate(wallColliderVerticalPrefab) as GameObject;
            GameObject wallOutlineHorizontal = Instantiate(wallPlacementOutlineHorizontalPrefab) as GameObject;
            GameObject wallOutlineVertical = Instantiate(wallPlacementOutlineVerticalPrefab) as GameObject;

            wallCollHorizontal.transform.position = newHorizontalPos;
            wallCollHorizontal.name = piece.name + "h";
            wallOutlineHorizontal.transform.position = new Vector3(newHorizontalPos.x + 2.5f, newHorizontalPos.y, newHorizontalPos.z) ;
            wallOutlineHorizontal.transform.SetParent(wallCollHorizontal.transform);
            wallOutlineHorizontal.GetComponent<SpriteRenderer>().color = Color.clear;

            wallCollVertical.transform.position = newVerticalPos;
            wallCollVertical.name = piece.name + "v";
            wallOutlineVertical.transform.position = new Vector3(newVerticalPos.x, newVerticalPos.y - 2.5f, newVerticalPos.z);
            wallOutlineVertical.transform.SetParent(wallCollVertical.transform);
            wallOutlineVertical.GetComponent<SpriteRenderer>().color = Color.clear;

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
           // wall.AddComponent<MoveWalls>();
            wall.AddComponent<MoveWallsProgramatically>();
        }

        GameObject[] p1walls = GameObject.FindGameObjectsWithTag("PlayerOneWall");

        foreach (GameObject wall in p1walls)
        {
            wall.AddComponent<MoveWallsProgramatically>();
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
