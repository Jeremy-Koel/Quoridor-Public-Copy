using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class OpeningSceneController : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer = GameObject.Find("OpeningVideoPlayer").GetComponent<VideoPlayer>();
        videoPlayer.Prepare();
        videoPlayer.Play();

        Invoke("ChangeScene", 13f);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
