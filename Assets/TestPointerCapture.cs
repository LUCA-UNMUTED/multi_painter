using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TestPointerCapture : NetworkBehaviour
{

    public HandPositionStruct handPosition = new();
    public Transform hand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hand = handPosition.handTransform;
    }
}
