using DreamQuiz;

[System.Serializable]
public class MenuSetupOptions
{
    public AbilityDataSO[] abilities;
    public CollectibleType collectibleType;
    public Collectible collectible;
    public StageInfoSO stageData;
    public ItemType storeSection;
    public NodeBase nodeData;
    public System.Action<int> onAnswerCallback = null;
    public int shards;
    public StoreItem storeItem;
    public LootboxItem lootboxItem;

    public MenuSetupOptions(AbilityDataSO[] setupAbilities)
    {
        abilities = setupAbilities;
    }

    public MenuSetupOptions(CollectibleType setupCollectibleType)
    {
        collectibleType = setupCollectibleType;
    }

    public MenuSetupOptions(Collectible setupCollectible)
    {
        collectible = setupCollectible;
    }

    public MenuSetupOptions(StageInfoSO setupStageData)
    {
        stageData = setupStageData;
    }

    public MenuSetupOptions(ItemType setupSection)
    {
        storeSection = setupSection;
    }

    public MenuSetupOptions(NodeBase setupNodeData, System.Action<int> setupOnAnswerCallback = null)
    {
        nodeData = setupNodeData;
        onAnswerCallback = setupOnAnswerCallback;
    }

    public MenuSetupOptions(Collectible setupCollectible, int setupShards)
    {
        collectible = setupCollectible;
        shards = setupShards;
    }

    public MenuSetupOptions(StoreItem setupStoreItem)
    {
        storeItem = setupStoreItem;
    }

    public MenuSetupOptions(LootboxItem setupLootboxItem)
    {
        lootboxItem = setupLootboxItem;
    }
}
