using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIChat
{
    public static string[] easyMessages = 
    {
        "Don't give up!",
        "Keep trying your hardest!",
        "You can do it!",
        "You're doing so good!",
        "Think carefully about each move you make.",
        "If you're mean to me, I'll remember it :)",
        "Show me what you got!",
        "Let's see what you can do!",
        "This is fun, let's do it again sometime!",
        "Does beating me make you feel better?"
    };

    public static string[] hardMessages =
    {
        "You have no chance against me!",
        "You may be good, but I'm better.",
        "Had enough yet?",
        "Hmph, such an amateur.",
        "You need more practice.",
        "You're being far too predictable.",
        "Challenge me again when you learn more.",
        "A good tactician has nothing to fear!",
        "I'm going to win, and there's nothing you can do about it!",
        "Are you scared yet?",
    };

    public static string GetAIMessage()
    {
        return "testing snark";
    }

    public static string GetEasyAIMessage()
    {
        System.Random rand = new System.Random();
        return easyMessages[rand.Next(easyMessages.Length)];
    }

    public static string GetHardAIMessage()
    {
        System.Random rand = new System.Random();
        return hardMessages[rand.Next(hardMessages.Length)];
    }
}
