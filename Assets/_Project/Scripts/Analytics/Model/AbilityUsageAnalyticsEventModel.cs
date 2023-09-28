using System;

namespace DreamQuiz
{
    public class AbilityUsageAnalyticsEventModel
    {
        public string UserID { get; private set; }
        public short Stage { get; private set; }
        public int AbilityName { get; private set; }
        public Guid QuestionID { get; private set; }

        public AbilityUsageAnalyticsEventModel(int stageId, Guid questionId, BaseAbilityBehaviour usedAbility)
        {
            UserID = LoginManager.Instance.UserModel.UserId;
            Stage = Convert.ToInt16(stageId);
            AbilityName = Convert.ToInt32(usedAbility.GetAbilityId());
            QuestionID = questionId;
        }

        public AbilityUsageAnalyticsDto ToDTO()
        {
            return AnalyticsHelper.Map<AbilityUsageAnalyticsEventModel, AbilityUsageAnalyticsDto>(this);
        }
    }
}