using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootFireBall : NetworkBehaviour
{
    [SerializeField] private GameObject fireball;
    [SerializeField] private Transform shootTransform;
    // List to hold all the instantiated fireballs;
    [SerializeField] private List<GameObject> spawnedFireballs = new();
    //private void OnFire(InputValue value)
    //{
    //    if (!IsOwner) { Debug.Log("not owner"); return; }
    //    GameObject go = Instantiate(fireball, shootTransform.position, shootTransform.rotation);
    //    go.GetComponent<NetworkObject>().Spawn();
    //}

    [SerializeField] private PlayerControls playerInput;


    private InputAction fire;

    //private Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        //animator = GetComponent<Animator>();
        playerInput = new();

    }
    private void OnEnable()
    {
        fire = playerInput.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        ShootServerRpc();
    }

    [ServerRpc]
    private void ShootServerRpc()
    {
        GameObject go = Instantiate(fireball, shootTransform.position, shootTransform.rotation);
        spawnedFireballs.Add(go);
        go.GetComponent<MoveProjectile>().parent = this;
        go.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership =false)] 
    public void DestroyServerRpc()
    {
        GameObject toDestroy = spawnedFireballs[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedFireballs.Remove(toDestroy);
        Destroy(toDestroy);
    }
}
