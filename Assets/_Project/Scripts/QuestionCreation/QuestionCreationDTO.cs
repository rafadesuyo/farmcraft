using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestionCreationDTO 
{
    [JsonProperty("questionText")] public string QuestionText;
    [JsonProperty("categoryID")] public string CategoryID;
    [JsonProperty("language")] public int Language;
    [JsonProperty("answerMap")] public Dictionary<string, string> AnswerMap;
    [JsonProperty("correctAnswerKey")] public int CorrectAnswerKey;
}
