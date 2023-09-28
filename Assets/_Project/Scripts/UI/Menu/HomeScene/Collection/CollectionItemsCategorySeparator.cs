using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionItemsCategorySeparator : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI categoryNameText;

    public void Setup(QuizCategory category)
    {
        categoryNameText.text = ProjectAssetsDatabase.Instance.GetCategoryName(category);
    }

    private void OnDisable()
    {
        this.ReleaseItem();
    }
}
