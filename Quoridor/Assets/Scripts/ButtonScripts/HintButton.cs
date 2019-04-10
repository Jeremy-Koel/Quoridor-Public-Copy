using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintButton : MonoBehaviour
{
    public InterfaceController interfaceController;
    public GameCoreController gameCoreController;
    public HighlightSquares highlightSquaresScript;


    private void Awake()
    {
        gameCoreController = GameObject.Find("GameController").GetComponent<GameCoreController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        highlightSquaresScript = GameObject.Find("GameBoard").GetComponent<HighlightSquares>();   
    }

    // Update is called once per frame
    void Update()
    {
        


    }

    public async void OnHintButtonClick()
    {
        if (highlightSquaresScript.showHint)
        {
            highlightSquaresScript.showHint = false;
        }
        else
        {

            string hint = await gameCoreController.GetHintForPlayer();
            //Debug.Log(hint);

            highlightSquaresScript.moveHint = hint;
            highlightSquaresScript.showHint = true;
        }
    }
    
}
