using UnityEngine;
using TMPro;

public class MathTestingUI : MonoBehaviour
{
    [SerializeField] private GameObject mathTestUI = null;
    [SerializeField] private TextMeshProUGUI questionTitleTxt = null;
    [SerializeField] private TextMeshProUGUI questionAnswersTxt = null;

    private int clicksNeededToOpen = 5;
    private int currentClickCount = 0;

    private void OnEnable()
    {
        currentClickCount = 0;
    }

    private void OpenTestUI()
    {
        mathTestUI.SetActive(true);
    }

    private void ShowQuestion(QuestionModel question)
    {
        questionTitleTxt.text = question.Title;

        string answers = "";

        foreach (string answer in question.Answers)
        {
            answers = $"{answers}\n{answer}";
        }

        questionAnswersTxt.text = answers;
    }

    public void OnHiddenClick()
    {
        currentClickCount++;

        if (currentClickCount >= clicksNeededToOpen)
        {
            OpenTestUI();
        }
    }

    public void ShowEasy()
    {
        ShowQuestion(MathQuestionManager.Instance.GetRandomQuestionByDifficulty(QuizDifficulty.Level.Easy));
    }

    public void ShowMedium()
    {
        ShowQuestion(MathQuestionManager.Instance.GetRandomQuestionByDifficulty(QuizDifficulty.Level.Medium));
    }

    public void CloseTestUI()
    {
        mathTestUI.SetActive(false);
    }
}
