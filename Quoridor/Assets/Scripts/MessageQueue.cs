﻿using GameSparks.Api.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageQueue : MonoBehaviour
{
    public Queue<ScriptMessage> startingPlayerSetQueue = new Queue<ScriptMessage>();
    public Queue<string> opponentMoveQueue = new Queue<string>();
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void EnqueueStartingPlayerSetQueue(ScriptMessage message)
    {
        startingPlayerSetQueue.Enqueue(message);
    }
    public ScriptMessage DequeueStartingPlayerSetQueue()
    {
        return startingPlayerSetQueue.Dequeue();
    }

    public void EnqueueOpponentMoveQueue(string move)
    {
        opponentMoveQueue.Enqueue(move);
    }
    public string DequeueOpponentMoveQueue()
    {
        return opponentMoveQueue.Dequeue();
    }

    public bool IsQueueEmpty(string queueName)
    {
        bool empty = true;
        if (queueName == "startingPlayerSetQueue")
        {
            if (startingPlayerSetQueue.Count != 0)
            {
                empty = false;
            }
        }
        else if (queueName == "opponentMoveQueue")
        {
            if (opponentMoveQueue.Count != 0)
            {
                empty = false;
            }
        }
        return empty;
    }
}