using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonsController : MonoBehaviour
{
    private GameObject resumeButton;
    private GameObject mainMenuButton;
    private GameObject newGameButton;
    private GameObject quitButton;

    private void Awake()
    {
        resumeButton = GameObject.Find("ReturnToGameButton");
        mainMenuButton = GameObject.Find("MainMenuButton");
        newGameButton = GameObject.Find("NewGameButton");
        quitButton = GameObject.Find("QuitButton");
    }

    // Start is called before the first frame update
    void Start()
    {
        // If we are in a multiplayer game, we want to hide the "restart" button 
        // as you cannot restart a multiplayer game 
        if (GameSession.GameMode == GameModeEnum.MULTIPLAYER)
        {
            newGameButton.SetActive(false);

            resumeButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0f, 80f, -1f);

            mainMenuButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0f, 0f, -1f);

            quitButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0f, -80f, -1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
