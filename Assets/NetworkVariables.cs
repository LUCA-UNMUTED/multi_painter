using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkVariables : NetworkBehaviour
{

    #region SingletonPattern
    private static NetworkVariables _instance;
    public static NetworkVariables Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("networkvariables are Null");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public NetworkVariable<bool> activeBrushMP = new(false);
    public NetworkVariable<Hand> activeHandMP = new(Hand.Left); // which hand is drawing
    public NetworkVariable<ulong> activeHandOwnerId = new(0); // which player is the owner of the hand

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
