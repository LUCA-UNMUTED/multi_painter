using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;


public class SingleplayerBrush : MonoBehaviour
{
    // Prefab to instantiate when we draw a new brush stroke
    [SerializeField] private GameObject _brushStrokePrefab = null;
    private GameObject brushStrokeGameObject;

    // Which hand should this brush instance track?
    public enum Hand { LeftHand, RightHand };
    [SerializeField] private Hand _hand = Hand.RightHand;

    public bool triggerPressed;

    public InputActionReference toggleRefLeft = null;
    public InputActionReference toggleRefRight = null;


    public GameObject leftHandObject;
    public GameObject rightHandObject;
    private GameObject activeHand;
 
    //// Used to keep track of the current brush tip position and the actively drawing brush stroke
    private Vector3 _activeHandPosition;
    private Quaternion _activeHandRotation;
    private BrushStroke_Netcode _activeBrushStroke;

    [SerializeField] BrushPointerCapture brushPointerCapture; // SINGLE PLAYER OR MULTIPLAYER


    private void Awake()
    {
        toggleRefLeft.action.started += ToggleLeft;
        toggleRefRight.action.started += ToggleRight;
        activeHand = leftHandObject;
    }

    private void OnDestroy()
    {

        toggleRefLeft.action.started -= ToggleLeft;
        toggleRefRight.action.started -= ToggleRight;
    }


    public void ToggleLeft(InputAction.CallbackContext context)
    {
        triggerPressed = !triggerPressed;
        activeHand = leftHandObject;
        Debug.Log("drawing left");
    }
    public void ToggleRight(InputAction.CallbackContext context)
    {
        triggerPressed = !triggerPressed;
        activeHand = rightHandObject;
        Debug.Log("drawing right");
    }
    private void Update()
    {
        _activeHandPosition = activeHand.transform.position;
        _activeHandRotation = activeHand.transform.rotation;

        // If the trigger is pressed and we haven't created a new brush stroke to draw, create one!
        if (triggerPressed && _activeBrushStroke == null)
        {

            brushStrokeGameObject = Instantiate(_brushStrokePrefab);

            //Grab the BrushStroke component from it
            _activeBrushStroke = brushStrokeGameObject.GetComponent<BrushStroke_Netcode>();

            // Tell the BrushStroke to begin drawing at the current brush position
            _activeBrushStroke.BeginBrushStrokeWithBrushTipPoint(_activeHandPosition, _activeHandRotation);
        }

        // If the trigger is pressed, and we have a brush stroke, move the brush stroke to the new brush tip position
        if (triggerPressed)
            _activeBrushStroke.MoveBrushTipToPoint(_activeHandPosition, _activeHandRotation);

        // If the trigger is no longer pressed, and we still have an active brush stroke, mark it as finished and clear it.
        if (!triggerPressed && _activeBrushStroke != null)
        {
            _activeBrushStroke.EndBrushStrokeWithBrushTipPoint(_activeHandPosition, _activeHandRotation);
            _activeBrushStroke = null;
        }
    }
}
