using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject singlePlayerSetupPanel;
    public GameObject settingsPanel;
    public GameObject loginPanel;
    //public GameObject registrationPanel;
    public GameObject lobbyPanel;
    public GameObject chatWindowPanel;
    public GameObject leaderboardPanel;
    public GameObject tutorialPanel;
    public GameObject dummyMenuPanel;
    public GameObject disconnectPanel;
    public GameObject guestDetailsPanel;
    //  public GameObject quitPanel;
  //  public Stack<GameObject> panelOrder;

    public GameObject challengeManager;
    public EventManager eventManager;

    private bool lobbyActivatedOnce;
    private bool matching;
    private bool adminLogin = false;
    private string adminUsername = "ADMIN";
    private string adminPassword = "PASSWORD";

    public AudioControllerMainMenu audioController;

    [SerializeField]
    private TMPro.TMP_InputField usernameLoginInput;
    [SerializeField]
    private TMPro.TMP_InputField passwordLoginInput;
    [SerializeField]
    private TMPro.TMP_InputField displayNameInput;
    //[SerializeField]
    //private InputField usernameRegisterInput;
    //[SerializeField]
    //private InputField passwordRegisterInput;
    [SerializeField]
    private Button loginButton;
    //[SerializeField]
    //private Button registerButton;
    [SerializeField]
    private TMPro.TextMeshProUGUI errorMessageLoginText;
    //[SerializeField]
    //private TMPro.TextMeshProUGUI errorMessageRegistrationText;
    [SerializeField]
    private Button matchmakingButton;
    [SerializeField]
    private Button joinGameButton;
    [SerializeField]
    private Button hostGameButton;

    public Button singlePlayerButton;
    public Button multiPlayerButton;
    public Button tutorialButton;
    public Button settingsButton;

    public TMPro.TextMeshProUGUI findingMatchText; 

    private bool usernameSelected = false;
    private bool passwordSelected = false;

    private void Awake()
    {
        Debug.Log("This is the current timescale: " + Time.timeScale);
        ScriptMessage_GuestAccountDetails.Listener += OnGuestAccountDetails;
        ChallengeStartedMessage.Listener += OnChallengeStarted;
        ChallengeIssuedMessage.Listener += OnChallengeIssued;
        if (chatWindowPanel == null)
        {
            chatWindowPanel = GameObject.Find("ChatWindowPanel");
        }        
        challengeManager = GameObject.Find("ChallengeManager");
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        matchmakingButton = GameObject.Find("MatchmakingSearchButton").GetComponent<Button>();
        audioController = GameObject.Find("MusicPlayer").GetComponent<AudioControllerMainMenu>();

        //usernameLoginInput.OnSelect.//AddListener("onUsernameSelect");
        if (GameSession.FastAnimations)
        {
            GameObject.Find("FastAnimToggle").GetComponent<Toggle>().isOn = true;
            GameObject.Find("SlowAnimToggle").GetComponent<Toggle>().isOn = false;
        }
        else
        {
            GameObject.Find("FastAnimToggle").GetComponent<Toggle>().isOn = false;
            GameObject.Find("SlowAnimToggle").GetComponent<Toggle>().isOn = true;
        }
    }

    void OnDestroy()
    {
        ChallengeStartedMessage.Listener -= OnChallengeStarted;
        eventManager = null;
        challengeManager = null;
        GameObject.Destroy(lobbyPanel);
    }

    // Start is called before the first frame update
    void Start()
    {
        mainMenuPanel = GameObject.Find("MainButtonPanel");
        singlePlayerSetupPanel = GameObject.Find("SinglePlayerSetupPanel");
        settingsPanel = GameObject.Find("SettingsPanel");
        loginPanel = GameObject.Find("LoginPanel");
        //registrationPanel = GameObject.Find("RegistrationPanel");
        lobbyPanel = GameObject.Find("LobbyPanel");
        tutorialPanel = GameObject.Find("TutorialPanel");
        
        dummyMenuPanel = GameObject.Find("DummyMenuPanel");
        disconnectPanel = GameObject.Find("DisconnectPanel");
        guestDetailsPanel = GameObject.Find("GuestDetailsPanel");
        // quitPanel = GameObject.Find("QuitPanel");

        GameSparksManager gsm = GameObject.Find("GameSparksManager").GetComponent<GameSparksManager>();
        gsm.connectedValueChanged.AddListener(OnLoginNoConnection);

        // panelOrder = new Stack<GameObject>();
        // panelOrder.Push(mainMenuPanel);
        findingMatchText.color = ColorPalette.invisible;
        mainMenuPanel.SetActive(true);
        dummyMenuPanel.SetActive(false);
        singlePlayerSetupPanel.SetActive(false);
        settingsPanel.SetActive(false);
        loginPanel.SetActive(false);
        //registrationPanel.SetActive(false);
        lobbyPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        disconnectPanel.SetActive(false);
        guestDetailsPanel.SetActive(false);

        mainMenuPanel.GetComponent<MoveMainMenuBoard>().moveBoard = true;
        matching = false;

        if (challengeManager.GetComponent<ChallengeManager>().ChallengeID != null)
        {
            challengeManager.GetComponent<ChallengeManager>().ChallengeGameLost();
        }
        
        //CancelMatchmaking();

        var disconnectPopup = GameObject.Find("DisconnectPopup").GetComponent<DisconnectPopup>();
        disconnectPopup.showPopup = true;
        disconnectPopup.disconnectLock = false;
        if (disconnectPopup.ScheduledReconnect())
        {
            onMultiplayerClick();
            //Login(GameSparksUserID.currentUsername, GameSparksUserID.currentPassword);
            //disconnectPopup.scheduledReconnect = false;
        }

    }

    private void Update()
    {
        if (Input.GetAxisRaw("Submit") == 1)
        {
            if (loginPanel.activeSelf)
            {
                Login(usernameLoginInput.text, passwordLoginInput.text);
            }
        }
        if (Input.GetAxisRaw("Tab") == 1)
        {
            if (loginPanel.activeSelf)
            {
                if (usernameLoginInput.isFocused)
                {
                    passwordLoginInput.Select();
                }
            }
        }

    }

    private void onUsernameSelect()
    {
        usernameSelected = true;
    }

    public void DeselectCurrentButton()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            var currentButtonText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>();
            currentButtonText.fontStyle = FontStyles.Normal;
        }
    }

    public void onSinglePlayerClick()
    {
        DeselectCurrentButton();
        audioController.PlayChalkWritingSound();        
        // Set session to singleplayer 
        GameSession.GameMode = GameModeEnum.SINGLE_PLAYER;

        mainMenuPanel.SetActive(false);
        singlePlayerSetupPanel.SetActive(true);
      //  panelOrder.Push(singlePlayerSetupPanel);
    }

    public void onMultiplayerClick()
    {
        DeselectCurrentButton();
        audioController.PlayChalkWritingSound();
        // Set session to multiplayer 
        GameSession.GameMode = GameModeEnum.MULTIPLAYER;
        eventManager.InvokeMultiplayerSelected();
        //mainMenuPanel.SetActive(false);

        if (LoggedIn())
        {
            //dummyMenuPanel.SetActive(true);
            mainMenuPanel.SetActive(true);
            SetMainMenuButtonsInactive();
            dummyMenuPanel.SetActive(true);
            lobbyPanel.SetActive(true);
            lobbyPanel.GetComponent<MoveMultiplayerScreen>().moveBoard = true;
            audioController.PlayChalkboardMovingSound();
            //panelOrder.Push(lobbyPanel);
        }
        else
        {
            mainMenuPanel.SetActive(false);
            loginPanel.SetActive(true);
           // panelOrder.Push(loginPanel);
        }
        var disconnectPopup = GameObject.Find("DisconnectPopup").GetComponent<DisconnectPopup>();
        if (disconnectPopup.ScheduledReconnect())
        {
            disconnectPopup.scheduledReconnect = false;
            Login(GameSparksUserID.currentUsername, GameSparksUserID.currentPassword);
        }
        leaderboardPanel.GetComponent<LeaderboardPanel>().GetLeaderboardData();
    }

    private void OnGuestAccountDetails(ScriptMessage_GuestAccountDetails message)
    {
        // Decode data
        string guestName = message.Data.GetString("name");
        // Log user in
        adminLogin = false;
        Login(guestName, "password");
        if (guestDetailsPanel == null)
        {
            var objectsOfGDP = Resources.FindObjectsOfTypeAll(typeof(GuestDetailsPanel));
            var objectsEnumer = objectsOfGDP.GetEnumerator();
            objectsEnumer.MoveNext();
            var gameObjectC = (GuestDetailsPanel)objectsEnumer.Current;
            guestDetailsPanel = gameObjectC.gameObject;
        }
        // Tell them their guest name
        var textsEnum = guestDetailsPanel.GetComponentsInChildren<TMPro.TextMeshProUGUI>().GetEnumerator();
        while (textsEnum.MoveNext())
        {
            var currentText = (TextMeshProUGUI)textsEnum.Current;
            if (currentText.name == "GuestNameText")
            {
                currentText.text = guestName;
            }
        }
        guestDetailsPanel.SetActive(true);
    }

    public void OnGuestDetailsOkay()
    {
        guestDetailsPanel.SetActive(false);
    }

    public void OnSkipLogin()
    {
        // log in as admin so we can communicate with server
        adminLogin = true;
        LoginAsAdmin();

        LogEventRequest_GetNewGuestUser getNewGuestUser = new LogEventRequest_GetNewGuestUser();
        getNewGuestUser.Send(OnGetNewGuestUserSuccess);
    }

    private void OnGetNewGuestUserSuccess(LogEventResponse message)
    {
        Debug.Log("New guest user successful");
        adminLogin = false;
    }

    public void onLoginClick()
    {
        DeselectCurrentButton();
        // Try to login using username and password
        if (usernameLoginInput.text != "")
        {
            if (passwordLoginInput.text != "")
            {
                //Login(usernameLoginInput.text, passwordLoginInput.text);
                Register();
            }
        }        
    }

    public void onFastAnimClick()
    {
        GameSession.FastAnimations = GameObject.Find("FastAnimToggle").GetComponent<Toggle>().isOn;
    }

    //public void onRegistrationSwitchClick()
    //{
    //    usernameRegisterInput.text = usernameLoginInput.text;
    //    passwordRegisterInput.text = passwordLoginInput.text;
    //    loginPanel.SetActive(false);

    //    registrationPanel.SetActive(true);
    //    panelOrder.Push(registrationPanel);
    //}

    //public void onRegistrationClick()
    //{
    //    usernameLoginInput.text = usernameRegisterInput.text;
    //    passwordLoginInput.text = passwordRegisterInput.text;
    //    // Try to register new user account using displayname, username, and password
    //    Register();
    //}

    public void onEasyButtonClick()
    {
        DeselectCurrentButton();
        GameSession.Difficulty = DifficultyEnum.EASY;
        SceneManager.LoadScene("GameBoard");
    }

    public void onHardButtonClick()
    {
        DeselectCurrentButton();
        GameSession.Difficulty = DifficultyEnum.HARD;
        //SceneManager.LoadScene("GameBoard");
        GameObject.Find("LevelChanger").GetComponent<LevelChanger>().FadeToLevel("GameBoard");
    }

    public void onPlayButtonClick()
    {
        DeselectCurrentButton();
        audioController.PlayChalkWritingSound();

        Toggle hardOption = GameObject.Find("HardOption").GetComponent<Toggle>();
        Toggle easyOption = GameObject.Find("EasyOption").GetComponent<Toggle>();
        Toggle goFirst = GameObject.Find("MoveFirst").GetComponent<Toggle>();
        Toggle goSecond = GameObject.Find("MoveSecond").GetComponent<Toggle>();
        Toggle goRandom = GameObject.Find("MoveRandom").GetComponent<Toggle>();

        if (hardOption.isOn)
        {
            GameSession.Difficulty = DifficultyEnum.HARD;
        }
        else if(easyOption.isOn)
        {
            GameSession.Difficulty = DifficultyEnum.EASY;
        }

        if(goFirst.isOn)
        {
            GameSession.PlayerTurnPref = PlayerTurnEnum.FIRST;
        }
        else if(goSecond.isOn)
        {
            GameSession.PlayerTurnPref = PlayerTurnEnum.SECOND;
        }
        else if(goRandom.isOn)
        {
            GameSession.PlayerTurnPref = PlayerTurnEnum.RANDOM;
        }

        //ActivateAllPanels();
        // SceneManager.LoadScene("GameBoard");
        GameObject.Find("LevelChanger").GetComponent<LevelChanger>().FadeToLevel("GameBoard");
    }

    public void onSettingsButtonClick(Button button)
    {
        DeselectCurrentButton();
        audioController.PlayChalkWritingSound();
        mainMenuPanel.SetActive(false);
        dummyMenuPanel.SetActive(false);
        singlePlayerSetupPanel.SetActive(false);
        settingsPanel.SetActive(true);
       // panelOrder.Push(settingsPanel);
    }

    public void onTutorialButtonClick(Button button)
    {
        DeselectCurrentButton();
        audioController.PlayChalkWritingSound();
       // mainMenuPanel.SetActive(false);
        singlePlayerSetupPanel.SetActive(false);
        settingsPanel.SetActive(false);
        dummyMenuPanel.SetActive(true);
        tutorialPanel.SetActive(true);
        tutorialPanel.GetComponent<MoveTutorialBoard>().moveBoard = true;
      //  panelOrder.Push(tutorialPanel);
    }

    public void onBackButtonClick()
    {
        //string name = EventSystem.current.currentSelectedGameObject.name;
        //DeselectCurrentButton();
      //  GameObject disableScreen = panelOrder.Pop();
       // GameObject enableScreen = panelOrder.Peek();
       // usernameLoginInput.text = "";
       // passwordLoginInput.text = "";
       if(singlePlayerSetupPanel.activeSelf)
        {
            singlePlayerSetupPanel.SetActive(false);
        }
       else if(loginPanel.activeSelf)
        {
            loginPanel.SetActive(false);
        }
       else if(settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
        }
        mainMenuPanel.SetActive(true);
        //if (disableScreen.name != "TutorialPanel" && disableScreen.name != "LobbyPanel")
        //{
        //    disableScreen.SetActive(false);
        //}


        //enableScreen.SetActive(true);
    }

    public void onTutorialBackButtonClick()
    {
        //DeselectCurrentButton();
        tutorialPanel.GetComponent<MoveTutorialBoard>().moveBoard = true;
        dummyMenuPanel.SetActive(false);
        onBackButtonClick();
    }

    public void onLobbyBackButtonClick()
    {
        // cancel any current matchmaking going on
        lobbyPanel.GetComponent<MoveMultiplayerScreen>().moveBoard = true;
       
        lobbyPanel.GetComponentInChildren<HostedGameLobbies>().CancelHosting();
        CancelMatchmaking();
        
        audioController.PlayChalkboardMovingSound();
        //if(panelOrder.Peek().name == "LoginPanel")
        //{
        //    panelOrder.Pop();
        //}
        //onBackButtonClick();
    }

    public void CancelMatchmaking()
    {
        if (matching)
        {
            findingMatchText.color = ColorPalette.invisible;
            matchmakingButton.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Find Match";
            Debug.Log("Making/sending matchmaking request");
            MatchmakingRequest request = new MatchmakingRequest();
            request.SetAction("cancel");
            request.SetMatchShortCode("DefaultMatch");
            request.SetSkill(0);
            request.Send(OnMatchmakingSuccess, OnMatchmakingError);
            matching = false;
            UnblockMatchInput();
            UnblockJoinHostGameButtons();
        }
    }

    public void onMatchMakingButtonClick()
    {
        BlockMatchInput();
        BlockJoinHostGameButtons();
        if (matching)
        {
            findingMatchText.color = ColorPalette.invisible;
            matchmakingButton.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Find Match";
            Debug.Log("Making/sending matchmaking request");
            MatchmakingRequest request = new MatchmakingRequest();
            request.SetAction("cancel");
            request.SetMatchShortCode("DefaultMatch");
            request.SetSkill(0);
            request.Send(OnMatchmakingSuccess, OnMatchmakingError);
            matching = false;
            UnblockMatchInput();
            UnblockJoinHostGameButtons();
        }
        else
        {
            findingMatchText.color = ColorPalette.white;
            matchmakingButton.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Stop";
            Debug.Log("Making/sending matchmaking request");
            MatchmakingRequest request = new MatchmakingRequest();
            request.SetMatchShortCode("DefaultMatch");
            request.SetSkill(0);
            request.Send(OnMatchmakingSuccess, OnMatchmakingError);
            matching = true;
            UnblockMatchInput();
        }        
    }

    public void OnMatchmakingSuccess(MatchmakingResponse response)
    {
        UnblockMatchInput();
        Debug.Log("Matchmaking Success");
    }

    public void OnMatchmakingError(MatchmakingResponse response)
    {
        UnblockMatchInput();
        Debug.Log("Matchmaking Error");
    }

    private void OnChallengeIssued(ChallengeIssuedMessage message)
    {
        Debug.Log("On Challenge Issued");
        var challengeInstaceId = message.Challenge.ChallengeId;
        Debug.Log("This challenge ID: " + challengeInstaceId);
        if (challengeInstaceId != null)
        {
            new AcceptChallengeRequest()
                .SetChallengeInstanceId(challengeInstaceId)
                //.SetMessage(message)
                .Send((response) => {
                    //string challengeInstanceId = response.ChallengeInstanceId;
                    //GSData scriptData = response.ScriptData;
                });
        }
    }

    private void OnChallengeStarted(ChallengeStartedMessage message)
    {
        UnblockMatchInput();
        HostedGameLobbies hostedGameLobbiesScript = GameObject.Find("HostedGameLobbies").GetComponent<HostedGameLobbies>();
        hostedGameLobbiesScript.RemoveRefreshHostedGamesListener();

        Debug.Log("Challenge Started");
        // Switch to GameBoard Scene connected to opponent
        // SceneManager.LoadScene("GameBoard");
        if (chatWindowPanel == null)
        {
            //chatWindowPanel = GameObject.Find("ChatWindowPanel");
            var objectsChatWindow = Resources.FindObjectsOfTypeAll(typeof(ChatWindowPanel));
            var objectsEnumer = objectsChatWindow.GetEnumerator();
            objectsEnumer.MoveNext();
            var gameObjectChat = (ChatWindowPanel)objectsEnumer.Current;
            chatWindowPanel = gameObjectChat.gameObject;
        }
        //chatWindowPanel.GetComponent<ChatWindowPanel>().ClearAllFriendsChats();
        GameObject.Find("LevelChanger").GetComponent<LevelChanger>().FadeToLevel("GameBoard");
    }

    // Login/Registration
    public void Login(string username, string password)
    {
        BlockLoginInput();
        GameSparksManager gsm = GameObject.Find("GameSparksManager").GetComponent<GameSparksManager>();
        gsm.CheckInternetConnection();

        GameSparksUserID.currentUsername = username;
        GameSparksUserID.currentPassword = password;
        AuthenticationRequest request = new AuthenticationRequest();
        request.SetUserName(username);
        request.SetPassword(password);
        request.Send(OnLoginSuccess, OnLoginError);
    }

    public void LoginFromReconnect()
    {
        AuthenticationRequest request = new AuthenticationRequest();
        request.SetUserName(GameSparksUserID.currentUsername);
        request.SetPassword(GameSparksUserID.currentPassword);
        request.Send(OnLoginReconnectSuccess, OnLoginReconnectError);
    }

    private void OnLoginReconnectSuccess(AuthenticationResponse response)
    {
        // do we need to do anything?
    }

    private void OnLoginReconnectError(AuthenticationResponse response)
    {
        // update disconnect error text
    }

    private void LoginAsAdmin()
    {
        BlockLoginInput();
        AuthenticationRequest request = new AuthenticationRequest();
        request.SetUserName(adminUsername);
        request.SetPassword(adminPassword);
        request.Send(OnAdminLoginSuccess, OnLoginError);
    }

    private void OnAdminLoginSuccess(AuthenticationResponse response)
    {
        UnblockLoginInput();
        Debug.Log("logged in as admin");
    }

    private void OnLoginSuccess(AuthenticationResponse response)
    {
        UnblockLoginInput();
        GameSparksManager gsm = GameObject.Find("GameSparksManager").GetComponent<GameSparksManager>();
        gsm.loggedInOnce = true;
        errorMessageLoginText.color = ColorPalette.invisible;
        if (adminLogin)
        {
            Debug.Log("Logged in as admin");
        }
        else
        {
            // Set the User's ID to an object for referencing later
            Debug.Log(response.UserId);
            GameObject gameSparksUserIDObject = GameObject.Find("GameSparksUserID");
            GameSparksUserID gameSparksUserIDScript = gameSparksUserIDObject.GetComponent<GameSparksUserID>();
            gameSparksUserIDScript.myUserID = response.UserId;
            gameSparksUserIDScript.myDisplayName = response.DisplayName;
            // Setup ChatWindowPanel
            if (chatWindowPanel == null)
            {
                //chatWindowPanel = GameObject.Find("ChatWindowPanel");
                var objectsChatWindow = Resources.FindObjectsOfTypeAll(typeof(ChatWindowPanel));
                var objectsEnumer = objectsChatWindow.GetEnumerator();
                objectsEnumer.MoveNext();
                var gameObjectChat = (ChatWindowPanel)objectsEnumer.Current;
                chatWindowPanel = gameObjectChat.gameObject;
            }
            ChatWindowPanel chatWindowPanelScript = chatWindowPanel.GetComponent<ChatWindowPanel>();
            chatWindowPanelScript.CheckTeams();
            chatWindowPanel.GetComponent<RectTransform>().ForceUpdateRectTransforms();
            // Setup ChallengeManager
            ChallengeManager challengeManagerScript = challengeManager.GetComponent<ChallengeManager>();
            challengeManagerScript.SetupChallengeListeners();

            if (loginPanel == null)
            {
                loginPanel = GameObject.Find("LoginPanel");
            }
            if (lobbyPanel == null)
            {
                var objectsOfMoveMS = Resources.FindObjectsOfTypeAll(typeof(MoveMultiplayerScreen));
                var objectsEnumer = objectsOfMoveMS.GetEnumerator();
                objectsEnumer.MoveNext();
                var gameObjectC = (MoveMultiplayerScreen)objectsEnumer.Current;
                lobbyPanel = gameObjectC.gameObject;
            }
            if (dummyMenuPanel == null)
            {
                var objectsOfDMP = Resources.FindObjectsOfTypeAll(typeof(DummyMenuPanel));
                var objectsEnumer = objectsOfDMP.GetEnumerator();
                objectsEnumer.MoveNext();
                var gameObjectC = (DummyMenuPanel)objectsEnumer.Current;
                dummyMenuPanel = gameObjectC.gameObject;
            }
            if (audioController == null)
            {
                var objectsOfAC = Resources.FindObjectsOfTypeAll(typeof(AudioControllerMainMenu));
                var objectsEnumer = objectsOfAC.GetEnumerator();
                objectsEnumer.MoveNext();
                audioController = (AudioControllerMainMenu)objectsEnumer.Current;
                //audioController = gameObjectC.gameObject;
            }

            // Switch to the Lobby Panel
            loginPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
            SetMainMenuButtonsInactive();
            dummyMenuPanel.SetActive(true);
            lobbyPanel.SetActive(true);
            lobbyPanel.GetComponent<MoveMultiplayerScreen>().moveBoard = true;
            audioController.PlayChalkboardMovingSound();
            OnLeaderboardsClick();
        }
    }

    private void SetMainMenuButtonsInactive()
    {
        singlePlayerButton.interactable = false;
        multiPlayerButton.interactable = false;
        tutorialButton.interactable = false;
        settingsButton.interactable = false;
    }
    public void SetMainMenuButtonsActive()
    {
        singlePlayerButton.interactable = true;
        multiPlayerButton.interactable = true;
        tutorialButton.interactable = true;
        settingsButton.interactable = true;
    }

    private void OnLoginError(AuthenticationResponse response)
    {
        UnblockLoginInput();
        if (GameObject.Find("GameSparksManager").GetComponent<GameSparksManager>().connected)
        {
            errorMessageLoginText.color = ColorPalette.maroonRed;
            var errorText = "TAKEN";
            errorMessageLoginText.text = errorText;
        }
    }

    private void OnLoginNoConnection()
    {
        if (!GameObject.Find("GameSparksManager").GetComponent<GameSparksManager>().connected) {
            errorMessageLoginText.color = ColorPalette.maroonRed;
            var errorText = "No connection...";
            errorMessageLoginText.text = errorText;
        }
        else
        {
            errorMessageLoginText.color = ColorPalette.invisible;
        }
    }

    private bool LoggedIn()
    {
        bool loggedIn = false;
        GameSparksManager gameSparksManager = GameObject.Find("GameSparksManager").GetComponent<GameSparksManager>();
        //loggedIn = gameSparksManager.connected;
        GameSparksUserID gameSparksUserID = GameObject.Find("GameSparksUserID").GetComponent<GameSparksUserID>();
        if (gameSparksUserID.myUserID != "" && gameSparksManager.connected && !adminLogin)
        {
            loggedIn = true;
        }
        return loggedIn;
    }

    private void Register()
    {
        BlockLoginInput();
        RegistrationRequest request = new RegistrationRequest();
        request.SetUserName(usernameLoginInput.text);
        request.SetDisplayName(usernameLoginInput.text);
        request.SetPassword(passwordLoginInput.text);
        request.Send(OnRegistrationSuccess, OnRegistrationError);
    }

    private void OnRegistrationSuccess(RegistrationResponse response)
    {
        Login(usernameLoginInput.text, passwordLoginInput.text);
    }

    private void OnRegistrationError(RegistrationResponse response)
    {
        UnblockLoginInput();
        Login(usernameLoginInput.text, passwordLoginInput.text);
    }

    private void OnLeaderboardsClick()
    {
        //LeaderboardPanel leaderboardPanelScript = GameObject.Find("LeaderboardPanel").GetComponent<LeaderboardPanel>();
        //leaderboardPanelScript.onRefreshLeaderboardButtonClick();
    }

    public void InactivateAllPanels()
    {
        mainMenuPanel.SetActive(false);
        singlePlayerSetupPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        settingsPanel.SetActive(false);
        loginPanel.SetActive(false);
        lobbyPanel.SetActive(false);
    }

    public void ActivateAllPanels()
    {
        mainMenuPanel.SetActive(true);
        singlePlayerSetupPanel.SetActive(true);
        tutorialPanel.SetActive(true);
        settingsPanel.SetActive(true);
        loginPanel.SetActive(true);
        lobbyPanel.SetActive(true);
    }

    private void BlockLoginInput()
    {
        if (GameObject.Find("LoginButton") != null)
        {
            loginButton = GameObject.Find("LoginButton").GetComponent<Button>();
            if (loginButton != null)
            {
                loginButton.interactable = false;
            }
        }     
    }

    private void UnblockLoginInput()
    {
        if (GameObject.Find("LoginButton") != null)
        {
            loginButton = GameObject.Find("LoginButton").GetComponent<Button>();
            if (loginButton != null)
            {
                loginButton.interactable = true;
            }
        }
    }

    private void BlockJoinHostGameButtons()
    {
        joinGameButton.interactable = false;
        hostGameButton.interactable = false;
        Destroy(hostGameButton.GetComponentInChildren<ButtonFontTMP>());
    }
    private void UnblockJoinHostGameButtons()
    {
        joinGameButton.interactable = true;
        hostGameButton.interactable = true;
        hostGameButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().gameObject.AddComponent<ButtonFontTMP>();
    }

    private void BlockMatchInput()
    {
        matchmakingButton.interactable = false;
    }

    private void UnblockMatchInput()
    {
        matchmakingButton.interactable = true;
    }
}
