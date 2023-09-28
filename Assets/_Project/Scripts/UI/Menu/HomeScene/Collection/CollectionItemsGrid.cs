using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionItemsGrid : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private RectTransform colletionItemsContainer;

    public void AddCollectionItemToGrid(CollectionItem collectionItem)
    {
        collectionItem.transform.SetParent(colletionItemsContainer);
        collectionItem.transform.SetAsLastSibling();
    }

    private void OnDisable()
    {
        this.ReleaseItem();
    }
}
