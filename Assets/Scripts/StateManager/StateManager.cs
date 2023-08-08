using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public StateMachine currentState;
    public TutorialState tutorialState = new();
    public ResearchState researchState = new();

    [Header("GameObjects")]
    public Tutorial tutorial;

    [Header("Debugging")]
    // trialPhase is for debugging purposes
    public string statePhase = "";

    // Start is called before the first frame update
    void Start()
    {
        // starting state for our state machine
        currentState = tutorialState;
        statePhase = "";
        // "this" is a reference to the context (this EXACT Monobehavior script)
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(StateMachine newState)
    {
        Debug.Log("moving to " + newState);
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
}
