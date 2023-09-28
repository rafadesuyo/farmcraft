using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCategoryScreen : MonoBehaviour
{
    [SerializeField] private CategoryDatabaseSO categoryDatabase;
    [SerializeField] private GameObject categorySelectItemPrefab;
    [SerializeField] private Transform itensParent;

    private void Start()
    {
        foreach (QuizCategory category in Enum.GetValues(typeof(QuizCategory)))
        {
            if (IsValidCategory(category))
            {
                GameObject instantiatedCategorySelectItem = Instantiate(categorySelectItemPrefab, itensParent);
                instantiatedCategorySelectItem.GetComponent<QuestionCategorySelectItem>().BuildItem(category, categoryDatabase.GetIconByCategory(category));
            }
        }
    }

    private bool IsValidCategory(QuizCategory category)
    {
        return category != QuizCategory.Random && 
               category != QuizCategory.None;
    }
}
