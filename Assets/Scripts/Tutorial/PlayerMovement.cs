using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using Unity.Netcode.Components;
public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float movementSpeed = 7.0f;
    //[SerializeField] private float rotationSpeed = 500.0f;
    [SerializeField]
    private float positionRange = 3f;
    [SerializeField] private PlayerControls playerInput;

    public static Dictionary<ulong, PlayerMovement> Players = new Dictionary<ulong, PlayerMovement>();

    private InputAction move;

    //private Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        //animator = GetComponent<Animator>();
        playerInput = new();
    }
    private void OnEnable()
    {
        move = playerInput.Player.Move;
        move.Enable();
    }

    public override void OnNetworkSpawn()
    {
        Players[OwnerClientId] = this;
        base.OnNetworkSpawn();
        UpdatePositionServerRpc();
    }

    private void OnDisable()
    {
        move.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        Vector3 movementDirection = move.ReadValue<Vector2>();
        movementDirection.Normalize();

        transform.Translate(new Vector3(movementDirection.x * movementSpeed * Time.deltaTime, 0, movementDirection.y * movementSpeed * Time.deltaTime), Space.World);

        //if(movementDirection!= Vector3.zero)
        //{
        //    Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        //}

        //animator.SetFloat("run", movementDirection.magnitude);
    }

    [ServerRpc(RequireOwnership =false)]
    private void UpdatePositionServerRpc()
    {
        transform.position = new Vector3(Random.Range(-positionRange, positionRange), 0, Random.Range(-positionRange, positionRange));
    }
}
