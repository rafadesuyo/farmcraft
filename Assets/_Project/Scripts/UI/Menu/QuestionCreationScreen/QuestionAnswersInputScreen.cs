using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

namespace DreamQuiz
{
    public class QuestionAnswersInputScreen : MonoBehaviour
    {
        [Header("HEADER INFO")]
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI categoryTitleText;
        [Header("INPUT FIELDS")]
        [SerializeField] private int questionMinimumInputFieldTextLength = 12;
        [SerializeField] private int answersMinimumInputFieldTextLength = 5;
        [SerializeField] private TMP_InputField questionInputField;
        [SerializeField] private TMP_InputField correctAnswerInputField;
        [SerializeField] private TMP_InputField[] wrongAnswersInputFields;
        [Header("WARNINGS")]
        [SerializeField] private GameObject specialCharactersWarning;
        [SerializeField] private TextMeshProUGUI questionCharacterWarning;
        private const string questionWarningPrefix = "Question must contain at least ";
        [SerializeField] private TextMeshProUGUI answersCharacterWarning;
        private const string answersWarningPrefix = "Answers must contain at least ";
        [Header("FOOTER INFO")]
        [SerializeField] private CanvasGroup sendButton;

        [Header("SENDING DATA UI")] //TEMP. There's no Figma for this UI. Task for updating UI: https://ocarinastudios.atlassian.net/browse/DQG-2257?atlOrigin=eyJpIjoiMjU5NWRlZjhhZTE3NGJiZGExYjY2NTBjN2FlY2Q5ZmUiLCJwIjoiaiJ9
        [SerializeField] private GameObject sendingDataPanel;
        [SerializeField] private GameObject successFeedback;
        [SerializeField] private GameObject failedFeedback;
        [SerializeField] private GameObject closeButton;

        private float sendButtonInvisibleValue = 0.4f;
        private QuizCategory currentCategory;

        private string question;
        private string correctAnswer;
        private string[] wrongAnswers = new string[5];
        private List<string> allAnswers;

        public static event Action<QuestionCreationDTO> OnPressedCreateQuestionButton;
        private void OnEnable()
        {
            QuestionCategorySelectItem.OnCategorySelected += ResetScreen;
            QuestionCategorySelectItem.OnCategorySelected += UpdateIconAndTitle;
            QuestionCreationRequestHandler.OnQuestionCreationDataSent += ToggleResultFeedback;
        }

        private void OnDisable()
        {
            QuestionCategorySelectItem.OnCategorySelected -= ResetScreen;
            QuestionCategorySelectItem.OnCategorySelected -= UpdateIconAndTitle;
            QuestionCreationRequestHandler.OnQuestionCreationDataSent -= ToggleResultFeedback;
        }

        private void Start()
        {
            SetWarningTexts();
            HideSendingDataUI();

            allAnswers = new List<string>();
            allAnswers.Add(correctAnswer);

            foreach (var answer in wrongAnswers)
            {
                allAnswers.Add(answer);
            }
        }

        private void SetWarningTexts()
        {
            questionCharacterWarning.text = $"{questionWarningPrefix} {questionMinimumInputFieldTextLength} characters.";
            answersCharacterWarning.text = $"{answersWarningPrefix} {answersMinimumInputFieldTextLength} characters.";
        }

        private void UpdateIconAndTitle(QuizCategory category, Sprite iconSprite)
        {
            iconImage.sprite = iconSprite;
            categoryTitleText.text = ProjectAssetsDatabase.Instance.GetCategoryName(category);
            currentCategory = category;
        }
        
        bool IsInputFieldFilled(TMP_InputField inputField, int minTextLength)
        {
            return inputField.text.Length >= minTextLength;
        }

        private bool AreAllInputFieldsFilled()
        {
            if (IsInputFieldFilled(questionInputField, questionMinimumInputFieldTextLength) == false)
            {
                return false;
            }

            if (IsInputFieldFilled(correctAnswerInputField, answersMinimumInputFieldTextLength) == false)
            {
                return false;
            }

            foreach (var field in wrongAnswersInputFields)
            {
                if (IsInputFieldFilled(field, answersMinimumInputFieldTextLength) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public void SetQuestionFieldCharacterWarning(bool isActive)
        {
            questionCharacterWarning.gameObject.SetActive(isActive);
        }

        public void SetAnswersFieldCharacterWarning(bool isActive)
        {
            answersCharacterWarning.gameObject.SetActive(isActive);
        }

        public void ToggleSendButtonVisibility()
        {
            sendButton.interactable = AreAllInputFieldsFilled();
            sendButton.alpha = sendButton.interactable ? 1 : sendButtonInvisibleValue;
        }

        public void UpdateQuestion()
        {
            SetQuestionFieldCharacterWarning(!IsInputFieldFilled(questionInputField, questionMinimumInputFieldTextLength));
            question = questionInputField.text;
        }

        public void UpdateCorrectAnswer()
        {
            SetAnswersFieldCharacterWarning(!IsInputFieldFilled(correctAnswerInputField, answersMinimumInputFieldTextLength));
            correctAnswer = correctAnswerInputField.text;
        }

        public void UpdateWrongAnswer(int index)
        {
            SetAnswersFieldCharacterWarning(!IsInputFieldFilled(wrongAnswersInputFields[index], answersMinimumInputFieldTextLength));
            wrongAnswers[index] = wrongAnswersInputFields[index].text;
        }

        public void SendQuestion()
        {
            allAnswers[0] = correctAnswer;

            for (int i = 1; i < allAnswers.Count; i++)
            {
                allAnswers[i] = wrongAnswers[i - 1];
            }

            var quizDifficultyLevel = QuizDifficulty.Level.Easy;
            int listCorrectIndex = 0;
            Guid guid = Guid.Empty;

            QuestionModel createdQuestionModel = new QuestionModel(guid, 
                                                                   question, 
                                                                   allAnswers, 
                                                                   currentCategory, 
                                                                   quizDifficultyLevel, 
                                                                   listCorrectIndex, 
                                                                   null);

            QuestionCreationFormatter questionCreationFormatter = new QuestionCreationFormatter();
            QuestionCreationDTO createdQuestionDTO = questionCreationFormatter.FormatQuestion(createdQuestionModel);

            if (createdQuestionDTO == null)
            {
                ShowSpecialCharactersWarning();
                return;
            }

            ShowSendingDataUI();
            OnPressedCreateQuestionButton?.Invoke(createdQuestionDTO);
        }

        private void ClearAllInputFiels()
        {
            List<TMP_InputField> allInputFields = new List<TMP_InputField>();

            allInputFields.Add(questionInputField);
            allInputFields.Add(correctAnswerInputField);

            foreach (var wrongAnswer in wrongAnswersInputFields)
            {
                allInputFields.Add(wrongAnswer);
            }

            foreach (var inputField in allInputFields)
            {
                inputField.text = "";
            }

        }

        private void ShowSendingDataUI()
        {
            closeButton.SetActive(false);
            successFeedback.SetActive(false);
            failedFeedback.SetActive(false);
            sendingDataPanel.SetActive(true);
        }

        public void HideSendingDataUI()
        {
            closeButton.SetActive(false);
            failedFeedback.SetActive(false);
            successFeedback.SetActive(false);
            sendingDataPanel.SetActive(false);
        }

        private void ToggleResultFeedback(bool isSuccess)
        {
            successFeedback.SetActive(isSuccess);
            failedFeedback.SetActive(!successFeedback.activeSelf);
            closeButton.SetActive(true);

            if (isSuccess)
            {
                ResetScreen();
            }
        }

        private void ShowSpecialCharactersWarning()
        {
            specialCharactersWarning.SetActive(true);
        }

        private void HideSpecialCharactersWarning()
        {
            specialCharactersWarning.SetActive(false);
        }

        private void ResetScreen(QuizCategory category = QuizCategory.None, Sprite sprite = null)
        {
            ClearAllInputFiels();
            HideSpecialCharactersWarning();
            SetQuestionFieldCharacterWarning(false);
            SetAnswersFieldCharacterWarning(false);
            ToggleSendButtonVisibility();
        }
    }
}
