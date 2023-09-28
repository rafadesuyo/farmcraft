using Newtonsoft.Json;
using System;

namespace DreamQuiz
{
    [Serializable]
    public class AbilityUsageAnalyticsDto
    {
        [JsonProperty("userId")] public Guid UserID { get; set; }
        [JsonProperty("stage")] public short Stage { get; set; }
        [JsonProperty("abilityName")] public int AbilityName { get; set; }
        [JsonProperty("questionID")] public Guid QuestionID { get; set; }
    }
}