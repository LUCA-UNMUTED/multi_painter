using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerCapabilities : MonoBehaviour
{

    public PlayerRole playerTurn;

    private GameObject Instructor;
    private GameObject Patient;

    private PlayerData InstructorData;
    private PlayerData PatientData;

    private GameObject brushInstructor;
    private GameObject brushPatient;

    public TurnClock turnClock;
    private GameObject[] Players;

    public Material InstructorColor;
    public Material PatientColor;
    public PlayerRole playerTurnColor;
    public GameObject BrushPrefab;

    [SerializeField] private PlayerUI playerUI;


    private void Start()
    {
        turnClock.SwitchToInstructorClock();
        //LookForPlayers();

    }


    public void SwitchBrush()
    {
        //PLACEHOLDER
        if (playerTurn == PlayerRole.Instructor)
        {
            brushInstructor.SetActive(true);
            brushPatient.SetActive(false);

        }
        else if (playerTurn == PlayerRole.Patient)
        {
            brushInstructor.SetActive(false);
            brushPatient.SetActive(true);
        }

    }

    public void SwitchPlayer()
    {
        var activePlayer = InstructorData.activePlayer ? Instructor : Patient;

        switch (playerTurn)
        {
            case PlayerRole.Instructor:

                playerTurn = PlayerRole.Patient;
                turnClock.SwitchToPatientClock();
                BrushPrefab.GetComponentInChildren<MeshRenderer>().material = InstructorColor;

                ////SwitchBrush();
                //InstructorData.canSwitch = false;
                //InstructorData.forceSwitch = true;

                if (PatientData != null)
                {
                    //    PatientData.canSwitch = true;
                    PatientData.activePlayer = true;
                }
                InstructorData.activePlayer = false;
                Debug.Log("Turn Patient");
                break;
            case PlayerRole.Patient:
                playerTurn = PlayerRole.Instructor;
                InstructorData.activePlayer = true;
                PatientData.activePlayer = false;
                turnClock.SwitchToInstructorClock();
                BrushPrefab.GetComponentInChildren<MeshRenderer>().material = PatientColor;
                //InstructorData.canSwitch = true;
                //InstructorData.forceSwitch = false;

                if (PatientData != null)
                    PatientData.canSwitch = false;
                Debug.Log("Turn Instructor");

                //SwitchBrush();
                break;

            default:
                Debug.LogError("wrong player shizzle");
                break;
        }
        playerUI.RefreshPlayerUI();

    }


    public void ClassifyPlayers()
    {
        Players = GameObject.FindGameObjectsWithTag("_Player");

        if (Players.Length > 0)
        {
            Instructor = Players[0];
            Instructor.name = "Instructor";
            InstructorData = Instructor.GetComponent<PlayerData>();
            //Instructor.tag = "Instructor";
            InstructorData.playerRole = PlayerRole.Instructor;
            //InstructorData.canSwitch = true; // starting value
            //InstructorData.forceSwitch = false; // starting value
            InstructorData.activePlayer = true;

            //FOR DEBUG MOET BIJ PATIENT_______________________________________________________________________________________________________________________________________________________
            turnClock.startTimeCount = true;
            if (Players.Length > 1)
            {
                Patient = Players[1];
                Patient.name = "Patient";
                //Patient.tag = "Patient";
                PatientData = Patient.GetComponent<PlayerData>();
                PatientData.playerRole = PlayerRole.Patient;
                PatientData.activePlayer = false;
            }

            //Find all players with tag player
            //hierarchy goes from top to bottom
            //first find == player that entered first == player1
            //second find == player that entered second == player2

            //change tags off players
        }
    }
}
