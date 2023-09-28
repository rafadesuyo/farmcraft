using Newtonsoft.Json;
using System;

namespace DreamQuiz
{
    [Serializable]
    public class AnswerAnalyticsDto
    {
        [JsonProperty("userId")] public Guid UserID { get; set; }
        [JsonProperty("questionID")] public Guid QuestionID { get; set; }
        [JsonProperty("result")] public short Result { get; set; }
        [JsonProperty("duration")] public int Duration { get; set; }
    }
}