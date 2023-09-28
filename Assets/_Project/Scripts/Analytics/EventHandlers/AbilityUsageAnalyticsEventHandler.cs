using DreamQuiz.Player;
using System;
using UnityEngine;

namespace DreamQuiz
{
    public class AbilityUsageAnalyticsEventHandler : MonoBehaviour
    {
        private PlayerStageInstance playerStageInstance;
        private QuizSystem quizSystem;

        private void Awake()
        {
            playerStageInstance = GetComponentInParent<PlayerStageInstance>();
            playerStageInstance.OnInitialized += PlayerStageInstance_OnInitialized;
        }

        private void PlayerStageInstance_OnInitialized()
        {
            playerStageInstance.OnInitialized -= PlayerStageInstance_OnInitialized;
            playerStageInstance.PlayerStageAbility.OnAbilityUse += PlayerStageAbility_OnAbilityUse;
        }

        private void OnEnable()
        {
            if (playerStageInstance.PlayerStageAbility != null)
            {
                playerStageInstance.PlayerStageAbility.OnAbilityUse += PlayerStageAbility_OnAbilityUse;
            }
        }

        private void OnDisable()
        {
            if (playerStageInstance.PlayerStageAbility != null)
            {
                playerStageInstance.PlayerStageAbility.OnAbilityUse -= PlayerStageAbility_OnAbilityUse;
            }
        }

        private void PlayerStageAbility_OnAbilityUse(BaseAbilityBehaviour abilityBehaviour)
        {
            SendAnalytics(abilityBehaviour);
        }

        private void SendAnalytics(BaseAbilityBehaviour abilityBehaviour)
        {
            //TODO: This is breaking on stage creator. Fix when implementing endpoint;
            //https://ocarinastudios.atlassian.net/browse/DQG-2089

            //Guid questionId = Guid.Empty;

            //if (quizSystem == null)
            //{
            //    quizSystem = StageSystemLocator.GetSystem<QuizSystem>();
            //}

            //if (quizSystem != null)
            //{
            //    questionId = quizSystem.CurrentQuizQuestion.QuestionId;
            //}

            //AbilityUsageAnalyticsEventModel abilityUsageAnalyticsModel = new AbilityUsageAnalyticsEventModel(playerStageInstance.stageId, questionId, abilityBehaviour);
            //AnalyticsManager.Instance.SendAbilityUsageAnalytics(abilityUsageAnalyticsModel);
        }
    }
}