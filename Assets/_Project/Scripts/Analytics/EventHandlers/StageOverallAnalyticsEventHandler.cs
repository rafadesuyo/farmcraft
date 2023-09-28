using DreamQuiz.Player;
using UnityEngine;

namespace DreamQuiz
{
    public class StageOverallAnalyticsEventHandler : MonoBehaviour
    {
        private PlayerStageInstance playerStageInstance;

        private void Awake()
        {
            playerStageInstance = GetComponentInParent<PlayerStageInstance>();
        }

        private void OnEnable()
        {
            StageManager.Instance.OnStageExit += Instance_OnStageExit;
        }

        private void OnDisable()
        {
            StageManager.Instance.OnStageExit -= Instance_OnStageExit;
        }

        private void Instance_OnStageExit()
        {
            SendAnalytics();
        }

        private void SendAnalytics()
        {
            StageOverallAnalyticsModel stageOverallAnalyticsModel = new StageOverallAnalyticsModel(playerStageInstance.PlayerStageGoal, playerStageInstance.PlayerStageData);
            AnalyticsManager.Instance.SendStageOverallAnalytics(stageOverallAnalyticsModel);
        }
    }
}