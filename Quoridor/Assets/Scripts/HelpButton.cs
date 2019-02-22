using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButton : MonoBehaviour
{
    private GameObject helpScreen;
    // Start is called before the first frame update
    void Start()
    {
        helpScreen = GameObject.Find("HelpMenu");
        helpScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onHelpButtonClick()
    {
        helpScreen.SetActive(true);
    }
}
