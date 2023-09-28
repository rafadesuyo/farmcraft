using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DreamQuiz
{
    public class QuestionCreationFormatter
    {
        private const string allowedCharacters = ",.;?!-_";

        public QuestionCreationDTO FormatQuestion(QuestionModel question, string categoryID = null)
        {
            QuestionCreationDTO dto = new QuestionCreationDTO();
            dto.QuestionText = question.Title;

            if (string.IsNullOrEmpty(categoryID))
            {
                dto.CategoryID = QuestionDatabaseManager.Instance.GetCategoryId(question.Category);
            }
            else
            {
                dto.CategoryID = categoryID;
            }

            dto.Language = 0;

            PopulateAnswerMapDictionary(question, dto);

            dto.CorrectAnswerKey = question.CorrectIndex;

            if (HasSpecialCharacters(dto) == false)
            {
                return dto;
            }

            Debug.LogWarning("UNABLE TO FORMAT! One or more fields are using special characters.");
            return null;
        }

        private static void PopulateAnswerMapDictionary(QuestionModel question, QuestionCreationDTO dto)
        {
            Dictionary<string, string> answerMap = new Dictionary<string, string>();

            for (int i = 0; i < question.Answers.Count; i++)
            {
                answerMap.Add((i + 1).ToString(), question.Answers[i]);
            }
            dto.AnswerMap = answerMap;
        }

        private bool HasSpecialCharacters(QuestionCreationDTO dto)
        {

            if (dto.QuestionText.Any(c => !char.IsLetterOrDigit(c) && c != ' ' && !allowedCharacters.Contains(c)))
            {
                return true;
            }

            foreach (var item in dto.AnswerMap)
            {
                string itemKey = item.Key;

                if (itemKey.Any(c => !char.IsLetterOrDigit(c) && c != ' ' && !allowedCharacters.Contains(c)))
                {
                    Debug.LogWarning(itemKey);
                    return true;
                }

                string itemValue = item.Value;
                if (itemValue.Any(c => !char.IsLetterOrDigit(c) && c != ' ' && !allowedCharacters.Contains(c)))
                {
                    Debug.LogWarning(itemValue);
                    return true;
                }
            }

            return false;
        }

        public QuizCategory GetCategoryByID(string id)
        {
            return QuestionDatabaseManager.Instance.GetCategoryByID(id);
        }
    }
}

