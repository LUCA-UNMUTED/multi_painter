using LSL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStreamInitializer : MonoBehaviour
{
    public OutletPassThrough outlet;
    public WsClient ws;
    // Start is called before the first frame update
    void Start()
    {
        outlet = OutletPassThrough.Instance;
        ws = WsClient.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
