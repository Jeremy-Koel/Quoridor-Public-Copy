using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    // Time left to countdown
    protected float timeLeft = 30f;
    private float timeDefault = 30f;
    public bool countingDown = false;
    //private EventManager eventManager;
    public UnityEvent timeUp = new UnityEvent();
    public UnityEvent valueChanged = new UnityEvent();

    private void Awake()
    {
        //eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();

    }


    // Start is called before the first frame update
    void Start()
    {
        //if (GameSession.GameMode == GameModeEnum.MULTIPLAYER)
        //{
        //    // Listen to game ending
        //    eventManager.ListenToGameOver(StartCountdown);
        //}
        //else
        //{

        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (countingDown)
        {
            timeLeft -= Time.deltaTime;
            valueChanged.Invoke();

            if (timeLeft < 0)
            {
                // time is up
                countingDown = false;
                timeUp.Invoke();
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

    public void SetTimeDefault(float time)
    {
        timeDefault = time;
        timeLeft = timeDefault;
    }
}
