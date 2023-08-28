using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class CommonBrush : NetworkBehaviour
{
    [Header("input")]
    public XRIDefaultInputActions playerInput;
    public InputAction brushActionLeft;
    public InputAction brushActionRight;
    public InputAction brushActionKeyboard;

    [Header("Brush")]
    // Prefab to instantiate when we draw a new brush stroke
    public GameObject _brushStrokePrefab = null;
    [HideInInspector] public GameObject brushStrokeGameObject;
    [HideInInspector] public bool triggerPressed = false;

    public List<GameObject> spawnedBrushStrokes = new();

    [HideInInspector] public BrushStroke_Netcode _activeBrushStroke;

    public bool isDrawing = false;

    private void Awake()
    {
        playerInput = new();
    }

    private void OnEnable()
    {
        brushActionLeft = playerInput.Player.BrushLeftHand;
        brushActionLeft.Enable();
        brushActionLeft.started += StartBrushLeft;
        brushActionLeft.canceled += StopBrush;

        brushActionRight = playerInput.Player.BrushRightHand;
        brushActionRight.Enable();
        brushActionRight.started += StartBrushRight;
        brushActionRight.canceled += StopBrush;

        brushActionKeyboard = playerInput.Player.KeyboardDraw;
        brushActionKeyboard.Enable();
        brushActionKeyboard.performed += ToggleBrushKeyboard;
    }

    private void OnDisable()
    {

        brushActionLeft.started -= StartBrushLeft;
        brushActionLeft.canceled -= StopBrush;
        brushActionLeft.Disable();

        brushActionRight.started -= StartBrushRight;
        brushActionRight.canceled -= StopBrush;
        brushActionRight.Disable();

        brushActionKeyboard.performed -= ToggleBrushKeyboard;
        brushActionKeyboard.Disable();

    }
    public virtual void ToggleBrushKeyboard(InputAction.CallbackContext context)
    {
    }
   
    public virtual void StartBrushRight(InputAction.CallbackContext context)
    {
        Debug.Log("Starting brush right");
    }

    public virtual void StartBrushLeft(InputAction.CallbackContext context)
    {
        Debug.Log("Starting brush left");
    }

    public virtual void StopBrush(InputAction.CallbackContext context)
    {
        Debug.Log("Stopping brush");
    }
}
