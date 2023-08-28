using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class ButtonTouch : NetworkBehaviour
{
    public Collider _collider;

    [SerializeField] private UnityEvent touchEvent;

    public bool isEnabled = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner) return;
        if (!isEnabled) return;
        Debug.Log("trigger! " + other);
        if (other.CompareTag("GameController"))
        {
            _collider = other;
            if (touchEvent != null) touchEvent.Invoke();
        }
    }
}
