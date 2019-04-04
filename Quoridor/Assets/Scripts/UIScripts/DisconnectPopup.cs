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
    public GameObject disconnectionAIDifficultySelectPanel;
    private bool disconnectionAIDiffPanelFound;
    private bool isCurrentSceneMainMenu;
    private EventManager eventManager;
    private bool inLobby;
    private bool inGameBoard;

    private InterfaceController interfaceController;

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
        eventManager.ListenToDisconnectAIEasy(AiSelectEasy);
        eventManager.ListenToDisconnectAIHard(AiSelectHard);
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

    // GenerateMoveForAI()
    private void SearchForDisconnectPanel()
    {
        IEnumerator sceneObjects = (isCurrentSceneMainMenu) ? SceneManager.GetSceneByName(mainMenuSceneName).GetRootGameObjects().GetEnumerator() :
            SceneManager.GetSceneByName(gameBoardSceneName).GetRootGameObjects().GetEnumerator();

        if (!isCurrentSceneMainMenu)
        {
            interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
        }

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
                        var disconnectPanelRectTransforms = disconnectPanel.GetComponentsInChildren<RectTransform>(true).GetEnumerator();
                        while (disconnectPanelRectTransforms.MoveNext())
                        {
                            var currentDisconnectPanelRectTransform = (RectTransform)disconnectPanelRectTransforms.Current;
                            if (currentDisconnectPanelRectTransform.name == "DisconnectionAIDifficultySelectPanel")
                                //if (currentCanvasPanelRectTransform.gameObject.name == "DisconnectionAIDifficultySelectPanel")
                            {
                                disconnectionAIDifficultySelectPanel = currentDisconnectPanelRectTransform.gameObject;
                                disconnectionAIDiffPanelFound = true;
                            }                            
                        }                        
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

    private void ActivateDisconnectionAiSelectPanel()
    {
        if (disconnectionAIDiffPanelFound)
        {
            disconnectionAIDifficultySelectPanel.SetActive(true);
        }
    }

    private void AiSelectEasy()
    {
        if (interfaceController.GetWhoseTurn() == GameCore.GameBoard.PlayerEnum.ONE) {
            // Let the player move
            //interfaceController.RecordLocalPlayerMove("");
        }
        else
        {
            // have the AI move
            //interfaceController.GenerateMoveForAI();
        }
        
        GameSession.Difficulty = DifficultyEnum.EASY;
        GameSession.GameMode = GameModeEnum.SINGLE_PLAYER;
        // make panels inactive
        disconnectionAIDifficultySelectPanel.SetActive(false);
        disconnectPanel.SetActive(false);
    }
    private void AiSelectHard()
    {
        GameSession.Difficulty = DifficultyEnum.HARD;
        GameSession.GameMode = GameModeEnum.SINGLE_PLAYER;
        // make panels inactive
        disconnectionAIDifficultySelectPanel.SetActive(false);
        disconnectPanel.SetActive(false);
    }

    private void ReconnectYes()
    {
        // handle reconnection UI logic
        // SWITCH TO AI GAME
        SearchForDisconnectPanel();
        ActivateDisconnectionAiSelectPanel();

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
                // Sends back to main menu
                SceneManager.LoadScene(mainMenuSceneName);
                // Send them back to lobby panel
                //if (SceneManager.GetActiveScene().name == mainMenuSceneName)
                //{
                //    MainMenu mainMenu = GameObject.Find("Main Camera").GetComponent<MainMenu>();
                //    mainMenu.InactivateAllPanels();
                //    mainMenu.lobbyPanel.SetActive(true);
                //}
            }
            disconnectPanel.SetActive(false);
        }
    }
}
