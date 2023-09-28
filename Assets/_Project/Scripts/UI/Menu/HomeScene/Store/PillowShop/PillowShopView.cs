using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowShopView : MenuView
{
    [SerializeField] private StoreSO storeData;
    [SerializeField] private GameObject pillowShopItemPrefab = null;
    [SerializeField] private RectTransform pillowItemsContainer = null;

    private bool pillowContainerPopulated = false;
    public override Menu Type => Menu.PillowShop;

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        GenericPool.CreatePool<PillowShopItem>(pillowShopItemPrefab, pillowItemsContainer);
        PopulateItems();
    }

    private void PopulateItems()
    {
        if (pillowContainerPopulated)
        {
            foreach (Transform child in pillowItemsContainer.transform)
            {
                child.gameObject.SetActive(true);
            }
            return;
        }

        List<StoreItemSO> itemsToShow = storeData.itemsToSell.FindAll(i => i.Section == ItemType.Pillows);

        for (int i = 0; i < itemsToShow.Count; i++)
        {
            var item = GenericPool.GetItem<PillowShopItem>();
            item.transform.SetAsLastSibling();
            item.Setup(itemsToShow[i]);
            item.transform.SetParent(pillowItemsContainer, false);
        }

        pillowContainerPopulated = true;
    }

    public void Show()
    {
        CanvasManager.Instance.OpenMenu(Menu.PillowShop);
    }
}
