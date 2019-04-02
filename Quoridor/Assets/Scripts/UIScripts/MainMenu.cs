using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject singlePlayerSetupPanel;
    public GameObject settingsPanel;
    public GameObject loginPanel;
    public GameObject registrationPanel;
    public GameObject lobbyPanel;
    public GameObject chatWindowPanel;
    public GameObject leaderboardPanel;
    public GameObject tutorialPanel;
    public GameObject dummyMenuPanel;
  //  public GameObject quitPanel;
    public Stack<GameObject> panelOrder;

    public GameObject challengeManager;

    private bool lobbyActivatedOnce;

    public AudioControllerMainMenu audioController;

    [SerializeField]
    private InputField usernameLoginInput;
    [SerializeField]
    private InputField passwordLoginInput;
    [SerializeField]
    private InputField displayNameInput;
    [SerializeField]
    private InputField usernameRegisterInput;
    [SerializeField]
    private InputField passwordRegisterInput;
    [SerializeField]
    private Button loginButton;
    [SerializeField]
    private Button registerButton;
    [SerializeField]
    private Text errorMessageLoginText;
    [SerializeField]
    private Text errorMessageRegistrationText;
    [SerializeField]
    private Button matchmakingButton;

    private void Awake()
    {
        ChallengeStartedMessage.Listener += OnChallengeStarted;
        ChallengeIssuedMessage.Listener += OnChallengeIssued;
        chatWindowPanel = GameObject.Find("ChatWindowPanel");
        challengeManager = GameObject.Find("ChallengeManager");
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
        registrationPanel = GameObject.Find("RegistrationPanel");
        lobbyPanel = GameObject.Find("LobbyPanel");
        tutorialPanel = GameObject.Find("TutorialPanel");
        dummyMenuPanel = GameObject.Find("DummyMenuPanel");
       // quitPanel = GameObject.Find("QuitPanel");

        panelOrder = new Stack<GameObject>();
        panelOrder.Push(mainMenuPanel);

        mainMenuPanel.SetActive(true);
        dummyMenuPanel.SetActive(false);
        singlePlayerSetupPanel.SetActive(false);
        settingsPanel.SetActive(false);
        loginPanel.SetActive(false);
        registrationPanel.SetActive(false);
        lobbyPanel.SetActive(false);
        tutorialPanel.SetActive(false);

        mainMenuPanel.GetComponent<MoveMainMenuBoard>().moveBoard = true;
       
    }

    public void onSinglePlayerClick()
    {
        // Set session to singleplayer 
        GameSession.GameMode = GameModeEnum.SINGLE_PLAYER;

        mainMenuPanel.SetActive(false);
        singlePlayerSetupPanel.SetActive(true);
        panelOrder.Push(singlePlayerSetupPanel);
    }

    public void onMultiplayerClick()
    {
        // Set session to multiplayer 
        GameSession.GameMode = GameModeEnum.MULTIPLAYER;

        mainMenuPanel.SetActive(false);
        
        if (LoggedIn())
        {
            lobbyPanel.SetActive(true);
            panelOrder.Push(lobbyPanel);
        }
        else
        {
            loginPanel.SetActive(true);
            panelOrder.Push(loginPanel);
        }        
        
    }

    public void onLoginClick()
    {
        // Try to login using username and password
        Login(usernameLoginInput.text, passwordLoginInput.text);
    }

    public void onRegistrationSwitchClick()
    {
        usernameRegisterInput.text = usernameLoginInput.text;
        passwordRegisterInput.text = passwordLoginInput.text;
        loginPanel.SetActive(false);

        registrationPanel.SetActive(true);
        panelOrder.Push(registrationPanel);
    }

    public void onRegistrationClick()
    {
        usernameLoginInput.text = usernameRegisterInput.text;
        passwordLoginInput.text = passwordRegisterInput.text;
        // Try to register new user account using displayname, username, and password
        Register();
    }

    public void onEasyButtonClick()
    {
        GameSession.Difficulty = DifficultyEnum.EASY;
        SceneManager.LoadScene("GameBoard");
    }

    public void onHardButtonClick()
    {
        GameSession.Difficulty = DifficultyEnum.HARD;
        SceneManager.LoadScene("GameBoard");
    }

    public void onPlayButtonClick()
    {
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


        SceneManager.LoadScene("GameBoard");
    }

    public void onSettingsButtonClick(Button button)
    {
        mainMenuPanel.SetActive(false);
        dummyMenuPanel.SetActive(false);
        singlePlayerSetupPanel.SetActive(false);
        settingsPanel.SetActive(true);
        panelOrder.Push(settingsPanel);
    }

    public void onTutorialButtonClick(Button button)
    {
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
        GameObject disableScreen = panelOrder.Pop();
        GameObject enableScreen = panelOrder.Peek();

        usernameLoginInput.text = "";
        passwordLoginInput.text = "";
        usernameRegisterInput.text = "";
        passwordRegisterInput.text = "";

        if (disableScreen.name != "TutorialPanel")
        {
            disableScreen.SetActive(false);
        }

        enableScreen.SetActive(true);
    }

    public void onTutorialBackButtonClick()
    {
        tutorialPanel.GetComponent<MoveTutorialBoard>().moveBoard = true;
        dummyMenuPanel.SetActive(false);
        onBackButtonClick();
    }

    public void onMatchMakingButtonClick()
    {
        BlockInput();
        Debug.Log("Making/sending matchmaking request");
        MatchmakingRequest request = new MatchmakingRequest();
        request.SetMatchShortCode("DefaultMatch");
        request.SetSkill(0);
        request.Send(OnMatchmakingSuccess, OnMatchmakingError);
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
        SceneManager.LoadScene("GameBoard");
    }

    // Login/Registration
    private void Login(string username, string password)
    {
        BlockInput();
        AuthenticationRequest request = new AuthenticationRequest();
        request.SetUserName(username);
        request.SetPassword(password);
        //request.SetUserName(usernameLoginInput.text);
        //request.SetPassword(passwordLoginInput.text);
        request.Send(OnLoginSuccess, OnLoginError);
    }

    private void OnLoginSuccess(AuthenticationResponse response)
    {
        UnblockInput();
        // Set the User's ID to an object for referencing later
        Debug.Log(response.UserId);
        GameObject gameSparksUserIDObject = GameObject.Find("GameSparksUserID");
        GameSparksUserID gameSparksUserIDScript = gameSparksUserIDObject.GetComponent<GameSparksUserID>();
        gameSparksUserIDScript.myUserID = response.UserId;
        gameSparksUserIDScript.myDisplayName = response.DisplayName;
        // Setup ChatWindowPanel
        ChatWindowPanel chatWindowPanelScript = chatWindowPanel.GetComponent<ChatWindowPanel>();
        chatWindowPanelScript.CheckTeams();
        // Setup ChallengeManager
        ChallengeManager challengeManagerScript = challengeManager.GetComponent<ChallengeManager>();
        challengeManagerScript.SetupChallengeListeners();

        // Switch to the Lobby Panel
        panelOrder.Push(loginPanel);
        loginPanel.SetActive(false);
        registrationPanel.SetActive(false);
        panelOrder.Push(lobbyPanel);
        lobbyPanel.SetActive(true);
        OnLeaderboardsClick();
    }

    private void OnLoginError(AuthenticationResponse response)
    {
        UnblockInput();
        errorMessageLoginText.color = Color.red;
        errorMessageLoginText.text = response.Errors.BaseData["DETAILS"].ToString();
    }

    private bool LoggedIn()
    {
        bool loggedIn = false;
        GameSparksUserID gameSparksUserID = GameObject.Find("GameSparksUserID").GetComponent<GameSparksUserID>();
        if (gameSparksUserID.myUserID != "")
        {
            loggedIn = true;
        }
        return loggedIn;
    }

    private void Register()
    {
        BlockInput();
        RegistrationRequest request = new RegistrationRequest();
        request.SetUserName(usernameRegisterInput.text);
        request.SetDisplayName(displayNameInput.text);
        request.SetPassword(passwordRegisterInput.text);
        request.Send(OnRegistrationSuccess, OnRegistrationError);
    }

    private void OnRegistrationSuccess(RegistrationResponse response)
    {
        Login(usernameRegisterInput.text, passwordRegisterInput.text);
    }

    private void OnRegistrationError(RegistrationResponse response)
    {
        UnblockInput();
        errorMessageRegistrationText.color = Color.red;
        errorMessageRegistrationText.text = response.Errors.BaseData["USERNAME"].ToString();
    }

    private void OnLeaderboardsClick()
    {
        LeaderboardPanel leaderboardPanelScript = GameObject.Find("LeaderboardPanel").GetComponent<LeaderboardPanel>();
        leaderboardPanelScript.onRefreshLeaderboardButtonClick();
    }

    private void BlockInput()
    {
        //userNameInput.interactable = false;
        //passwordInput.interactable = false;
        loginButton.interactable = false;
        registerButton.interactable = false;
        matchmakingButton.interactable = false;
    }

    private void UnblockInput()
    {
        //userNameInput.interactable = true;
        //passwordInput.interactable = true;
        loginButton.interactable = true;
        registerButton.interactable = true;
        matchmakingButton.interactable = true;
    }
}
