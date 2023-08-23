using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardPlayerBrush : NetworkBehaviour
{
    [Header("input")]
    public PlayerControls playerInput;
    public InputAction brushActionLeft;
    public InputAction brushActionRight;

    [Header("Brush")]
    // Prefab to instantiate when we draw a new brush stroke
    [SerializeField] private GameObject _brushStrokePrefab = null;
    private GameObject brushStrokeGameObject;
    [SerializeField] private bool _brushIsEnabled = false;

    [SerializeField] private List<GameObject> spawnedBrushStrokes = new();

    public PlayerSettings playerSettings;
    public BrushStroke brushStroke;
    private void Awake()
    {
        playerSettings = this.GetComponent<PlayerSettings>();
        playerInput = new();
        _brushIsEnabled = false;
    }

    private void OnEnable()
    {
        brushActionLeft = playerInput.Player.BrushLeftHand;
        brushActionRight = playerInput.Player.BrushRightHand;

        brushActionLeft.Enable();
        brushActionRight.Enable();

        brushActionLeft.performed += StartBrushLeft;
        brushActionLeft.canceled += StopBrushLeft;
        brushActionRight.performed += StartBrushRight;
        brushActionRight.canceled += StopBrushRight;




    }

    private void OnDisable()
    {

        brushActionLeft.performed -= StartBrushLeft;
        brushActionLeft.canceled -= StopBrushLeft;
        brushActionRight.performed -= StartBrushRight;
        brushActionRight.canceled -= StopBrushRight;

        brushActionRight.Disable();
    }


    private void StartBrushRight(InputAction.CallbackContext context)
    {

        
        playerSettings.activeHand = playerSettings.RightHand;
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

    private void StopBrushRight(InputAction.CallbackContext context)
    {
        if (GetComponent<PlayerSettings>().isAllowedToDraw.Value)
        {
           _brushIsEnabled = !_brushIsEnabled;
             //switch brush mode
            EndBrushServerRpc();
        }

    }

    private void StartBrushLeft(InputAction.CallbackContext context)
    {

        playerSettings.activeHand = playerSettings.LeftHand;

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

    private void StopBrushLeft(InputAction.CallbackContext context)
    {
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

        // deze lijn wordt op de server uitgevoerd, niet nuttig zo, indien erase nodig, kunnen we deze proberen implementeren
        //spawnedBrushStrokes.Add(brushStrokeGameObject);  
        brushStrokeGameObject.GetComponent<NetworkObject>().Spawn();
        brushStrokeGameObject.GetComponent<BrushStroke>().active.Value = true;
        brushStrokeGameObject.GetComponent<NetworkObject>().ChangeOwnership(senderClientId);
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
        brushStrokeGameObject.GetComponent<BrushStroke>().active.Value = false;
    }
}
