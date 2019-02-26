using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore;

public class PlayerBoxUpdate : MonoBehaviour
{
    const string YOURTURNTEXT = "Moving...";
    const string OTHERPLAYERTEXT = "Waiting on other player...";
    private Text playerOneTurn;
    private Text playerTwoTurn;
    private Text playerOneWallCount;
    private Text playerTwoWallCount;
    private Controller controller;
    private EventManager eventManager;

    // Start is called before the first frame update
    void Start()
    {
        playerOneTurn = GameObject.Find("PlayerOneTurn").GetComponent<Text>();
        playerOneTurn.text = YOURTURNTEXT;
        playerOneTurn.fontStyle = FontStyle.Italic;
        playerTwoTurn = GameObject.Find("PlayerTwoTurn").GetComponent<Text>();
        playerTwoTurn.text = OTHERPLAYERTEXT;
        playerTwoTurn.fontStyle = FontStyle.Italic;
        playerOneWallCount = GameObject.Find("PlayerOneWallsLeft").GetComponent<Text>();
        playerOneWallCount.text = "10";
        playerTwoWallCount = GameObject.Find("PlayerTwoWallsLeft").GetComponent<Text>();
        playerTwoWallCount.text = "10";
        controller = controller = GameObject.Find("GameController").GetComponent<Controller>();
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {
            eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
            eventManager.ListenToChallengeTurnTaken(UpdatePlayerBoxes);
            
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameModeStatus.GameMode == GameModeEnum.MULTIPLAYER)
        {

        }
        else
        {
            GameBoard.PlayerEnum player = controller.GetWhoseTurn();

            if (player == GameBoard.PlayerEnum.ONE)
            {
                playerOneTurn.text = YOURTURNTEXT;
                //playerOneTurn.fontStyle = FontStyle.Normal;
                playerTwoTurn.text = OTHERPLAYERTEXT;
                //playerTwoTurn.fontStyle = FontStyle.Italic;
            }
            else if (player == GameBoard.PlayerEnum.TWO)
            {
                playerTwoTurn.text = YOURTURNTEXT;
                //playerTwoTurn.fontStyle = FontStyle.Normal;
                playerOneTurn.text = OTHERPLAYERTEXT;
                //playerOneTurn.fontStyle = FontStyle.Italic;
            }

            playerOneWallCount.text = controller.GetPlayerWallCount(GameBoard.PlayerEnum.ONE).ToString();
            playerTwoWallCount.text = controller.GetPlayerWallCount(GameBoard.PlayerEnum.TWO).ToString();

        }
    }

    public void UpdatePlayerBoxes()
    {
        playerOneTurn.text = playerOneTurn.text == YOURTURNTEXT ?
            OTHERPLAYERTEXT : YOURTURNTEXT;
        playerTwoTurn.text = playerTwoTurn.text == YOURTURNTEXT ?
            OTHERPLAYERTEXT : YOURTURNTEXT;
        playerOneWallCount.text = controller.GetPlayerWallCount(GameBoard.PlayerEnum.ONE).ToString();
        playerTwoWallCount.text = controller.GetPlayerWallCount(GameBoard.PlayerEnum.TWO).ToString();
    }

}
