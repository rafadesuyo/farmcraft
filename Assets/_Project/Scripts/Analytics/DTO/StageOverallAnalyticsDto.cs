using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DreamQuiz
{
    [Serializable]
    public class StageOverallAnalyticsDto
    {
        [JsonProperty("userId")] public Guid UserID { get; set; }
        [JsonProperty("result")] public char Result { get; private set; }
        [JsonProperty("team")] public List<int> Team { get; private set; }
        [JsonProperty("stageDuration")] public int StageDuration { get; private set; }
        [JsonProperty("goldEarned")] public int GoldEarned { get; private set; }
        [JsonProperty("numberOfMovements")] public int NumberOfMovements { get; private set; }
        [JsonProperty("numberOfSkillsUsed")] public int NumberOfSkillsUsed { get; private set; }
    }
}