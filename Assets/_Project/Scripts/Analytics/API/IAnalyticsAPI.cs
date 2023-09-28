using System;
using System.Collections;

namespace DreamQuiz
{
    public interface IAnalyticsAPI
    {
        IEnumerator SendStageOverallAnalytics(StageOverallAnalyticsDto stageOverallAnalyticsDto, Action onCompleteCallback = null);
        IEnumerator SendAbilityUsageAnalytics(AbilityUsageAnalyticsDto abilityUsageAnalyticsDto, Action onCompleteCallback = null);
        IEnumerator SendSessionAnalytics(SessionAnalyticsDto sessionAnalyticsDto, Action onCompleteCallback = null);
        IEnumerator SendAnswerAnalytics(AnswerAnalyticsDto answerAnalyticsDto, Action onCompleteCallback = null);
    }
}