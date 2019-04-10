using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FastAnimations
{
    public static float CalculateSpeed(float speed)
    {
        float step;
        return step = GameSession.FastAnimations ? (speed * 2) * Time.deltaTime : speed * Time.deltaTime;
    }
}
