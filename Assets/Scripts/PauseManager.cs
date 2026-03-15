using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PauseManager
{
    public static bool IsPaused {get; private set;}

    public static void Pause()
    {
        if(IsPaused)
            return;
        IsPaused = true;
        Time.timeScale = 0;
    }

    public static void Resume()
    {
        if(!IsPaused)
            return;
        IsPaused = false;
        Time.timeScale = 1;
    }
}
