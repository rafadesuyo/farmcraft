using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class QuestionCategorySelectItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI categoryTitleText;
    [SerializeField] Image iconImage;
    QuizCategory itemCategory;
    Sprite itemSprite;

    public static event Action<QuizCategory, Sprite> OnCategorySelected;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(SelectItem);
    }

    public void BuildItem(QuizCategory category, Sprite itemSprite)
    {
        itemCategory = category;
        this.itemSprite = itemSprite;
        iconImage.sprite = itemSprite;
        categoryTitleText.text = ProjectAssetsDatabase.Instance.GetCategoryName(category);
    }

    private void SelectItem()
    {
        OnCategorySelected?.Invoke(itemCategory,itemSprite);
    }
}
