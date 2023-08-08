using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPoint : MonoBehaviour
{

    [SerializeField] private Color _highlightColor;
    [SerializeField] private Color _defaultColor;

    public bool isTouched = false;
    // Start is called before the first frame update
    void Start()
    {
        _defaultColor = gameObject.GetComponent<Renderer>().material.GetColor("_BaseColor");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableTouchPoint()
    {
        isTouched = false;
        gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", _highlightColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        //first check if it's a controller, then if it's the active drawing one
        if (other.CompareTag("GameController") && other.GetComponent<ActiveDrawing>().isDrawing)
        {
            //set isTouched true, so the tutorial gameobject can see this
            isTouched = true;
            //visualize it to the user
            gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", _defaultColor);
        }
    }
}
