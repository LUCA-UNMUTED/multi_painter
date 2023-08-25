using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class Brush : MonoBehaviour
{
    // Prefab to instantiate when we draw a new brush stroke
    [SerializeField] private GameObject _brushStrokePrefab = null;

    // Which hand should this brush instance track?
    private enum Hand { LeftHand, RightHand };
    [SerializeField] private Hand _hand = Hand.RightHand;

    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    [Header("input")]
    public PlayerControls playerInput;
    public InputAction brushActionLeft;
    public InputAction brushActionRight;
    public InputAction brushActionKeyboard;


    // Used to keep track of the current brush tip position and the actively drawing brush stroke
    private Vector3 _handPosition;
    private Quaternion _handRotation;
    private BrushStroke _activeBrushStroke;

    private bool triggerPressed = false;
    private void Update()
    {
        // Start by figuring out which hand we're tracking
        //XRNode node    = _hand == Hand.LeftHand ? XRNode.LeftHand : XRNode.RightHand;
        //string trigger = _hand == Hand.LeftHand ? "Left Trigger" : "Right Trigger";

        GameObject hand = _hand == Hand.LeftHand ? leftHand : rightHand;
        // Get the position & rotation of the hand
        bool handIsTracking = UpdatePose(hand, ref _handPosition, ref _handRotation);

        //// Figure out if the trigger is pressed or not
        //triggerPressed = Input.GetAxisRaw(trigger) > 0.1f;

        // If we lose tracking, stop drawing
        if (!handIsTracking)
            triggerPressed = false;

        // If the trigger is pressed and we haven't created a new brush stroke to draw, create one!
        if (triggerPressed && _activeBrushStroke == null)
        {
            // Instantiate a copy of the Brush Stroke prefab.
            GameObject brushStrokeGameObject = Instantiate(_brushStrokePrefab);

            // Grab the BrushStroke component from it
            _activeBrushStroke = brushStrokeGameObject.GetComponent<BrushStroke>();

            // Tell the BrushStroke to begin drawing at the current brush position
            _activeBrushStroke.BeginBrushStrokeWithBrushTipPoint(_handPosition, _handRotation);
        }

        // If the trigger is pressed, and we have a brush stroke, move the brush stroke to the new brush tip position
        if (triggerPressed)
            _activeBrushStroke.MoveBrushTipToPoint(_handPosition, _handRotation);

        // If the trigger is no longer pressed, and we still have an active brush stroke, mark it as finished and clear it.
        if (!triggerPressed && _activeBrushStroke != null)
        {
            _activeBrushStroke.EndBrushStrokeWithBrushTipPoint(_handPosition, _handRotation);
            _activeBrushStroke = null;
        }
    }

    //// Utility

    // Given an XRNode, get the current position & rotation. If it's not tracking, don't modify the position & rotation.
    private static bool UpdatePose(GameObject hand, ref Vector3 position, ref Quaternion rotation)
    {

        Vector3 nodePosition = hand.transform.position;
        Quaternion nodeRotation = hand.transform.rotation;

        position = nodePosition;
        rotation = nodeRotation;

        return true;
    }


    private void Awake()
    {
        playerInput = new();
    }

    private void OnEnable()
    {
        brushActionLeft = playerInput.Player.BrushLeftHand;
        brushActionLeft.Enable();
        brushActionLeft.performed += SwitchBrushLeft;

        brushActionRight = playerInput.Player.BrushRightHand;
        brushActionRight.Enable();
        brushActionRight.performed += SwitchBrushRight;


        brushActionKeyboard = playerInput.Player.KeyboardDraw;
        brushActionKeyboard.Enable();
        brushActionKeyboard.performed += SwitchBrushLeft;
    }

    private void OnDisable()
    {

        brushActionLeft.performed -= SwitchBrushLeft;
        brushActionLeft.Disable();

        brushActionRight.performed -= SwitchBrushRight;
        brushActionRight.Disable();

        brushActionKeyboard.performed -= SwitchBrushLeft;

    }

    private void SwitchBrushLeft(InputAction.CallbackContext context)
    {
        triggerPressed = !triggerPressed;
        _hand = Hand.LeftHand;
    }
    private void SwitchBrushRight(InputAction.CallbackContext context)
    {
        triggerPressed = !triggerPressed;
        _hand = Hand.RightHand;
    }
}
