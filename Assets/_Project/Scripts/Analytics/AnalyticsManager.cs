using System;
using UnityEngine;

namespace DreamQuiz
{
    public class AnalyticsManager : PersistentSingleton<AnalyticsManager>
    {
        private static IAnalyticsAPI analyticsApi = new AnalyticsAPI();

        private void Start()
        {
            Application.wantsToQuit += Application_wantsToQuit;
        }

        private void OnDestroy()
        {
            Application.wantsToQuit -= Application_wantsToQuit;
        }

        private bool Application_wantsToQuit()
        {
            Application.wantsToQuit -= Application_wantsToQuit;
            SendSessionAnalytics(
                new SessionAnalyticsModel(0),
                () =>
                {
                    Application.Quit();
                });

            return false;
        }

        public void SendStageOverallAnalytics(StageOverallAnalyticsModel stageOverallAnalyticsModel, Action onCompleteCallback = null)
        {
            StartCoroutine(analyticsApi.SendStageOverallAnalytics(stageOverallAnalyticsModel.ToDTO(), onCompleteCallback));
        }

        public void SendAbilityUsageAnalytics(AbilityUsageAnalyticsEventModel abilityUsageAnalyticsModel, Action onCompleteCallback = null)
        {
            StartCoroutine(analyticsApi.SendAbilityUsageAnalytics(abilityUsageAnalyticsModel.ToDTO(), onCompleteCallback));
        }

        public void SendSessionAnalytics(SessionAnalyticsModel sessionAnalyticsModel, Action onCompleteCallback = null)
        {
            StartCoroutine(analyticsApi.SendSessionAnalytics(sessionAnalyticsModel.ToDTO(), onCompleteCallback));
        }

        public void SendAnswerAnalytics(AnswerAnalyticsModel answerAnalyticsModel, Action onCompleteCallback = null)
        {
            StartCoroutine(analyticsApi.SendAnswerAnalytics(answerAnalyticsModel.ToDTO(), onCompleteCallback));
        }
    }
}