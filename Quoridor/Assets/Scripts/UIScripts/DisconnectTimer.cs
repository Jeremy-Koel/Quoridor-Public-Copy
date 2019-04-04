using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectTimer : MonoBehaviour
{
    // Time left to countdown
    private float timeLeft = 30f;
    private readonly float timeDefault = 30f;
    public bool countingDown = false;
    private EventManager eventManager;

    private void Awake()
    {
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();

    }


    // Start is called before the first frame update
    void Start()
    {
        if (GameSession.GameMode == GameModeEnum.MULTIPLAYER)
        {
            // Listen to game ending
            eventManager.ListenToGameOver(StartCountdown);
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
       if (countingDown)
        {
            timeLeft -= Time.deltaTime;
            eventManager.InvokeCountdownTimerValueChanged();
            
            if (timeLeft < 0)
            {
                // time is up
                countingDown = false;
                eventManager.InvokeCountdownTimer();
            }
        } 
    }

    public void ResetTimer()
    {
        timeLeft = timeDefault;
    }

    public void CancelCountdown()
    {
        countingDown = false;
        ResetTimer();
    }

    public int GetTimerValue()
    {
        return Convert.ToInt32(timeLeft);
    }

    public void StartCountdown()
    {
        countingDown = true;
    }
}
