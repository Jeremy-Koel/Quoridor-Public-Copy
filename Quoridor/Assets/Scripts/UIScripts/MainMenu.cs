﻿using GameSparks.Api.Messages;
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
    public Stack<GameObject> panelOrder;

    public GameObject challengeManager;

    private bool lobbyActivatedOnce;

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
        leaderboardPanel = GameObject.Find("LeaderboardPanel");
        tutorialPanel = GameObject.Find("TutorialPanel");

        panelOrder = new Stack<GameObject>();
        panelOrder.Push(mainMenuPanel);

        mainMenuPanel.SetActive(true);
        singlePlayerSetupPanel.SetActive(false);
        settingsPanel.SetActive(false);
        loginPanel.SetActive(false);
        registrationPanel.SetActive(false);
        lobbyPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        tutorialPanel.SetActive(false);
    }

    public void onSinglePlayerClick()
    {
        mainMenuPanel.SetActive(false);

        singlePlayerSetupPanel.SetActive(true);
        panelOrder.Push(singlePlayerSetupPanel);
    }

    public void onMultiplayerClick()
    {
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
        SessionStates.Difficulty = DifficultyEnum.EASY;
        SceneManager.LoadScene("GameBoard");
    }

    public void onHardButtonClick()
    {
        SessionStates.Difficulty = DifficultyEnum.HARD;
        SceneManager.LoadScene("GameBoard");
    }

    public void onSettingsButtonClick(Button button)
    {
        mainMenuPanel.SetActive(false);
        singlePlayerSetupPanel.SetActive(false);
        settingsPanel.SetActive(true);
        panelOrder.Push(settingsPanel);
    }

    public void onTutorialButtonClick(Button button)
    {
        mainMenuPanel.SetActive(false);
        singlePlayerSetupPanel.SetActive(false);
        settingsPanel.SetActive(false);
        tutorialPanel.SetActive(true);
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

        disableScreen.SetActive(false);
        enableScreen.SetActive(true);
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

        // Set click listener for leaderboards button
        Button leaderboardButton = GameObject.Find("LeaderboardButton").GetComponent<Button>();
        leaderboardButton.onClick.AddListener(OnLeaderboardsClick);
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
        lobbyPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
        panelOrder.Push(leaderboardPanel);

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
