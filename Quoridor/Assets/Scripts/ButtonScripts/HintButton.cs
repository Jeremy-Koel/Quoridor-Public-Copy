using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
public class HintButton : MonoBehaviour
{
    public InterfaceController interfaceController;
    public GameCoreController gameCoreController;
    public HighlightSquares highlightSquaresScript;
    private GameObject hintWallHighlight;

    private void Awake()
    {
        gameCoreController = GameObject.Find("GameController").GetComponent<GameCoreController>();
        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        highlightSquaresScript = GameObject.Find("GameBoard").GetComponent<HighlightSquares>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (interfaceController.GetWhoseTurn() != GameBoard.PlayerEnum.ONE)
        {
            hintWallHighlight.GetComponent<SpriteRenderer>().color = Color.clear;
        }
    }

    public async void OnHintButtonClick()
    {
        if (highlightSquaresScript.showHint)
        {
            highlightSquaresScript.showHint = false;
            if (hintWallHighlight != null)
            {
                hintWallHighlight.GetComponent<SpriteRenderer>().color = Color.clear;
            }
        }
        else
        {

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
    }
    
}
