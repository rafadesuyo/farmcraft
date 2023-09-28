using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

namespace DreamQuiz.Tests
{
    public class QuizSystemTest : MonoBehaviour
    {
        QuizSystem quizSystem;

        [SetUp]
        public void SetupQuizSystem()
        {
            var gameObject = new GameObject("QuizSystem");
            quizSystem = gameObject.AddComponent<QuizSystem>();
        }

        [UnityTest]
        public IEnumerator Initialize_HappyPath()
        {
            //Act
            yield return null;
            bool isSystemRegistered = StageSystemLocator.IsSystemRegistered<QuizSystem>();

            //Assert
            Assert.IsTrue(quizSystem.IsReady,
                $"[Initialize] System not initialized");

            Assert.IsTrue(isSystemRegistered,
                $"[Initialize] System not registered");
        }

        [UnityTest]
        public IEnumerator Initialize_MoreThanOneSystemInScene()
        {
            //Arrange
            List<QuizSystem> quizSystemList = new List<QuizSystem>();

            for (int i = 0; i < 2; i++)
            {
                var gameObject = new GameObject($"QuizSystem {i + 1}");
                var duplicateQuizSystem = gameObject.AddComponent<QuizSystem>();
                quizSystemList.Add(duplicateQuizSystem);
            }

            //Act
            yield return null;
            var fetchedQuizSystem = StageSystemLocator.GetSystem<QuizSystem>();

            //Assert
            for (int i = 0; i < quizSystemList.Count; i++)
            {
                Assert.IsFalse(
                      quizSystemList[i].IsReady,
                    $"[MoreThanOneSystemInScene] Additional system number {i} was initialized");
            }

            for (int i = 0; i < quizSystemList.Count; i++)
            {
                Assert.AreNotEqual(
                    fetchedQuizSystem,
                    quizSystemList[i],
                    $"[MoreThanOneSystemInScene] Additional system number {i} was registered");
            }
        }
    }
}