using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public Rigidbody rb;
    public float moveSpeed = 5f;
    public InputAction playerControls;
    Vector2 moveDirection = Vector2.zero;
    // Start is called before the first frame update
    private float rotationSpeed = 500F;
    private void OnEnable()
    {

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();

    }

    private void FixedUpdate()
    {

        // moveDirection.z = rb.velocity.z;
        //moveDirection.y = rb.velocity.y;
        moveDirection = playerControls.ReadValue<Vector2>();

        Vector3 movementDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
        moveDirection.Normalize();
        transform.Translate(new Vector3(movementDirection.x * moveSpeed * Time.deltaTime, 0, movementDirection.y * moveSpeed * Time.deltaTime));

        //this.transform.rotation = Quaternion.LookRotation (moveDirection);
        //this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (moveDirection), Time.deltaTime * 40f);

        Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);




    }
}
