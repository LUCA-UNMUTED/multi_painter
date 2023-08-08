using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerRole { Instructor, Patient };

public class PlayerData : MonoBehaviour
{
    public bool canSwitch;

    public bool forceSwitch = false;

    public bool activePlayer;

    public PlayerRole playerRole = PlayerRole.Instructor;
}
