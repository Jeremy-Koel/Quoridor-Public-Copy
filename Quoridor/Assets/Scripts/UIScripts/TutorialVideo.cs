using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TutorialVideo : MonoBehaviour
{
    private VideoPlayer tutorialVideoPlayer;

    
    // Start is called before the first frame update
    void Start()
    {
        tutorialVideoPlayer = GameObject.Find("TutorialVideoPlayer").GetComponent<VideoPlayer>();
        //tutorialVideoPlayer.clip = Resources.Load<VideoClip>("SampleVideo_640x360_1mb");
        //tutorialVideoPlayer.Play();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
