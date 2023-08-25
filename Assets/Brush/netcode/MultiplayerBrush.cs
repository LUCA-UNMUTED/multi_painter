using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerBrush : NetworkBehaviour
{
    [Header("input")]
    public PlayerControls playerInput;
    public InputAction brushActionLeft;
    public InputAction brushActionRight;
    public InputAction brushActionKeyboard;

    [Header("Brush")]
    // Prefab to instantiate when we draw a new brush stroke
    [SerializeField] private GameObject _brushStrokePrefab = null;
    private GameObject brushStrokeGameObject;
    [SerializeField] private bool _brushIsEnabled = false;

    [SerializeField] private List<GameObject> spawnedBrushStrokes = new();

    public PlayerSettings playerSettings;
    public BrushStroke_Netcode brushStroke;
    [SerializeField] BrushPointerCapture_multi_player brushPointerCapture; // SINGLE PLAYER OR MULTIPLAYER

    private void Awake()
    {
        playerSettings = this.GetComponent<PlayerSettings>();
        playerInput = new();
        _brushIsEnabled = false;
    }

    private void OnEnable()
    {
        brushActionLeft = playerInput.Player.BrushLeftHand;
        brushActionLeft.Enable();
        brushActionLeft.performed += StartBrushLeft;
        brushActionLeft.canceled += StopBrush;

        brushActionRight = playerInput.Player.BrushRightHand;
        brushActionRight.Enable();
        brushActionRight.performed += StartBrushRight;
        brushActionRight.canceled += StopBrush;


        brushActionKeyboard = playerInput.Player.KeyboardDraw;
        brushActionKeyboard.Enable();
        brushActionKeyboard.performed += StartBrushKeyboard;
    }

    private void OnDisable()
    {

        brushActionLeft.performed -= StartBrushLeft;
        brushActionLeft.canceled -= StopBrush;
        brushActionLeft.Disable();

        brushActionRight.performed -= StartBrushRight;
        brushActionRight.canceled -= StopBrush;
        brushActionRight.Disable();

        brushActionKeyboard.performed -= StartBrushKeyboard;

    }
    private void StartBrushKeyboard(InputAction.CallbackContext context)
    {
        StartBrushCommon();
    }
    private void StartBrushCommon()
    {
        Debug.Log("StartBrush");
        if (!IsOwner) return;
        if (GetComponent<PlayerSettings>().isAllowedToDraw.Value)
        {
            //switch brush mode
            _brushIsEnabled = !_brushIsEnabled;
            if (_brushIsEnabled) StartBrushServerRPC();
            else EndBrushServerRpc();
        }
        else
        {
            Debug.Log("Nope, not allowed to draw boy");
        }
    }
    private void StartBrushRight(InputAction.CallbackContext context)
    {
        playerSettings.activeHand = playerSettings.RightHand;
        StartBrushCommon();
    }

    private void StartBrushLeft(InputAction.CallbackContext context)
    {
        playerSettings.activeHand = playerSettings.LeftHand;
        StartBrushCommon();
    }

    private void StopBrush(InputAction.CallbackContext context)
    {
        Debug.Log("Stopping the brush");
        if (GetComponent<PlayerSettings>().isAllowedToDraw.Value)
        {
            _brushIsEnabled = !_brushIsEnabled;
            //switch brush mode
            EndBrushServerRpc();
        }
    }


    [ServerRpc]
    private void StartBrushServerRPC(ServerRpcParams serverRpcParams = default)
    {

        brushStrokeGameObject = Instantiate(_brushStrokePrefab, Vector3.zero, Quaternion.identity);
        var senderClientId = serverRpcParams.Receive.SenderClientId;
        var senderPlayerObject = PlayerSettings.Players[senderClientId].NetworkObject;

        brushStrokeGameObject.GetComponent<BrushStroke_Netcode>().pointerObject = senderPlayerObject.GetComponent<PlayerSettings>().activeHand.transform;
        brushPointerCapture = brushStrokeGameObject.AddComponent<BrushPointerCapture_multi_player>();
        // deze lijn wordt op de server uitgevoerd, niet nuttig zo, indien erase nodig, kunnen we deze proberen implementeren
        //spawnedBrushStrokes.Add(brushStrokeGameObject);  
        brushStrokeGameObject.GetComponent<NetworkObject>().Spawn();
        brushStrokeGameObject.GetComponent<NetworkObject>().ChangeOwnership(senderClientId);
        brushPointerCapture.activeBrushMP.Value = true;

        UpdateBrushStrokeListClientRpc();
    }

    [ClientRpc]
    void UpdateBrushStrokeListClientRpc()
    {
        // this only gets executed on the instantiater of the brushstroke, so the other player doesn't see this info.
        if (IsOwner)
        {
            spawnedBrushStrokes.Add(brushStrokeGameObject);
        }
    }

    [ServerRpc]
    private void EndBrushServerRpc()
    {
        brushPointerCapture.activeBrushMP.Value = false;
    }
}
