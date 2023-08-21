using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class ActivateButton : NetworkBehaviour
{

    [SerializeField] private Color active;
    [SerializeField] private Color neutral;


    private NetworkVariable<bool> isActive = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private MeshRenderer _mesh;

    // Start is called before the first frame update
    void Start()
    {
        _mesh = GetComponent<MeshRenderer>();
        _mesh.material.SetColor("_BaseColor", neutral);

    }

    private void OnCollisionEnter(Collision collision)
    {
        bool isActivePlayer = collision.gameObject.GetComponent<PlayerSettings>().isAllowedToDraw.Value;
        PlayerRole isHostPlayer = collision.gameObject.GetComponent<PlayerSettings>().isHostPlayer.Value;

        AlterActiveServerRpc(isActivePlayer, isHostPlayer);
    }

    [ServerRpc(RequireOwnership = false)]
    private void AlterActiveServerRpc(bool isActivePlayer, PlayerRole isHostPlayer)
    {
        //initialise color
        Color color = Color.white;
        // if the player is active, we can alter the active player
        // EXCEPT is we're the host
        if (isActivePlayer || (isHostPlayer==PlayerRole.Instructor))
        {
            //isActive.Value = !isActive.Value;
            foreach (PlayerMovement _player in PlayerMovement.Players.Values)
            {
                _player.GetComponent<PlayerSettings>().isAllowedToDraw.Value = !_player.GetComponent<PlayerSettings>().isAllowedToDraw.Value;
                //update the color for the NEW active player
                if (_player.GetComponent<PlayerSettings>().isAllowedToDraw.Value)
                {
                    color = _player.GetComponent<PlayerSettings>().PlayerColor;
                }
            }
            //Make sure all clients update the color of their button
            SetButtonColorClientRpc(color);
        }
    }

    // this functions sets the color of the button on all clients at once
    [ClientRpc]
    private void SetButtonColorClientRpc(Color color)
    {
        active = color;
        _mesh.material.SetColor("_BaseColor", active);
    }
}