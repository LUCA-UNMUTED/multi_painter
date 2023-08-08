using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Normal.Realtime;
using UnityEngine.InputSystem;


public class Brush : MonoBehaviour
{
    // Reference to Realtime to use to instantiate brush strokes
    [SerializeField] private Realtime _realtime = null;

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
    private BrushStroke _activeBrushStroke;


    private void Awake()
    {
        toggleRefLeft.action.started += ToggleLeft;
        toggleRefRight.action.started += ToggleRight;
        activeHand = _hand == Hand.LeftHand ? leftHandObject: rightHandObject; 
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
            if (_realtime == null || !_realtime.connected)
                return;

            //Instantiate a copy of the Brush Stroke prefab, set it to be owned by us.
            Realtime.InstantiateOptions _options = new();
            _options.ownedByClient = true;
            _options.useInstance = _realtime;
            brushStrokeGameObject = Realtime.Instantiate(_brushStrokePrefab.name, _options);

            //Grab the BrushStroke component from it
            _activeBrushStroke = brushStrokeGameObject.GetComponent<BrushStroke>();

            // Tell the BrushStroke to begin drawing at the current brush position
            _activeBrushStroke.BeginBrushStrokeWithBrushTipPoint(_activeHandPosition, _activeHandRotation);
        }

        // If the trigger is pressed, and we have a brush stroke, move the brush stroke to the new brush tip position
        if (triggerPressed)
        {
            _activeBrushStroke.MoveBrushTipToPoint(_activeHandPosition, _activeHandRotation);
            // set the correct controller to actively drawing
            activeHand.GetComponent<ActiveDrawing>().isDrawing = true;
        }

        // If the trigger is no longer pressed, and we still have an active brush stroke, mark it as finished and clear it.
        if (!triggerPressed && _activeBrushStroke != null)
        {
            _activeBrushStroke.EndBrushStrokeWithBrushTipPoint(_activeHandPosition, _activeHandRotation);
            _activeBrushStroke = null;

            // unset the correct controller to actively drawing
            activeHand.GetComponent<ActiveDrawing>().isDrawing = false;
        }
    }
}
