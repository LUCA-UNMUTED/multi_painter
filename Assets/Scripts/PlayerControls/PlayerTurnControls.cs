using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerTurnControls : MonoBehaviour
{
    public InputActionReference switchTurn = null;
    public GameObject Instructor;
    public GameObject Patient;

    public PlayerData PlayerData;
    public PlayerCapabilities playerCapabilities;
    private void Awake()
    {
       switchTurn.action.started += SwitchTurn;
    }

    private void OnDestroy()
    {

        switchTurn.action.started -= SwitchTurn;
    }

    private void Start()
    {

        PlayerData = this.GetComponent<PlayerData>();
        playerCapabilities = FindAnyObjectByType<PlayerCapabilities>();
        playerCapabilities.ClassifyPlayers();

    }

    public void SwitchTurn(InputAction.CallbackContext context)
    {
        if (PlayerData.canSwitch == true) // || PlayerData.forceSwitch == true)
        {
            playerCapabilities.SwitchPlayer();
        }
    }

    //TODO make copy of switchturn, but based on physical buttons: one for regular (can switch, visible on both sides - force switch, visible only for instructor)
}
