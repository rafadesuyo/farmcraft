using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType
{
    None,
    CatlynNoir,
    Charlotte,
    CraigNolan,
    DrDiverfish,
    Max,
    OrniRinco,
    SleepyBiscuit,
    Smartowl,
    Virgil,
    Careme,
    Astrish
}

public class CollectibleAbility
{
    private AbilityDataSO abilityDataSO;
    private int level = 0;

    public int RawLevel => level;

    public AbilityDataSO AbilityDataSO
    {
        get
        {
            return abilityDataSO;
        }
    }

    public int Level
    {
        get
        {
            if (IsUnlocked == true)
            {
                return level - abilityDataSO.UnlockLevel;
            }

            return 0;
        }
    }

    public bool IsUnlocked
    {
        get
        {
            return level >= abilityDataSO.UnlockLevel;
        }
    }

    public CollectibleAbility(AbilityDataSO abilityDataSO, int level = 0)
    {
        this.abilityDataSO = abilityDataSO;
        this.level = level;
    }

    public void SetLevel(int value)
    {
        level = value;
    }
}

[System.Serializable]
public class Collectible
{
    //Constants
    private const int defaultBaseLevel = 1;

    //Variables
    private CollectibleSO data;
    private List<CollectibleAbility> collectibleAbilities;

    private int currentShards;
    private int currentLevel;

    private bool isUnlocked;
    private int baseLevel;

    //Getters
    public CollectibleSO Data => data;
    public List<CollectibleAbility> CollectibleAbilities => collectibleAbilities;

    //TODO: https://ocarinastudios.atlassian.net/browse/DQG-2046
    public CollectibleAbility SelectableAbility { get => collectibleAbilities[0];}

    public int CurrentShards => currentShards;
    public int CurrentLevel => currentLevel;
    
    public bool IsUnlocked => isUnlocked;
    public int MaxLevel => data.MaxLevel;
    public bool IsMaxLevel => currentLevel >= data.MaxLevel;
    public bool HasEnoughShardsToLevelUp => currentShards >= ShardsToNextLevel;

    public int ShardsToNextLevel
    {
        get
        {
            if (currentLevel >= Data.MaxLevel)
            {
                return Data.ShardsRequiredPerLevel[CurrentLevel - 1];
            }

            return Data.ShardsRequiredPerLevel[CurrentLevel];
        }
    }

    public Collectible(CollectibleSO collectibleSO, CollectibleProgress newProgress, int newBaseLevel = defaultBaseLevel)
    {
        data = collectibleSO;
        currentShards = newProgress.shards;
        currentLevel = newProgress.level;

        isUnlocked = newProgress.isUnlocked;
        baseLevel = newBaseLevel;

        collectibleAbilities = new List<CollectibleAbility>();

        foreach (var abilityData in collectibleSO.AbilityDataList)
        {
            collectibleAbilities.Add(new CollectibleAbility(abilityData, currentLevel));
        }
    }

    public void OnReceiveShards(int shardCount)
    {
        // TODO: Define if shards of collectibles in the max level will be sold/farmable. Link: https://ocarinastudios.atlassian.net/browse/DQG-1860?atlOrigin=eyJpIjoiNWU4NDhhOWUzYTUzNGM0NTg3N2MzYzMzNjNjNWU5ZDQiLCJwIjoiaiJ9
        currentShards += shardCount;

        // Level up automatically
        if (isUnlocked == false && HasEnoughShardsToLevelUp == true)
        {
            Unlock();
        }
    }

    private void Unlock()
    {
        isUnlocked = true;

        currentShards -= ShardsToNextLevel;
        currentLevel = baseLevel;
        
        UpdateAbilityLevel();

        EventsManager.Publish(EventsManager.onUnlockCollectible);

    }

    public void LevelUp()
    {
        if(IsMaxLevel == true)
        {
            Debug.LogWarning("Collectible shouldn't level up past the max level!");
            return;
        }

        currentShards -= ShardsToNextLevel;
        currentLevel++;

        UpdateAbilityLevel();

        EventsManager.Publish(EventsManager.onLevelUpCollectible, new OnLevelUpCollectibleEvent(currentLevel));
    }

    private void UpdateAbilityLevel()
    {
        foreach (var collectibleAbility in collectibleAbilities)
        {
            collectibleAbility.SetLevel(CurrentLevel);
        }
    }
}
