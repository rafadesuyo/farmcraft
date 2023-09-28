using DreamQuiz.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DreamQuiz
{
    public class StageOverallAnalyticsModel
    {
        public string UserID { get; private set; }
        public char Result { get; private set; }
        public List<int> Team { get; private set; }
        public int StageDuration { get; private set; }
        public int GoldEarned { get; private set; }
        public int NumberOfMovements { get; private set; }
        public int NumberOfSkillsUsed { get; private set; }

        public StageOverallAnalyticsModel(PlayerStageGoal playerStageGoal, PlayerStageData playerStageData)
        {
            UserID = LoginManager.Instance.UserModel.UserId;
            Result = AnalyticsHelper.ParseResult(playerStageGoal.State);
            Team = AnalyticsHelper.ParseTeam(playerStageData.Team);
            StageDuration = Mathf.RoundToInt(Time.timeSinceLevelLoad);
            GoldEarned = playerStageData.GoldCount;
            NumberOfMovements = playerStageData.NumberOfNodeMovements;
            NumberOfSkillsUsed = playerStageData.UsedAbilities.Count;
        }

        public StageOverallAnalyticsDto ToDTO()
        {
            return AnalyticsHelper.Map<StageOverallAnalyticsModel, StageOverallAnalyticsDto>(this);
        }
    }
}