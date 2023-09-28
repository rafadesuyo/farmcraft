using DreamQuiz;

public class TutorialSystem : BaseStageSystem
{
    private void OnEnable()
    {
        StageManager.Instance.OnStateEnter += Instance_OnStateEnter;
    }

    private void OnDisable()
    {
        StageManager.Instance.OnStateEnter -= Instance_OnStateEnter;
    }

    private void Instance_OnStateEnter(StageState.StateName state)
    {
        if (state == StageState.StateName.Tutorial)
        {
            BeginTutorial();
        }
    }

    public override void Initialize()
    {
        //initialize the timeline and UI elements

        IsReady = true;
    }

    private void BeginTutorial()
    {
        //start playing timeline
    }

    public void EndTutorial()
    {
        StageManager.Instance.SetStageState(StageState.StateName.Turn);
    }

    protected override void RegisterSystem()
    {
        StageSystemLocator.RegisterSystem(this);
    }

    protected override void UnregisterSystem()
    {
        StageSystemLocator.UnregisterSystem<TutorialSystem>();
    }
}
