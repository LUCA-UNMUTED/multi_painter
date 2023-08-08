using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerTurnControls : MonoBehaviour
{


    public InputActionReference switchTurn = null;
    public GameObject Player1;
    public GameObject Player2;

    public PlayerTurnData PlayerData;


    public TurnSwitch turnSwitch;
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



        PlayerData = this.GetComponent<PlayerTurnData>();
        turnSwitch = FindAnyObjectByType<TurnSwitch>();
        turnSwitch.LookForPlayers();


        Player1 = GameObject.FindWithTag("Player1");
        Player2 = GameObject.FindWithTag("Player2");


        turnSwitch.Instructor = GameObject.FindWithTag("Player1");
        if (Player2 != null)
            turnSwitch.Patient = GameObject.FindWithTag("Player2");



        turnSwitch.InstructorData = Player1.GetComponent<PlayerTurnData>();

        if (Player2 != null)
            turnSwitch.PatientData = Player2.GetComponent<PlayerTurnData>();

        turnSwitch.InstructorData.canSwitch = true;
    }

    public void SwitchTurn(InputAction.CallbackContext context)
    {

        if (PlayerData.canSwitch == true)
        {
            turnSwitch.SwitchPlayer();
        }


    }
}
