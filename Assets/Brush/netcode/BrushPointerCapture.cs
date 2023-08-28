using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class BrushPointerCapture : NetworkBehaviour
{
    public abstract void CapturePosition();
    public Transform pointerObject;
    public Vector3 parentPos;
    public Quaternion parentRot;

    public BrushStroke_Netcode brushStroke;


}
