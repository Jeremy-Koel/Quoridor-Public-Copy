using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToGameButton : MonoBehaviour
{
    public GameObject menuScreen;
    private GameObject helpScreen;
    // Start is called before the first frame update
    void Start()
    {
        helpScreen = GameObject.Find("HelpMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickReturnToGameButton()
    {
        if (menuScreen.activeSelf == true)
        {
            //menuScreen.SetActive(false);
            menuScreen.GetComponentInChildren<MoveBoards>().moveBoard = true;
        }
        if (helpScreen.activeSelf == true)
        {
           // helpScreen.SetActive(false);
           helpScreen.GetComponentInChildren<MoveBoards>().moveBoard = true;
        }
    }
}
