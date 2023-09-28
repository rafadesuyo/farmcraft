using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

namespace DreamQuiz.Tests
{
    public class QuestionReportRequestTest
    {
        [UnityTest]
        public IEnumerator SendReportQuestionRequest_HappyPath()
        {
            // Arrange
            var gameObject = new GameObject();
            var handler = gameObject.AddComponent<QuestionCreationRequestHandler>();
            string questionReviewID = "8dbd3356-a036-4f6f-b958-d04f9f2555c0";
            List<int> testReasonsList = new List<int> { 0, 5, 7 };
            string testComment = "Testing!";

            UnityWebRequest request = handler.ReportWebRequest(questionReviewID, testReasonsList, testComment);

            //Act
            yield return request.SendWebRequest();

            //Assert
            Assert.IsTrue(request.result == UnityWebRequest.Result.Success);
        }

        [UnityTest]
        public IEnumerator SendReportQuestionRequest_InvalidReviewID()
        {
            // Arrange
            var gameObject = new GameObject();
            var handler = gameObject.AddComponent<QuestionCreationRequestHandler>();
            string questionReviewID = "abcde"; //simulating invalid reviewID
            List<int> testReasonsList = new List<int> { 0, 5, 7 };
            string testComment = "Testing!";

            UnityWebRequest request = handler.ReportWebRequest(questionReviewID, testReasonsList, testComment);

            //Act
            yield return request.SendWebRequest();

            //Assert
            Assert.IsFalse(request.result == UnityWebRequest.Result.Success);
        }
    }
}
