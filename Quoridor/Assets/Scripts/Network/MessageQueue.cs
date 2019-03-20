using GameSparks.Api.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageQueue : MonoBehaviour
{
    public Queue<ScriptMessage> startingPlayerSetQueue = new Queue<ScriptMessage>();
    public Queue<ScriptMessage> matchmakingGroupNumberQueue = new Queue<ScriptMessage>();
    public Queue<ScriptMessage> challengeMoveQueue = new Queue<ScriptMessage>();
    public Queue<string> opponentMoveQueue = new Queue<string>();

    public enum QueueNameEnum { STARTINGPLAYER, MATCHMAKINGGROUPNUMBER, CHALLENGEMOVE,  OPPONENTMOVE}
    
    private void Awake()
    {
        int numberOfActiveThises = FindObjectsOfType<MessageQueue>().Length;
        if (numberOfActiveThises != 1)
        {
            Destroy(this.gameObject);
        }
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

    public void EnqueueMatchmakingGroupNumber(ScriptMessage message)
    {
        matchmakingGroupNumberQueue.Enqueue(message);
    }
    public ScriptMessage DequeueMatchmakingGroupNumber()
    {
        return matchmakingGroupNumberQueue.Dequeue();
    }

    public void EnqueueChallengeMove(ScriptMessage message)
    {
        challengeMoveQueue.Enqueue(message);
    }
    public ScriptMessage DequeueChallengeMove()
    {
        return challengeMoveQueue.Dequeue();
    }


    public void EnqueueOpponentMoveQueue(string move)
    {
        opponentMoveQueue.Enqueue(move);
    }
    public string DequeueOpponentMoveQueue()
    {
        return opponentMoveQueue.Dequeue();
    }

    public IEnumerator WaitForQueueNotEmpty(QueueNameEnum queueName)
    {
        while (this.IsQueueEmpty(queueName))
        {
            Debug.Log("Queue is empty");
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator CheckQueueNotEmpty(QueueNameEnum queueName)
    {
        if (this.IsQueueEmpty(queueName))
        {
            Debug.Log("Queue is empty");
            yield return new WaitForSeconds(0.1f);
        }
    }

    public bool IsQueueEmpty(QueueNameEnum queueName)
    {
        bool empty = true;
        switch (queueName) {
            case QueueNameEnum.STARTINGPLAYER:
                empty = startingPlayerSetQueue.Count.Equals(0);
                break;
            case QueueNameEnum.OPPONENTMOVE:
                empty = opponentMoveQueue.Count.Equals(0);
                break;
            case QueueNameEnum.CHALLENGEMOVE:
                empty = challengeMoveQueue.Count.Equals(0);
                break;
            case QueueNameEnum.MATCHMAKINGGROUPNUMBER:
                empty = matchmakingGroupNumberQueue.Count.Equals(0);
                break;
        }
        return empty;
    }
}
