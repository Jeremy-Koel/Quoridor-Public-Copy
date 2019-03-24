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
    public GameObject playModePanel;
    public GameObject difficultyLevelPanel;
    public GameObject settingsPanel;
    public GameObject loginPanel;
    public GameObject registrationPanel;
    public GameObject lobbyPanel;
    public GameObject chatWindowPanel;
    //public GameObject previousPanel;
    //public GameObject currentPanel;
    public Stack<GameObject> panelOrder;

    public GameObject challengeManager;

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
        playModePanel = GameObject.Find("PlayModePanel");
        difficultyLevelPanel = GameObject.Find("DifficultyLevelPanel");
        settingsPanel = GameObject.Find("SettingsPanel");
        loginPanel = GameObject.Find("LoginPanel");
        registrationPanel = GameObject.Find("RegistrationPanel");
        lobbyPanel = GameObject.Find("LobbyPanel");
        // previousPanel = new GameObject();
        //currentPanel = new GameObject();
        panelOrder = new Stack<GameObject>();
        panelOrder.Push(mainMenuPanel);

        mainMenuPanel.SetActive(true);
        playModePanel.SetActive(false);
        difficultyLevelPanel.SetActive(false);
        settingsPanel.SetActive(false);
        loginPanel.SetActive(false);
        registrationPanel.SetActive(false);
        lobbyPanel.SetActive(false);
    }

    public void onPlayButtonClick()
    {
        mainMenuPanel.SetActive(false);
        panelOrder.Push(mainMenuPanel);

        playModePanel.SetActive(true);
        panelOrder.Push(playModePanel);

    }

    public void onSinglePlayerClick()
    {
        playModePanel.SetActive(false);
        //previousPanel = playModePanel;

        difficultyLevelPanel.SetActive(true);
        panelOrder.Push(difficultyLevelPanel);
    }

    public void onMultiplayerClick()
    {
        playModePanel.SetActive(false);
        
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
        SceneManager.LoadScene("GameBoard");
    }

    public void onSettingsButtonClick(Button button)
    {
       // previousPanel = button.transform.parent.gameObject;
        mainMenuPanel.SetActive(false);
        playModePanel.SetActive(false);
        difficultyLevelPanel.SetActive(false);
        settingsPanel.SetActive(true);
        //currentPanel = settingsPanel;
        panelOrder.Push(settingsPanel);
    }

    public void onBackButtonClick()
    {
        //currentPanel.SetActive(false);
        //previousPanel.SetActive(true);
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
