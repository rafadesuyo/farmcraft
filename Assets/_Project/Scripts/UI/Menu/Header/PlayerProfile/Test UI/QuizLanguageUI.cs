using DG.Tweening;
using DreamQuiz;
using UnityEngine;
using UnityEngine.UI;

public class QuizLanguageUI : MonoBehaviour
{
    [SerializeField] private Button toEnBtn = null;
    [SerializeField] private Button toPtBtn = null;

    private void Awake()
    {
        SetupButtons();
        UpdateButtons();
    }

    private void SetupButtons()
    {
        toEnBtn.onClick.RemoveAllListeners();
        toEnBtn.onClick.AddListener(ChangeQuizToEn);

        toPtBtn.onClick.RemoveAllListeners();
        toPtBtn.onClick.AddListener(ChangeQuizToPT);
    }

    private void UpdateButtons()
    {
        bool currentLanguageIsEn = PlayerProgress.QuizLanguage == QuizLanguageType.en;

        toEnBtn.interactable = !currentLanguageIsEn;
        toPtBtn.interactable = currentLanguageIsEn;
    }

    private void ChangeQuizToEn()
    {
        ChangeLanguage(QuizLanguageType.en);
        UpdateButtons();
    }

    private void ChangeQuizToPT()
    {
        ChangeLanguage(QuizLanguageType.pt);
        UpdateButtons();
    }

    private void ChangeLanguage(QuizLanguageType type)
    {
        PlayerProgress.QuizLanguage = type;
        QuestionDatabaseManager.Instance.ChangeDatabaseLanguage(type);
    }
}
