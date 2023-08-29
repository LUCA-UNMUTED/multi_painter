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
        if (!IsOwner) return;
        triggerPressed = !triggerPressed;
        if (triggerPressed)
        {
            Hand _hand = (Hand)Random.Range(0, 2);
            Debug.Log("keyboard hand " + _hand);
            playerSettings.activeHand.Value = _hand;
            StartBrushCommon(_hand);
        }
        else
        {
            StopBrush(context);
        }
    }
    private void StartBrushCommon(Hand _hand)
    {
        //Debug.Log("StartBrush");
        if (GetComponent<PlayerSettings>().isAllowedToDraw.Value)
        {
            if (brushStrokeGameObject != null) return; //make sure we can't draw simultaneously 
            ServerRpcParams serverRpcParams = new();
            StartBrushServerRPC(_hand, serverRpcParams);
            isDrawing = true;
        }
        else
        {
            Debug.Log("Nope, not allowed to draw boy");
        }
    }

    public override void StartBrushRight(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        Debug.Log("drawing right");
        playerSettings.activeHand.Value = Hand.Right;
        StartBrushCommon(Hand.Right);
    }

    public override void StartBrushLeft(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        Debug.Log("drawing left");
        playerSettings.activeHand.Value = Hand.Left;
        StartBrushCommon(Hand.Left);
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
    private void StartBrushServerRPC(Hand _hand, ServerRpcParams serverRpcParams = default)
    {
        //instantiate the stroke
        brushStrokeGameObject = Instantiate(_brushStrokePrefab, Vector3.zero, Quaternion.identity);
        //get some data from the owner
        var senderClientId = serverRpcParams.Receive.SenderClientId;
        Debug.Log("drawing with " + _hand + " client id "+ senderClientId);// senderPlayerObject.GetComponent<PlayerSettings>().activeHand.Value);

        var senderPlayerObject = PlayerSettings.Players[senderClientId].gameObject;

        brushPointerCapture = brushStrokeGameObject.GetComponent<BrushPointerCapture_multi_player>();

        brushStrokeGameObject.GetComponent<NetworkObject>().Spawn();
        brushStrokeGameObject.GetComponent<NetworkObject>().ChangeOwnership(senderClientId); //TODO wil ik wel ownership veranderen?

        // wie is aan het tekenen?
        brushPointerCapture.activeHandOwnerId.Value = senderClientId;
        // met welk hand tekenen we?
       
        brushPointerCapture.activeHandMP.Value = _hand;// senderPlayerObject.GetComponent<PlayerSettings>().activeHand.Value;
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
