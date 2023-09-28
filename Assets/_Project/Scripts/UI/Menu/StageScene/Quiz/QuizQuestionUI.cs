using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class QuizQuestionUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup = null;

    [Space(10)]

    [SerializeField] private Image categoryIcon = null;

    [Space(10)]

    [SerializeField] private TextMeshProUGUI questionTxt = null;
    [SerializeField] private Slider timeSlider = null;

    [Space(10)]

    [SerializeField] private List<AnswerItem> answerItems = new List<AnswerItem>();

    private Action<string> onAnswerCallback;

    public void Initialize(Action<string> onAnswerCallback)
    {
        this.onAnswerCallback = onAnswerCallback;
    }

    public void UpdateQuestionUI(QuestionModel question)
    {
        questionTxt.text = question.Title;
        categoryIcon.sprite = ProjectAssetsDatabase.Instance.GetCategoryIcon(question.Category);
        List<string> orderedAnswers = question.Answers.ToList();
        orderedAnswers.Shuffle();

        for (int i = 0; i < answerItems.Count; i++)
        {
            string answer = orderedAnswers[i];
            answerItems[i].Setup(answer, question.IsAnswerCorrect(answer), this);
        }
    }

    public void SetAnswer(string answerText)
    {
        foreach (var answerItem in answerItems)
        {
            answerItem.UpdateToAnsweredState();
        }

        onAnswerCallback?.Invoke(answerText);
    }

    public void TimeoutUpdate(float timeRatio)
    {
        timeSlider.value = timeRatio;
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
