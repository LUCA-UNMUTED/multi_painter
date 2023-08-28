using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum Hand: byte
{
    Left, Right
}

public class BrushPointerCapture_multi_player : BrushPointerCapture
{
    public NetworkVariable<bool> activeBrushMP = new(false);
    public NetworkVariable<Hand> activeHandMP = new(Hand.Left); // which hand is drawing
    public NetworkVariable<ulong> activeHandOwnerId = new(0); // which player is the owner of the hand
    
    public override void CapturePosition()
    {
        throw new System.NotImplementedException();
    }


    // Start is called before the first frame update
    void Start()
    {
        pointerObject = GetComponent<BrushStroke_Netcode>().pointerObject;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        brushStroke = GetComponent<BrushStroke_Netcode>();
        activeBrushMP.OnValueChanged += SignalBrushStroke;

    }

    // Update is called once per frame
    void Update()
    {
        var activePlayer = PlayerSettings.Players[activeHandOwnerId.Value].gameObject;
        pointerObject = activeHandMP.Value == Hand.Left ? activePlayer.GetComponent<PlayerSettings>().LeftHand.transform : activePlayer.GetComponent<PlayerSettings>().RightHand.transform;

        if (pointerObject == null) return;
        parentPos = pointerObject.transform.position; 
        parentRot = pointerObject.transform.rotation; 
    }
    private void SignalBrushStroke(bool previous, bool current)
    {
        brushStroke.active = current;
    }
}
