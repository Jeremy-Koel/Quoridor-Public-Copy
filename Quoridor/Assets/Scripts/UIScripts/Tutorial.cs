using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Tutorial : MonoBehaviour
{
    private VideoPlayer tutorialVideoPlayer;
    private VideoClip[] tutorialClips;
    private int index = -1;
    private GameObject prevButton;
    private GameObject nextButton;
    private string[] instructList;
    private TMPro.TextMeshProUGUI instructTextBox;
    private GameObject tutorialImage;
    private GameObject logoImage;
    private int numberOfTutorialSlides = 5;
    private GameObject goalText;
    private GameObject cheeseImage;
    // Start is called before the first frame update
    void Start()
    {
        tutorialVideoPlayer = GameObject.Find("TutorialVideoPlayer").GetComponent<VideoPlayer>();
        tutorialClips = Resources.LoadAll<VideoClip>("tutorialVideos");
        logoImage = GameObject.Find("LogoTutorialImage");
        tutorialImage = GameObject.Find("TutorialRawImage");
        // tutorialVideoPlayer.clip = tutorialClips[index];
        tutorialImage.SetActive(false);
        prevButton = GameObject.Find("PreviousSlideButton");
        nextButton = GameObject.Find("NextSlideButton");
        instructTextBox = GameObject.Find("TutorialText").GetComponent<TMPro.TextMeshProUGUI>();
        goalText = GameObject.Find("GoalText");
        cheeseImage = GameObject.Find("CheeseImage");
        cheeseImage.SetActive(false);
        goalText.SetActive(false);
        instructList = createInstructList();
        instructTextBox.text = instructList[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (index == numberOfTutorialSlides - 2 && index > -1)
        {
            nextButton.SetActive(false);
            prevButton.SetActive(true);
            //logoImage.SetActive(false);
            //tutorialImage.SetActive(true);
        }

        if (index == -1 && index < numberOfTutorialSlides - 2)
        {
            prevButton.SetActive(false);
            nextButton.SetActive(true);
            //tutorialImage.SetActive(false);
            //logoImage.SetActive(true);

        }

        if (index > -1 && index < numberOfTutorialSlides - 2)
        {

            nextButton.SetActive(true);
            prevButton.SetActive(true);
        }

        if (numberOfTutorialSlides == 1)
        {
            nextButton.SetActive(false);
            prevButton.SetActive(false);
        }
    }

    public void onPreviousButtonClick()
    {
        index -= 1;

        if(!instructTextBox.IsActive())
        {
            instructTextBox.gameObject.SetActive(true);
        }

        if (index > -2)
        {
            if (index == -1)
            {
                logoImage.SetActive(true);
                tutorialImage.SetActive(false);
                goalText.SetActive(false);
                cheeseImage.SetActive(false);
                instructTextBox.text = instructList[index + 1];
                
            }
            else if (index > -1)
            {
                logoImage.SetActive(false);
                tutorialImage.SetActive(true);
                goalText.SetActive(false);
                cheeseImage.SetActive(false);
                tutorialVideoPlayer.clip = tutorialClips[index];
                instructTextBox.text = instructList[index + 1];
            }
        }
        else
        {
            index = -1;
        }
    }

    public void onNextButtonClick()
    {
        index += 1;

        if (index < numberOfTutorialSlides)
        {
            if (index == numberOfTutorialSlides - 2)
            {
                //logoImage.SetActive(true);
                goalText.SetActive(true);
                cheeseImage.SetActive(true);
                tutorialImage.SetActive(false);
                //instructTextBox.text = instructList[index + 1];
                instructTextBox.gameObject.SetActive(false);
            }
            else
            {
                logoImage.SetActive(false);
                goalText.SetActive(false);
                cheeseImage.SetActive(false);
                tutorialImage.SetActive(true);
                tutorialVideoPlayer.clip = tutorialClips[index];

                instructTextBox.text = instructList[index + 1];
            }

        }
        else
        {
            index = numberOfTutorialSlides - 1;
        }
    }

    private string[] createInstructList()
    {
        string[] temp = new string[numberOfTutorialSlides];

        temp[0] = "Welcome to the Great Gouda Gambit! Can you help our scientists train their lab rats?";
        temp[1] = "To move your mouse, click one of the blue squares. If your mouse reaches the top row of the board, you win!";
        temp[2] = "To place a wall, just hover over the area between two squares and click! You only have 10 walls, so use them wisely!";
       // temp[3] = "If your opponent is directly in front of you, and there isn't a wall behind them, you can jump over them.";
       // temp[4] = "If your opponent is directly in front of you, and there is a wall behind them, you can move diagonally to a space on either side of them.";
        //temp[3] = "When placing walls, you can place them anywhere, UNLESS placing the wall blocks you or your opponent from reaching their goal.";
        temp[3] = "You can slow down your opponent with walls, but you can't completely block them! You must leave them at least one path to the goal!";
        // temp[6] = "Walls also cannot overlap.";
        temp[4] = "If your mouse is the first to reach the other side, then it wins cheese! Good luck!";

        return temp;
    }
}
