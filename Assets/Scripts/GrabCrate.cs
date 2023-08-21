using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabCrate : NetworkBehaviour
{
    public float GrabDistance = 5.0f;

    private Rigidbody m_Rigidbody;
    private Material m_Material;

    private NetworkVariable<bool> m_IsGrabbed = new NetworkVariable<bool>();
    private bool m_IsGrabbable = false;

    [SerializeField] private PlayerControls playerInput;
    private InputAction release;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void Awake()
    {
        playerInput = new();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Material = GetComponent<Renderer>().material;
    }
    // Update is called once per frame
    void Update()
    {
        if (NetworkManager == null)
        {
            return;
        }

        if (m_IsGrabbed.Value)
        {
            m_Material.color = Color.cyan;
        }
        else
        {
            m_Material.color = Color.white;

            var localPlayerObject = NetworkManager?.SpawnManager?.GetLocalPlayerObject();

            if (localPlayerObject != null)
            {
                var distance = Vector3.Distance(transform.position, localPlayerObject.transform.position);
                if (distance <= GrabDistance)
                {
                    m_Material.color = Color.yellow;
                    m_IsGrabbable = true;
                }
                else
                {
                    m_IsGrabbable = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (NetworkManager == null)
        {
            return;
        }

        if (m_Rigidbody)
        {
            m_Rigidbody.isKinematic = !IsServer || m_IsGrabbed.Value;
        }
    }

    private void OnEnable()
    {
        release = playerInput.Player.Space;
        release.Enable();
        release.performed += Alter;
    }

    private void Alter(InputAction.CallbackContext context)
    {
        if (m_IsGrabbed.Value)
        {
            Debug.Log("Releasing");
            ReleaseServerRpc();
        }
        else
        {
            Debug.Log("trying to grab");

            if (m_IsGrabbable)
            {
                Debug.Log("trying to grab");
                GrabCrateToOwnerServerRPC();
            }
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void GrabCrateToOwnerServerRPC(ServerRpcParams serverRpcParams = default)
    {
        if (!m_IsGrabbed.Value)
        {
            var senderClientId = serverRpcParams.Receive.SenderClientId;
            var senderPlayerObject = PlayerMovement.Players[senderClientId].NetworkObject;
            Debug.Log("sender Client Id " + senderClientId);
            Debug.Log("sender player object" + senderPlayerObject);

            if (senderPlayerObject != null)
            {
                NetworkObject.ChangeOwnership(senderClientId);

                transform.parent = senderPlayerObject.transform;

                m_IsGrabbed.Value = true;
            }
        }
    }

    [ServerRpc]
    private void ReleaseServerRpc()
    {
        if (m_IsGrabbed.Value)
        {
            NetworkObject.RemoveOwnership();

            transform.parent = null;

            m_IsGrabbed.Value = false;
        }
    }
}
