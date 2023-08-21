using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class DumpCrate : NetworkBehaviour
{

    [SerializeField] private GameObject crate;

    [SerializeField] private PlayerControls playerInput;
    private InputAction dump;
    // Start is called before the first frame update
    void Start()
    {
    }
    private void Awake()
    {
        playerInput = new();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        dump = playerInput.Player.Fire;
        dump.Enable();
        dump.performed += Dump;
    }

    private void Dump(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        DumpCrateServerRPC();
    }

    [ServerRpc]
    private void DumpCrateServerRPC()
    {
        GameObject go = Instantiate(crate, new Vector3(transform.position.x + 2.0f, transform.position.y + 1.55f, transform.position.z), this.transform.rotation);
        go.GetComponent<NetworkObject>().Spawn();
    }
}
