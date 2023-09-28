using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestionListDto
{
    public List<QuestionDto> questionsList = new List<QuestionDto>();
}

[Serializable]
public class QuestionDto 
{
    [JsonProperty("questionId")] public string QuestionId { get; private set; }
    [JsonProperty("categoryID")] public string CategoryId { get; private set; }
    [JsonProperty("subCategoryID")] public string SubCategoryID { get; private set; }
    [JsonProperty("tags")] public string[] Tags { get; private set; }
    [JsonProperty("difficulty")] public string DifficultyName { get; private set; }
    [JsonProperty("questionText")] public string Title { get; private set; }
    [JsonProperty("answerMap")] public Dictionary<string, string> AnswerMap { get; private set; }
    [JsonProperty("correctAnswerKey")] public string CorrectIndex { get; private set; }
    [JsonProperty("hintText")] public string Hint { get; private set; }
    [JsonProperty("questionSource")] public string Source { get; private set; }
}
