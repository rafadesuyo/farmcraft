using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizViewAnimation : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private RectTransform wellDoneRect;

    [SerializeField] private TextMeshProUGUI wellDoneText;
    [SerializeField] private CanvasGroup wellDoneCanvasGroup;
    [SerializeField] private TextMeshProUGUI wrongText;
    [SerializeField] private RectTransform wrongRect;
    [SerializeField] private CanvasGroup wrongCanvasGroup;
    [SerializeField] private RectTransform wrongTextSpawnPoint;
    [SerializeField] private float wrongTextTypingSpeed = 0.05f;
    [SerializeField] private float wrongTextRevealDuration = 0.5f;
    [SerializeField] private float anchorWrongTextDuration = 0.5f;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private float typingSpeed = 0.0001f;
    [SerializeField] private float typingSpeedWrongText = 0.1f;

    [Header("Question Buttons")]
    [SerializeField] private List<GameObject> answers = new List<GameObject>();

    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float scaleDuration;
    [SerializeField] private float quizViewSpawnPointOffSetY;

    [Header("Icons")]
    [SerializeField] private CanvasGroup timerIcon = null;

    [SerializeField] private CanvasGroup timerSlider = null;
    [SerializeField] private RectTransform timeIconRect;
    [SerializeField] private float iconRotationDuration;
    private Tween timerTween;

    [Header("QuizView Panel")]
    [SerializeField] private RectTransform quizViewPanel = null;

    [SerializeField] private float quizRevealDuration = 0.2f;
    [SerializeField] private RectTransform quizViewSpawnPoint = null;

    [Header("Script References")]
    private QuizSystem quizSystem;

    [Header("Answer button references")]
    [SerializeField] private List<Button> answerButtons;

    private void Start()
    {
        quizSystem = StageSystemLocator.GetSystem<QuizSystem>();
        DisableAnswerClick();

        quizSystem.OnQuestionShow += PlayTextAnimation;
        quizSystem.OpenQuiz += OpenQuizViewAnimation;
        quizSystem.OnQuestionAnswerRight += ResetAnswers;
        quizSystem.ResetAnswers += ResetAnswers;
        quizSystem.ResetAnswers += CongratulationsTextHide;
        quizSystem.OnQuestionAnswerRight += CongratulationsTextReveal;
        quizSystem.ResetAnswers += WrongTextReveal;
    }

    //quizview
    private void OpenQuizViewAnimation()
    {
        quizSystem.IsTimeoutSuspended = true;
        quizViewPanel.anchoredPosition = new Vector2(quizViewSpawnPoint.anchoredPosition.x, quizViewSpawnPoint.anchoredPosition.y);
        Sequence sequence = DOTween.Sequence();

        sequence.Append(quizViewPanel.DOAnchorPosY(quizViewSpawnPoint.anchoredPosition.x + quizViewSpawnPointOffSetY, quizRevealDuration).SetEase(Ease.OutFlash));

        sequence.Play();
    }

    private void CloseQuizViewAnimation()
    {
        quizViewPanel.anchoredPosition = new Vector2(quizViewSpawnPoint.anchoredPosition.x, -quizViewSpawnPoint.anchoredPosition.y);
        Sequence sequence = DOTween.Sequence();
        StopTimerAnimation();

        sequence.Append(quizViewPanel.DOAnchorPosY(quizViewSpawnPoint.anchoredPosition.y + quizViewSpawnPointOffSetY, quizRevealDuration).SetEase(Ease.OutFlash));

        sequence.Play();
    }

    //Question title animation
    private void PlayTextAnimation(QuestionModel Title)
    {
        DisableIcons();
        StartCoroutine(WriteText(questionText, typingSpeed, OnTypingEnd));
    }

    private IEnumerator WriteText(TextMeshProUGUI textMesh, float speed, Action onTypingEnd)
    {
        string tmp = textMesh.text;
        int totalCharacters = tmp.Length;
        textMesh.maxVisibleCharacters = 0;

        for (int i = 0; i <= totalCharacters; i++)
        {
            textMesh.maxVisibleCharacters = i;

            yield return new WaitForSeconds(speed);
        }
        onTypingEnd?.Invoke();
    }

    private void OnTypingEnd()
    {
        NewQuestionAnimation();
    }

    //Answer button animation
    public void NewQuestion()
    {
        Sequence sequence = DOTween.Sequence();

        foreach (var answer in answers)
        {
            CanvasGroup itemCanvas = answer.GetComponent<CanvasGroup>();

            sequence.Append(answer.transform.DOScaleX(0.3f, scaleDuration)
                .SetEase(Ease.OutQuad));

            sequence.Join(itemCanvas.DOFade(1f, fadeDuration)
                .SetEase(Ease.Linear));

            sequence.Append(answer.transform.DOScaleX(1.1f, scaleDuration)
                .SetEase(Ease.InQuad));

            sequence.Append(answer.transform.DOScaleX(1.0f, scaleDuration)
                .SetEase(Ease.OutQuad));

            sequence.AppendInterval(scaleDuration);
        }

        sequence.OnComplete(() =>
        {
            EnableAnswerClick();
            Debug.Log("Sequence completed");
            EnableIcons();
            RotateTimerIcon();
            quizSystem.IsTimeoutSuspended = false;
        });
    }

    public void NewQuestionAnimation()
    {
        NewQuestion();
    }

    private void EnableAnswerClick()
    {
        foreach (Button button in answerButtons)
        {
            button.interactable = true;
        }
    }

    private void DisableAnswerClick()
    {
        foreach (Button button in answerButtons)
        {
            button.interactable = false;
        }
    }

    public void ResetAnswers()
    {
        foreach (var answer in answers)
        {
            CanvasGroup itemCanvas = answer.GetComponent<CanvasGroup>();
            itemCanvas.alpha = 0f;
        }
    }

    //Icons
    private void EnableIcons()
    {
        timeIconRect.DORestart();
        timerIcon.alpha = 1f;
        timerSlider.alpha = 1f;
    }

    private void DisableIcons()
    {
        timerIcon.alpha = 0f;
        timerSlider.alpha = 0f;
    }

    //Rotation of the timer icon
    private void RotateTimerIcon()
    {
        StopTimerAnimation();

        float rotationAngle = -360f;
        float rotationDuration = iconRotationDuration;

        timerTween = timeIconRect.DORotate(new Vector3(0f, 0f, rotationAngle), rotationDuration, RotateMode.FastBeyond360)
        .SetEase(Ease.Linear);
        timerTween.SetLoops(-1, LoopType.Restart);
    }

    public void StopTimerAnimation()
    {
        if (timerTween != null)
        {
            timerTween.Kill();
            timerTween = null;
        }
    }

    //Congratulations text
    private void CongratulationsTextReveal()
    {
        DisableAnswerClick();
        WrongTextHide();
        wellDoneCanvasGroup.alpha = 1f;
        StartCoroutine(WriteText(wellDoneText, wrongTextTypingSpeed, null));
    }

    private void CongratulationsTextHide()
    {
        wellDoneCanvasGroup.alpha = 0f;
    }

    //Wrong Text
    private void WrongTextReveal()
    {
        DisableAnswerClick();
        wrongCanvasGroup.DOFade(1, wrongTextRevealDuration);
        wrongRect.anchoredPosition = new Vector2(0, 0);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(wrongRect.DOAnchorPosY(wrongTextSpawnPoint.anchoredPosition.y, anchorWrongTextDuration).SetEase(Ease.OutFlash));
        sequence.Play();
        StartCoroutine(WriteText(wrongText, typingSpeedWrongText, null));
    }

    private void WrongTextHide()
    {
        wrongCanvasGroup.alpha = 0f;
    }
}