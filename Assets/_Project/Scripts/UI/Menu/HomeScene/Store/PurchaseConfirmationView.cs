using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseConfirmationView : MenuView
{
    //Components
    [Header("Components")]
    [SerializeField] private Image itemIconImg;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemValueTxt;
    [SerializeField] private TextMeshProUGUI itemPriceTxt;

    //Variables
    private StoreItem currentStoreItem;
    
    //Getters
    public override Menu Type => Menu.PurchaseConfirmation;

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        currentStoreItem = setupOptions.storeItem;

        UpdateVariables();
    }

    private void UpdateVariables()
    {
        StoreItemSO storeItemSO = currentStoreItem.Item;
        
        itemIconImg.sprite = storeItemSO.Icon;
        itemName.text = $"{storeItemSO.StoreName}".ToUpper();
        if (currentStoreItem != null)
        {
            if (currentStoreItem.Item.Section == ItemType.Shards)
            {
                itemValueTxt.text = $"x{storeItemSO.RandomQuantity} {storeItemSO.Section}".ToUpper();
            }
            else if (currentStoreItem.Item.Section == ItemType.Hearts)
            {
                itemValueTxt.text = $"x{storeItemSO.Quantity}";
            }
        }

        itemPriceTxt.text = $"{storeItemSO.Price}";
    }

    public void ConfirmPurchase()
    {
        StoreItemSO storeItem = currentStoreItem.Item;
        CanvasManager.Instance.ReturnMenu();

        PlayerProgress.SavingPurchaseCount(storeItem);
        currentStoreItem.UpdateInfoHandler();

        switch (storeItem.CurrencyType)
        {
            case CurrencyType.Gold:
                if (PlayerProgress.TotalGold >= storeItem.Price)
                {
                    PlayerProgress.TotalGold -= storeItem.Price;
                }
                else
                {
                    // TODO: implement insuficient currency popup https://ocarinastudios.atlassian.net/browse/DQG-2026
                    return;
                }
                break;
            case CurrencyType.Oneekoin:
                if (PlayerProgress.TotalOneekoin >= storeItem.Price)
                {
                    PlayerProgress.TotalOneekoin -= storeItem.Price;
                }
                else
                {
                    // TODO: implement insuficient currency popup https://ocarinastudios.atlassian.net/browse/DQG-2026
                    return;
                }
                break;
            default:  
                Debug.LogError($"Unhandled currency type");
                return;
        }

        if (storeItem.Section == ItemType.Hearts)
        {
            HeartManager.Instance.AddHearts(storeItem.Quantity, true);
        }
        else if (storeItem.Section == ItemType.Shards)
        {
            CollectibleManager.Instance.GiveShardsTo(storeItem.ShardType, storeItem.Quantity, true);
            if (!currentStoreItem.shardsContainer.activeInHierarchy)
            {
                currentStoreItem.shardsContainer.SetActive(true);
                currentStoreItem.UpdateShardsSliderAndText();
            }
            currentStoreItem.UpdateShardsSliderAndText();
        }

        GameManager.Instance.SaveGame();
    }
}
