using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject menuScreen;
    public GameObject returnToGameButton;
    // Start is called before the first frame update
    void Start()
    {
        menuScreen.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onMenuButtonClick()
    {
        if (winScreen.activeSelf)
        {
            //winScreen.transform.position = new Vector3(winScreen.transform.position.x, winScreen.transform.position.y, 2);
            winScreen.SetActive(false);
            menuScreen.SetActive(true);
            returnToGameButton.SetActive(false);
        }
        else if (!winScreen.activeSelf)
        {
            //winScreen.transform.position = new Vector3(winScreen.transform.position.x, winScreen.transform.position.y, 2);
            menuScreen.SetActive(true);
            returnToGameButton.SetActive(true);
        }

    }
}
