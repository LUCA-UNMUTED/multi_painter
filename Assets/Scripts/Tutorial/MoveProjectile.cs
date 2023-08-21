using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MoveProjectile : NetworkBehaviour
{
    // Start is called before the first frame update
    //[SerializeField] private GameObject hitParticles;
    [SerializeField] private float shootForce;
    public ShootFireBall parent;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = rb.transform.forward * shootForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        //GameObject hitImpact = Instantiate(hitParticles, transform.position, Quaternion.identity);
        //hitImpact.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
        //Destroy(gameObject);
        if (!IsOwner) return;
        parent.DestroyServerRpc();
    }


}
