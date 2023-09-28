using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace DreamQuiz
{
    public class RemoveReportScreen : MonoBehaviour
    {
        [SerializeField] private GameObject mainContent;
        [SerializeField] private ReviewReasonButton[] reviewReasonButtons;
        [SerializeField] private CanvasGroup saveAndExitButtonCanvasGrp;

        [Header("OTHER REASON")]
        [SerializeField] private GameObject otherReasonContainer;
        [SerializeField] private CanvasGroup otherReason_SaveButtonCanvasGrp;
        [SerializeField] private TMP_InputField otherReason_inputField;
        [SerializeField] private TextMeshProUGUI charsWarningText;

        private List<int> reasonsList = new List<int>();
        private const int otherReason_minChars = 5;
        private const float nonInteractableAlpha = 0.2f;
        private QuestionReviewDTO questionDTO;

        public static event Action<string, List<int>, string> OnQuestionReported;
        public static event Action OnSaveAndExitReportScreen;

        private const int server_EnumFirstValue = 1;
        //On server, our Enum for rejectionsList starts with "1.

        private void OnEnable()
        {
            ResetScreen();
            QuestionCreationRequestHandler.OnRandomQuestionReceived += GetDTO;
            ReviewReasonButton.OnReasonPicked += ToggleSaveAndExitButton;
            ReviewReasonButton.OnReasonPicked += SaveSelectedReasons;
        }

        private void Start()
        {
            charsWarningText.text = $"Comment must have at least {otherReason_minChars} characters.";
            mainContent.SetActive(false);
        }

        private void OnDisable()
        {
            ReviewReasonButton.OnReasonPicked -= ToggleSaveAndExitButton;
            ReviewReasonButton.OnReasonPicked -= SaveSelectedReasons;
        }

        private void GetDTO(QuestionReviewDTO dto)
        {
            questionDTO = dto;
        }

        public void ResetScreen()
        {
            otherReasonContainer.SetActive(false);
            charsWarningText.gameObject.SetActive(false);
            ToggleSaveAndExitButton();
            OtherReason_ToggleSaveButton();
        }

        private bool WasReasonPicked()
        {
            foreach (var button in reviewReasonButtons)
            {
                if (button.IsSelected == true)
                {
                    return true;
                }
            }

            return false;
        }

        public void ToggleSaveAndExitButton()
        {
            if (WasReasonPicked())
            {
                saveAndExitButtonCanvasGrp.blocksRaycasts = true;
                saveAndExitButtonCanvasGrp.alpha = 1;
            }
            else
            {
                saveAndExitButtonCanvasGrp.blocksRaycasts = false;
                saveAndExitButtonCanvasGrp.alpha = nonInteractableAlpha;
            }
        }

        public void OtherReason_ToggleSaveButton()
        {
            if (otherReason_inputField.text.Length >= otherReason_minChars)
            {
                otherReason_SaveButtonCanvasGrp.blocksRaycasts = true;
                otherReason_SaveButtonCanvasGrp.alpha = 1;
            }
            else
            {
                otherReason_SaveButtonCanvasGrp.blocksRaycasts = false;
                otherReason_SaveButtonCanvasGrp.alpha = nonInteractableAlpha;
            }
        }

        public void ToggleCharsWarning()
        {
            charsWarningText.gameObject.SetActive(otherReason_inputField.text.Length < otherReason_minChars);
        }

        public void ClearInputField()
        {
            otherReason_inputField.text = "";
        }

        private void SaveSelectedReasons()
        {
            reasonsList.Clear();

            for (int i = 0; i < reviewReasonButtons.Length; i++)
            {
                if (reviewReasonButtons[i].IsSelected)
                {
                    reasonsList.Add(i+server_EnumFirstValue);
                }
            }

            string listContent = string.Join(", ", reasonsList);
            Debug.LogWarning($"Current list is: {listContent}.");
        }

        public void ReportQuestion()
        {
            ClearInputField();
            otherReasonContainer.SetActive(false);
            OnQuestionReported?.Invoke(questionDTO.QuestionReviewID.ToString(), reasonsList, otherReason_inputField.text);
            OnSaveAndExitReportScreen?.Invoke();
            ReviewAQuestionScreen.OnReadyToReviewQuestion?.Invoke();
        }
    }
}