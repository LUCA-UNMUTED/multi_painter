using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;


public class SingleplayerBrush : CommonBrush
{
    // Which hand should this brush instance track?
    //public enum Hand { LeftHand, RightHand };
    [Header("Tracking objects")]
    //[SerializeField] private Hand _hand = Hand.RightHand;

    public GameObject leftHandObject;
    public GameObject rightHandObject;
    private GameObject activeHand;

    public override void ToggleBrushKeyboard(InputAction.CallbackContext context)
    {
        triggerPressed = !triggerPressed;
        if (triggerPressed)
        {
            StartBrushLeft(context);
        }
        else
        {
            StopBrush(context);
        }
    }

    public override void StartBrushLeft(InputAction.CallbackContext context)
    {
        Debug.Log("drawing left");
        triggerPressed = true;
        activeHand = leftHandObject;
    }
    public override void StartBrushRight(InputAction.CallbackContext context)
    {
        Debug.Log("drawing right");
        triggerPressed = true;
        activeHand = rightHandObject;
    }

    public override void StopBrush(InputAction.CallbackContext context)
    {
        Debug.Log("Stopping the brush");
        isDrawing = false;
    }
    private void Update()
    {
        // If the trigger is pressed and we haven't created a new brush stroke to draw, create one!
        if (triggerPressed && _activeBrushStroke == null)
        {
            // make clear we're drawing
            isDrawing = true;

            brushStrokeGameObject = Instantiate(_brushStrokePrefab);
            brushStrokeGameObject.AddComponent<BrushPointerCapture_single_player>();
            brushStrokeGameObject.GetComponent<BrushPointerCapture_single_player>().activeBrush = true;
            //Grab the BrushStroke component from it
            _activeBrushStroke = brushStrokeGameObject.GetComponent<BrushStroke_Netcode>();
            _activeBrushStroke.pointerObject = activeHand.transform;
        }
        // If the trigger is no longer pressed, and we still have an active brush stroke, mark it as finished and clear it.
        if (!triggerPressed && _activeBrushStroke != null)
        {
            isDrawing = false;

            brushStrokeGameObject.GetComponent<BrushPointerCapture_single_player>().activeBrush = false;
        }
    }
}
