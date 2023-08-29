using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class BrushPointerCapture_multi_player : BrushPointerCapture
{
    //public NetworkVariable<bool> activeBrushMP = new(false);
    //public NetworkVariable<Hand> activeHandMP = new(Hand.Left); // which hand is drawing
    //public NetworkVariable<ulong> activeHandOwnerId = new(2); // which player is the owner of the hand
    [SerializeField] private NetworkVariables networkVariables;
    public override void CapturePosition()
    {
        throw new System.NotImplementedException();
    }


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Break();
        pointerObject = GetComponent<BrushStroke_Netcode>().pointerObject;
        if (networkVariables == null)
            networkVariables = NetworkVariables.Instance;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        brushStroke = GetComponent<BrushStroke_Netcode>();
        if (networkVariables == null)
            networkVariables = NetworkVariables.Instance;
        networkVariables.activeBrushMP.OnValueChanged += SignalBrushStroke;
        networkVariables.activeHandOwnerId.OnValueChanged += UpdatePlayerObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<BrushStroke_Netcode>().stopped) return;


        if (pointerObject == null && networkVariables.activeBrushMP.Value)
        {
            var activePlayer = PlayerSettings.Players[networkVariables.activeHandOwnerId.Value].gameObject;
            pointerObject = networkVariables.activeHandMP.Value == Hand.Left ? activePlayer.GetComponent<PlayerSettings>().LeftHand.transform : activePlayer.GetComponent<PlayerSettings>().RightHand.transform;
            _color = activePlayer.GetComponent<PlayerSettings>().PlayerColor;
        }
        if (pointerObject != null)
        {
            parentPos = pointerObject.transform.position;
            parentRot = pointerObject.transform.rotation;
        }
    }
    private void SignalBrushStroke(bool previous, bool current)
    {
        brushStroke.active = current;
    }

    private void UpdatePlayerObject(ulong previousOwner, ulong newOwner)
    {
        Debug.Log("updated the player id from " + previousOwner + " to " + newOwner);
    }
}
