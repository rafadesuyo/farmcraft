using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DreamQuiz.Tests
{
    public class QuestionCreationFormatterTest
    {
        [Test]
        public void FormatQuestion_HappyPath()
        {
            // Arrange
            string scienceID = "593b18f9-42b6-415e-b9d8-5df89f546ef6";
            Guid testGuid = Guid.Empty;
            string questionTitle = "Test Question";
            List<string> answersList = new List<string> { "Answer1", "Answer2", "Answer3" };
            QuizCategory currentCategory = QuizCategory.Sports;
            QuizDifficulty.Level quizDifficultyLevel = QuizDifficulty.Level.Easy;
            int listCorrectIndex = 0;

            QuestionModel testQuestionModel = new QuestionModel(testGuid,
                                                                questionTitle,
                                                                answersList,
                                                                currentCategory,
                                                                quizDifficultyLevel,
                                                                listCorrectIndex,
                                                                null);

            var formatter = new QuestionCreationFormatter();

            // Act
            QuestionCreationDTO result = formatter.FormatQuestion(testQuestionModel, scienceID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(questionTitle, result.QuestionText);
            Assert.AreEqual(scienceID, result.CategoryID);
            Assert.AreEqual(0, result.Language);
            Assert.AreEqual(answersList.Count, result.AnswerMap.Count);

            for (int i = 0; i < answersList.Count; i++)
            {
                Assert.AreEqual(answersList[i], result.AnswerMap[(i + 1).ToString()]);
            }

            Assert.AreEqual(listCorrectIndex, result.CorrectAnswerKey);
        }

        [Test]
        public void FormatQuestion_SpecialCharacters()
        {
            // Arrange
            string scienceID = "593b18f9-42b6-415e-b9d8-5df89f546ef6";
            Guid testGuid = Guid.Empty;
            string questionTitle = "_T&$t Qu&$tion_"; //Simulating a question text with special characters.
            List<string> answersList = new List<string> { "Answer1", "Answer2", "Answer3" };
            QuizCategory currentCategory = QuizCategory.Sports;
            QuizDifficulty.Level quizDifficultyLevel = QuizDifficulty.Level.Easy;
            int listCorrectIndex = 0;

            QuestionModel testQuestionModel = new QuestionModel(testGuid,
                                                                questionTitle,
                                                                answersList,
                                                                currentCategory,
                                                                quizDifficultyLevel,
                                                                listCorrectIndex,
                                                                null);

            var formatter = new QuestionCreationFormatter();

            // Act
            QuestionCreationDTO result = formatter.FormatQuestion(testQuestionModel, scienceID);

            // Assert
            Assert.IsTrue(result == null);
        }
    }
}
