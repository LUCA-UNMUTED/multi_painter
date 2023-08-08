using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TurnSwitch : MonoBehaviour
{


    public enum PlayerTurn { Instructor, player2 };



    public PlayerTurn playerTurn;

    public GameObject Instructor;
    public GameObject Patient;

    public PlayerTurnData InstructorData;
    public PlayerTurnData PatientData;

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
        if (playerTurn == PlayerTurn.Instructor)
        {
            brushInstructor.SetActive(true);
            brushPatient.SetActive(false);

        }
        else if (playerTurn == PlayerTurn.player2)
        {
            brushInstructor.SetActive(false);
            brushPatient.SetActive(true);
        }

    }
    public void SwitchPlayer()
    {

        if (playerTurn == PlayerTurn.Instructor)
        {
            playerTurn = PlayerTurn.player2;
            //SwitchBrush();
            InstructorData.canSwitch = false;
            if (PatientData != null)
                PatientData.canSwitch = true;
            InstructorTurnUI.gameObject.SetActive(false);
            PatientTurnUI.gameObject.SetActive(true);
            Debug.Log("Turn Patient");
        }
        else if (playerTurn == PlayerTurn.player2)
        {
            playerTurn = PlayerTurn.Instructor;

            InstructorData.canSwitch = true;
            if (PatientData != null)
                PatientData.canSwitch = false;

            InstructorTurnUI.gameObject.SetActive(true);
            PatientTurnUI.gameObject.SetActive(false);

            Debug.Log("Turn Instructor");

            //SwitchBrush();
        }
    }


    public void LookForPlayers()
    {

        Players = GameObject.FindGameObjectsWithTag("_Player");

        if (Players.Length > 0 && Players.Length <= 1)
        {
            Instructor = Players[0];
            Instructor.tag = "Instructor";
        }
        else if (Players.Length > 1 && Players.Length <= 2)
        {
            Patient = Players[1];
            Patient.tag = "Patient";
        }


        //Find all players with tag player
        //hierarchy goes from top to bottom
        //first find == player that entered first == player1
        //second find == player that entered second == player2

        //change tags off players
    }
}