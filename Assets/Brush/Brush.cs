//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.XR;
//using Normal.Realtime;
//using UnityEngine.InputSystem;


//public class Brush : MonoBehaviour
//{
//    // Reference to Realtime to use to instantiate brush strokes
//    [SerializeField] private Realtime _realtime = null;

//    // Prefab to instantiate when we draw a new brush stroke
//    [SerializeField] private GameObject _brushStrokePrefab = null;
//    private GameObject brushStrokeGameObject;

//    // Which hand should this brush instance track?
//    public enum Hand { LeftHand, RightHand };
//    [SerializeField] private Hand _hand = Hand.RightHand;

//    public bool triggerPressed;

//    public InputActionReference toggleRefLeft = null;
//    public InputActionReference toggleRefRight = null;


//    public GameObject leftHandObject;
//    public GameObject rightHandObject;
//    private GameObject activeHand;
//    //public Transform handRotation;
//    //public Transform handPosition;



//    //// Used to keep track of the current brush tip position and the actively drawing brush stroke
//    private Vector3 _activeHandPosition;
//    private Quaternion _activeHandRotation;
//    private BrushStroke _activeBrushStroke;


//    private void Awake()
//    {

//        toggleRefLeft.action.started += ToggleLeft;
//        toggleRefRight.action.started += ToggleRight;
//        activeHand = leftHandObject;

//    }

//    private void OnDestroy()
//    {

//        toggleRefLeft.action.started -= ToggleLeft;
//        toggleRefRight.action.started -= ToggleRight;

//    }


//    public void ToggleLeft(InputAction.CallbackContext context)
//    {
//        triggerPressed = !triggerPressed;
//        activeHand = leftHandObject;
//        Debug.Log("drawing left");
//    }
//    public void ToggleRight(InputAction.CallbackContext context)
//    {
//        triggerPressed = !triggerPressed;
//        activeHand = rightHandObject;
//        Debug.Log("drawing right");
//    }
//    private void Update()
//    {



//        _activeHandPosition = activeHand.transform.position;
//        _activeHandRotation = activeHand.transform.rotation;

//        // Start by figuring out which hand we're tracking
//        // XRNode node = _hand == Hand.LeftHand ? XRNode.LeftHand : XRNode.RightHand;
//        //string trigger = _hand == Hand.LeftHand ? "Left Trigger" : "Right Trigger";

//        // Get the position & rotation of the hand
//        //bool handIsTracking = UpdatePose(node, ref _handPosition, ref _handRotation);

//        // Figure out if the trigger is pressed or not
//        //bool triggerPressed = Input.GetAxisRaw(trigger) > 0.1f;

//        // If we lose tracking, stop drawing
//        // if (!handIsTracking)
//        //{
//        // triggerPressed = false;
//        //   Debug.Log("trackingNotFound");
//        // }
//        // If the trigger is pressed and we haven't created a new brush stroke to draw, create one!
//        if (triggerPressed && _activeBrushStroke == null)
//        {
//            if (_realtime == null || !_realtime.connected)
//                return;

//            //Instantiate a copy of the Brush Stroke prefab, set it to be owned by us.
//            Realtime.InstantiateOptions _options = new();
//            _options.ownedByClient = true;
//            _options.useInstance = _realtime;
//            brushStrokeGameObject = Realtime.Instantiate(_brushStrokePrefab.name, _options);

//            //Grab the BrushStroke component from it
//            _activeBrushStroke = brushStrokeGameObject.GetComponent<BrushStroke>();

//            // Tell the BrushStroke to begin drawing at the current brush position
//            _activeBrushStroke.BeginBrushStrokeWithBrushTipPoint(_activeHandPosition, _activeHandRotation);
//        }

//        // If the trigger is pressed, and we have a brush stroke, move the brush stroke to the new brush tip position
//        if (triggerPressed)
//            _activeBrushStroke.MoveBrushTipToPoint(_activeHandPosition, _activeHandRotation);

//        // If the trigger is no longer pressed, and we still have an active brush stroke, mark it as finished and clear it.
//        if (!triggerPressed && _activeBrushStroke != null)
//        {
//            _activeBrushStroke.EndBrushStrokeWithBrushTipPoint(_activeHandPosition, _activeHandRotation);
//            _activeBrushStroke = null;
//        }
//    }

//    //// Utility

//    // Given an XRNode, get the current position & rotation. If it's not tracking, don't modify the position & rotation.
//    //private static bool UpdatePose(XRNode node, ref Vector3 position, ref Quaternion rotation)
//    //{
//    //    List<XRNodeState> nodeStates = new List<XRNodeState>();
//    //    InputTracking.GetNodeStates(nodeStates);

//    //    foreach (XRNodeState nodeState in nodeStates)
//    //    {
//    //        if (nodeState.nodeType == node)
//    //        {
//    //            Vector3 nodePosition;
//    //            Quaternion nodeRotation;
//    //            bool gotPosition = nodeState.TryGetPosition(out nodePosition);
//    //            bool gotRotation = nodeState.TryGetRotation(out nodeRotation);

//    //            if (gotPosition)
//    //                position = nodePosition;
//    //            if (gotRotation)
//    //                rotation = nodeRotation;

//    //            return gotPosition;
//    //        }
//    //    }

//    //    return false;
//    //}
//}
