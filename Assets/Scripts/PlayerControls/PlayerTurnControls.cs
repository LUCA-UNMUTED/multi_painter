using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerTurnControls : MonoBehaviour
{
    public InputActionReference switchTurn = null;
    public GameObject Instructor;
    public GameObject Patient;

    public PlayerTurnData PlayerData;
    public PlayerCapabilities turnSwitch;
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
        turnSwitch = FindAnyObjectByType<PlayerCapabilities>();
        turnSwitch.ClassifyPlayers();


        //Instructor = GameObject.FindWithTag("Instructor");
        //Patient = GameObject.FindWithTag("Patient");


        //turnSwitch.Instructor = GameObject.FindWithTag("Instructor");
        //if (Patient != null)
        //    turnSwitch.Patient = GameObject.FindWithTag("Patient");



        //turnSwitch.InstructorData = Instructor.GetComponent<PlayerTurnData>();

        //if (Patient != null)
        //    turnSwitch.PatientData = Patient.GetComponent<PlayerTurnData>();

        //turnSwitch.InstructorData.canSwitch = true;
    }

    public void SwitchTurn(InputAction.CallbackContext context)
    {

        if (PlayerData.canSwitch == true || PlayerData.forceSwitch == true)
        {
            turnSwitch.SwitchPlayer();
        }
    }

    //TODO make copy of switchturn, but based on physical buttons: one for regular (can switch, visible on both sides - force switch, visible only for instructor)
}
