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
    private bool inLobby;
    private bool inGameBoard;

    private void Awake()
    {
        int numberOfActiveThises = FindObjectsOfType<DisconnectPopup>().Length;
        if (numberOfActiveThises != 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);

        //FindSceneGameObjects();

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
        if (SceneManager.GetActiveScene().name == mainMenuSceneName)
        {
            isCurrentSceneMainMenu = true;
            if (GameObject.Find("Main Camera").GetComponent<MainMenu>().lobbyPanel.activeSelf)
            {
                inLobby = true;
            }
        }
        else if (SceneManager.GetActiveScene().name == gameBoardSceneName)
        {
            isCurrentSceneMainMenu = false;
            inGameBoard = true;
        }

        if (inLobby || inGameBoard)
        {
            SearchForDisconnectPanel();

            ActivateDisconnectPanel();
        }        
    }

    private void SearchForDisconnectPanel()
    {
        IEnumerator sceneObjects = (isCurrentSceneMainMenu) ? SceneManager.GetSceneByName(mainMenuSceneName).GetRootGameObjects().GetEnumerator() :
            SceneManager.GetSceneByName(gameBoardSceneName).GetRootGameObjects().GetEnumerator();

        // NEEDS TO ONLY ACTIVATE IF IT IS ON THE LOBBY OR IN GAME
        bool doneSearching = false;
        while (sceneObjects.MoveNext() && !doneSearching)
        {
            GameObject currentRootGameObject = (GameObject)sceneObjects.Current;
            if (currentRootGameObject.name == "GameController" || currentRootGameObject.name == "Canvas")
            {
                // look for disconnect panel
                var canvasPanelRectTransforms = currentRootGameObject.GetComponentsInChildren<RectTransform>(true);
                for (int index = 0; index < (canvasPanelRectTransforms.Length - 1); index++)
                {
                    var currentCanvasPanelRectTransform = (RectTransform)canvasPanelRectTransforms.GetValue(index);
                    // This is the disconnect panel
                    if (currentCanvasPanelRectTransform.gameObject.name == "DisconnectPanel")
                    {
                        disconnectPanel = currentCanvasPanelRectTransform.gameObject;
                        index = (canvasPanelRectTransforms.Length - 1);
                        doneSearching = true;
                    }
                }
            }
        }
    }

    private void ActivateDisconnectPanel()
    {
        disconnectPanel.SetActive(true);
    }


    private void ReconnectYes()
    {
        // handle reconnection UI logic
    }

    private void ReconnectNo()
    {
        if (inLobby || inGameBoard)
        {
            // handle disconnect UI logic
            if (isCurrentSceneMainMenu)
            {
                // Go to main menu
                MainMenu mainMenu = GameObject.Find("Main Camera").GetComponent<MainMenu>();
                mainMenu.InactivateAllPanels();
                mainMenu.mainMenuPanel.SetActive(true);
            }
            else
            {
                // Send them back to lobby panel
                SceneManager.LoadScene(mainMenuSceneName);
                MainMenu mainMenu = GameObject.Find("Main Camera").GetComponent<MainMenu>();
                mainMenu.InactivateAllPanels();
                mainMenu.lobbyPanel.SetActive(true);
            }
            disconnectPanel.SetActive(false);
        }
    }
}
