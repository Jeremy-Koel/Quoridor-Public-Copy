using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ThrottleHandler
{
    public static bool IsRequestThrottled(string jsonString)
    {
        if (jsonString.Contains("Throttled") || jsonString.Contains("THROTTLED") || jsonString.Contains("throttled"))
        {
            return true;
        }
        return false;
    }

    public static float GetRandomTime()
    {
        return UnityEngine.Random.Range(0f, 2f);
    }
}
