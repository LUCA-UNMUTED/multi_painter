using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPoint : MonoBehaviour
{

    [SerializeField] private Color _highlightColor;
    [SerializeField] private Color _defaultColor;

    public bool isTouched = false;

    [SerializeField] private bool testTouch = false;
    // Start is called before the first frame update
    void Start()
    {
        _defaultColor = gameObject.GetComponent<Renderer>().material.GetColor("_BaseColor");
    }

    // Update is called once per frame
    void Update()
    {
        if (testTouch)
        {
            testTouch = false;
            SetPointTouched();
        }
    }

    public void EnableTouchPoint(Vector3 _relMoveToLocation)
    {
        Hashtable ht = iTween.Hash("y", _relMoveToLocation.y, "islocal", true, "time", 1.0f, "easetype", "easeInOutExpo", "oncomplete", "SetTouchPointColor");
        // I prefer using moveto instead of moveby, to avoid double jumping if called twice.
        iTween.MoveTo(gameObject, ht);
        isTouched = false;
    }

    private void SetTouchPointColor()
    {
        gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", _highlightColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        //first check if it's a controller, then if it's the active drawing one
        if (other.CompareTag("GameController") && other.GetComponent<ActiveDrawing>().isDrawing)
        {
            SetPointTouched();
        }
    }

    private void SetPointTouched()
    {
        //set isTouched true, so the tutorial gameobject can see this
        isTouched = true;
        //visualize it to the user
        gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", _defaultColor);
    }
}
