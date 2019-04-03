using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisconnectPopup : MonoBehaviour
{
    public bool disconnected;
    private const string mainMenuSceneName = "MainMenu";
    private const string gameBoardSceneName = "GameBoard";
    public GameObject disconnectPanel;
    private bool isCurrentSceneMainMenu;
    private EventManager eventManager;

    private void Awake()
    {
        int numberOfActiveThises = FindObjectsOfType<DisconnectPopup>().Length;
        if (numberOfActiveThises != 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);

        FindSceneGameObjects();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        eventManager.ListenToLostConnection(FindSceneGameObjects);
        eventManager.ListenToDisconnectReconnectionYes(ReconnectYes);
        eventManager.ListenToDisconnectReconnectionNo(ReconnectNo);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // We need a different panel if we are in the main menu vs game board
    private void FindSceneGameObjects()
    {
        disconnectPanel = GameObject.Find("DisconnectPanel");

        if (SceneManager.GetActiveScene().name == mainMenuSceneName)
        {
            isCurrentSceneMainMenu = true;
        }
        else if (SceneManager.GetActiveScene().name == gameBoardSceneName)
        {
            isCurrentSceneMainMenu = false;
        }
    }

    private void ReconnectYes()
    {
        // handle reconnection UI logic
    }

    private void ReconnectNo()
    {
        // handle disconnect UI logic
        if (isCurrentSceneMainMenu)
        {
            // 
        }
        else
        {
            // Send them back to lobby panel
            SceneManager.LoadScene(mainMenuSceneName);
            MainMenu mainMenu = GameObject.Find("Main Camera").GetComponent<MainMenu>();
            mainMenu.lobbyPanel.SetActive(true);
        }
    }
}
