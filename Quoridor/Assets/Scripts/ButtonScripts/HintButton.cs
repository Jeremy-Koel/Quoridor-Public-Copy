using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using UnityEngine.UI;
using TMPro;

public class HintButton : MonoBehaviour
{
    private HighlightSquares highlightSquaresScript;
    private InterfaceController interfaceController;
    private GameCoreController gameCoreController;
    private EventManager eventManager;
    private Button hintButton;
    private ButtonFontTMP hintButtonFont;
    private TextMeshProUGUI hintButtonTextMesh;
    private GameObject hintWallHighlight;
    private GameObject hintSquare;
    private bool clicked = false;
    private Material hintMat;
    private Material highlightMat;
    private int count = 0;
    private string hintString = "";
    public bool flash;

    private void Awake()
    {
        gameCoreController = GameObject.Find("GameController").GetComponent<GameCoreController>();
        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
    }

    private void OnDestroy()
    {
        eventManager = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        highlightSquaresScript = GameObject.Find("GameBoard").GetComponent<HighlightSquares>();
        hintButton = GameObject.Find("HintButton").GetComponent<Button>();
        hintButtonFont = hintButton.transform.GetChild(0).gameObject.GetComponent<ButtonFontTMP>();
        hintMat = Resources.Load("hintColor", typeof(Material)) as Material;
        highlightMat = Resources.Load("highlightColor", typeof(Material)) as Material;
        hintButtonTextMesh = GameObject.Find("HintButton").GetComponentInChildren<TextMeshProUGUI>();
        hintButtonTextMesh.text = "Hint (3)";
    }

    // Update is called once per frame
    void Update()
    {
        if (interfaceController.GetWhoseTurn() != GameBoard.PlayerEnum.ONE || gameCoreController.GetHintsRemaining() < 1)
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

    private void UpdateHintButtonText()
    {
        hintButtonTextMesh.text = "Hint (" + gameCoreController.GetHintsRemaining() + ")";
    }

    public async void OnHintButtonClick()
    {
        if (gameCoreController.GetHintsRemaining() > 0)
        {
            // Show popup
            eventManager.InvokeHintCalcStart();

            // Flag used in Update() 
            clicked = true;

            // Grap hint from game core 
            hintString = await gameCoreController.GetHintForPlayer();
            Debug.Log(hintString);

            // Hide popup 
            eventManager.InvokeHintCalcEnd();

            // Change text of button to reflect how many hints are left 
            UpdateHintButtonText();

            if (hintString.Length < 3)
            {

                highlightSquaresScript.moveHint = hintString;
                hintSquare = GameObject.Find(hintString);
                flash = true;
                Invoke("flashHighlight", .25f);
                Invoke("flashHighlight", .5f);
                Invoke("flashHighlight", .75f);
                Invoke("flashHighlight", 1f);
                Invoke("flashHighlight", 1.25f);

            }
            else
            {
                hintWallHighlight = GameObject.Find(hintString).transform.GetChild(0).gameObject;
                Invoke("flashHighlight", .25f);
                Invoke("flashHighlight", .5f);
                Invoke("flashHighlight", .75f);
                Invoke("flashHighlight", 1f);
                Invoke("flashHighlight", 1.25f);
                hintWallHighlight.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
    }
    
    private void flashHighlight()
    {
        count++;

        if(count%2 != 0)
        {
            if (hintString.Length < 3)
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
            if (hintString.Length < 3)
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
            if (hintString.Length < 3)
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
