using UnityEngine;

public class StageTimeUI : StageHeaderItem
{
    //Variables
    private TimeTrackingSystem timeTrackingSystem;

    private StageGoalProgress stageGoalProgress;
    private int elapsedTime;

    private int maxTime
    {
        get
        {
            if (stageGoalProgress != null && stageGoalProgress.StageGoal != null)
            {
                return stageGoalProgress.StageGoal.TargetValue;
            }

            return 0;
        }
    }

    private Color progressColor
    {
        get
        {
            if (stageGoalProgress != null
                && !stageGoalProgress.IsComplete
                && elapsedTime > maxTime)
            {
                return incompletedTextColor;
            }

            return normalTextColor;
        }
    }

    private Color maxProgressColor
    {
        get
        {
            return completedTextColor;
        }
    }

    private void Start()
    {
        timeTrackingSystem = StageSystemLocator.GetSystem<TimeTrackingSystem>();
        timeTrackingSystem.OnTimeCount += TimeTrackingSystem_OnTimeCount;
        elapsedTime = timeTrackingSystem.ElapsedTime;
    }

    public void Setup(StageGoalProgress stageGoalProgress = null)
    {
        this.stageGoalProgress = stageGoalProgress;

        elapsedTime = 0;

        UpdateTime();
    }

    private void TimeTrackingSystem_OnTimeCount(int timeCount)
    {
        elapsedTime = timeCount;
        UpdateTime();
    }

    public void UpdateTime()
    {
        string formatedCurrentTime = TextFormatter.TimeToMMSS(elapsedTime);
        string goalText = TextFormatter.InlineColor(formatedCurrentTime, progressColor);

        if (maxTime > 0)
        {
            string formatedMaxTime = TextFormatter.TimeToMMSS(maxTime);
            goalText += TextFormatter.InlineColor($"/{formatedMaxTime}", maxProgressColor);
        }

        progressText.text = goalText;
    }
}