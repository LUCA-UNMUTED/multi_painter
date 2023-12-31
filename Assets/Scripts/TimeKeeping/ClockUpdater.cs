using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Tobii.G2OM;
using System;
using Unity.Netcode;


/// <summary>
/// ClockUpdater updates a textmeshpro with the current time
/// 
/// </summary>
public class ClockUpdater : NetworkBehaviour, IGazeFocusable
{
    [SerializeField] private TextMeshProUGUI clock;
    private float timePassed = 0f;
    [SerializeField] private bool testClock = false;

    public PlayerSettings playerSettings;
    public GameObject clockObject;

    private bool updateTime = false;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {


        playerSettings = this.GetComponentInParent<PlayerSettings>();

        if (playerSettings != null)
        {
            if (playerSettings.isHostPlayer.Value == PlayerRole.Patient)
            {
                clockObject.SetActive(false);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (updateTime)
        {
            SetTime();
        }
        if (testClock)
        {
            testClock = false;
            SetTime();
        }
    }


    void SetTime()
    {
        if(playerSettings == null)
        {
            playerSettings = this.GetComponentInParent<PlayerSettings>();
        }
        timePassed = Time.realtimeSinceStartup;
        TimeSpan timeSpan = TimeSpan.FromSeconds(timePassed);
        int minutes = timeSpan.Minutes;
        int seconds = timeSpan.Seconds;
        string updatedTime = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        clock.SetText(updatedTime);
    }
    public void GazeFocusChanged(bool hasFocus)
    {
        updateTime = hasFocus;
    }
}
