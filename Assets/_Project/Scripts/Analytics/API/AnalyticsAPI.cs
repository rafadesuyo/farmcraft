using System;
using System.Collections;
using UnityEngine;

namespace DreamQuiz
{
    public class AnalyticsAPI : IAnalyticsAPI
    {
        //TODO: Implement to use the server endpoints
        //https://ocarinastudios.atlassian.net/browse/DQG-2089

        public IEnumerator SendAbilityUsageAnalytics(AbilityUsageAnalyticsDto abilityUsageAnalyticsDto, Action onCompleteCallback = null)
        {
            yield return null;
            Debug.Log($"Ability usage analytics sent");
            onCompleteCallback?.Invoke();
        }

        public IEnumerator SendAnswerAnalytics(AnswerAnalyticsDto answerAnalyticsDto, Action onCompleteCallback = null)
        {
            yield return null;
            Debug.Log($"Answer analytics sent");
            onCompleteCallback?.Invoke();
        }

        public IEnumerator SendSessionAnalytics(SessionAnalyticsDto sessionAnalyticsDto, Action onCompleteCallback = null)
        {
            yield return null;
            Debug.Log($"Session analytics sent");
            onCompleteCallback?.Invoke();
        }

        public IEnumerator SendStageOverallAnalytics(StageOverallAnalyticsDto stageOverallAnalyticsDto, Action onCompleteCallback = null)
        {
            yield return null;
            Debug.Log($"Stage Overall analytics sent");
            onCompleteCallback?.Invoke();
        }
    }
}