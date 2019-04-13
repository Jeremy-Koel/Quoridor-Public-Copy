using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisconnectPanel : MonoBehaviour
{
    public Button disconnectYesButton;
    public Button disconnectNoButton;
    public Button aiDiffSelectEasy;
    public Button aiDiffSelectHard;
    private EventManager eventManager;
    public GameObject disconnectPanel;
    public GameObject disconnectionAIDifficultySelectPanel;
    private bool disconnectionAIDiffPanelFound = false;

    private void OnDestroy()
    {
        eventManager = null;
    }

    private void Awake()
    {
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        eventManager.ListenToMultiplayerSelected(FindGameSceneObjects);
    }

    // Start is called before the first frame update
    void Start()
    {
        FindGameSceneObjects();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FindGameSceneObjects()
    {
        if (GameSession.GameMode == GameModeEnum.MULTIPLAYER)
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                if (disconnectYesButton == null || disconnectNoButton == null)
                {
                    disconnectYesButton = GameObject.Find("DisconnectYesButton").GetComponent<Button>();
                    disconnectNoButton = GameObject.Find("DisconnectNoButton").GetComponent<Button>();
                }
                else
                {

                }
                disconnectYesButton.onClick.AddListener(OnDisconnectYesClick);
                disconnectNoButton.onClick.AddListener(OnDisconnectNoClick);
            }
            else
            {
                if (disconnectYesButton == null || disconnectNoButton == null)
                {
                    disconnectYesButton = GameObject.Find("DisconnectYesButton").GetComponent<Button>();
                    disconnectNoButton = GameObject.Find("DisconnectNoButton").GetComponent<Button>();
                }
                if (aiDiffSelectEasy == null || aiDiffSelectHard == null)
                {
                    aiDiffSelectEasy = GameObject.Find("AIDifficultySelectEasy").GetComponent<Button>();
                    aiDiffSelectHard = GameObject.Find("AIDifficultySelectHard").GetComponent<Button>();
                }
                disconnectYesButton.onClick.AddListener(OnDisconnectYesClick);
                disconnectNoButton.onClick.AddListener(OnDisconnectNoClick);
                
                aiDiffSelectEasy.onClick.AddListener(OnEasySelect);
                aiDiffSelectHard.onClick.AddListener(OnHardSelect);
            }
        }   
        else
        {
            
        }
        
    }


    public void OnDisconnectYesClick()
    {
        eventManager.InvokeDisconnectReconnectionYes();
    }

    public void OnDisconnectNoClick()
    {
        eventManager.InvokeDisconnectReconnectionNo();
    }

    public void OnEasySelect()
    {
        eventManager.InvokeDisconnectAIEasy();
    }

    public void OnHardSelect()
    {
        eventManager.InvokeDisconnectAIHard();
    }
}