﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisconnectPanel : MonoBehaviour
{
    private Button disconnectYesButton;
    private Button disconnectNoButton;
    private EventManager eventManager;

    private void Awake()
    {
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        disconnectYesButton = GameObject.Find("DisconnectYesButton").GetComponent<Button>();
        disconnectNoButton = GameObject.Find("DisconnectNoButton").GetComponent<Button>();
        disconnectYesButton.onClick.AddListener(OnDisconnectYesClick);
        disconnectNoButton.onClick.AddListener(OnDisconnectNoClick);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDisconnectYesClick()
    {
        eventManager.InvokeDisconnectReconnectionYes();
    }

    public void OnDisconnectNoClick()
    {
        eventManager.InvokeDisconnectReconnectionNo();
    }
}