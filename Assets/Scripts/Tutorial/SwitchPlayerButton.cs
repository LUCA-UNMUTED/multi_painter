using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlayerButton : MonoBehaviour
{
    public bool isTouched = false;
    private MeshRenderer _mesh;
    private CapsuleCollider _coll;

    [SerializeField] private Vector3 _shakeDimensions;

    [Header("Debug")]
    [SerializeField] private bool testSwitch = false;
    // Start is called before the first frame update
    void Start()
    {
        _mesh = GetComponent<MeshRenderer>();
        _coll = GetComponent<CapsuleCollider>();
        _coll.enabled = false;
        _mesh.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (testSwitch)
        {
            testSwitch = false;
            EnableSwitch();
        }
    }

    public void EnableSwitch()
    {
        _coll.enabled = true;
        _mesh.enabled = true;
        Hashtable ht = iTween.Hash("amount", _shakeDimensions, "time", 2.0f);
        iTween.ShakeScale(this.gameObject, ht);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            Debug.Log("Touched by " + other);
            isTouched = true;
            _coll.enabled = false;
            _mesh.enabled = false;
        }
    }
}
