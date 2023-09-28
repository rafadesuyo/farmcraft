using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TeamSelectionItem : MonoBehaviour
{
    [SerializeField] private Image iconImg = null;
    [SerializeField] private Image categoryImg = null;
    [SerializeField] private TextMeshProUGUI nameText = null;
    [SerializeField] private Button selectBtn = null;

    public void Setup(CollectibleType type, Action<CollectibleType> onSelect)
    {
        var collectibleData = CollectibleManager.Instance.GetCollectibleDataByType(type);
        categoryImg.sprite = ProjectAssetsDatabase.Instance.GetCategoryIcon(collectibleData.Category);
        iconImg.sprite = collectibleData.Icon;
        nameText.text = collectibleData.Name;

        selectBtn.interactable = false;
        selectBtn.onClick.RemoveAllListeners();
        selectBtn.onClick.AddListener(() => onSelect(type));

        UpdateSelection(type);
    }

    private void UpdateSelection(CollectibleType type)
    {
        if (CollectibleManager.Instance.IsCollectibleUnlocked(type))
        {
            if (CollectibleManager.Instance.CanAddCollectibleToCurrentTeam(type))
            {
                iconImg.color = Color.white;
                categoryImg.color = Color.white;
                selectBtn.interactable = true;
            }
            else
            {
                iconImg.color = Color.grey;
                categoryImg.color = Color.grey;
            }
        }
        else
        {
            nameText.text = "????";
            iconImg.color = Color.black;
            categoryImg.color = Color.black;
        }
    }

    private void OnDisable()
    {
        this.ReleaseItem();
    }
}
