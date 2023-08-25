using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushPointerCapture_single_player : BrushPointerCapture
{
    public bool activeBrush = false;

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
        brushStroke = GetComponent<BrushStroke_Netcode>();
        pointerObject = brushStroke.pointerObject;
    }

    // Update is called once per frame
    void Update()
    {
        parentPos = pointerObject.transform.position; 
        parentRot = pointerObject.transform.rotation;
        brushStroke.active = activeBrush;
    }
}
