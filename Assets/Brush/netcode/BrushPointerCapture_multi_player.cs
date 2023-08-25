using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BrushPointerCapture_multi_player : BrushPointerCapture
{
    public NetworkVariable<bool> activeBrushMP = new(false);

    public Vector3 parentPos;
    public Quaternion parentRot;
    public Transform pointerObject;

    private BrushStroke_Netcode brushStroke;
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
        parentPos = pointerObject.transform.position; //TODO
        parentRot = pointerObject.transform.rotation; //TODO    }
    }
    private void SignalBrushStroke(bool previous, bool current)
    {
        Debug.Log("updating the brush to " + current + " "+brushStroke);
        brushStroke.active = current;
    }
}
