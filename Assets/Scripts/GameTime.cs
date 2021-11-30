using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameTime
{
    public static bool isPaused = false;
    public static float deltaTime { get { return isPaused ? 0 : Time.deltaTime; } }

    public static void Play()
    {
        isPaused = false;
    }

    public static void Pause()
    {
        isPaused = true;
    }
}