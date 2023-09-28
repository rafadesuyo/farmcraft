using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CollectionItem : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image collectibleImage;
    [SerializeField] private Outline outline;
    [SerializeField] private CollectibleLevelProgressHandler collectibleLevelHandler;

    //Variables
    private CollectibleType collectibleType;
    private bool canSelectThisCollectible = false;
    private Action<CollectionItem> selectCallback = null;

    //Getters
    public CollectibleType CollectibleType => collectibleType;

    public void Setup(CollectibleSO collectibleData, Action<CollectionItem> onSelectCallback)
    {
        selectCallback = onSelectCallback;

        collectibleType = collectibleData.Type;

        backgroundImage.color = ProjectAssetsDatabase.Instance.GetCategoryColor(collectibleData.Category);

        collectibleImage.sprite = collectibleData.PortraitColored;

        UpdateCollectionDisplay(collectibleData);

        var collectible = CollectibleManager.Instance.GetCollectibleByType(collectibleType);

        // Null = no progress.
        if (collectible != null)
        {
            collectibleLevelHandler.SetupLevel(collectible.CurrentLevel);
            return;
        }

        collectibleLevelHandler.SetupLevel(0);
    }

    public void UpdateBorder(float size, Color color)
    {
        outline.effectColor = color;
        outline.effectDistance = new Vector2(size, size);
    }

    public void Select()
    {
        if (!canSelectThisCollectible)
        {
            return;
        }

        selectCallback?.Invoke(this);
    }

    private void UpdateCollectionDisplay(CollectibleSO collectibleData)
    {
        bool isCollectibleUnlocked = CollectibleManager.Instance.IsCollectibleUnlocked(collectibleType);
        canSelectThisCollectible = isCollectibleUnlocked;

        if (isCollectibleUnlocked)
        {
            collectibleImage.sprite = collectibleData.PortraitColored;
        }
        else
        {
            collectibleImage.sprite = collectibleData.PortraitBlackWhite;
        }
    }

    private void OnDisable()
    {
        this.ReleaseItem();
    }
}
