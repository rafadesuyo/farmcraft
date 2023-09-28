using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class StageHeaderItem : MonoBehaviour
{
    //Components
    [Header("Base Components")]
    [SerializeField] protected Image iconImg = null;
    [SerializeField] protected TextMeshProUGUI progressText = null;

    //Variables
    [Header("Default Variables")]
    [SerializeField] protected Color normalTextColor;
    [SerializeField] protected Color completedTextColor;
    [SerializeField] protected Color incompletedTextColor;

    protected void SetupIcon(StageGoal.StageGoalRequisite requisite)
    {
        StageGoalDataSO.StageGoalDataPair stageGoalInfo = ProjectAssetsDatabase.Instance.GetStageGoalDataByRequisite(requisite);

        iconImg.sprite = stageGoalInfo.GoalIcon;
    }
}
