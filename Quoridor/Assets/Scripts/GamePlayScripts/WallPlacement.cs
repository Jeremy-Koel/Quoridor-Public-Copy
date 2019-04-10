using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WallPlacement : MonoBehaviour
{

    private GameObject wallHighlight;
    private InterfaceController interfaceController;
    private bool wallPlacedHere =false;
    private EventManager eventManager;
    private MoveArms armTwoScript;
    private MoveMouse opponentMoveMouseScript;

    // Start is called before the first frame update
    void Start()
    {
        wallHighlight = this.transform.GetChild(0).gameObject;
        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        armTwoScript = GameObject.Find("ScientistArmTwo").GetComponent<MoveArms>();
        opponentMoveMouseScript = GameObject.Find("opponentMouse").GetComponent<MoveMouse>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(controller.GetWhoseTurn() == GameBoard.PlayerEnum.ONE)
        //{
        //    this.enabled = true;
        //}
        //else
        //{
        //    this.enabled = false;
        //}
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("Entered Collider");
            if (interfaceController.GetWhoseTurn() == GameBoard.PlayerEnum.ONE && !wallPlacedHere && armTwoScript.moveArm == false && opponentMoveMouseScript.moveMouse == false)
            {
                SpriteRenderer wallHighlighSpriteRenderer = wallHighlight.GetComponent<SpriteRenderer>();
                if (wallHighlighSpriteRenderer.color != Color.green)
                {
                    wallHighlighSpriteRenderer.color = Color.white;
                }
            }
        }
    }

    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (interfaceController.GetWhoseTurn() == GameBoard.PlayerEnum.ONE && !wallPlacedHere && armTwoScript.moveArm == false && opponentMoveMouseScript.moveMouse == false)
            {
                SpriteRenderer wallHighlighSpriteRenderer = wallHighlight.GetComponent<SpriteRenderer>();
                if (wallHighlighSpriteRenderer.color != Color.green)
                {
                    wallHighlighSpriteRenderer.color = Color.white;
                }
            }
        }
    }
    private void OnMouseExit()
    {
        //Debug.Log("Exited Collider");
        SpriteRenderer wallHighlighSpriteRenderer = wallHighlight.GetComponent<SpriteRenderer>();
        if (wallHighlighSpriteRenderer.color != Color.green)
        {
            wallHighlighSpriteRenderer.color = Color.clear;
        }
    }

    private void OnMouseUpAsButton()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (interfaceController.GetWhoseTurn() == GameBoard.PlayerEnum.ONE && !wallPlacedHere && armTwoScript.moveArm == false && opponentMoveMouseScript.moveMouse == false)
            {
                TryToPlaceAWall();
            }
        }
    }

    private GameObject GetUnusedWall()
    {
        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("PlayerOneWall"))
        {
            if (!wall.GetComponent<MoveWallsProgramatically>().IsOnBoard())
            {
                return wall;
            }
        }

        return null;
    }

    private void TryToPlaceAWall()
    {
        //GameBoard.PlayerEnum player = controller.GetWhoseTurn();

        if (interfaceController.RecordLocalPlayerMove(name))
        {
            GameObject wall = GetUnusedWall();
            MoveWallsProgramatically moveWallsProgramatically = wall.GetComponent<MoveWallsProgramatically>();
            wall.transform.localScale = moveWallsProgramatically.GetWallSize(this.transform.localScale);
            moveWallsProgramatically.SetTarget(this.transform.position, this.transform.localScale);
            moveWallsProgramatically.moveWall = true;
            moveWallsProgramatically.SetIsOnBoard(true);
            eventManager.InvokeLocalPlayerMoved();
            wallPlacedHere = true;
        }
        else
        {
            // Invalid move
        }
    }

    public void SetWallPlacedHere(bool setValue)
    {
        wallPlacedHere = setValue;
    }
    
}
