using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// The netcode variant (used by single as well as multiplayer setup) of the normcore drawing example moves the responsibilty of drawing towards the stroke itselves
/// Instead of the player pushing his position and rotation, we capture the transform and track it.
/// </summary>
public class BrushStroke_Netcode : MonoBehaviour
{
    [SerializeField] private BrushStrokeMesh _mesh;

    // Ribbon State
    struct RibbonPoint
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    private List<RibbonPoint> _ribbonPoints = new List<RibbonPoint>();

    private Vector3 _brushTipPosition;
    private Quaternion _brushTipRotation;
    private bool _brushStrokeFinalized;

    // Smoothing
    private Vector3 _ribbonEndPosition;
    private Quaternion _ribbonEndRotation = Quaternion.identity;

    // Mesh
    private Vector3 _previousRibbonPointPosition;
    private Quaternion _previousRibbonPointRotation = Quaternion.identity;

    public bool active = false;

    [SerializeField] private bool started = false;
    [SerializeField] private bool stopped = false;
    [SerializeField] private Vector3 positionOffset = new(0f, 0f, 0f);

    public Color singleplayerColor = Color.red;
    public Transform pointerObject;

    private void Awake()
    {
        //CreateMeshServerRpc();
        _mesh = GetComponent<BrushStrokeMesh>();
    }

    // Unity Events
    private void Update()
    {
        // Animate the end of the ribbon towards the brush tip
        AnimateLastRibbonPointTowardsBrushTipPosition();

        // Add a ribbon segment if the end of the ribbon has moved far enough
        AddRibbonPointIfNeeded();

        if (pointerObject == null) return; // as long as we don't have the pointer object set correctly we can't commence drawing

        Vector3 _pointerPos = pointerObject.position;
        Quaternion _pointerRot = pointerObject.rotation;

        if (active)
        {
            //Debug.Log("position of drawer " + _parentPos);
            if (!started)
            {
                // Tell the BrushStroke to begin drawing at the current brush position

                BeginBrushStrokeWithBrushTipPoint(_pointerPos + positionOffset, _pointerRot);
            }
            // If the trigger is pressed, and we have a brush stroke, move the brush stroke to the new brush tip position

            MoveBrushTipToPoint(_pointerPos + positionOffset, _pointerRot);
            // Animate the end of the ribbon towards the brush tip
            AnimateLastRibbonPointTowardsBrushTipPosition();

            // Add a ribbon segment if the end of the ribbon has moved far enough
            AddRibbonPointIfNeeded();
        }
        else
        {
            if (!stopped && started)
            {
                EndBrushStrokeWithBrushTipPoint(_pointerPos + positionOffset, _pointerRot);
                stopped = true;
            }
        }
    }

    // Interface
    public void BeginBrushStrokeWithBrushTipPoint(Vector3 position, Quaternion rotation)
    {

        // adapt the material to our instantiating player
        Material currentMat = GetComponent<MeshRenderer>().material;
        currentMat.SetColor("_BaseColor", singleplayerColor);

        // Update the model
        _brushTipPosition = position;
        _brushTipRotation = rotation;

        // Update last ribbon point to match brush tip position & rotation
        _ribbonEndPosition = position;
        _ribbonEndRotation = rotation;
        _mesh.UpdateLastRibbonPoint(_ribbonEndPosition, _ribbonEndRotation);

        // notify the brushstroke that we're initialized
        started = true;
    }

    public void MoveBrushTipToPoint(Vector3 position, Quaternion rotation)
    {
        _brushTipPosition = position;
        _brushTipRotation = rotation;
    }


    public void EndBrushStrokeWithBrushTipPoint(Vector3 position, Quaternion rotation)
    {
        // Add a final ribbon point and mark the stroke as finalized
        AddRibbonPoint(position, rotation);
        _brushStrokeFinalized = true;
    }


    // Ribbon drawing
    private void AddRibbonPointIfNeeded()
    {
        // If the brush stroke is finalized, stop trying to add points to it.
        if (_brushStrokeFinalized)
            return;

        if (Vector3.Distance(_ribbonEndPosition, _previousRibbonPointPosition) >= 0.01f ||
            Quaternion.Angle(_ribbonEndRotation, _previousRibbonPointRotation) >= 10.0f)
        {

            // Add ribbon point model to ribbon points array. This will fire the RibbonPointAdded event to update the mesh.
            AddRibbonPoint(_ribbonEndPosition, _ribbonEndRotation);

            // Store the ribbon point position & rotation for the next time we do this calculation
            _previousRibbonPointPosition = _ribbonEndPosition;
            _previousRibbonPointRotation = _ribbonEndRotation;
        }
    }

    private void AddRibbonPoint(Vector3 position, Quaternion rotation)
    {
        // Create the ribbon point
        RibbonPoint ribbonPoint = new RibbonPoint();
        ribbonPoint.position = position;
        ribbonPoint.rotation = rotation;
        _ribbonPoints.Add(ribbonPoint);

        // Update the mesh
        _mesh.InsertRibbonPoint(position, rotation);
    }

    // Brush tip + smoothing

    private void AnimateLastRibbonPointTowardsBrushTipPosition()
    {
        // If the brush stroke is finalized, skip the brush tip mesh, and stop animating the brush tip.
        if (_brushStrokeFinalized)
        {
            _mesh.skipLastRibbonPoint = true;
            return;
        }

        Vector3 brushTipPosition = _brushTipPosition;
        Quaternion brushTipRotation = _brushTipRotation;

        // If the end of the ribbon has reached the brush tip position, we can bail early.
        if (Vector3.Distance(_ribbonEndPosition, brushTipPosition) <= 0.0001f &&
            Quaternion.Angle(_ribbonEndRotation, brushTipRotation) <= 0.01f)
        {
            return;
        }

        // Move the end of the ribbon towards the brush tip position
        _ribbonEndPosition = Vector3.Lerp(_ribbonEndPosition, brushTipPosition, 25.0f * Time.deltaTime);
        _ribbonEndRotation = Quaternion.Slerp(_ribbonEndRotation, brushTipRotation, 25.0f * Time.deltaTime);

        // Update the end of the ribbon mesh
        _mesh.UpdateLastRibbonPoint(_ribbonEndPosition, _ribbonEndRotation);
    }
}