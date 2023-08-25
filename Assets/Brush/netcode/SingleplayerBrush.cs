using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;


public class SingleplayerBrush : MonoBehaviour
{
    [Header("input")]
    public XRIDefaultInputActions playerInput;
    public InputAction brushActionLeft;
    public InputAction brushActionRight;
    public InputAction brushActionKeyboard;
    public bool triggerPressed;

    [Header("Brush")]
    // Prefab to instantiate when we draw a new brush stroke
    [SerializeField] private GameObject _brushStrokePrefab = null;
    private GameObject brushStrokeGameObject;

    // Which hand should this brush instance track?
    public enum Hand { LeftHand, RightHand };
    [Header("Tracking objects")]
    [SerializeField] private Hand _hand = Hand.RightHand;

    public GameObject leftHandObject;
    public GameObject rightHandObject;
    private GameObject activeHand;
 
    //// Used to keep track of the current brush tip position and the actively drawing brush stroke
    private BrushStroke_Netcode _activeBrushStroke;

    private void Awake()
    {
        activeHand = leftHandObject;
        playerInput = new();
    }


    private void OnEnable()
    {
        brushActionLeft = playerInput.Player.BrushLeftHand;
        brushActionLeft.Enable();
        brushActionLeft.performed += ToggleLeft;

        brushActionRight = playerInput.Player.BrushRightHand;
        brushActionRight.Enable();
        brushActionRight.performed += ToggleRight;


        brushActionKeyboard = playerInput.Player.KeyboardDraw;
        brushActionKeyboard.Enable();
        brushActionKeyboard.performed += ToggleLeft;
    }

    private void OnDisable()
    {

        brushActionLeft.performed -= ToggleLeft;
        brushActionLeft.Disable();

        brushActionRight.performed -= ToggleRight;
        brushActionRight.Disable();

        brushActionKeyboard.performed -= ToggleLeft;

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
        // If the trigger is pressed and we haven't created a new brush stroke to draw, create one!
        if (triggerPressed && _activeBrushStroke == null)
        {
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
            brushStrokeGameObject.GetComponent<BrushPointerCapture_single_player>().activeBrush = false;
        }
    }
}
