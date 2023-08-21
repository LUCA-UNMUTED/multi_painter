using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new();
        //public NetworkVariable<NetworkObject> ownedObject = new();

        [SerializeField] private NetworkObject ObjectToGet;
        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
            }
        }

        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        public void GetObject()
        {
            if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
            {
                ObjectToGet.transform.parent = this.transform;
            }
            else
            {
                //SubmitOwnershipRequestServerRpc(OwnerClientId, ObjectToGet);
            }
        }


        //[ServerRpc]
        //void SubmitOwnershipRequestServerRpc(ulong newOwnerClientId, NetworkObject networkObject)
        //{
        //    //networkObject.ChangeOwnership(newOwnerClientId);
        //    //Debug.Log("ownership is now owned by server?" + networkObject.IsOwnedByServer);
        //    //networkObject.transform.parent = transform;
        //}


        void Update()
        {
            transform.position = Position.Value;
        }
    }
}