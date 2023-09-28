using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using UnityEngine.UI;

namespace DreamQuiz
{
    public class ReviewAQuestionScreen : MonoBehaviour
    {
        [SerializeField] private CategoryDatabaseSO categoryDatabase;
        [SerializeField] private GameObject bottomFeedbackButtons;
        [SerializeField] private ReviewAnswerButton[] answerButtons;
        [SerializeField] private TextMeshProUGUI categoryTitle;
        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject retrievingQuestionWarning;
        [SerializeField] private float intentionalDelaySeconds;

        public static Action OnReadyToReviewQuestion;
        public static event Action<string, int> OnLikeOrDislikedQuestion;
        public static event Action OnQuestionUpdated;

        private QuestionReviewDTO questionDTO = null;

        private void OnEnable()
        {
            QuestionCreationRequestHandler.OnRandomQuestionReceived += GetDTO;
            QuestionReviewScreensController.OnReviewAQuestionScreenSelected += FetchQuestion;
            OnReadyToReviewQuestion += PrepareUIForNewQuestion;
        }

        private void OnDisable()
        {
            QuestionCreationRequestHandler.OnRandomQuestionReceived -= GetDTO;
            QuestionReviewScreensController.OnReviewAQuestionScreenSelected -= FetchQuestion;
            OnReadyToReviewQuestion -= PrepareUIForNewQuestion;
        }

        private void GetDTO(QuestionReviewDTO dto)
        {
            questionDTO = dto;
            StartCoroutine(UpdateQuestionContentCoroutine());
        }

        public void FetchQuestion()
        {
            OnReadyToReviewQuestion?.Invoke();
        }

        public void RefreshButtonsState()
        {
            foreach (var item in answerButtons)
            {
                item.ResetAnswerFeedback();
            }

            bottomFeedbackButtons.SetActive(false);
        }

        private void PrepareUIForNewQuestion()
        {
            retrievingQuestionWarning.SetActive(true);
            iconImage.color = new Color(1, 1, 1, 0);
            RefreshButtonsState();
            ClearAllTexts();
        }

        private IEnumerator UpdateQuestionContentCoroutine()
        {
            yield return new WaitForSeconds(intentionalDelaySeconds);

            retrievingQuestionWarning.SetActive(false);
            QuestionCreationFormatter formatter = new QuestionCreationFormatter();
            QuizCategory category = formatter.GetCategoryByID(questionDTO.CategoryID.ToString());
            categoryTitle.text = ProjectAssetsDatabase.Instance.GetCategoryName(category);
            questionText.text = questionDTO.QuestionText;
            iconImage.sprite = categoryDatabase.GetIconByCategory(category);

            Dictionary<string,string> answers = questionDTO.AnswerMap;

            int count = 0;
            foreach (KeyValuePair<string, string> entry in answers)
            {
                if (count >= answerButtons.Length)
                {
                    break;
                }

                answerButtons[count].UpdateText(entry.Value);

                if (count == questionDTO.CorrectAnswerKey)
                {
                    answerButtons[count].SetAsCorrectAnswer();
                }

                count++;
            }

            iconImage.color = new Color(1, 1, 1, 1);
            OnQuestionUpdated?.Invoke();
        }

        private void ClearAllTexts()
        {
            categoryTitle.text = "";
            questionText.text = "";
            foreach (var item in answerButtons)
            {
                item.UpdateText("");
            }
        }

        public void LikeOrDislikeQuestion(int result) //like = 1, dislike = 2
        {
            OnLikeOrDislikedQuestion?.Invoke(questionDTO.QuestionReviewID.ToString(), result);
        }

    }
}
