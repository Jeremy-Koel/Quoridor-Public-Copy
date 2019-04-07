using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuitButton : MonoBehaviour, IPointerClickHandler
{

    //private GameObject mainMenuPanel;
    private GameObject quitPanel;
    private EventManager eventManager;

    // Start is called before the first frame update
    void Start()
    {
        //mainMenuPanel = GameObject.Find("MainMenuPanel");
        quitPanel = GameObject.Find("QuitPanel");
        quitPanel.SetActive(false);
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnQuitButtonClick()
    {
        quitPanel.SetActive(true);
        quitPanel.GetComponentInChildren<MoveBoards>().moveBoard = true;
    }

    public void OnPointerClick(PointerEventData data)
    {
       // mainMenuPanel.SetActive(false);
        quitPanel.SetActive(true);
        quitPanel.GetComponentInChildren<MoveBoards>().moveBoard = true;
    }


    public void OnYesButtonClick()
    {
        eventManager.RemoveAllListeners();
        Application.Quit();
    }

    public void OnNoButtonClick()
    {
        //mainMenuPanel.SetActive(true);
        var moveBoardsComponent = quitPanel.GetComponentInChildren<MoveBoards>();
        if (moveBoardsComponent != null)
        {
            moveBoardsComponent.moveBoard = true;
        }
      //  quitPanel.SetActive(false);
    }
}
