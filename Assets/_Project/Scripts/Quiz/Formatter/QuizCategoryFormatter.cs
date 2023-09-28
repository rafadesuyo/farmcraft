using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class QuizCategoryMaps
{
    private static Dictionary<QuizCategory, string> categoryMap = new Dictionary<QuizCategory, string>()
    {
        {
            QuizCategory.GeneralKnowledge, "general knowledge"
        },
        {
            QuizCategory.ArtsAndEntertainment, "arts and entertainment"
        },
        {
            QuizCategory.Science, "science"
        },
        {
            QuizCategory.Puzzles, "puzzles"
        },
        {
            QuizCategory.Math, "math"
        },
        {
            QuizCategory.HumanSciences, "human sciences"
        },
        {
            QuizCategory.Sports, "sports"
        }
    };

    public static QuizCategory GetQuizCategoryByCategoryName(string categoryName)
    {
        if (categoryMap.ContainsValue(categoryName))
        {
            return categoryMap.FirstOrDefault(c => c.Value == categoryName).Key;
        }

        Debug.LogError($"{categoryName} not found on map");

        return QuizCategory.Random;
    }

    public static string GetCategoryNameByQuizCategory(QuizCategory category)
    {
        return categoryMap[category];
    }

    public static QuizCategory[] GetAllCategories()
    {
        return categoryMap.Keys.ToArray();
    }
}
