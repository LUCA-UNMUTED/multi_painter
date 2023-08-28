using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class MultiplayerBrush : CommonBrush
{
    public PlayerSettings playerSettings;
    [SerializeField] BrushPointerCapture_multi_player brushPointerCapture; // SINGLE PLAYER OR MULTIPLAYER
    

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        playerSettings = GetComponent<PlayerSettings>();

    }
    public override void ToggleBrushKeyboard(InputAction.CallbackContext context)
    {
        triggerPressed = !triggerPressed;
        if (triggerPressed)
        {
            StartBrushCommon();
        }
        else
        {
            StopBrush(context);
        }
    }
    private void StartBrushCommon()
    {
        //Debug.Log("StartBrush");
        if (!IsOwner) return;
        if (GetComponent<PlayerSettings>().isAllowedToDraw.Value)
        {
            if (brushStrokeGameObject != null) return; //make sure we can't draw simultaneously 
            StartBrushServerRPC();
            isDrawing = true;
        }
        else
        {
            Debug.Log("Nope, not allowed to draw boy");
        }
    }

    public override void StartBrushRight(InputAction.CallbackContext context)
    {
        //Debug.Log("drawing right");

        playerSettings.activeHand = playerSettings.RightHand;
        StartBrushCommon();
    }

    public override void StartBrushLeft(InputAction.CallbackContext context)
    {
        //Debug.Log("drawing left");
        playerSettings.activeHand = playerSettings.LeftHand;
        StartBrushCommon();
    }

    public override void StopBrush(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        //Debug.Log("Stopping the brush");
        if (GetComponent<PlayerSettings>().isAllowedToDraw.Value)
        {
            isDrawing = false;
            EndBrushServerRpc();
        }
    }


    [ServerRpc]
    private void StartBrushServerRPC(ServerRpcParams serverRpcParams = default)
    {
        //instantiate the stroke
        brushStrokeGameObject = Instantiate(_brushStrokePrefab, Vector3.zero, Quaternion.identity);
        //get some data from the owner
        var senderClientId = serverRpcParams.Receive.SenderClientId;
        var senderPlayerObject = PlayerSettings.Players[senderClientId].NetworkObject;

        brushPointerCapture = brushStrokeGameObject.GetComponent<BrushPointerCapture_multi_player>();

        brushStrokeGameObject.GetComponent<NetworkObject>().Spawn();
        brushStrokeGameObject.GetComponent<NetworkObject>().ChangeOwnership(senderClientId); //TODO wil ik wel ownership veranderen?
        
        // wie is aan het tekenen?
        brushPointerCapture.activeHandOwnerId.Value = senderClientId;
        // met welk hand tekenen we?
        var activeHand = senderPlayerObject.GetComponent<PlayerSettings>().activeHand.CompareTag("leftHand") ? Hand.Left : Hand.Right;
        brushPointerCapture.activeHandMP.Value = activeHand;
        // we mogen tekenen
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
        _activeBrushStroke = null;
        brushStrokeGameObject = null;
    }
}
