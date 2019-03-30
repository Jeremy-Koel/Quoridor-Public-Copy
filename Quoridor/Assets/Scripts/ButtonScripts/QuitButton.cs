using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuitButton : MonoBehaviour, IPointerClickHandler
{

    //private GameObject mainMenuPanel;
    private GameObject quitPanel;

    // Start is called before the first frame update
    void Start()
    {
        //mainMenuPanel = GameObject.Find("MainMenuPanel");
        quitPanel = GameObject.Find("QuitPanel");
        quitPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnQuitButtonClick()
    {
        quitPanel.SetActive(true);
    }

    public void OnPointerClick(PointerEventData data)
    {
       // mainMenuPanel.SetActive(false);
        quitPanel.SetActive(true);
    }


    public void OnYesButtonClick()
    {
        Application.Quit();
    }

    public void OnNoButtonClick()
    {
        //mainMenuPanel.SetActive(true);
        quitPanel.SetActive(false);
    }
}
