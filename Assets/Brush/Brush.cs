using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class Brush : MonoBehaviour
{
    // Prefab to instantiate when we draw a new brush stroke
    [SerializeField] private GameObject _brushStrokePrefab = null;

    // Which hand should this brush instance track?
    private enum Hand { LeftHand, RightHand };
    [SerializeField] private Hand _hand = Hand.RightHand;
    public bool triggerPressed;

    public LineThickness lineThickness;


    public InputActionReference toggleRefLeft = null;
    public InputActionReference toggleRefRight = null;
    public InputAction brushAction;
public XRIDefaultInputActions   playerInput;



    public GameObject leftHandObject;
    public GameObject rightHandObject;
    public GameObject activeHand;
    // Used to keep track of the current brush tip position and the actively drawing brush stroke
    private Vector3 _activeHandPosition;
    private Quaternion _activeHandRotation;
    private BrushStroke _activeBrushStroke;
    private void Awake()
    {

        playerInput = new();
        Invoke("FindDep", 1f);

    }
    private void OnEnable()
    {


        brushAction = playerInput.DrawCommands.LeftControllerDraw;
        brushAction.Enable();
        brushAction.performed += ToggleLeft;



        //brush = GameObject.FindGameObjectWithTag("Brush");
        toggleRefLeft.action.started += ToggleLeft;
        toggleRefRight.action.started += ToggleRight;
        toggleRefLeft.action.canceled += ToggleLeft;
        toggleRefRight.action.canceled += ToggleRight;
        leftHandObject = GameObject.FindWithTag("leftHand");
        rightHandObject = GameObject.FindWithTag("rightHand");
        activeHand = leftHandObject;

    }
    private void OnDisable()
    {
        brushAction.performed -= ToggleLeft;

        toggleRefLeft.action.started -= ToggleLeft;
        toggleRefRight.action.started -= ToggleRight;
        toggleRefLeft.action.canceled -= ToggleLeft;
        toggleRefRight.action.canceled -= ToggleRight;

    }
    private void FindDep()
    {
        leftHandObject = GameObject.FindWithTag("leftHand");
        rightHandObject = GameObject.FindWithTag("rightHand");
        activeHand = leftHandObject;

    }

    public void ToggleLeft(InputAction.CallbackContext context)
    {

        Debug.Log("drawing left");

        triggerPressed = !triggerPressed;
        activeHand = leftHandObject;
        lineThickness.brushStrokeMesh = _brushStrokePrefab.GetComponentInChildren<BrushStrokeMesh>();



    }
    public void ToggleRight(InputAction.CallbackContext context)
    {
        triggerPressed = !triggerPressed;
        activeHand = rightHandObject;
        lineThickness.brushStrokeMesh = _brushStrokePrefab.GetComponentInChildren<BrushStrokeMesh>();

        Debug.Log("drawing right");
    }
    private void Update()
    {

        if (activeHand != null)
        {


            _activeHandPosition = activeHand.transform.position;
            _activeHandRotation = activeHand.transform.rotation;


        }


        // If the trigger is pressed and we haven't created a new brush stroke to draw, create one!
        if (triggerPressed && _activeBrushStroke == null)
        {
            // Instantiate a copy of the Brush Stroke prefab.
            GameObject brushStrokeGameObject = Instantiate(_brushStrokePrefab);

            // Grab the BrushStroke component from it
            _activeBrushStroke = brushStrokeGameObject.GetComponent<BrushStroke>();

            // Tell the BrushStroke to begin drawing at the current brush position
            _activeBrushStroke.BeginBrushStrokeWithBrushTipPoint(_activeHandPosition, _activeHandRotation);

            Debug.Log("Drawing a line");
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

    //// Utility

    // Given an XRNode, get the current position & rotation. If it's not tracking, don't modify the position & rotation.
    private static bool UpdatePose(XRNode node, ref Vector3 position, ref Quaternion rotation)
    {
        List<XRNodeState> nodeStates = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodeStates);

        foreach (XRNodeState nodeState in nodeStates)
        {
            if (nodeState.nodeType == node)
            {
                Vector3 nodePosition;
                Quaternion nodeRotation;
                bool gotPosition = nodeState.TryGetPosition(out nodePosition);
                bool gotRotation = nodeState.TryGetRotation(out nodeRotation);

                if (gotPosition)
                    position = nodePosition;
                if (gotRotation)
                    rotation = nodeRotation;

                return gotPosition;
            }
        }

        return false;
    }
}
