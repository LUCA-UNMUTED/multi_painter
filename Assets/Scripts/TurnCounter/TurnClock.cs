using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnClock : MonoBehaviour
{

    public float timeEndInstructor = 0;
    public float timeEndPatient = 0;

    private string instructorString;
    private string patientString;

    private TextMeshPro instructorTimeUI;
    private TextMeshPro patientTimeUI;
    //Bools veranderen naar player roles
    public bool instructorTimeRun;
    public bool patientTimeRun;

    public bool startTimeCount;
    public float timeCount;
    public string timeCountString;
    private TextMeshPro timeCountUI;


    void Start()
    {

        instructorTimeUI = GameObject.FindWithTag("InstructorTimer").GetComponent<TextMeshPro>();
        patientTimeUI = GameObject.FindWithTag("PatientTimer").GetComponent<TextMeshPro>();
        timeCountUI = GameObject.FindWithTag("Timer").GetComponent<TextMeshPro>();

        instructorTimeRun = false;
        patientTimeRun = false;
    }

    public void SwitchToPatientClock()
    {
        instructorTimeRun = false;
        patientTimeRun = true;
    }
    public void SwitchToInstructorClock()
    {
        instructorTimeRun = true;
        patientTimeRun = false;
    }

    public void StartTotalTime()
    {
        startTimeCount = true;
    }
    public void StopTotalTime()
    {
        startTimeCount = false;
    }
    void Update()
    {

        if (startTimeCount == true)
        {


            timeCount += Time.deltaTime;
            timeCountString = timeCount.ToString();
            timeCountUI.SetText("Time:" + timeCountString);

        }
        //Time RUNNING bools kunnen veranderd worden naar de playerRoles 
        if (instructorTimeRun == true)
        {


            timeEndInstructor += Time.deltaTime;
            instructorString = timeEndInstructor.ToString();
            instructorTimeUI.SetText("Time:" + instructorString);

        }

        if (patientTimeRun == true)
        {


            timeEndPatient += Time.deltaTime;
            timeEndPatient.ToString(patientString);
            patientString = timeEndPatient.ToString();
            patientTimeUI.SetText("Time:" + patientString);


        }
    }
}