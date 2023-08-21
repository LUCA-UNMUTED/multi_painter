using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class BrushNetcode : NetworkBehaviour
{
    // Prefab to instantiate when we draw a new brush stroke
    [SerializeField] private GameObject _brushStrokePrefab = null;
    private GameObject brushStrokeGameObject;

    [SerializeField] private bool activeBrush = false;


    [Header("positions")]
    [SerializeField] private Vector3 positionOffset = new(0f, 1.0f, 0f);
    [SerializeField] private Quaternion rotationOffset = new(0f,1.0f,0f,0f);
    //// Used to keep track of the current brush tip position and the actively drawing brush stroke
    [SerializeField] private GameObject gameObjectToFollow;

    private BrushStroke _activeBrushStroke;

    public void EnableBrush()
    {
        Debug.Log("Enabling Brush");
        activeBrush = true;
    }

    public void DisableBrush()
    {
        Debug.Log("Disabling Brush");
        activeBrush = false;
    }

    public void SetGameObjectToFollow(GameObject go)
    {
        Debug.Log("GameObject to Follow is " + go);
        gameObjectToFollow = go;
    }

    private void Update()
    {

        // If the trigger is pressed and we haven't created a new brush stroke to draw, create one!
        if (activeBrush && _activeBrushStroke == null)
        {
            brushStrokeGameObject = Instantiate(_brushStrokePrefab);

            brushStrokeGameObject.GetComponent<NetworkObject>().Spawn();

            //Grab the BrushStroke component from it
            _activeBrushStroke = brushStrokeGameObject.GetComponent<BrushStroke>();

            // Tell the BrushStroke to begin drawing at the current brush position
            _activeBrushStroke.BeginBrushStrokeWithBrushTipPoint(gameObjectToFollow.transform.position + positionOffset, gameObjectToFollow.transform.rotation);
        }

        // If the trigger is pressed, and we have a brush stroke, move the brush stroke to the new brush tip position
        if (activeBrush)
            _activeBrushStroke.MoveBrushTipToPoint(gameObjectToFollow.transform.position + positionOffset, gameObjectToFollow.transform.rotation);

        // If the trigger is no longer pressed, and we still have an active brush stroke, mark it as finished and clear it.
        if (!activeBrush && _activeBrushStroke != null)
        {
            _activeBrushStroke.EndBrushStrokeWithBrushTipPoint(gameObjectToFollow.transform.position + positionOffset, gameObjectToFollow.transform.rotation);
            _activeBrushStroke = null;
        }
    }
}
