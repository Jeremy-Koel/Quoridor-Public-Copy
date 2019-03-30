using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class WinnerScreen : MonoBehaviour
{
    // Start is called before the first frame update
    //private GameObject winPanel;
    public InterfaceController interfaceController;
    private SoundEffectController soundEffectController;
    private VideoPlayer winVideoPlayer;
    private VideoClip winClip;
    private VideoClip loseClip;

    private void Awake()
    {
        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
        soundEffectController = GameObject.Find("GameController").GetComponent<SoundEffectController>();
        winVideoPlayer = GameObject.Find("WinVideoPlayer").GetComponent<VideoPlayer>();
        winClip = Resources.Load<VideoClip>("Win Animation");
        loseClip = Resources.Load<VideoClip>("Lose Animation");
    }

    void Start()
    {
        RenderTexture texture = GetNewRenderTexture();
        winVideoPlayer.targetTexture = texture;

        RawImage winImage = GameObject.Find("WinImage").GetComponent<RawImage>();
        winImage.texture = texture;

        string winnerString = getWhoWon();
        if (winnerString == "You Win!" || winnerString == interfaceController.GetLocalPlayerName()+" Wins!")
        {
            Debug.Log("playing win sound");
            soundEffectController.PlayWinSound();

            winVideoPlayer.clip = winClip;
        }
        else
        {
            Debug.Log("playing lose sound");
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
