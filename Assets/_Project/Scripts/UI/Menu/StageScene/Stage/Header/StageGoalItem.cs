using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DreamQuiz.Player;

public class StageGoalItem : StageHeaderItem
{
    [Header("Components")]
    [SerializeField] private RectTransform completionImg = null;

    private StageGoalProgress stageGoalProgress;

    public void Setup(StageGoalProgress stageGoalProgress)
    {
        this.stageGoalProgress = stageGoalProgress;

        SetupIcon(stageGoalProgress.StageGoal.Requisite);

        UpdateGoal();

        stageGoalProgress.OnProgress += StageGoalProgress_OnProgress;
    }

    public void UpdateGoal()
    {
        int progress = stageGoalProgress.ProgressValue;
        int progressMax = stageGoalProgress.StageGoal.TargetValue;
        bool goalCompleted = stageGoalProgress.IsComplete;

        string progressColor;
        string progressMaxColor;

        string goalText;

        if (goalCompleted == false)
        {
            progressColor = ColorUtility.ToHtmlStringRGB(incompletedTextColor);
            progressMaxColor = ColorUtility.ToHtmlStringRGB(normalTextColor);
        }
        else
        {
            progressColor = ColorUtility.ToHtmlStringRGB(completedTextColor);
            progressMaxColor = ColorUtility.ToHtmlStringRGB(completedTextColor);
        }

        goalText = $"<color=#{progressColor}>{progress}</color><color=#{progressMaxColor}>/{progressMax}</color>";

        progressText.text = goalText;
        completionImg.gameObject.SetActive(goalCompleted);
    }

    private void StageGoalProgress_OnProgress(int value)
    {
        UpdateGoal();
    }

    private void OnDisable()
    {
        stageGoalProgress.OnProgress -= StageGoalProgress_OnProgress;
        this.ReleaseItem();
    }
}
