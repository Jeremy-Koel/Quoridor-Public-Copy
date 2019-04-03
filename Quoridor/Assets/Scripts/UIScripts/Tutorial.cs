using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Tutorial : MonoBehaviour
{
    private VideoPlayer tutorialVideoPlayer;
    private VideoClip[] tutorialClips;
    private int index = 0;
    private GameObject prevButton;
    private GameObject nextButton;
    private string[] instructList;
    private Text instructTextBox;

    // Start is called before the first frame update
    void Start()
    {
        tutorialVideoPlayer = GameObject.Find("TutorialVideoPlayer").GetComponent<VideoPlayer>();
        tutorialClips = Resources.LoadAll<VideoClip>("tutorialVideos");

        tutorialVideoPlayer.clip = tutorialClips[index];

        prevButton = GameObject.Find("PreviousSlideButton");
        nextButton = GameObject.Find("NextSlideButton");
        instructTextBox = GameObject.Find("TutorialText").GetComponent<Text>();
        instructList = createInstructList();
        instructTextBox.text = instructList[0];
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
            instructTextBox.text = instructList[index];
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

            instructTextBox.text = instructList[index];
        }
        else
        {
            index = tutorialClips.Length - 1;
        }
    }

    private string[] createInstructList()
    {
        string[] temp = new string[5];

        
        temp[0] = "To move your mouse, click one of the green squares - these show what moves you are allowed to make.";
        temp[1] = "To place a wall, hover over the spot you want to place it and click.";
        temp[2] = "If your opponent is directly in front of you, and there isn't a wall behind them, you can jump over them.";
        temp[3] = "If your opponent is directly in front of you, and there is a wall behind them, you can move diagonally to a space on either side of them.";
        temp[4] = "When placing walls, you can place them anywhere, UNLESS placing the wall blocks you or your opponent from reaching their goal.";

        return temp;
    }
}
