using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class BrushPointerCapture : NetworkBehaviour
{
    public abstract void CapturePosition();
}
