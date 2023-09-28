using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

namespace DreamQuiz.Tests
{
    public class QuestionReviewRequestTest
    {
        [UnityTest]
        public IEnumerator SendReviewQuestionRequest_HappyPath()
        {
            // Arrange
            var gameObject = new GameObject();
            var handler = gameObject.AddComponent<QuestionCreationRequestHandler>();
            string questionReviewID = "8dbd3356-a036-4f6f-b958-d04f9f2555c0";
            int result = 1;
            UnityWebRequest request = handler.ReviewWebRequest(questionReviewID, result);

            //Act
            yield return request.SendWebRequest();

            //Assert
            Assert.IsTrue(request.result == UnityWebRequest.Result.Success);
        }

        [UnityTest]
        public IEnumerator SendReviewQuestionRequest_InvalidResult()
        {
            // Arrange
            var gameObject = new GameObject();
            var handler = gameObject.AddComponent<QuestionCreationRequestHandler>();
            string questionReviewID = "8dbd3356-a036-4f6f-b958-d04f9f2555c0";
            int result = 25; //simulating invalid result
            UnityWebRequest request = handler.ReviewWebRequest(questionReviewID, result);

            //Act
            yield return request.SendWebRequest();

            //Assert
            Assert.IsFalse(request.result == UnityWebRequest.Result.Success);
        }

        [UnityTest]
        public IEnumerator SendReviewQuestionRequest_InvalidReviewID()
        {
            // Arrange
            var gameObject = new GameObject();
            var handler = gameObject.AddComponent<QuestionCreationRequestHandler>();
            string questionReviewID = "abcd"; //simulating invalid reviewID
            int result = 25; 
            UnityWebRequest request = handler.ReviewWebRequest(questionReviewID, result);

            //Act
            yield return request.SendWebRequest();

            //Assert
            Assert.IsFalse(request.result == UnityWebRequest.Result.Success);
        }
    }
}
