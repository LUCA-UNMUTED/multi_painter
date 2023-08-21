using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;

public enum PlayerRole : byte
{
    Instructor,
    Patient
}

public class PlayerSettings : NetworkBehaviour
{
    [SerializeField] private MeshRenderer bodyMeshRenderer;
    [SerializeField] private MeshRenderer eyesMeshRenderer;
    [SerializeField] private TextMeshProUGUI playerName;
    private NetworkVariable<FixedString128Bytes> networkPlayerName = new("Player: 0", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public Color PlayerColor;
    public List<Color> colors = new();

    public NetworkVariable<bool> isAllowedToDraw = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<PlayerRole> isHostPlayer = new(PlayerRole.Patient, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        PlayerColor = colors[(int)OwnerClientId % colors.Count];
        bodyMeshRenderer.material.color = PlayerColor;
        networkPlayerName.Value = "Player:" + (OwnerClientId + 1);
        playerName.text = networkPlayerName.Value.ToString();
        gameObject.name = networkPlayerName.Value.ToString(); // in Unity editor more clearly

        //initial settings to decide role & who can start drawing

        if (IsServer || IsHost)
        {
            // the server can distribute the roles, we link the server to the instructor, as (s)he owns the instructor player
            if (IsOwner)
            {
                isAllowedToDraw.Value = true;
                isHostPlayer.Value = PlayerRole.Instructor;
            }
            else
            {
                isAllowedToDraw.Value = false;
                isHostPlayer.Value = PlayerRole.Patient;
            }
        }
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (isAllowedToDraw.Value)
        {
            SetEyes(Color.green);

        }
        else
        {
            SetEyes(Color.white);
        }
    }

    public void SetEyes(Color color)
    {
        eyesMeshRenderer.material.SetColor("_BaseColor", color);
    }
}