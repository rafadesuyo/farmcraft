using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuizFeedbackUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup = null;

    [Space(10)]

    [SerializeField] private Image dreamerIcon = null;

    [Space(10)]

    [SerializeField] private RectTransform feedbackContainer = null;
    [SerializeField] private RectTransform answerResultContainer = null;
    [SerializeField] private AnswerItem answerResult = null;

    [Space(10)]

    [SerializeField] private RectTransform badFeedbackContainer = null;
    [SerializeField] private Toggle[] toggles = new Toggle[0];

    [Space(10)]

    [SerializeField] private Sprite dreamerCorrectAnswer;
    [SerializeField] private Sprite dreamerIncorrectAnswer;

    [Space(10)]

    Action onFeedbackCallback;

    private Guid questionId;
    private bool isShowingBadFeedback;

    public void SetupAndShowFeedbackUI(Guid questionId, string answerText, bool isCorrect, Action onFeedbackCallback)
    {
        this.questionId = questionId;
        dreamerIcon.sprite = isCorrect ? dreamerCorrectAnswer : dreamerIncorrectAnswer;

        answerResult.SetupAnswerResultButton(answerText, isCorrect);
        this.onFeedbackCallback = onFeedbackCallback;
        ShowFeedbackPanel();
        Show();
    }

    public void ShowFeedbackPanel()
    {
        isShowingBadFeedback = false;

        feedbackContainer.gameObject.SetActive(!isShowingBadFeedback);
        badFeedbackContainer.gameObject.SetActive(isShowingBadFeedback);
    }

    public void ShowBadFeedbackUI()
    {
        foreach (Toggle toggle in toggles)
        {
            toggle.isOn = false;
        }

        isShowingBadFeedback = true;

        feedbackContainer.gameObject.SetActive(!isShowingBadFeedback);
        badFeedbackContainer.gameObject.SetActive(isShowingBadFeedback);
    }

    public void SendGoodFeedback()
    {
        FeedbackHelper.SendGoodFeedback(questionId);
        ExitFeedback();
    }

    public void SendBadFeedback()
    {
        List<int> idsList = new List<int>();

        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn == true)
            {
                idsList.Add(i + 1);
            }
        }

        FeedbackHelper.SendBadFeedback(questionId, idsList.ToArray());
        ExitFeedback();
    }

    public void ExitFeedback()
    {
        onFeedbackCallback?.Invoke();
    }

    public void Next()
    {
        if (isShowingBadFeedback)
        {
            SendBadFeedback();
        }
        else
        {
            ExitFeedback();
        }
    }

    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}
