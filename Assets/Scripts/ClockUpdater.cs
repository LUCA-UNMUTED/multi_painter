using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Tobii.G2OM;
using System;


/// <summary>
/// ClockUpdater updates a textmeshpro with the current time
/// 
/// </summary>
public class ClockUpdater : MonoBehaviour, IGazeFocusable
{
    [SerializeField] private TextMeshProUGUI clock;
    private float timePassed = 0f;

    [SerializeField] private bool testClock = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (testClock)
        {
            testClock = false;
            SetTime();
        }
    }


    void SetTime()
    {
        timePassed = Time.realtimeSinceStartup;
        TimeSpan timeSpan = TimeSpan.FromSeconds(timePassed);
        int minutes = timeSpan.Minutes;
        int seconds = timeSpan.Seconds;
        string updatedTime = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        clock.SetText(updatedTime);
    }
    public void GazeFocusChanged(bool hasFocus)
    {
        SetTime();
    }
}
