using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Stopwatch : MonoBehaviour
{
    static bool stopwatchActive;
    public static float currentTime;
    public TMP_Text timeText;
    void Start()
    {
        stopwatchActive = false;
        currentTime = 0f;
    }

    void Update()
    {
        if(stopwatchActive)
        {
            currentTime += Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timeText.text = time.ToString(@"mm\:ss\:fff");
    }

    public static void StartStopwatch()
    {
        stopwatchActive = true;
    }

    public static void StopStopwatch()
    {
        stopwatchActive = false;
    }
}
