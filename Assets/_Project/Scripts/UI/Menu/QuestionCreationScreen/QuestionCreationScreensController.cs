using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionCreationScreensController : BaseQuestionScreensController
{
    private void OnEnable()
    {
        QuestionCategorySelectItem.OnCategorySelected += ShowQuestionAnswersScreen;
    }

    private void OnDisable()
    {
        QuestionCategorySelectItem.OnCategorySelected -= ShowQuestionAnswersScreen;
    }

    private void ShowQuestionAnswersScreen(QuizCategory category, Sprite iconSprite)
    {
        ShowScreen(screens.Length-1);
    }
}
