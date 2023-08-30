using LSL;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class DataAnnotation : NetworkBehaviour
{
    [Header("input")]
    public XRIDefaultInputActions playerInput;
    public InputAction buttonA;

    [Header("data streams")]
    private WsClient ws;
    private OutletPassThrough lsl;

    [Header("data annotation")]
    private DataAnnotationEvent dataAnnotation;

    [Header("feedback")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private ParticleSystem particle;

    private void Awake()
    {
        playerInput = new();
        ws = WsClient.Instance;
        lsl = OutletPassThrough.Instance;
    }

    private void OnEnable()
    {

        buttonA = playerInput.Player.AnnotateData;
        buttonA.Enable();
        buttonA.performed += AnnotateData;

    }

    private void OnDisable()
    {
        buttonA.Disable();
        buttonA.performed -= AnnotateData;
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner && IsHost)
        {
            dataAnnotation = new(GetComponent<PlayerSettings>().playerName);
        }

    }

    private void AnnotateData(InputAction.CallbackContext context)
    {
        Debug.Log("M");
        if (IsOwner && IsHost)
        {
            _audioSource.Play();
            particle.Play();
            ws.SendWSMessage(dataAnnotation.SaveToString());
            lsl.SendMarker(Marker.start_change);
        }
    }
}


public class DataAnnotationEvent
{
    public float _time;
    public string msg = "annotation";
    public string ownerName;



    public DataAnnotationEvent(string _ownerName)
    {
        ownerName = _ownerName;
    }

    public string SaveToString()
    {
        _time = Time.time;
        return JsonUtility.ToJson(this);
    }
}
