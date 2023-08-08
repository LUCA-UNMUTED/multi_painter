using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    [Header("Tutorial info")]
    public bool tutorialIsFinished = false;
    public UnityEvent tutorialIsFinishedEvent = null;

    [Header("Touch points info")]
    [SerializeField] private List<GameObject> _touchPoints;
    private int _touchPointIndex = 0;
    [SerializeField] private bool _waitingForTouch = false;

    [Header("button to switch state")]
    [SerializeField] private SwitchPlayerButton switchPlayerButton;

    [Header("positions")]
    [SerializeField] private Vector3 _relMoveToLocation;
    private Vector3 _startingLocation;

    [Header("debug")]
    [SerializeField] private bool testStart = false;
    // Start is called before the first frame update
    void Start()
    {
        _startingLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (testStart)
        {
            testStart = false;
            StartTutorial();
        }
        if (_waitingForTouch)
        {
            if (_touchPoints[_touchPointIndex].GetComponent<TouchPoint>().isTouched)
            {
                if (_touchPointIndex < _touchPoints.Count - 1)
                {
                    //Debug.Log("on to the next touchpoint");
                    _touchPointIndex++;
                    EnableNextTouchPoint();
                }
                else
                {
                    //Debug.Log("enabling switch");
                    _waitingForTouch = false;
                    switchPlayerButton.EnableSwitch();
                }
            }
        }
        if (switchPlayerButton.isTouched)
        {
            switchPlayerButton.isTouched = false;
            Debug.Log("finished the tutorial");
            FinishTutorial();
        }
    }



    public void StartTutorial()
    {
        Debug.Log("Starting tutorial");
        Hashtable ht = iTween.Hash("position", _relMoveToLocation, "islocal", true, "time", 1.0f, "easetype", "easeInOutExpo", "oncomplete", "EnableNextTouchPoint");
        // I prefer using moveto instead of moveby, to avoid double jumping if called twice.
        iTween.MoveTo(gameObject, ht);
    }

    private void FinishTutorial()
    {
        // move back to starting position
        Hashtable ht = iTween.Hash("position", _startingLocation, "islocal", true, "time", 1.0f, "easetype", "easeInOutExpo");
        // I prefer using moveto instead of moveby, to avoid double jumping if called twice.
        iTween.MoveTo(gameObject, ht);

        // Tutorial finished
        tutorialIsFinished = true;
        if (tutorialIsFinishedEvent != null)
        {
            tutorialIsFinishedEvent.Invoke();
        }

        //destroy the created lines
        BrushStroke[] brushStrokes = FindObjectsByType<BrushStroke>(FindObjectsSortMode.None);
        foreach(BrushStroke b in brushStrokes)
        {
            Destroy(b.gameObject);
        }

    }
    private void EnableNextTouchPoint()
    {
        //Debug.Log("enabling next touchpoint " + _touchPointIndex);
        _touchPoints[_touchPointIndex].GetComponent<TouchPoint>().EnableTouchPoint();
        _waitingForTouch = true;
    }


}
