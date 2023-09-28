using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

namespace DreamQuiz.Tests
{
    public class QuestionCreationRequestHandlerTest
    {
        private QuestionCreationRequestHandler handler;

        [SetUp]
        public void SetUp()
        {
            handler = CreateTestHandler();
        }

        private QuestionCreationRequestHandler CreateTestHandler()
        {
            var gameObject = new GameObject();
            return gameObject.AddComponent<QuestionCreationRequestHandler>();
        }

        private QuestionCreationDTO CreateTestDTO()
        {
            string questionTitle = "PlayMode Test Question Creation";
            string scienceID = "593b18f9-42b6-415e-b9d8-5df89f546ef6";
            Dictionary<string, string> testAnswers = new Dictionary<string, string>();
            int correctAnswerIndex = 0;
            int answersCount = 6;
            var formatter = new QuestionCreationFormatter();

            for (int i = 0; i < answersCount; i++)
            {
                testAnswers.Add((i + 1).ToString(), "unitTest_" + i);
            }

            var testDTO = new QuestionCreationDTO();
            testDTO.QuestionText = questionTitle;
            testDTO.CategoryID = scienceID;
            testDTO.Language = 0;
            testDTO.AnswerMap = testAnswers;
            testDTO.CorrectAnswerKey = correctAnswerIndex;

            return testDTO;
        }

        [UnityTest]
        public IEnumerator QuestionCreationRequestHandlerTest_HappyPath()
        {
            //Arrange
            QuestionCreationDTO testDTO = CreateTestDTO();

            bool success = false;
            QuestionCreationRequestHandler.OnQuestionCreationDataSent += (isSuccess) =>
            {
                success = isSuccess;
            };

            //Act
            handler.SendCreatedQuestionRequest(testDTO);

            yield return new WaitForSeconds(2f);

            //Assert
            Assert.IsTrue(success);
        }

        [UnityTest]
        public IEnumerator QuestionCreationRequestHandlerTest_MissingField()
        {
            //Arrange
            QuestionCreationDTO testDTO = CreateTestDTO();
            testDTO.CategoryID = null; //Simulating missing field

            bool success = false;
            QuestionCreationRequestHandler.OnQuestionCreationDataSent += (isSuccess) =>
            {
                success = isSuccess;
            };

            //Act
            handler.SendCreatedQuestionRequest(testDTO);

            yield return new WaitForSeconds(2f);

            // Assert
            Assert.IsFalse(success);
        }
    }
}
