using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject InstructorUI;
    [SerializeField] private GameObject PatientUI;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshPlayerUI()
    {
        var Players = GameObject.FindGameObjectsWithTag("_Player");
        foreach(GameObject player in Players)
        {
            var playerRole = player.GetComponent<PlayerData>().playerRole;
            bool active = player.GetComponent<PlayerData>().activePlayer;
            if (active)
            {
                switch(playerRole){
                    case PlayerRole.Instructor:
                        InstructorUI.SetActive(true);
                        PatientUI.SetActive(false);
                        break;
                    case PlayerRole.Patient:
                        InstructorUI.SetActive(false);
                        PatientUI.SetActive(true);
                        break;
                }
            }
        }
    }
}
