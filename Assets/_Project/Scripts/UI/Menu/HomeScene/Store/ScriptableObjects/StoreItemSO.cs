using UnityEngine;

[CreateAssetMenu(fileName = "New StoreItem", menuName = "Store/StoreItem")]
public class StoreItemSO : ScriptableObject
{
    [SerializeField] private int itemId = 0;
    [SerializeField] private CollectibleType shardType = CollectibleType.None;
    [SerializeField] private ItemType section = ItemType.Hearts;
    [SerializeField] private CurrencyType currenctyType = CurrencyType.Gold;
    [SerializeField] private Sprite icon = null;
    [SerializeField] private int quantity = 0;
    [SerializeField] private string storeName = "";
    [SerializeField] private int purchaseCount = 0;
    [SerializeField] private int purchaseLimit = 5;
    [SerializeField] private int randomQuantity = 0;
    public CollectibleType ShardType => shardType;
    public ItemType Section => section;
    public CurrencyType CurrencyType => currenctyType;
    public Sprite Icon => icon;
    public string StoreName => storeName;
    public int ItemId => itemId;
    public int PurchaseLimit
    {
        get
        {
            return purchaseLimit;
        }
        set 
        { 
            purchaseLimit = value;
        }
    }
    public int Price
    {
        get
        {
            return GameDifficultyHandler.Instance.GetItemPrice(section, Quantity);
        }
    }
    public int PurchaseCount
    {
        get
        {
            return purchaseCount;
        }
        set
        {
            purchaseCount = value;
        }
    }
    public int Quantity
    {
        get
        {
            return quantity;
        }
        set
        {
            quantity = value;
        }
    }
    public int RandomQuantity
    {
        get => randomQuantity;
        set => randomQuantity = value;
    }

    public bool CanBePurchased
    {
        get
        {
            return PurchaseCount < PurchaseLimit;
        }
    }
}

