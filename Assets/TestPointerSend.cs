using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TestPointerSend : NetworkBehaviour
{
    public TestPointerCapture cap;
    [SerializeField] private Transform hand;
    // Start is called before the first frame update
    void Start()
    {
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        cap = GameObject.FindGameObjectWithTag("Cube").GetComponent<TestPointerCapture>();

        cap.handPosition.handTransform = hand;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
