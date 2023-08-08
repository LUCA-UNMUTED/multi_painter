public class ResearchState : StateMachine
{
    public override void EnterState(StateManager state)
    {
        state.statePhase = "researchState";
    }

    public override void ExitState(StateManager state)
    {
    }

    public override void UpdateState(StateManager state)
    {

    }
}
