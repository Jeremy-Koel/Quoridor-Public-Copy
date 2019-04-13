using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class WinnerScreen : MonoBehaviour
{
    // Start is called before the first frame update
    //private GameObject winPanel;
    public InterfaceController interfaceController;
    private SoundEffectController soundEffectController;
    private ParticleSystem cheeseGenerator;
    private VideoPlayer winVideoPlayer;
    private VideoClip winClip;
    private VideoClip loseClip;

    private EventManager eventManager;
    private GameObject winScreenTimerText;

    private void OnDestroy()
    {
        eventManager = null;
    }

    private void Awake()
    {
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
        soundEffectController = GameObject.Find("GameController").GetComponent<SoundEffectController>();
        cheeseGenerator = GameObject.Find("CheeseGenerator").GetComponent<ParticleSystem>();
        winVideoPlayer = GameObject.Find("WinVideoPlayer").GetComponent<VideoPlayer>();
        winClip = Resources.Load<VideoClip>("Win Animation");
        loseClip = Resources.Load<VideoClip>("Lose Animation");
    }

    private void timerEnded()
    {
        // Send player back to main menu
        //SceneManager.LoadScene("MainMenu");
        GameObject.Find("LevelChanger").GetComponent<LevelChanger>().FadeToLevel("MainMenu");
    }

    private void updateTimerText()
    {
        winScreenTimerText.GetComponent<Text>().text = GameObject.Find("GameController").GetComponent<DisconnectTimer>().GetTimerValue().ToString();
    }

    void Start()
    {
        interfaceController.TurnIndicatorLightsOff();
        winScreenTimerText = GameObject.Find("WinScreenTimerText");
        if (GameSession.GameMode == GameModeEnum.MULTIPLAYER)
        {
            eventManager.ListenToCountdownTimerValueChanged(updateTimerText);
            eventManager.ListenToCountdownTimer(timerEnded);
            winScreenTimerText.SetActive(true);
        }
        else
        {
            if (winScreenTimerText.activeSelf)
            {
                winScreenTimerText.SetActive(false);
            }            
        }

        RenderTexture texture = GetNewRenderTexture();
        winVideoPlayer.targetTexture = texture;

        RawImage winImage = GameObject.Find("WinImage").GetComponent<RawImage>();
        winImage.texture = texture;

        string winnerString = getWhoWon();
        if (winnerString == "You Win!" || winnerString == interfaceController.GetLocalPlayerName()+" Wins!")
        {
            ++GameSession.WinCount;

            cheeseGenerator.Play();
            soundEffectController.PlayWinSound();
            winVideoPlayer.clip = winClip;
        }
        else
        {
            soundEffectController.PlayLoseSound();

            winVideoPlayer.clip = loseClip;
        }
        
        winVideoPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
       
        //if(controller.IsGameOver())
        //{
        //    //winPanel.transform.position = new Vector3(winPanel.transform.position.x, winPanel.transform.position.y, -2);
        //    winText.text = "Game Over!";
           
        //}
    }

    private string getWhoWon()
    {
        return interfaceController.WhoWon();
    }

    private RenderTexture GetNewRenderTexture()
    {
        RenderTexture texture = new RenderTexture(768, 432, 16);
        texture.Create();
        return texture;
    }
}
