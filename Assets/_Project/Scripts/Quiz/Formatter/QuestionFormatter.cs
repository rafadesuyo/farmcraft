using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace DreamQuiz
{
    [Serializable]
    public class QuestionDataRelationship
    {
        public Dictionary<string, string> categoryNames = null;
        public Dictionary<string, string> subCategoryNames = null;
        public Dictionary<string, string> tagNames = null;
        public Dictionary<string, string[]> categoriesMap = null;
        public Dictionary<string, int> difficultyMap = null;
        public Dictionary<string, int> questionSourcesMap = null;
    }

    public class QuestionFormatter : IQuestionFormatter
    {
        private const string baseUrl = "https://dreamquizapi.ocarinastudio.com/api/questions/info/relationship"; 
        private const int connectionTimeout = 30;
        private bool isDebug = true;

        private readonly Dictionary<string, QuizCategory> categoryMap = new Dictionary<string, QuizCategory>()
        {
            { "general knowledge", QuizCategory.GeneralKnowledge },
            { "sports", QuizCategory.Sports },
            { "science", QuizCategory.Science },
            { "puzzles", QuizCategory.Puzzles },
            { "arts and entertainment", QuizCategory.ArtsAndEntertainment },
            { "human sciences", QuizCategory.HumanSciences }
        };

        private readonly Dictionary<int, QuizDifficulty.Level> difficultyMap = new Dictionary<int, QuizDifficulty.Level>()
        {
            { 0, QuizDifficulty.Level.Easy },
            { 1, QuizDifficulty.Level.Medium },
            { 2, QuizDifficulty.Level.Hard },
            { 3, QuizDifficulty.Level.Easy }
        };

        private QuestionDataRelationship questionDataRelationship;
        private FormatterResponseData formatterResponseData;

        public QuestionFormatter()
        {
            formatterResponseData = new FormatterResponseData();
        }

        public FormatterResponseData GetFormatterResponseData()
        {
            return formatterResponseData;
        }

        public IEnumerator FetchDataRelationship()
        {
            formatterResponseData = new FormatterResponseData();

            UnityWebRequest request = UnityWebRequest.Get(baseUrl);
            request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = connectionTimeout;

            yield return request.SendWebRequest();

            formatterResponseData.IsDone = true;

            if (request.result == UnityWebRequest.Result.Success)
            {
                questionDataRelationship = JsonConvert.DeserializeObject<QuestionDataRelationship>(request.downloadHandler.text);
            }
            else
            {
                formatterResponseData.HasError = true;
                formatterResponseData.ErrorMessage = request.error;
            }
        }

        public bool TryParseQuestion(QuestionDto questionDto, out QuestionModel questionModel)
        {
            questionModel = null;
            Guid questionId = new Guid(questionDto.QuestionId);

            if (TryParseCategory(questionDto.CategoryId, out QuizCategory quizCategory) == false)
            {
                if (isDebug)
                {
                    Debug.LogWarning($"[QuestionFormatter] Failed to parse category {questionDto.CategoryId} for question {questionDto.QuestionId}");
                }

                return false;
            }

            if (TryParseDifficulty(questionDto.DifficultyName, out QuizDifficulty.Level quizDifficulty) == false)
            {
                if (isDebug)
                {
                    Debug.LogWarning($"[QuestionFormatter] Failed to parse difficulty {questionDto.DifficultyName} for question {questionDto.QuestionId}");
                }

                return false;
            }

            int correctIndex = Convert.ToInt32(questionDto.CorrectIndex);
            List<string> answers = new List<string>(questionDto.AnswerMap.Values);

            questionModel = new QuestionModel(questionId,
                questionDto.Title,
                answers,
                quizCategory,
                quizDifficulty,
                correctIndex,
                questionDto.Hint);

            return true;
        }

        public List<QuestionModel> ParseQuestionList(QuestionListDto questionListDto)
        {
            int parsedCount = 0;
            List<QuestionModel> parsedQuestions = new List<QuestionModel>();

            foreach (var questionDto in questionListDto.questionsList)
            {
                if (TryParseQuestion(questionDto, out QuestionModel questionModel))
                {
                    parsedQuestions.Add(questionModel);
                    parsedCount++;
                }
            }

            if (isDebug)
            {
                Debug.Log($"[QuestionFormatter] Parsed {parsedCount} questions from {questionListDto.questionsList.Count}");
            }

            return parsedQuestions;
        }

        public bool TryParseCategory(string categoryId, out QuizCategory quizCategory)
        {
            quizCategory = QuizCategory.Random;

            if (string.IsNullOrEmpty(categoryId))
            {
                return false;
            }

            string categoryName = questionDataRelationship.categoryNames.FirstOrDefault(c => c.Value == categoryId).Key;

            if (string.IsNullOrEmpty(categoryName))
            {
                return false;
            }

            if (categoryMap.TryGetValue(categoryName, out quizCategory))
            {
                return true;
            }

            return false;
        }

        public bool TryParseDifficulty(string difficultyName, out QuizDifficulty.Level quizzDificulty)
        {
            int index = Convert.ToInt32(difficultyName);

            if (difficultyMap.TryGetValue(index, out quizzDificulty))
            {
                return true;
            }

            return false;
        }

        public string GetCategoryId(QuizCategory quizCategory)
        {
            string categoryName = categoryMap.FirstOrDefault(c => c.Value == quizCategory).Key;
            return questionDataRelationship.categoryNames[categoryName];
        }

        public string GetDifficultyId(QuizDifficulty.Level quizDifficulty)
        {
            int index = difficultyMap.FirstOrDefault(c => c.Value == quizDifficulty).Key;
            return index.ToString();
        }
    }
}