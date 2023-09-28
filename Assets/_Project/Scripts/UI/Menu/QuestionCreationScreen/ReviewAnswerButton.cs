using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace DreamQuiz
{
    public class ReviewAnswerButton : MonoBehaviour
    {
        [SerializeField] private GameObject correctAnswerFeedback;
        [SerializeField] private TextMeshProUGUI answerText;
        [SerializeField] private CanvasGroup buttonCanvasGrp;
        [SerializeField] private bool isCorrectAnswer;

        public static event Action OnAnswerPicked;
        private const float buttonInactiveAlpha = 0.5f;

        private void OnEnable()
        {
            OnAnswerPicked += ShowCorrectAnswerFeedback;

            ReviewAQuestionScreen.OnReadyToReviewQuestion += DisableButton;
            ReviewAQuestionScreen.OnQuestionUpdated += EnableButton;
        }

        private void Start()
        {
            correctAnswerFeedback.SetActive(false);
        }

        private void OnDisable()
        {
            ReviewAQuestionScreen.OnReadyToReviewQuestion -= DisableButton;
            OnAnswerPicked -= ShowCorrectAnswerFeedback;
            ReviewAQuestionScreen.OnQuestionUpdated -= EnableButton;
        }

        public void SetAsCorrectAnswer()
        {
            isCorrectAnswer = true;
            ShufflePosition();
        }

        private void ShufflePosition()
        {
            int randomIndex = UnityEngine.Random.Range(0, transform.parent.childCount);
            transform.SetSiblingIndex(randomIndex);
        }

        public void SelectAnswer()
        {
            OnAnswerPicked?.Invoke();
        }

        private void ShowCorrectAnswerFeedback()
        {
            correctAnswerFeedback.SetActive(isCorrectAnswer);
            SetButtonVisibility();
        }

        public void ResetAnswerFeedback()
        {
            isCorrectAnswer = false;
            correctAnswerFeedback.SetActive(false);
            EnableButton();
        }

        public void UpdateText(string text)
        {
            answerText.text = text;
        }

        public void EnableButton()
        {
            buttonCanvasGrp.alpha = 1;
            buttonCanvasGrp.blocksRaycasts = true;
        }

        public void DisableButton()
        {
            buttonCanvasGrp.alpha = buttonInactiveAlpha;
            buttonCanvasGrp.blocksRaycasts = false;
        }

        public void SetButtonVisibility()
        {
            if (isCorrectAnswer)
            {
                buttonCanvasGrp.alpha = 1f;
            }
            else
            {
                buttonCanvasGrp.alpha = buttonInactiveAlpha;
            }

            buttonCanvasGrp.blocksRaycasts = isCorrectAnswer;
        }
    }
}