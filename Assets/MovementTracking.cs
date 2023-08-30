using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MovementTracking : NetworkBehaviour
{

    private WsClient ws;
    private MovementEvent movementEvent;

    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject head;

    private string playerName;

    private bool readyToTransmitData = false;
    // Start is called before the first frame update
    void Start()
    {
        ws = WsClient.Instance;
        movementEvent = new();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsOwner) return;

        movementEvent.headPos = head.transform.position;
        movementEvent.leftHandPos = leftHand.transform.position;
        movementEvent.rightHandPos = rightHand.transform.position;
        movementEvent.headRot = head.transform.rotation;
        movementEvent.leftHandRot = leftHand.transform.rotation;
        movementEvent.rightHandRot = rightHand.transform.rotation;
        movementEvent.ownerName = playerName;
        ws.SendWSMessage(movementEvent.SaveToString());
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) return;
        playerName = GetComponent<PlayerSettings>().playerName;
        readyToTransmitData = true;
    }
}

public class MovementEvent
{
    public float _time;
    public string websocketMessage = "movement";
    public string ownerName;
    public Vector3 leftHandPos;
    public Vector3 rightHandPos;
    public Vector3 headPos;
    public Quaternion leftHandRot;
    public Quaternion rightHandRot;
    public Quaternion headRot;


    public MovementEvent()
    {
    }

    public string SaveToString()
    {
        _time = Time.time;
        return JsonUtility.ToJson(this);
    }
}
