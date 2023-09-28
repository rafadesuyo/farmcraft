using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootboxItem : MonoBehaviour
{
    private const int MINIMUM_RANGE = 5;
    private const int MAXIMUM_RANGE = 15;

    [Header("Components")]
    [SerializeField] private Image iconImg = null;
    [SerializeField] private TextMeshProUGUI valueTxt = null;
    [SerializeField] private Button buyBtn = null;
    [SerializeField] private TextMeshProUGUI buyButtonText = null;
    [SerializeField] private TextMeshProUGUI purchaseLimitText;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image currencyIcon = null;
    [SerializeField] private int itemId;
    [SerializeField] private LootboxType lootboxType;
    [SerializeField] private StoreSO storeData;
    private StoreItemSO item;

    public StoreItemSO Item => item;
    public LootboxType LootboxType => lootboxType;

    private void OnEnable()
    {
        EventsManager.AddListener(EventsManager.onGoldChange, UpdateGoldButtonInteractivity);
        Setup(itemId);
    }

    private void OnDisable()
    {
        EventsManager.RemoveListener(EventsManager.onGoldChange, UpdateGoldButtonInteractivity);
    }

    public void Setup(int newItemId, System.Action onBuyCallback = null)
    {
        itemId = newItemId;

        // Find the StoreItemSO by itemId
        StoreItemSO newItem = FindStoreItemById(newItemId);
        if (newItem == null)
        {
            Debug.LogError($"StoreItemSO with itemId {newItemId} not found.");
            return;
        }

        Debug.Log($"Setting up item: {newItem.StoreName}, itemId: {newItemId}");

        item = newItem;

        SetupVisuals();

        SetupBuyButton(onBuyCallback);

        GameManager.Instance.SaveGame();
    }

    private void SetupVisuals()
    {
        iconImg.sprite = item.Icon;

        if (item.CurrencyType == CurrencyType.Oneekoin)
        {
            currencyIcon.color = Color.green;
        }

        valueTxt.text = $"x{item.Quantity}";
        item.PurchaseCount = PlayerProgress.LoadingPurchaseCount(item);
        itemName.text = item.StoreName.ToUpper();

        UpdateInfoHandler();
    }

    private StoreItemSO FindStoreItemById(int id)
    {
        StoreItemSO foundItem = storeData.itemsToSell.Find(item => item.ItemId == id);
        if (foundItem == null)
        {
            foundItem = storeData.itemsToSell.Find(item => item.ItemId == id);
        }
        return foundItem;
    }

    private void SetupBuyButton(System.Action onBuyCallback)
    {
        buyBtn.onClick.RemoveAllListeners();
        onBuyCallback += Buy;
        buyBtn.onClick.AddListener(() => onBuyCallback?.Invoke());
    }

    private void UpdateGoldButtonInteractivity(IGameEvent gameEvent = null)
    {
        buyBtn.interactable = PlayerProgress.TotalGold >= item.Price;
    }

    public void UpdateInfoHandler()
    {
        UpdatePrice();
        UpdatePurchaseLimitText();
        UpdateGoldButtonInteractivity();
    }

    public void UpdatePrice()
    {
        int quantity = item.Quantity;
        int price = GameDifficultyHandler.Instance.GetItemPrice(item.Section, quantity);
        buyButtonText.text = $"{item.Price}";
    }

    public void UpdatePurchaseLimitText()
    {
        purchaseLimitText.text = $"{item.PurchaseCount}/{item.PurchaseLimit}";
    }

    private void Buy()
    {
        if (item.CanBePurchased)
        {
            CanvasManager.Instance.OpenMenu(Menu.LootboxPurchaseConfirmationView, new MenuSetupOptions(this));
            AudioManager.Instance.Play("Button");
        }
        else
        {
            Debug.Log("Limit Reached");
        }
    }
}
