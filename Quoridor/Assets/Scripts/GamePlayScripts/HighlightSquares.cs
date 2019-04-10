using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;

public class HighlightSquares : MonoBehaviour
{
    public bool showHint = false;
    public string moveHint = "";

    private Material highlightMat;
    private Material hintMat;
    private Material gameSquareMat;
    private List<string> possibleMoves;
    private MoveArms armTwoScript;
    private MoveMouse opponentMoveMouseScript;
    private InterfaceController interfaceController;

    // Start is called before the first frame update
    void Start()
    {
        armTwoScript = GameObject.Find("ScientistArmTwo").GetComponent<MoveArms>();
        opponentMoveMouseScript = GameObject.Find("opponentMouse").GetComponent<MoveMouse>();

        // playerClickMouseScript = playerMouse.GetComponent<ClickMouse>();
        // opponentClickMouseScript = opponentMouse.GetComponent<ClickMouse>();

        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
        highlightMat = Resources.Load("highlightColor", typeof(Material)) as Material;
        hintMat = Resources.Load("hintColor", typeof(Material)) as Material;
        possibleMoves = new List<string>();
        gameSquareMat = Resources.Load("cubeColor", typeof(Material)) as Material;
    }

    // Update is called once per frame
    void Update()
    {
        //if (armTwoScript.moveArm == false && opponentMoveMouseScript.moveMouse == false)
        //{
        if (interfaceController.GetWhoseTurn() == GameBoard.PlayerEnum.ONE && armTwoScript.moveArm == false && opponentMoveMouseScript.moveMouse == false)
        {

            possibleMoves = interfaceController.GetPossibleMoves();
            foreach (string move in possibleMoves)
            {
                GameObject square = GameObject.Find(move);
                square.GetComponent<Renderer>().material = highlightMat;
            }

            if (showHint)
            {
                GameObject hintSquare = GameObject.Find(moveHint);
                hintSquare.GetComponent<Renderer>().material = hintMat;
            }
        }
        else
        {
            foreach (string move in possibleMoves)
            {
                GameObject square = GameObject.Find(move);
                square.GetComponent<Renderer>().material = gameSquareMat;
            }

            showHint = false;
        }
        //}
        //else
        //{
        //    foreach (string move in possibleMoves)
        //    {
        //        GameObject square = GameObject.Find(move);
        //        square.GetComponent<Renderer>().material = gameSquareMat;
        //    }
        //}



    }
}
