using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Tutorial : MonoBehaviour
{
    private VideoPlayer tutorialVideoPlayer;
    private VideoClip[] tutorialClips;
    private int index = 0;
    private GameObject prevButton;
    private GameObject nextButton;

    // Start is called before the first frame update
    void Start()
    {
        tutorialVideoPlayer = GameObject.Find("TutorialVideoPlayer").GetComponent<VideoPlayer>();
        tutorialClips = Resources.LoadAll<VideoClip>("tutorialVideos");

        tutorialVideoPlayer.clip = tutorialClips[index];

        prevButton = GameObject.Find("PreviousSlideButton");
        nextButton = GameObject.Find("NextSlideButton");

    }

    // Update is called once per frame
    void Update()
    {
        if(index == tutorialClips.Length - 1 && index > 0)
        {
            nextButton.SetActive(false);
            prevButton.SetActive(true);
        }

        if (index == 0 && index < tutorialClips.Length - 1)
        {
            prevButton.SetActive(false);
            nextButton.SetActive(true);
        }

        if (index > 0 && index < tutorialClips.Length - 1)
        {
            nextButton.SetActive(true);
            prevButton.SetActive(true);
        }

        if(tutorialClips.Length == 1)
        {
            nextButton.SetActive(false);
            prevButton.SetActive(false);
        }
    }

    public void onPreviousButtonClick()
    {
        index -= 1;

        if (index > -1)
        {
            tutorialVideoPlayer.clip = tutorialClips[index];
        }
        else
        {
            index = 0;
        }
    }

    public void onNextButtonClick()
    {
        index += 1;

        if (index < tutorialClips.Length)
        {
            tutorialVideoPlayer.clip = tutorialClips[index];
        }
        else
        {
            index = tutorialClips.Length - 1;
        }
    }
}
