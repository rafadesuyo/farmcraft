using System;
using TMPro;
using UnityEngine;

public class QuizView : MonoBehaviour
{
    //Components
    [Header("Elements")]

    [SerializeField] private CanvasGroup canvasGroup;

    [Space(10)]

    [SerializeField] private TextMeshProUGUI questionCount = null;
    [SerializeField] private QuizQuestionUI quizQuestionUI = null;
    [SerializeField] private QuizFeedbackUI quizFeedbackUI = null;

    [Space(10)]

    [Header("Menus")]
    [SerializeField] private UIControllerAnimated confirmationToExitQuizUI = null;

    [Header("Quiz")]
    [SerializeField] private QuizSystem quizSystem;

    private QuestionModel currentQuestion;

    private void Start()
    {
        quizQuestionUI.Initialize(OnAnswerCallback);
        quizQuestionUI.Hide();
        quizFeedbackUI.Hide();
        HideUI();
    }

    private void OnEnable()
    {
        quizSystem.OnQuizStateChange += QuizSystem_OnQuizStateChange;
        quizSystem.OnQuestionShow += QuizSystem_OnQuestionShow;
        quizSystem.OnQuestionUpdate += QuizSystem_OnQuestionUpdate;
        quizSystem.OnAnswer += QuizSystem_OnAnswer;
        quizSystem.OnTimeoutChange += QuizSystem_OnTimeoutChange;
    }

    private void OnDisable()
    {
        quizSystem.OnQuizStateChange -= QuizSystem_OnQuizStateChange;
        quizSystem.OnQuestionShow -= QuizSystem_OnQuestionShow;
        quizSystem.OnQuestionUpdate -= QuizSystem_OnQuestionUpdate;
        quizSystem.OnTimeoutChange -= QuizSystem_OnTimeoutChange;
    }

    private void QuizSystem_OnQuestionUpdate(QuestionModel question)
    {
        currentQuestion = question;
        quizQuestionUI.UpdateQuestionUI(currentQuestion);
    }

    private void QuizSystem_OnQuestionShow(QuestionModel question)
    {
        currentQuestion = question;
        quizQuestionUI.UpdateQuestionUI(currentQuestion);
        ShowUI();
    }

    private void QuizSystem_OnAnswer(QuizAnswerEventArgs quizAnswerEventArgs)
    {
        ShowFeedbackUI(currentQuestion.QuestionId, quizAnswerEventArgs.AnswerText, quizAnswerEventArgs.IsCorrect);
    }

    private void QuizSystem_OnTimeoutChange(float timeRatio)
    {
        quizQuestionUI.TimeoutUpdate(timeRatio);
    }

    private void QuizSystem_OnQuizStateChange(QuizState quizState)
    {
        if (quizState == QuizState.None)
        {
            HideUI();
        }

        UpdateQuestionCountUI();
    }

    private void ShowUI()
    {
        AudioManager.Instance.Play("QuizPopup");

        quizQuestionUI.Show();
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    private void HideUI()
    {
        confirmationToExitQuizUI.CloseUI();

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    private void ShowFeedbackUI(Guid questionId, string answerText, bool isCorrect)
    {
        quizQuestionUI.Hide();
        quizFeedbackUI.SetupAndShowFeedbackUI(questionId, answerText, isCorrect, FeedbackEndCallback);
    }

    private void FeedbackEndCallback()
    {
        quizFeedbackUI.Hide();
        quizSystem.FinishQuestion();
    }

    private void OnAnswerCallback(string answerText)
    {
        quizSystem.AnswerQuestion(answerText);
    }

    public void OnSelectExitButton()
    {
        OpenConfirmationToExitQuizUI();
    }

    private void OpenConfirmationToExitQuizUI()
    {
        confirmationToExitQuizUI.OpenUI();
    }

    private void UpdateQuestionCountUI()
    {
        questionCount.text = $"{quizSystem.CurrentQuestionCount}/{ quizSystem.TargetQuestionCount}";
    }

    public void CancelQuiz()
    {
        confirmationToExitQuizUI.CloseUI();
        quizSystem.CancelQuiz();
    }
}