using System.Collections;
using System.Collections.Generic;
using Tobii.XR;
using UnityEngine;
using Unity.Netcode;

public class LaunchEyeTracking : NetworkBehaviour
{
    private PlayerSettings playerSettings;
    private bool eyeTrackingStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        playerSettings = GetComponent<PlayerSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSettings.playerSettingsSet & !eyeTrackingStarted) {
            // launch the XR settings for this player
            var settings = new TobiiXR_Settings();
            TobiiXR.Start(settings);
            eyeTrackingStarted = true;
        }
    }
}
