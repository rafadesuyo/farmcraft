using Newtonsoft.Json;
using System;

public class Feedback
{
    public string questionID = "";
    public string playerID = "";
    public int[] negativeFeedbackOptions = new int[] { };
    public bool positiveFeedback = false;
    public string userComment = "";
}

public class FeedbackHelper
{
    public static void SendGoodFeedback(Guid questionId)
    {
        string feedbackUrl = ServerURL.GetCreateFeedbackUrl();
        SendFeedback(questionId.ToString());
    }


    /// <summary>
    /// NegativeFeedbackOptions { None = 0, Offensive = 1, GrammarOrSpelling = 2, PossiblyDated = 3, WrongCategory = 4, WrongLanguage = 5, Clarity = 6, TooSpecific = 7, NotFun = 8 }
    /// </summary>
    public static void SendBadFeedback(Guid questionId, int[] negativeIds)
    {
        string feedbackUrl = ServerURL.GetCreateFeedbackUrl();
        SendFeedback(questionId.ToString(), false, negativeIds);
    }

    private static void SendFeedback(string questionId, bool positive = true, int[] negativeIds = null)
    {
        string feedbackUrl = ServerURL.GetCreateFeedbackUrl();

        Feedback feedback = new Feedback();
        feedback.questionID = questionId;
        feedback.playerID = questionId;

        if (negativeIds != null)
        {
            feedback.negativeFeedbackOptions = negativeIds;
        }

        feedback.positiveFeedback = positive;
        feedback.userComment = "none";

        string bodyData = JsonConvert.SerializeObject(feedback);

        ServerManager.Instance.SendPostRequest(feedbackUrl, bodyData);
    }
}
