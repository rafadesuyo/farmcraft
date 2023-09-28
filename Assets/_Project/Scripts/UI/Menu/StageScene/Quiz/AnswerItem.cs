using UnityEngine;
using UnityEngine.UI;

public class AnswerItem : MonoBehaviour
{
    [SerializeField] private Button questionBtn = null;
    [SerializeField] private Image itemImg = null;
    [SerializeField] private TMPro.TextMeshProUGUI txtAnswer = null;

    [Header("Sprites")]
    [SerializeField] private Sprite defaultSprite;

    [SerializeField] private Sprite wrongAnswerSprite;
    [SerializeField] private Sprite rightAnswerSprite;
    [SerializeField] private Sprite disabledSprite;

    private string answerText = "";
    private bool isCorrect = false;
    private bool isSelected = false;
    private QuizQuestionUI quizQuestionUI;

    private void Awake()
    {
        questionBtn.onClick.AddListener(() => OnPlayerSelect());
    }

    private void OnPlayerSelect()
    {
        isSelected = true;
        SetButtonResultSprite();

        if (isCorrect)
        {
            AudioManager.Instance.Play("Correct");
        }
        else
        {
            AudioManager.Instance.Play("Incorrect");
        }

        quizQuestionUI.SetAnswer(answerText);
    }

    public void Setup(string answer, bool correct, QuizQuestionUI quizQuestionUI)
    {
        SetAnswerText(answer);
        itemImg.sprite = defaultSprite;
        isCorrect = correct;
        this.quizQuestionUI = quizQuestionUI;
        isSelected = false;
    }

    public void SetupAnswerResultButton(string answer, bool correct)
    {
        SetAnswerText(answer);
        isCorrect = correct;

        SetButtonResultSprite();
    }

    public void UpdateToAnsweredState()
    {
        if (isSelected)
        {
            return;
        }

        DisableQuestion();
    }

    public void DisableQuestion()
    {
        itemImg.sprite = disabledSprite;
    }

    private void SetAnswerText(string answer)
    {
        answerText = answer;
        txtAnswer.text = $"{answerText}";
    }

    private void SetButtonResultSprite()
    {
        if (isCorrect)
        {
            itemImg.sprite = rightAnswerSprite;
        }
        else
        {
            itemImg.sprite = wrongAnswerSprite;
        }
    }
}