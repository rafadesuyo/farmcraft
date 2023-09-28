using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreItem : MonoBehaviour
{
    private const int MINIMUM_RANGE = 5;
    private const int MAXIMUM_RANGE = 15;

    [Header("Components")]
    [SerializeField] private Image iconImg = null;
    [SerializeField] private ItemType type;
    [SerializeField] private TextMeshProUGUI valueTxt = null;
    [SerializeField] private Button buyBtn = null;
    [SerializeField] private TextMeshProUGUI buyButtonText = null;
    [SerializeField] private Slider shardProgressSlider;
    [SerializeField] private TextMeshProUGUI shardsCountText;
    [SerializeField] public GameObject shardsContainer;
    [SerializeField] private TextMeshProUGUI purchaseLimitText;
    [SerializeField] private Image currencyIcon = null;
    [SerializeField] private int itemId;
    [SerializeField] private StoreSO storeData;
    private bool hasRandomItem = false;
    private bool isAlreadySetup = false;
    private StoreItemSO item;

    public StoreItemSO Item => item;

    private void OnEnable()
    {
        EventsManager.AddListener(EventsManager.onGoldChange, UpdateGoldButtonInteractivity);
        GetItemIdAndSetup();
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

        if (item.Section == ItemType.Shards)
        {
            SetupShardsItem();
        }
        else
        {
            shardsContainer.SetActive(false);
        }

        UpdateInfoHandler();
    }

    private void SetupShardsItem()
    {
        UpdateShardsSliderAndText();
        SetupRandomQuantity();
    }

    private void SetupRandomQuantity()
    {
        if (item.RandomQuantity == 0)
        {
            item.RandomQuantity = PlayerProgress.LoadingRandomQuantity(item);
            item.Quantity = item.RandomQuantity;

            if (item.RandomQuantity == 0)
            {
                item.RandomQuantity = Random.Range(MINIMUM_RANGE, MAXIMUM_RANGE);
                PlayerProgress.SavingRandomQuantity(item, item.RandomQuantity);
                item.Quantity = item.RandomQuantity;
            }
        }

        valueTxt.text = $"x{item.RandomQuantity}";
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
        if (item.CurrencyType == CurrencyType.Gold)
        {
            buyBtn.interactable = PlayerProgress.TotalGold >= item.Price;
        }
        else if (item.CurrencyType == CurrencyType.Oneekoin)
        {
            buyBtn.interactable = PlayerProgress.TotalOneekoin >= item.Price;
        }
    }

    public void UpdateInfoHandler()
    {
        UpdatePrice();
        UpdatePurchaseLimitText();
        UpdateGoldButtonInteractivity();
        UpdateShardsSliderAndText();
    }

    public void UpdateShardsSliderAndText()
    {
        CollectibleType shardType = item.ShardType;
        Collectible collectible = CollectibleManager.Instance.GetCollectibleByType(shardType);

        if (collectible != null)
        {
            shardsContainer.SetActive(true);
            float progress = Mathf.InverseLerp(0, collectible.ShardsToNextLevel, collectible.CurrentShards);
            shardProgressSlider.value = progress;
            shardsCountText.text = collectible.IsMaxLevel ? "MAX LEVEL" : $"{collectible.CurrentShards}/{collectible.ShardsToNextLevel}";
        }
        else if (collectible == null)
        {
            shardsContainer.SetActive(false);
        }
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
            CanvasManager.Instance.OpenMenu(Menu.PurchaseConfirmation, new MenuSetupOptions(this));
            AudioManager.Instance.Play("Button");
        }
        else
        {
            Debug.Log("Limit Reached");
        }
    }

    private void GetItemIdAndSetup()
    {
        Setup(itemId);
    }
}