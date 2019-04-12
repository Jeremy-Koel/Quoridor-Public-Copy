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
    private EventManager eventManager;
    private Button hintButton;
    private ButtonFontTMP hintButtonFont;
    private GameObject hintWallHighlight;
    private GameObject hintSquare;
    private bool clicked = false;
    private Material hintMat;
    private Material highlightMat;
    private int count = 0;
    private string hint = "";
    public bool flash;
    private void Awake()
    {
        gameCoreController = GameObject.Find("GameController").GetComponent<GameCoreController>();
        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();

    }

    // Start is called before the first frame update
    void Start()
    {
        highlightSquaresScript = GameObject.Find("GameBoard").GetComponent<HighlightSquares>();
        hintButton = GameObject.Find("HintButton").GetComponent<Button>();
        hintButtonFont = hintButton.transform.GetChild(0).gameObject.GetComponent<ButtonFontTMP>();
        hintMat = Resources.Load("hintColor", typeof(Material)) as Material;
        highlightMat = Resources.Load("highlightColor", typeof(Material)) as Material;
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
        eventManager.InvokeHintCalcStart();
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

        hint = await gameCoreController.GetHintForPlayer();
        eventManager.InvokeHintCalcEnd();
        Debug.Log(hint);

        if (hint.Length < 3)
        {

            highlightSquaresScript.moveHint = hint;
            hintSquare = GameObject.Find(hint);
            flash = true;
            Invoke("flashHighlight", .25f);
            Invoke("flashHighlight", .5f);
            Invoke("flashHighlight", .75f);
            Invoke("flashHighlight", 1f);
            Invoke("flashHighlight", 1.25f);
            //Invoke("flashHighlight", .30f);
            //highlightSquaresScript.showHint = true;

        }
        else
        {
            hintWallHighlight = GameObject.Find(hint).transform.GetChild(0).gameObject;
            Invoke("flashHighlight", .25f);
            Invoke("flashHighlight", .5f);
            Invoke("flashHighlight", .75f);
            Invoke("flashHighlight", 1f);
            Invoke("flashHighlight", 1.25f);
            hintWallHighlight.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
    
   // }
    
    private void flashHighlight()
    {
        count++;

        if(count%2 != 0)
        {
            if (hint.Length < 3)
            {
                //highlightSquaresScript.showHint = false;
                hintSquare.GetComponent<Renderer>().material = hintMat;
                //Invoke("flashHighlight", .50f);
            }
            else
            {
                hintWallHighlight.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
        else
        {
            if (hint.Length < 3)
            {
                //highlightSquaresScript.showHint = true;
                hintSquare.GetComponent<Renderer>().material = highlightMat;
            }
            else
            {
                hintWallHighlight.GetComponent<SpriteRenderer>().color = Color.clear;
            }
        }

        if (count == 5)
        {
            if (hint.Length < 3)
            {
                highlightSquaresScript.showHint = true;
            }
            else
            {
                hintWallHighlight.GetComponent<SpriteRenderer>().color = Color.green;
            }

            flash = false;
            count = 0;
        }


    }
}
