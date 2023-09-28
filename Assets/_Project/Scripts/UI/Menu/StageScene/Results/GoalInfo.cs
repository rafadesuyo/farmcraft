using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalInfo : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Image goalIcon;
    [SerializeField] private TMP_Text goalText;
    [SerializeField] private TMP_Text goalValue;

    //Variables
    [Header("Default Variables")]
    [SerializeField] private string textToReplaceWithValue = "{value}";

    private StageGoal.StageGoalRequisite requisite;

    //Getters
    public StageGoal.StageGoalRequisite Requisite => requisite;

    public void SetGoalRequisite(StageGoal.StageGoalRequisite requisite)
    {
        this.requisite = requisite;
    }

    public void SetGoalIcon(Sprite image)
    {
        goalIcon.sprite = image;
    }

    public void SetGoalText(string name)
    {
        goalText.text = name;
    }

    public void SetGoalValue(string value)
    {
        goalValue.text = value;
    }

    public void ReplaceTextWithValue(string value)
    {
        goalText.text = goalText.text.Replace(textToReplaceWithValue, value);
    }
}
