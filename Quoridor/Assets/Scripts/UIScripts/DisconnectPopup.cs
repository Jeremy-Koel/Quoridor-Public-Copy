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

    private void OnDestroy()
    {
        eventManager = null;
    }

    private void Awake()
    {
        int numberOfActiveThises = FindObjectsOfType<DisconnectPopup>().Length;
        if (numberOfActiveThises != 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);     
    }

    // Start is called before the first frame update
    void Start()
    {
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        eventManager.ListenToMultiplayerSelected(SetupListeners);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetupListeners()
    {
        if (GameSession.GameMode == GameModeEnum.MULTIPLAYER)
        {
            eventManager.ListenToLostConnection(ActivateDisconnectPanel);
            eventManager.ListenToDisconnectReconnectionYes(ReconnectYes);
            eventManager.ListenToDisconnectReconnectionNo(ReconnectNo);
            eventManager.ListenToDisconnectAIEasy(AiSelectEasy);
            eventManager.ListenToDisconnectAIHard(AiSelectHard);
            FindSceneGameObjects();
        }
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

            //ActivateDisconnectPanel();
        }
    }

    // GenerateMoveForAI()
    private void SearchForDisconnectPanel()
    {
        IEnumerator sceneObjects = (isCurrentSceneMainMenu) ? SceneManager.GetSceneByName(mainMenuSceneName).GetRootGameObjects().GetEnumerator() :
            SceneManager.GetSceneByName(gameBoardSceneName).GetRootGameObjects().GetEnumerator();
        if (disconnectPanel == null)
        {
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
                            if (disconnectionAIDifficultySelectPanel == null)
                            {
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
                            }                            
                            doneSearching = true;
                        }
                    }
                }
            }
        }        
    }

    private void ActivateDisconnectPanel()
    {
        FindSceneGameObjects();
        disconnectPanel.SetActive(true);
        disconnectPanel.GetComponentInChildren<MoveBoards>().moveBoard = true;
        disconnectionAIDifficultySelectPanel.SetActive(false);
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
        GameSession.Difficulty = DifficultyEnum.EASY;
        StartSingleplayer();
    }
    private void AiSelectHard()
    {
        GameSession.Difficulty = DifficultyEnum.HARD;
        StartSingleplayer();
    }

    private void StartSingleplayer()
    {
        GameSession.GameMode = GameModeEnum.SINGLE_PLAYER;
        if (interfaceController.GetWhoseTurn() == GameCore.GameBoard.PlayerEnum.ONE)
        {
            // Let the player move
            GameSession.ForceAiMove = false;
        }
        else
        {
            // have the AI move
            GameSession.ForceAiMove = true;
        }
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
                //SceneManager.LoadScene(mainMenuSceneName);
                GameObject.Find("LevelChanger").GetComponent<LevelChanger>().FadeToLevel(mainMenuSceneName);
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
