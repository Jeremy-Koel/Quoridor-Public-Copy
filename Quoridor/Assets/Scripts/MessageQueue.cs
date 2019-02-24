using GameSparks.Api.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageQueue : MonoBehaviour
{
    public Queue<ScriptMessage> startingPlayerSetQueue = new Queue<ScriptMessage>();
    
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
}
