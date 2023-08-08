using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnClock : MonoBehaviour
{

    public float timeEnd = 50;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timeEnd < 50){


            timeEnd += Time.deltaTime;
        }
    }
}
