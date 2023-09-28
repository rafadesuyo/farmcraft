using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CategoryData
{
    [SerializeField] private QuizCategory category;
    [SerializeField] private string name;
    [SerializeField] private Color color;
    [SerializeField] private Sprite icon;

    public QuizCategory Category => category;
    public string Name => name;
    public Color Color => color;
    public Sprite Icon => icon;
}

[CreateAssetMenu(menuName = "Database/CategoryDatabaseSO")]
public class CategoryDatabaseSO : ScriptableObject
{
    [SerializeField] private List<CategoryData> categoryDatabase = new List<CategoryData>();

    public Sprite GetIconByCategory(QuizCategory category)
    {
        if (TryGetCategoryDataOfType(category, out CategoryData categoryData))
        {
            return categoryData.Icon;
        }

        return null;
    }

    public Color GetColorByCategory(QuizCategory category)
    {
        if (TryGetCategoryDataOfType(category, out CategoryData categoryData))
        {
            return categoryData.Color;
        }

        return default;
    }

    public string GetNameByCategory(QuizCategory category)
    {
        if (TryGetCategoryDataOfType(category, out CategoryData categoryData))
        {
            return categoryData.Name;
        }

        return string.Empty;
    }

    private bool TryGetCategoryDataOfType(QuizCategory category, out CategoryData categoryData)
    {
        categoryData = categoryDatabase.Find(categoryData => categoryData.Category == category);

        return categoryData != null;
    }
}
