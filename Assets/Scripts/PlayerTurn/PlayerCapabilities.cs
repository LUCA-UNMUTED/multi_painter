using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerCapabilities : MonoBehaviour
{

    public PlayerRole playerTurn;

    public GameObject Instructor;
    public GameObject Patient;

    public PlayerData InstructorData;
    public PlayerData PatientData;

    public GameObject brushInstructor;
    public GameObject brushPatient;


    public GameObject[] Players;

    public GameObject InstructorTurnUI;
    public GameObject PatientTurnUI;

    private void Start()
    {

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
        switch (playerTurn)
        {
            case PlayerRole.Instructor:

                playerTurn = PlayerRole.Patient;
                //SwitchBrush();
                InstructorData.canSwitch = false;
                InstructorData.forceSwitch = true;

                if (PatientData != null)
                {
                    PatientData.canSwitch = true;
                }
                InstructorTurnUI.gameObject.SetActive(false);
                PatientTurnUI.gameObject.SetActive(true);
                Debug.Log("Turn Patient");
                break;
            case PlayerRole.Patient:

                playerTurn = PlayerRole.Instructor;

                InstructorData.canSwitch = true;
                InstructorData.forceSwitch = false;

                if (PatientData != null)
                    PatientData.canSwitch = false;

                InstructorTurnUI.gameObject.SetActive(true);
                PatientTurnUI.gameObject.SetActive(false);

                Debug.Log("Turn Instructor");

                //SwitchBrush();
                break;

            default:
                Debug.LogError("wrong player shizzle");
                break;
        }
    }


    public void ClassifyPlayers()
    {
        Players = GameObject.FindGameObjectsWithTag("_Player");

        if (Players.Length > 0)
        {
            Instructor = Players[0];
            PlayerData instructorData = Instructor.GetComponent<PlayerData>();
            //Instructor.tag = "Instructor";
            instructorData.playerRole = PlayerRole.Instructor;
            instructorData.canSwitch = true; // starting value
            instructorData.forceSwitch = false; // starting value

            if (Players.Length > 1)
            {
                Patient = Players[1];
                //Patient.tag = "Patient";
                PlayerData patientData = Patient.GetComponent<PlayerData>();
                patientData.playerRole = PlayerRole.Patient;
            }

            //Find all players with tag player
            //hierarchy goes from top to bottom
            //first find == player that entered first == player1
            //second find == player that entered second == player2

            //change tags off players
        }
    }
}