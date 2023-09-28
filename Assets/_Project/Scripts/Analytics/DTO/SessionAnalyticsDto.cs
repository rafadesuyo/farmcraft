using Newtonsoft.Json;
using System;

namespace DreamQuiz
{
    [Serializable]
    public class SessionAnalyticsDto
    {
        [JsonProperty("userId")] public Guid UserID { get; set; }
        [JsonProperty("sessionType")] public short SessionType { get; set; }
        [JsonProperty("sessionDuration")] public int SessionDuration { get; set; }
    }
}