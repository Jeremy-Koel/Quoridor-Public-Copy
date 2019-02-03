using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
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
    //public GameObject previousPanel;
    //public GameObject currentPanel;
    public Stack<GameObject> panelOrder;

    [SerializeField]
    private InputField userNameInput;
    [SerializeField]
    private InputField passwordInput;
    [SerializeField]
    private Button loginButton;
    [SerializeField]
    private Button registerButton;
    [SerializeField]
    private Text errorMessageText;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuPanel = GameObject.Find("MainButtonPanel");
        playModePanel = GameObject.Find("PlayModePanel");
        difficultyLevelPanel = GameObject.Find("DifficultyLevelPanel");
        settingsPanel = GameObject.Find("SettingsPanel");
        loginPanel = GameObject.Find("LoginPanel");
        registrationPanel = GameObject.Find("RegistrationPanel");
        // previousPanel = new GameObject();
        //currentPanel = new GameObject();
        panelOrder = new Stack<GameObject>();

        mainMenuPanel.SetActive(true);
        playModePanel.SetActive(false);
        difficultyLevelPanel.SetActive(false);
        settingsPanel.SetActive(false);
        loginPanel.SetActive(false);
        registrationPanel.SetActive(false);
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

        loginPanel.SetActive(true);
        panelOrder.Push(loginPanel);
    }

    public void onLoginClick()
    {
        // Try to login using username and password
    }

    public void onRegistrationSwitchClick()
    {
        loginPanel.SetActive(false);

        registrationPanel.SetActive(true);
        panelOrder.Push(registrationPanel);
    }

    public void onRegistrationClick()
    {
        // Try to register new user account using displayname, username, and password
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

        disableScreen.SetActive(false);
        enableScreen.SetActive(true);
    }

    // Login/Registration
    private void Login()
    {
        BlockInput();
        AuthenticationRequest request = new AuthenticationRequest();
        request.SetUserName(userNameInput.text);
        request.SetPassword(passwordInput.text);
        request.Send(OnLoginSuccess, OnLoginError);
    }

    private void OnLoginSuccess(AuthenticationResponse response)
    {
        UnblockInput();
        Debug.Log(response.UserId);
    }

    private void OnLoginError(AuthenticationResponse response)
    {
        UnblockInput();
        errorMessageText.color = Color.red;
        errorMessageText.text = response.Errors.JSON.ToString();
    }

    private void Register()
    {
        BlockInput();
        RegistrationRequest request = new RegistrationRequest();
        request.SetUserName(userNameInput.text);
        request.SetDisplayName(userNameInput.text);
        request.SetPassword(passwordInput.text);
        request.Send(OnRegistrationSuccess, OnRegistrationError);
    }

    private void OnRegistrationSuccess(RegistrationResponse response)
    {
        Login();
    }

    private void OnRegistrationError(RegistrationResponse response)
    {
        UnblockInput();
        errorMessageText.color = Color.red;
        errorMessageText.text = response.Errors.JSON.ToString();
    }

    private void BlockInput()
    {
        userNameInput.interactable = false;
        passwordInput.interactable = false;
        loginButton.interactable = false;
        registerButton.interactable = false;
    }

    private void UnblockInput()
    {
        userNameInput.interactable = true;
        passwordInput.interactable = true;
        loginButton.interactable = true;
        registerButton.interactable = true;
    }
}
