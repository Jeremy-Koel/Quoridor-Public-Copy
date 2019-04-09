using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintButton : MonoBehaviour
{
    public InterfaceController interfaceController;
    public GameCoreController gameCoreController;

    private void Awake()
    {
        gameCoreController = GameObject.Find("GameController").GetComponent<GameCoreController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void OnHintButtonClick()
    {
        string hint = await gameCoreController.GetHintForPlayer();
        Debug.Log(hint);
    }
    
}
