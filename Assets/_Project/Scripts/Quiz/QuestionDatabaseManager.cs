using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DreamQuiz
{
    public class QuestionDatabaseManager : PersistentSingleton<QuestionDatabaseManager>
    {
        [Serializable]
        public class QuestionDistributionInfo
        {
            public int Count = 10;
            public int MinCountThreshold = 5;
            public QuizCategory QuizCategory;
            public QuizDifficulty.Level QuizDifficulty;
        }

        [SerializeField] private QuestionDatabaseSO questionDatabase = null;
        [SerializeField] private QuestionDatabaseSO fallbackQuestionDatabase = null;
        [SerializeField] private List<QuestionDistributionInfo> questionDistributionInfos;
        private IQuestionFormatter questionFormatter = null;
        private IQuestionRepository questionRepository = null;
        private QuizLanguageType databaseLanguageType = QuizLanguageType.en;

        public bool IsFormatterReady { get; private set; }
        public bool IsRepositoryReady { get; private set; }

        private void Start()
        {
            databaseLanguageType = PlayerProgress.QuizLanguage;

            InitializeFormatter();
        }

        private void InitializeFormatter()
        {
            IsFormatterReady = false;
            questionFormatter = new QuestionFormatter();
            StartCoroutine(InitializeFormatterRoutine());
        }

        private IEnumerator InitializeFormatterRoutine()
        {
            yield return questionFormatter.FetchDataRelationship();

            var responseData = questionFormatter.GetFormatterResponseData();

            if (responseData.HasError == false)
            {
                IsFormatterReady = true;
                InitializeRepository();
            }
            else
            {
                Debug.LogError($"[QuestionDatabaseManager] Failed to get data relationship. Error: {responseData.ErrorMessage}");
            }
        }

        private void InitializeRepository()
        {
            IsRepositoryReady = false;
            questionRepository = new QuestionRepository(questionFormatter);
            questionDatabase.ResetQuestions();

            foreach (var questionDistributionInfo in questionDistributionInfos)
            {
                FetchQuestionsFromRepository(questionDistributionInfo.Count, questionDistributionInfo.QuizCategory, questionDistributionInfo.QuizDifficulty);
            }

            IsRepositoryReady = true;
        }

        public void ChangeDatabaseLanguage(QuizLanguageType quizLanguageType)
        {
            databaseLanguageType = quizLanguageType;
            InitializeRepository();
        }

        private void FetchQuestionsFromRepository(int questionCount, QuizCategory quizCategory, QuizDifficulty.Level quizDifficulty)
        {
            StartCoroutine(questionRepository.FetchQuestionsFromRepository(questionCount, quizCategory, quizDifficulty, databaseLanguageType, OnFetchQuestionSuccess, OnFetchQuestionError));
        }

        private void OnFetchQuestionSuccess(List<QuestionModel> questions)
        {
            questionDatabase.AddQuestions(questions);
        }

        private void OnFetchQuestionError(string error)
        {
            Debug.LogError($"[QuestionDatabaseManager] Failed to get questions. Error: {error}");
        }

        public QuestionModel GetQuestionFromDatabase(QuizCategory category, QuizDifficulty.Level difficulty)
        {
            QuestionModel question = questionDatabase.GetRandomQuestionByCategoryAndDifficulty(category, difficulty);

            if (question == null)
            {
                question = fallbackQuestionDatabase.GetRandomQuestionByCategoryAndDifficulty(category, difficulty);
            }

            CheckIfNeedToRepopulateDatabase(category, difficulty);

            return question;
        }

        public void CheckIfNeedToRepopulateDatabase(QuizCategory category, QuizDifficulty.Level difficulty)
        {
            int count = questionDatabase.GetQuestionCountByCategoryAndDifficulty(category, difficulty);

            var questionDistributionInfo = questionDistributionInfos.Find(q => q.QuizCategory == category && q.QuizDifficulty == difficulty);

            if (questionDistributionInfo != null && questionDistributionInfo.MinCountThreshold > count)
            {
                FetchQuestionsFromRepository(questionDistributionInfo.Count, category, difficulty);
            }
        }

        public string GetCategoryId(QuizCategory category)
        {
            if (!IsFormatterReady)
            {
                Debug.LogError("Formatter is not ready!");
                return "";
            }
            return questionFormatter.GetCategoryId(category);
        }

        public QuizCategory GetCategoryByID(string id)
        {
            if (!IsFormatterReady)
            {
                Debug.LogError("Formatter is not ready!");
                return QuizCategory.None;
            }

            QuizCategory category = QuizCategory.Random;

            if (questionFormatter.TryParseCategory(id, out category))
            {
                return category;
            }

            return QuizCategory.None;
        }
    }
}