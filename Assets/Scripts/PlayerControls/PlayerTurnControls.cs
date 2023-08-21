using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerTurnControls : MonoBehaviour
{
    public InputActionReference switchTurn = null;
    //public GameObject Instructor;
    //public GameObject Patient;
    public GameObject playerCapabilitiesObject;
    public PlayerData playerData;
    public PlayerCapabilities playerCapabilities;

    [SerializeField] private bool testSwitch = false;
    private void Awake()
    {
        switchTurn.action.started += SwitchTurn;
    }

    private void OnDestroy()
    {

        switchTurn.action.started -= SwitchTurn;
    }

    private void Start()
    {

        playerData = this.GetComponent<PlayerData>();
        Invoke("Find", 2f);

    }

    void Find()
    {
        playerCapabilities = GameObject.FindWithTag("playerCapabilities").GetComponent<PlayerCapabilities>();
        playerCapabilities.ClassifyPlayers();


    }

    public void SwitchTurn(InputAction.CallbackContext context)
    {
        if (playerData.activePlayer == true) // || PlayerData.forceSwitch == true)
        {
            Debug.Log("switching via " + playerData.playerRole);

            playerCapabilities.SwitchPlayer();
        }
    }

    public void SwitchTurn()
    {
        if (playerData.activePlayer == true) // || PlayerData.forceSwitch == true)
        {
            Debug.Log("switching via " + playerData.playerRole);

            playerCapabilities.SwitchPlayer();
        }
    }
    //TODO make copy of switchturn, but based on physical buttons: one for regular (can switch, visible on both sides - force switch, visible only for instructor)


    private void Update()
    {
        if (testSwitch)
        {
            testSwitch = false;
            SwitchTurn();
        }
    }
}
