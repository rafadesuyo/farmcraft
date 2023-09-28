public class EndStageState : StageState
{
    public override StateName GetStateName()
    {
        return StateName.End;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        var timeSystem = StageSystemLocator.GetSystem<TimeTrackingSystem>();
        timeSystem?.PauseCountingTime();

        stageManager.ExitStage();
    }
}
