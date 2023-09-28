using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace DreamQuiz.Tests
{
    public struct QuestionRepositoryTestValues
    {
        public int Count;
        public QuizCategory QuizCategory;
        public QuizDifficulty.Level QuizDifficulty;
        public QuizLanguageType QuizLanguageType;
    }

    public class QuestionRepositoryTest
    {
        private const int questionCountValue = 5;
        private static QuestionRepositoryTestValues[] questionRepositoryTestValues = new QuestionRepositoryTestValues[]
        {
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.ArtsAndEntertainment,
                QuizDifficulty=QuizDifficulty.Level.Easy,
                QuizLanguageType=QuizLanguageType.en
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.GeneralKnowledge,
                QuizDifficulty=QuizDifficulty.Level.Easy,
                QuizLanguageType=QuizLanguageType.en
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.HumanSciences,
                QuizDifficulty=QuizDifficulty.Level.Easy,
                QuizLanguageType=QuizLanguageType.en
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.Science,
                QuizDifficulty=QuizDifficulty.Level.Easy,
                QuizLanguageType=QuizLanguageType.en
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.Sports,
                QuizDifficulty=QuizDifficulty.Level.Easy,
                QuizLanguageType=QuizLanguageType.en
            },
              new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.ArtsAndEntertainment,
                QuizDifficulty=QuizDifficulty.Level.Medium,
                QuizLanguageType=QuizLanguageType.en
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.GeneralKnowledge,
                QuizDifficulty=QuizDifficulty.Level.Medium,
                QuizLanguageType=QuizLanguageType.en
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.HumanSciences,
                QuizDifficulty=QuizDifficulty.Level.Medium,
                QuizLanguageType=QuizLanguageType.en
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.Science,
                QuizDifficulty=QuizDifficulty.Level.Medium,
                QuizLanguageType=QuizLanguageType.en
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.Sports,
                QuizDifficulty=QuizDifficulty.Level.Medium,
                QuizLanguageType=QuizLanguageType.en
            },
              new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.ArtsAndEntertainment,
                QuizDifficulty=QuizDifficulty.Level.Hard,
                QuizLanguageType=QuizLanguageType.en
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.GeneralKnowledge,
                QuizDifficulty=QuizDifficulty.Level.Hard,
                QuizLanguageType=QuizLanguageType.en
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.HumanSciences,
                QuizDifficulty=QuizDifficulty.Level.Hard,
                QuizLanguageType=QuizLanguageType.en
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.Science,
                QuizDifficulty=QuizDifficulty.Level.Hard,
                QuizLanguageType=QuizLanguageType.en
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.Sports,
                QuizDifficulty=QuizDifficulty.Level.Hard,
                QuizLanguageType=QuizLanguageType.en
            },
             new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.ArtsAndEntertainment,
                QuizDifficulty=QuizDifficulty.Level.Easy,
                QuizLanguageType=QuizLanguageType.pt
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.GeneralKnowledge,
                QuizDifficulty=QuizDifficulty.Level.Easy,
                QuizLanguageType=QuizLanguageType.pt
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.HumanSciences,
                QuizDifficulty=QuizDifficulty.Level.Easy,
                QuizLanguageType=QuizLanguageType.pt
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.Science,
                QuizDifficulty=QuizDifficulty.Level.Easy,
                QuizLanguageType=QuizLanguageType.pt
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.Sports,
                QuizDifficulty=QuizDifficulty.Level.Easy,
                QuizLanguageType=QuizLanguageType.pt
            },
              new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.ArtsAndEntertainment,
                QuizDifficulty=QuizDifficulty.Level.Medium,
                QuizLanguageType=QuizLanguageType.pt
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.GeneralKnowledge,
                QuizDifficulty=QuizDifficulty.Level.Medium,
                QuizLanguageType=QuizLanguageType.pt
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.HumanSciences,
                QuizDifficulty=QuizDifficulty.Level.Medium,
                QuizLanguageType=QuizLanguageType.pt
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.Science,
                QuizDifficulty=QuizDifficulty.Level.Medium,
                QuizLanguageType=QuizLanguageType.pt
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.Sports,
                QuizDifficulty=QuizDifficulty.Level.Medium,
                QuizLanguageType=QuizLanguageType.pt
            },
              new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.ArtsAndEntertainment,
                QuizDifficulty=QuizDifficulty.Level.Hard,
                QuizLanguageType=QuizLanguageType.pt
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.GeneralKnowledge,
                QuizDifficulty=QuizDifficulty.Level.Hard,
                QuizLanguageType=QuizLanguageType.pt
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.HumanSciences,
                QuizDifficulty=QuizDifficulty.Level.Hard,
                QuizLanguageType=QuizLanguageType.pt
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.Science,
                QuizDifficulty=QuizDifficulty.Level.Hard,
                QuizLanguageType=QuizLanguageType.pt
            },
            new QuestionRepositoryTestValues()
            {
                Count=questionCountValue,
                QuizCategory=QuizCategory.Sports,
                QuizDifficulty=QuizDifficulty.Level.Hard,
                QuizLanguageType=QuizLanguageType.pt
            }
        };

        private QuestionRepository questionRepository;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            var questionFormatter = new QuestionFormatter();

            yield return questionFormatter.FetchDataRelationship();

            questionRepository = new QuestionRepository(questionFormatter);
        }

        private string FormatTestValuesToLog(QuestionRepositoryTestValues questionRepositoryTestValues)
        {
            return $"Requested data: [Count] {questionRepositoryTestValues.Count}, [Category] {questionRepositoryTestValues.QuizCategory}, [Difficulty] {questionRepositoryTestValues.QuizDifficulty}, [Language] {questionRepositoryTestValues.QuizLanguageType}";
        }

        [UnityTest]
        public IEnumerator FetchQuestionsFromRepository_HappyPath([ValueSource("questionRepositoryTestValues")] QuestionRepositoryTestValues testValues)
        {
            List<QuestionModel> fetchedQuestions = new List<QuestionModel>();
            bool hasError = false;
            string errorMessage = "";

            yield return questionRepository.FetchQuestionsFromRepository(
                testValues.Count,
                testValues.QuizCategory,
                testValues.QuizDifficulty,
                testValues.QuizLanguageType,
                (questions) =>
                {
                    fetchedQuestions = questions;
                },
                (error) =>
                {
                    hasError = true;
                    errorMessage = error;
                });

            Assert.IsFalse(fetchedQuestions.Count == 0, $"No quesion was fetched | {FormatTestValuesToLog(testValues)}");
            Assert.IsTrue(fetchedQuestions.Count == testValues.Count, $"Tried to fetch {testValues.Count} questions and successfully fetched {fetchedQuestions.Count} | {FormatTestValuesToLog(testValues)}");
            Assert.IsFalse(hasError, $"Error fetching question: {errorMessage} | {FormatTestValuesToLog(testValues)}");
        }
    }
}