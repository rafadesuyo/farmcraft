using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace DreamQuiz.Tests
{
    public class QuestionFormatterTest
    {
        [UnityTest]
        public IEnumerator FetchDataRelationship_HappyPath()
        {
            var questionFormatter = new QuestionFormatter();

            yield return questionFormatter.FetchDataRelationship();

            var responseData = questionFormatter.GetFormatterResponseData();

            Assert.IsTrue(responseData.IsDone,
                $"[FetchDataRelationship] Response data not done");

            Assert.IsFalse(responseData.HasError,
                $"[FetchDataRelationship] Error fetching data: {responseData.ErrorMessage}");
        }
    }
}