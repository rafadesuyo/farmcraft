using UnityEngine;
using System;

public class TeamSelectionUI : MonoBehaviour
{
    [SerializeField] private Transform listContainer = null;
    [SerializeField] private GameObject listItemPrefab = null;

    private Action<CollectibleType> onSelectCallback = null;

    public void Setup(Action<CollectibleType> newSelectCallback)
    {
        PopulateCollectibles();
        onSelectCallback = newSelectCallback;
    }

    private void PopulateCollectibles()
    {
        GenericPool.CreatePool<TeamSelectionItem>(listItemPrefab, listContainer);

        foreach (CollectibleSO collectible in CollectibleManager.Instance.CollectiblesData)
        {
            TeamSelectionItem listItem = GenericPool.GetItem<TeamSelectionItem>();
            listItem.Setup(collectible.Type, OnSelect);
        }
    }

    private void OnSelect(CollectibleType type)
    {
        onSelectCallback?.Invoke(type);
    }
}
