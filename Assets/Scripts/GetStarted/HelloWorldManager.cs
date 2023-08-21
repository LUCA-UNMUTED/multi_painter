using Unity.Netcode;
using UnityEngine;
using TMPro;

namespace HelloWorld
{
    public class HelloWorldManager : NetworkBehaviour
    {
        private NetworkVariable<int> playersNum = new(0, NetworkVariableReadPermission.Everyone);
        [SerializeField] private TMPro.TMP_Text playerCountText;
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();

                DisconnectButton();
                //SubmitNewPosition();

                //RequestOwnership();
            }

            GUILayout.EndArea();
        }


        private void Update()
        {
            playerCountText.text = playersNum.Value.ToString();
            if (!IsServer) return;
            playersNum.Value = NetworkManager.Singleton.ConnectedClients.Count;
        }
        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        static void DisconnectButton()
        {
            if (GUILayout.Button("Disconnect 1")) NetworkManager.Singleton.DisconnectClient(0);
            if (GUILayout.Button("Disconnect 2")) NetworkManager.Singleton.DisconnectClient(1);

        }
        static void SubmitNewPosition()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
            {
                if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
                {
                    foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
                        NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<HelloWorldPlayer>().Move();
                }
                else
                {
                    var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                    var player = playerObject.GetComponent<HelloWorldPlayer>();
                    player.Move();
                }
            }

        }

        static void RequestOwnership()
        {
            if (NetworkManager.Singleton.IsClient)
            {
                if (GUILayout.Button("request ownership"))
                {
                    var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                    var player = playerObject.GetComponent<HelloWorldPlayer>();
                    player.GetObject();
                }
            }
        }
    }
}