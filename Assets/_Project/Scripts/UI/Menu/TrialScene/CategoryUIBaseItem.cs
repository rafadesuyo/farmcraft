using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CategoryUIBaseItem: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI categoryTitleText;
    [SerializeField] private int siblingIndexOffset;
    [SerializeField]protected QuizCategory quizCategory;

    public void SetCategory(QuizCategory quizCategory)
    {
        this.quizCategory = quizCategory;
    }

    protected void SetCategoriesNames()
    {
        int quizCategoriesCount = Enum.GetValues(typeof(QuizCategory)).Length;

        for (int i = 0; i < quizCategoriesCount; i++)
        {
            if (transform.GetSiblingIndex() - siblingIndexOffset == i)
            {
                categoryTitleText.text = ProjectAssetsDatabase.Instance.GetCategoryName(quizCategory);
            }
        }
    }

    protected void SetupItem()
    {
        SetCategoriesNames();
    }

}
