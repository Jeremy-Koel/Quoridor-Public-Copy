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
        
        if (GameSession.GameMode == GameModeEnum.MULTIPLAYER)
        {
            GameObject.Find("DisconnectPopup").GetComponent<DisconnectPopup>().showPopup = false;
            EventManager eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
            eventManager.InvokeLeavingOpponent();
        }

        //SceneManager.LoadScene("MainMenu"); 
        GameObject.Find("LevelChanger").GetComponent<LevelChanger>().FadeToLevel("MainMenu");
    }
}
