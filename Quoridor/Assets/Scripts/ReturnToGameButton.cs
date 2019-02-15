using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToGameButton : MonoBehaviour
{
    public GameObject menuScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickReturnToGameButton()
    {
        menuScreen.SetActive(false);
    }
}
