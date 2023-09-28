using System.Collections;

public class StartStageState : StageState
{
    public override StateName GetStateName()
    {
        return StateName.Start;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stageManager.StartCoroutine(WaitForStart());
    }

    private IEnumerator WaitForStart()
    {
        yield return null;

        var tutorialSystem = StageSystemLocator.GetSystem<TutorialSystem>();

        if (tutorialSystem != null)
        {
            stageManager.SetStageState(StateName.Tutorial);
        }

        stageManager.SetStageState(StateName.Turn);
    }

    public override void OnLeave()
    {
        base.OnLeave();

        var timeSystem = StageSystemLocator.GetSystem<TimeTrackingSystem>();
        timeSystem?.StartCountingTime();
    }
}
