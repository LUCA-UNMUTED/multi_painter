public class TutorialState : StateMachine
{
    public override void EnterState(StateManager state)
    {
        state.statePhase = "tutorialState";
        state.tutorial.StartTutorial();
    }

    public override void ExitState(StateManager state)
    {
    }

    public override void UpdateState(StateManager state)
    {
        if (state.tutorial.tutorialIsFinished)
        {
            state.SwitchState(state.researchState);
        }
    }
}
