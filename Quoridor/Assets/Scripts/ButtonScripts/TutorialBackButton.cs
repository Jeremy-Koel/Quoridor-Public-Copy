using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialBackButton : MonoBehaviour, IPointerClickHandler
{
    public MainMenu mainMenu;
    
    private void Awake()
    {
        mainMenu = GameObject.Find("Main Camera").GetComponent<MainMenu>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        mainMenu.onTutorialBackButtonClick();
    }

}
