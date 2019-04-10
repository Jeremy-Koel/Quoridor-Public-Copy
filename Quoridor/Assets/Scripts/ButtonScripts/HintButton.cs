using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using UnityEngine.UI;

public class HintButton : MonoBehaviour
{
    private HighlightSquares highlightSquaresScript;
    private InterfaceController interfaceController;
    private GameCoreController gameCoreController;
    private Button hintButton;
    private ButtonFontTMP hintButtonFont;
    private GameObject hintWallHighlight;
    private bool clicked = false;


    private void Awake()
    {
        gameCoreController = GameObject.Find("GameController").GetComponent<GameCoreController>();
        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        highlightSquaresScript = GameObject.Find("GameBoard").GetComponent<HighlightSquares>();
        hintButton = GameObject.Find("HintButton").GetComponent<Button>();
        hintButtonFont = hintButton.transform.GetChild(0).gameObject.GetComponent<ButtonFontTMP>();
    }

    // Update is called once per frame
    void Update()
    {
        if (interfaceController.GetWhoseTurn() != GameBoard.PlayerEnum.ONE)
        {
            if (hintWallHighlight != null)
            {
                hintWallHighlight.GetComponent<SpriteRenderer>().color = Color.clear;
                hintWallHighlight = null;
            }

            clicked = false;
            hintButton.interactable = false;
            hintButtonFont.SetFontToNormal();
            hintButtonFont.enabled = false;
            
        }
        else
        {
            if (!clicked)
            {
                hintButton.interactable = true;
                hintButtonFont.enabled = true;
            }
            else
            {
                hintButton.interactable = false;
                hintButtonFont.SetFontToNormal();
                hintButtonFont.enabled = false;
            }
        }
    }

    public async void OnHintButtonClick()
    {
        //if (highlightSquaresScript.showHint)
        //{
        //    highlightSquaresScript.showHint = false;
        //    if (hintWallHighlight != null)
        //    {
        //        hintWallHighlight.GetComponent<SpriteRenderer>().color = Color.clear;
        //    }
        //}
        //else
        //{

        //if (!highlightSquaresScript.showHint && hintWallHighlight == null && interfaceController.GetWhoseTurn() == GameBoard.PlayerEnum.ONE)
        //{

        clicked = true;

            string hint = await gameCoreController.GetHintForPlayer();
            Debug.Log(hint);

            if (hint.Length < 3)
            {

                highlightSquaresScript.moveHint = hint;
                highlightSquaresScript.showHint = true;
            }
            else
            {
                hintWallHighlight = GameObject.Find(hint).transform.GetChild(0).gameObject;
                hintWallHighlight.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
    
   // }
    
}
