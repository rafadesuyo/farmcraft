using System.Collections.Generic;
using UnityEngine;

public interface IGameEvent { }

public class OnCompleteStageEvent : IGameEvent
{
    public int id;

    public OnCompleteStageEvent(int newId)
    {
        id = newId;
    }
}

public class OnLevelUpCollectibleEvent : IGameEvent
{
    public int level;

    public OnLevelUpCollectibleEvent(int newLevel)
    {
        level = newLevel;
    }
}

public class OnCutAnswerEvent : IGameEvent
{
    public QuizCategory category;
    public int count;
    public int dreamEnergyCost;

    public OnCutAnswerEvent(QuizCategory newCategory, int newCount, int cost)
    {
        category = newCategory;
        count = newCount;
        dreamEnergyCost = cost;
    }
}

public class OnQuestionIsAnsweredEvent : IGameEvent
{
    public string answerText;
    public bool isAnswerCorrect;
    public QuizCategory category;

    public OnQuestionIsAnsweredEvent(string newAnswerText, bool newAnswerResult, QuizCategory newCategory)
    {
        answerText = newAnswerText;
        isAnswerCorrect = newAnswerResult;
        category = newCategory;
    }
}

public class OnSelectCollectibleAbilityPreviewEvent : IGameEvent
{
    public List<CollectibleAbility> abilitiesToShow;

    public OnSelectCollectibleAbilityPreviewEvent(List<CollectibleAbility> abilitiesToShow)
    {
        this.abilitiesToShow = abilitiesToShow;
    }
}

public class OnUpdateStageStatusEvent : IGameEvent
{
    public StageStatus type;
    public string value = string.Empty;

    public OnUpdateStageStatusEvent(StageStatus newType, int newValue)
    {
        type = newType;

        if (newValue > 0)
        {
            value = $"{newValue}";
        }
    }

    public OnUpdateStageStatusEvent(StageStatus newType, string newValue)
    {
        type = newType;
        value = newValue;
    }
}

public class OnUpdateStageGoalEvent : IGameEvent
{
    private StageGoal.StageGoalRequisite requisite;

    public StageGoal.StageGoalRequisite Requisite => requisite;

    public OnUpdateStageGoalEvent(StageGoal.StageGoalRequisite requisite)
    {
        this.requisite = requisite;
    }
}

public class OnUseAbilityEvent : IGameEvent
{
    public int value;

    public OnUseAbilityEvent(int newValue)
    {
        value = newValue;
    }
}

public class OnMissionStateChangedEvent : IGameEvent
{
    public DailyMission dailyMission;

    public OnMissionStateChangedEvent(DailyMission dailyMission)
    {
        this.dailyMission = dailyMission;
    }
}