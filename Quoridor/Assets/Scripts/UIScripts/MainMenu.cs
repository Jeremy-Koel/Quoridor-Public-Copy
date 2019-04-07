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
    public Stack<GameObject> panelOrder;

    public GameObject challengeManager;
    public EventManager eventManager;

    private bool lobbyActivatedOnce;
    private bool matching;
    private bool adminLogin = false;
    private string adminUsername = "ADMIN";
    private string adminPassword = "PASSWORD";

    public AudioControllerMainMenu audioController;

    [SerializeField]
    private InputField usernameLoginInput;
    [SerializeField]
    private InputField passwordLoginInput;
    [SerializeField]
    private InputField displayNameInput;
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

    private void Awake()
    {
        ScriptMessage_GuestAccountDetails.Listener += OnGuestAccountDetails;
        ChallengeStartedMessage.Listener += OnChallengeStarted;
        ChallengeIssuedMessage.Listener += OnChallengeIssued;
        chatWindowPanel = GameObject.Find("ChatWindowPanel");
        challengeManager = GameObject.Find("ChallengeManager");
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        matchmakingButton = GameObject.Find("MatchmakingSearchButton").GetComponent<Button>();
        audioController = GameObject.Find("MusicPlayer").GetComponent<AudioControllerMainMenu>();
    }

    void OnDestroy()
    {
        ChallengeStartedMessage.Listener -= OnChallengeStarted;
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

        panelOrder = new Stack<GameObject>();
        panelOrder.Push(mainMenuPanel);

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

    }

    public void DeselectCurrentButton()
    {
        var currentButtonText = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>();
        currentButtonText.fontStyle = FontStyles.Normal;
    }

    public void onSinglePlayerClick()
    {
        DeselectCurrentButton();
        audioController.PlayChalkWritingSound();        
        // Set session to singleplayer 
        GameSession.GameMode = GameModeEnum.SINGLE_PLAYER;

        mainMenuPanel.SetActive(false);
        singlePlayerSetupPanel.SetActive(true);
        panelOrder.Push(singlePlayerSetupPanel);
    }

    public void onMultiplayerClick()
    {
        DeselectCurrentButton();
        audioController.PlayChalkWritingSound();
        // Set session to multiplayer 
        GameSession.GameMode = GameModeEnum.MULTIPLAYER;
        eventManager.InvokeMultiplayerSelected();

        mainMenuPanel.SetActive(false);
        
        if (LoggedIn())
        {
            dummyMenuPanel.SetActive(true);
            lobbyPanel.SetActive(true);
            lobbyPanel.GetComponent<MoveMultiplayerScreen>().moveBoard = true;
            //panelOrder.Push(lobbyPanel);
        }
        else
        {
            loginPanel.SetActive(true);
            panelOrder.Push(loginPanel);
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
        panelOrder.Push(settingsPanel);
    }

    public void onTutorialButtonClick(Button button)
    {
        DeselectCurrentButton();
        audioController.PlayChalkWritingSound();
        mainMenuPanel.SetActive(false);
        singlePlayerSetupPanel.SetActive(false);
        settingsPanel.SetActive(false);
        dummyMenuPanel.SetActive(true);
        tutorialPanel.SetActive(true);
        tutorialPanel.GetComponent<MoveTutorialBoard>().moveBoard = true;
        panelOrder.Push(tutorialPanel);
    }

    public void onBackButtonClick()
    {
        //string name = EventSystem.current.currentSelectedGameObject.name;
        //DeselectCurrentButton();
        GameObject disableScreen = panelOrder.Pop();
        GameObject enableScreen = panelOrder.Peek();
        usernameLoginInput.text = "";
        passwordLoginInput.text = "";

        if (disableScreen.name != "TutorialPanel" && disableScreen.name != "LobbyPanel")
        {
            disableScreen.SetActive(false);
        }


        enableScreen.SetActive(true);
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
        dummyMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        lobbyPanel.GetComponent<MoveMultiplayerScreen>().moveBoard = true;
        if(panelOrder.Peek().name == "LoginPanel")
        {
            panelOrder.Pop();
        }
        //onBackButtonClick();
    }

    public void onMatchMakingButtonClick()
    {
        BlockInput();
        BlockJoinHostGameButtons();
        if (matching)
        {
            matchmakingButton.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Find Match";
            Debug.Log("Making/sending matchmaking request");
            MatchmakingRequest request = new MatchmakingRequest();
            request.SetAction("cancel");
            request.SetMatchShortCode("DefaultMatch");
            request.SetSkill(0);
            request.Send(OnMatchmakingSuccess, OnMatchmakingError);
            matching = false;
            UnblockInput();
            UnblockJoinHostGameButtons();
        }
        else
        {
            matchmakingButton.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Stop";
            Debug.Log("Making/sending matchmaking request");
            MatchmakingRequest request = new MatchmakingRequest();
            request.SetMatchShortCode("DefaultMatch");
            request.SetSkill(0);
            request.Send(OnMatchmakingSuccess, OnMatchmakingError);
            matching = true;
            UnblockInput();
        }        
    }

    public void OnMatchmakingSuccess(MatchmakingResponse response)
    {
        //UnblockInput();
        Debug.Log("Matchmaking Success");
    }

    public void OnMatchmakingError(MatchmakingResponse response)
    {
        //UnblockInput();
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
        UnblockInput();
        HostedGameLobbies hostedGameLobbiesScript = GameObject.Find("HostedGameLobbies").GetComponent<HostedGameLobbies>();
        hostedGameLobbiesScript.RemoveRefreshHostedGamesListener();

        Debug.Log("Challenge Started");
        // Switch to GameBoard Scene connected to opponent
        // SceneManager.LoadScene("GameBoard");
        GameObject.Find("LevelChanger").GetComponent<LevelChanger>().FadeToLevel("GameBoard");
    }

    // Login/Registration
    private void Login(string username, string password)
    {
        BlockInput();
        AuthenticationRequest request = new AuthenticationRequest();
        request.SetUserName(username);
        request.SetPassword(password);
        request.Send(OnLoginSuccess, OnLoginError);

    }

    private void LoginAsAdmin()
    {
        BlockInput();
        AuthenticationRequest request = new AuthenticationRequest();
        request.SetUserName(adminUsername);
        request.SetPassword(adminPassword);
        request.Send(OnAdminLoginSuccess, OnLoginError);
    }

    private void OnAdminLoginSuccess(AuthenticationResponse response)
    {
        UnblockInput();
        Debug.Log("logged in as admin");
    }

    private void OnLoginSuccess(AuthenticationResponse response)
    {
        UnblockInput();
        errorMessageLoginText.color = new Color(0, 0, 0, 0);
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
            ChatWindowPanel chatWindowPanelScript = chatWindowPanel.GetComponent<ChatWindowPanel>();
            chatWindowPanelScript.CheckTeams();
            chatWindowPanel.GetComponent<RectTransform>().ForceUpdateRectTransforms();
            // Setup ChallengeManager
            ChallengeManager challengeManagerScript = challengeManager.GetComponent<ChallengeManager>();
            challengeManagerScript.SetupChallengeListeners();

            // Switch to the Lobby Panel
            //panelOrder.Push(loginPanel);
            loginPanel.SetActive(false);
            dummyMenuPanel.SetActive(true);
            //registrationPanel.SetActive(false);
           // panelOrder.Push(lobbyPanel);
            lobbyPanel.SetActive(true);
            lobbyPanel.GetComponent<MoveMultiplayerScreen>().moveBoard = true;
            OnLeaderboardsClick();
        }
    }

    private void OnLoginError(AuthenticationResponse response)
    {
        UnblockInput();
        errorMessageLoginText.color = Color.red;
        var errorText = "TAKEN";
        errorMessageLoginText.text = errorText;
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
        BlockInput();
        RegistrationRequest request = new RegistrationRequest();
        //request.SetUserName(usernameRegisterInput.text);
        //request.SetDisplayName(displayNameInput.text);
        //request.SetPassword(passwordRegisterInput.text);
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
        UnblockInput();
        Login(usernameLoginInput.text, passwordLoginInput.text);
    }

    private void OnLeaderboardsClick()
    {
        LeaderboardPanel leaderboardPanelScript = GameObject.Find("LeaderboardPanel").GetComponent<LeaderboardPanel>();
        leaderboardPanelScript.onRefreshLeaderboardButtonClick();
    }

    public void InactivateAllPanels()
    {
        mainMenuPanel.SetActive(false);
        singlePlayerSetupPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        settingsPanel.SetActive(false);
        loginPanel.SetActive(false);
        //registrationPanel.SetActive(false);
        lobbyPanel.SetActive(false);
    }

    public void ActivateAllPanels()
    {
        mainMenuPanel.SetActive(true);
        singlePlayerSetupPanel.SetActive(true);
        tutorialPanel.SetActive(true);
        settingsPanel.SetActive(true);
        loginPanel.SetActive(true);
        //registrationPanel.SetActive(true);
        lobbyPanel.SetActive(true);
    }

    private void BlockInput()
    {
        //userNameInput.interactable = false;
        //passwordInput.interactable = false;
        loginButton.interactable = false;
        //registerButton.interactable = false;
        matchmakingButton.interactable = false;
    }

    private void UnblockInput()
    {
        //userNameInput.interactable = true;
        //passwordInput.interactable = true;
        loginButton.interactable = true;
        //registerButton.interactable = true;
        matchmakingButton.interactable = true;
    }

    private void BlockJoinHostGameButtons()
    {
        joinGameButton.interactable = false;
        hostGameButton.interactable = false;
    }
    private void UnblockJoinHostGameButtons()
    {
        joinGameButton.interactable = true;
        hostGameButton.interactable = true;
    }
}
