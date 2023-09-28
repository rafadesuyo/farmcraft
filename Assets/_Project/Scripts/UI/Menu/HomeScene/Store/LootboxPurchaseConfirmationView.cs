using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum LootboxType
{
    Common,
    Rare,
    Legendary,
    Arts
}

public class LootboxPurchaseConfirmationView : MenuView
{
    //Components
    [Header("Components")]
    [SerializeField] private Image itemIconImg;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemValueTxt;
    [SerializeField] private TextMeshProUGUI itemPriceTxt;

    //Variables
    private LootboxItem currentStoreItem;

    //Getters
    public override Menu Type => Menu.LootboxPurchaseConfirmationView;

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        currentStoreItem = setupOptions.lootboxItem;

        UpdateVariables();
    }

    private void UpdateVariables()
    {
        StoreItemSO storeItemSO = currentStoreItem.Item;

        itemIconImg.sprite = storeItemSO.Icon;
        itemName.text = $"{storeItemSO.StoreName}".ToUpper();
        itemValueTxt.text = $"x{storeItemSO.Quantity}".ToUpper();
        itemPriceTxt.text = $"{storeItemSO.Price}";
    }

    public void ConfirmPurchase()
    {
        StoreItemSO storeItem = currentStoreItem.Item;
        CanvasManager.Instance.ReturnMenu(); // Close the menu before confirming the purchase.

        PlayerProgress.SavingPurchaseCount(storeItem);
        currentStoreItem.UpdateInfoHandler();

        // Deduct the appropriate currency based on the item's currency type
        if (PlayerProgress.TotalOneekoin >= storeItem.Price)
        {
            PlayerProgress.TotalOneekoin -= storeItem.Price;
        }

        AddLootbooxItems(currentStoreItem.LootboxType);

        GameManager.Instance.SaveGame();
    }

    private void AddLootbooxItems(LootboxType lootboxType)
    {
        HeartManager.Instance.AddHearts(currentStoreItem.Item.Quantity, true); ;
        CollectibleManager.Instance.GiveRandomShardToRandomCollectible(lootboxType);
    }
}
