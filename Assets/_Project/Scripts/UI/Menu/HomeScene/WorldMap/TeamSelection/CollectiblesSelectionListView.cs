using System.Collections.Generic;
using UnityEngine;

//TODO: delete this class and its prefabs
public class CollectiblesSelectionListView : UIControllerAnimated
{
    //Components
    [Header("Components")]
    [SerializeField] private GameObject collectibleListItemBase;

    [Space(10)]

    [SerializeField] private RectTransform collectibleListContainer;

    //Events
    // public event CollectionItem.CollectibleEvent OnCollectibleSelected;

    //Variables
    [Header("Variables")]
    [SerializeField] private CollectibleListSO collectibleList;

    private List<CollectionItem> collectibleListItems = new List<CollectionItem>();

    protected override void OnAwake()
    {
        collectibleListItemBase.gameObject.SetActive(false);
    }

    protected override void OnOpen()
    {
        collectibleListContainer.anchoredPosition = Vector2.zero;

        // CreateCollectibles();
    }

    protected override void OnClose()
    {
        ResetCollectibles();
    }

    // ======== THIS SCRIPT WILL BE DELETED IN THE NEXT CLEANING ========
    // This is why I'm only commenting it out
    // private void CreateCollectibles()
    // {
    //     List<Collectible> unlockedCollectibles = PlayerProgress.Collectibles;

    //     for (int i = 0; i < unlockedCollectibles.Count; i++)
    //     {
    //         Collectible collectible = unlockedCollectibles[i];

    //         CollectionItem collectibleListItem = Instantiate(collectibleListItemBase, collectibleListContainer).GetComponent<CollectionItem>();
    //         collectibleListItem.gameObject.SetActive(true);

    //         collectibleListItem.Setup(collectible.Data, true);

    //         if (!PlayerProgress.IsCollectibleEquipped(collectible.Data.Type))
    //         {
    //             // TODO: Create new class for CollectibleSelectionItem or update CollectionItem
    //             // collectibleListItem.OnCollectibleSelected += CollectibleSelected;
    //             // collectibleListItem.SlotSelected(false);
    //         }
    //         else
    //         {
    //             // collectibleListItem.OnCollectibleSelected += EquippedCollectibleSelected;
    //             // collectibleListItem.SlotSelected(true);
    //         }

    //         collectibleListItems.Add(collectibleListItem);
    //     }
    // }

    private void ResetCollectibles()
    {
        foreach (CollectionItem collectibleListItem in collectibleListItems)
        {
            Destroy(collectibleListItem.gameObject);
        }

        collectibleListItems.Clear();
    }

    // private void CollectibleSelected(CollectibleSO collectible)
    // {
    //     OnCollectibleSelected?.Invoke(collectible);
    // }

    private void EquippedCollectibleSelected(CollectibleSO _)
    {
        Debug.Log("This collectible is already equipped!");
    }
}
