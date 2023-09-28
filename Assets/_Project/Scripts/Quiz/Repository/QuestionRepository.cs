using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace DreamQuiz
{
    public class QuestionRepository : IQuestionRepository
    {
        private const string baseUrl = "https://dreamquizapi.ocarinastudio.com/api/questions/quiz/random/category";
        private const int connectionTimeout = 30;

        private IQuestionFormatter questionFormatter = null;

        public QuestionRepository(IQuestionFormatter questionFormatter)
        {
            this.questionFormatter = questionFormatter;
        }

        public IEnumerator FetchQuestionsFromRepository(int questionCount, QuizCategory category, QuizDifficulty.Level difficulty, QuizLanguageType language, Action<List<QuestionModel>> onSuccess, Action<string> onError)
        {
            if (category == QuizCategory.Math || category == QuizCategory.Puzzles)
            {
                GetQuestionsFromGenerator(questionCount, category, difficulty, language, onSuccess, onError);
            }
            else
            {
                yield return GetQuestionsFromServer(questionCount, category, difficulty, language, onSuccess, onError);
            }
        }

        private void GetQuestionsFromGenerator(int questionCount, QuizCategory category, QuizDifficulty.Level difficulty, QuizLanguageType language, Action<List<QuestionModel>> onSuccess, Action<string> onError)
        {
            List<QuestionModel> questions = new List<QuestionModel>();

            for (int i = 0; i < questionCount; i++)
            {
                questions.Add(MathQuestionManager.Instance.GetRandomQuestionByDifficulty(difficulty));
            }

            onSuccess(questions);
        }

        private IEnumerator GetQuestionsFromServer(int questionCount, QuizCategory category, QuizDifficulty.Level difficulty, QuizLanguageType language, Action<List<QuestionModel>> onSuccess, Action<string> onError)
        {
            if (questionFormatter == null)
            {
                Debug.LogError("[QuestionRepository] Repository doesn't have a IQuestionFormatter");
            }

            UnityWebRequest request = CreateWebRequest(questionCount, category, difficulty, language);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var deserializedData = DeserializeDTO(request.downloadHandler.text);
                var questions = questionFormatter.ParseQuestionList(deserializedData);
                onSuccess?.Invoke(questions);
            }
            else
            {
                onError?.Invoke(request.error);
            }
        }

        private UnityWebRequest CreateWebRequest(int questionCount, QuizCategory category, QuizDifficulty.Level difficulty, QuizLanguageType language)
        {
            string categoryId = questionFormatter.GetCategoryId(category);
            string difficultyId = questionFormatter.GetDifficultyId(difficulty);
            string url = $"{baseUrl}/{categoryId}?sampleSize={questionCount}&difficulty={difficultyId}&language={language}";

            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = connectionTimeout;

            return request;
        }

        private QuestionListDto DeserializeDTO(string data)
        {
            return JsonConvert.DeserializeObject<QuestionListDto>(data);
        }
    }
}