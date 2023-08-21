using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkUI : MonoBehaviour
{
    [SerializeField]    private Button hostButton;
    [SerializeField]    private Button clientButton;

    [Header("Debug")]
    [SerializeField] private bool testHost;
    [SerializeField] private bool testClient;

    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });

        
    }

    private void Update()
    {
        if (testClient)
        {
            testClient = false;
            NetworkManager.Singleton.StartClient();

        }  
        if (testHost)
        {
            testHost = false;
            NetworkManager.Singleton.StartHost();

        }
    }


}
