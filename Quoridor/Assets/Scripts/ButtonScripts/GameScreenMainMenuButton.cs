using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScreenMainMenuButton : MonoBehaviour
{
    private InterfaceController interfaceController;

    private void Awake()
    {
        interfaceController = GameObject.Find("GameController").GetComponent<InterfaceController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onMainMenuButtonClick()
    {
        interfaceController.TurnIndicatorLightsOff();

        // Get ChallengeManager/EventManager/MessageQueue DontDestroyOnLoad Objects and reset them
        if (GameSession.GameMode == GameModeEnum.MULTIPLAYER)
        {
            ChallengeManager challengeManager = GameObject.Find("ChallengeManager").GetComponent<ChallengeManager>();
            challengeManager.RemoveAllChallengeListeners();
            challengeManager = new ChallengeManager();

            MessageQueue messageQueue = GameObject.Find("MessageQueue").GetComponent<MessageQueue>();
            messageQueue = new MessageQueue();
        }

        EventManager eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        eventManager.RemoveAllListeners();
        eventManager = new EventManager();

        //PlayerTurnPopup playerTurnPopup = GameObject.Find("PlayerTurnBox").GetComponent<PlayerTurnPopup>();
        //playerTurnPopup = new PlayerTurnPopup();

        //SceneManager.LoadScene("MainMenu"); 
        GameObject.Find("LevelChanger").GetComponent<LevelChanger>().FadeToLevel("MainMenu");
    }
}
