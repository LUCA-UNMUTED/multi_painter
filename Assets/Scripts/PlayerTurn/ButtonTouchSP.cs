using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class ButtonTouchSP : MonoBehaviour
{
    public Collider _collider;

    [SerializeField] private UnityEvent touchEvent;

    public bool isEnabled = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!isEnabled) return;
        Debug.Log("trigger! " + other);
        if (other.CompareTag("leftHand") || other.CompareTag("rightHand") || other.CompareTag("GameController"))
        {
            _collider = other;
            if (touchEvent != null) touchEvent.Invoke();
        }
    }
}
