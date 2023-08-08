
using UnityEngine;

public abstract class StateMachine
{
    public abstract void EnterState(StateManager state);
    public abstract void UpdateState(StateManager state);
    public abstract void ExitState(StateManager state);
}