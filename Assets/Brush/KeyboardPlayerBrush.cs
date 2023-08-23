using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardPlayerBrush : NetworkBehaviour
{
    [Header("input")]
    [SerializeField] private PlayerControls playerInput;
    private InputAction brushAction;

    [Header("Brush")]
    // Prefab to instantiate when we draw a new brush stroke
    [SerializeField] private GameObject _brushStrokePrefab = null;
    private GameObject brushStrokeGameObject;
    [SerializeField] private bool _brushIsEnabled = false;

    [SerializeField] private List<GameObject> spawnedBrushStrokes = new();



    private void Awake()
    {
        playerInput = new();
        _brushIsEnabled = false;
    }

    private void OnEnable()
    {
        brushAction = playerInput.Player.Brush;
        brushAction.Enable();
        brushAction.performed += Brush;
    }

    private void OnDisable()
    {
        brushAction.performed -= Brush;
        brushAction.Disable();
    }


    private void Brush(InputAction.CallbackContext context)
    {
        Debug.Log("Brush");
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
