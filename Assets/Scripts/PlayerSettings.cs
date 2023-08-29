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
public enum Hand : byte
{
    Left = 0, Right = 1
}
public class PlayerSettings : NetworkBehaviour
{
    [SerializeField] private MeshRenderer bodyMeshRenderer;
    [SerializeField] private List<MeshRenderer> eyesMeshRenderer;
    [SerializeField] private TextMeshProUGUI playerName;
    private NetworkVariable<FixedString128Bytes> networkPlayerName = new("Player: 0", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public Color PlayerColor;
    public List<Color> colors = new();

    public NetworkVariable<bool> isAllowedToDraw = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<PlayerRole> isHostPlayer = new(PlayerRole.Patient, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<Hand> activeHand = new(Hand.Right, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public bool playerSettingsSet = false;

    public static Dictionary<ulong, PlayerSettings> Players = new Dictionary<ulong, PlayerSettings>();//todo is this necessary

    public GameObject LeftHand;
    public GameObject RightHand;

    //public Hand activeHand;

    [SerializeField] private bool testSwitchHands = false;

    public override void OnNetworkSpawn()
    {

        PlayerColor = colors[(int)OwnerClientId % colors.Count];
        Players[OwnerClientId] = this;
        bodyMeshRenderer.material.color = PlayerColor;
        networkPlayerName.Value = "Player:" + (OwnerClientId + 1);
        playerName.text = networkPlayerName.Value.ToString();
        gameObject.name = networkPlayerName.Value.ToString(); // in Unity editor more clearly

        if (IsServer || IsHost) // TODO rewrite with serverRPC
        {
            // the server can distribute the roles, we link the server to the instructor, as (s)he owns the instructor player
            if (IsOwner)
            {
                isAllowedToDraw.Value = true;
                isHostPlayer.Value = PlayerRole.Instructor;
                //activeHand.Value = Hand.Left; // startersSettings

            }
            else
            {
                isAllowedToDraw.Value = false;
                isHostPlayer.Value = PlayerRole.Patient;
                //activeHand.Value = Hand.Left; // startersSettings
            }
            playerSettingsSet = true;
        }
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (testSwitchHands)
        {
            testSwitchHands = false;
            if (activeHand.Value == Hand.Left)
            {
                activeHand.Value = Hand.Right;
            }
            if (activeHand.Value == Hand.Right)
            {
                activeHand.Value = Hand.Left;
            }
        }
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
        foreach (MeshRenderer eye in eyesMeshRenderer)
        {
            eye.material.SetColor("_BaseColor", color);
        }
    }
    [ServerRpc] 
    private void SwitchHandsServerRpc()
    {
        if(activeHand.Value == Hand.Left)
        {
            activeHand.Value = Hand.Right;
        }
        if(activeHand.Value == Hand.Right)
        {
            activeHand.Value = Hand.Left;
        }
    }
}
