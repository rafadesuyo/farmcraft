using DreamQuiz.Player;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndNodeCalculationItem : RewardCalculationItem
{
    [SerializeField] private Image resultIconImg = null;
    [SerializeField] private Sprite winIcon = null;
    [SerializeField] private TextMeshProUGUI nodeCompletedText = null;

    public void Setup()
    {
        if (PlayerManager.CurrentPlayerInstance.PlayerStageGoal.State==PlayerStageGoal.PlayerStageGoalState.Win)
        {
            resultIconImg.enabled = true;
            resultIconImg.sprite = winIcon;
            nodeCompletedText.gameObject.SetActive(false);
        }
        else
        {
            resultIconImg.enabled = false;
            nodeCompletedText.gameObject.SetActive(true);
        }
    }
}
