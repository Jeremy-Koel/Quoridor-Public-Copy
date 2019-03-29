using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuitButton : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    public void OnPointerClick(PointerEventData data)
    {
        Debug.Log("ClickQuit");
        Application.Quit();
    }
}
