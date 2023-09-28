using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : LocalSingleton<CollectibleManager>
{
    private const int MIN_COMMONBOX_SHARDS = 7;
    private const int MAX_COMMONBOX_SHARDS = 15;
    private const int MIN_RAREBOX_SHARDS = 10;
    private const int MAX_RAREBOX_SHARDS = 20;
    private const int MIN_LEGENDARYBOX_SHARDS = 15;
    private const int MAX_LEGENDARYBOX_SHARDS = 30;
    private const int MIN_CATEGORYBOX_SHARDS = 7;
    private const int MAX_CATEGORYBOX_SHARDS = 12;

    [SerializeField] private CollectibleListSO allCollectibles;
    private List<Collectible> colletiblesWithProgression = new List<Collectible>();

    public List<CollectibleSO> CollectiblesData
    {
        get
        {
            return allCollectibles.Collectibles;
        }
    }

    public List<Collectible> PlayerCollectibles
    {
        get
        {
            return colletiblesWithProgression;
        }
    }

    private void OnEnable()
    {
        SetupCollectibles();
    }

    public bool IsCollectibleUnlocked(CollectibleType collectibleType)
    {
        Collectible collectible = GetCollectibleByType(collectibleType);

        if (collectible == null)
        {
            return false;
        }

        return collectible.IsUnlocked;
    }

    public List<Collectible> GetCurrentCollectibleTeam()
    {
        List<Collectible> team = new List<Collectible>();

        foreach (CollectibleType collectibleType in GetCurrentTeam())
        {
            var collectible = GetCollectibleByType(collectibleType);

            if (collectible != null)
            {
                team.Add(collectible);
            }
        }

        return team;
    }

    public List<CollectibleType> GetCurrentTeam()
    {
        return PlayerProgress.CurrentTeam;
    }

    public bool CanSelectCollectible(CollectibleType type, bool isToTeam)
    {
        if (isToTeam)
        {
            return CanAddCollectibleToCurrentTeam(type);
        }
        else
        {
            return IsCollectibleUnlocked(type);
        }
    }

    public bool CanAddCollectibleToCurrentTeam(CollectibleType type)
    {
        return IsCollectibleUnlocked(type) && !IsCollectibleOnCurrentTeam(type);
    }

    public bool IsCollectibleOnCurrentTeam(CollectibleType type)
    {
        return PlayerProgress.CurrentTeam.Contains(type);
    }

    public void AddCollectibleToCurrentTeam(CollectibleType type)
    {
        ChangeCollectibleEquippedState(type, true);
    }

    public void RemoveCollectibleFromCurrentTeam(CollectibleType type)
    {
        ChangeCollectibleEquippedState(type, false);
    }

    public CollectibleSO GetCollectibleDataByType(CollectibleType collectibleType)
    {
        return allCollectibles.GetCollectibleByType(collectibleType);
    }

    public Collectible GetCollectibleByType(CollectibleType collectibleType)
    {
        return colletiblesWithProgression.Find(c => c.Data.Type == collectibleType);
    }

    public int GetCollectibleLevelByType(CollectibleType collectibleType)
    {
        Collectible collectible = GetCollectibleByType(collectibleType);

        if (collectible != null)
        {
            return collectible.CurrentLevel;
        }
        else
        {
            return 0;
        }
    }

    public void GiveShardsTo(CollectibleType collectibleType, int shardsCount, bool openShardsReceivedMenu = false)
    {
        Collectible collectible = GetCollectibleByType(collectibleType);

        if (collectible == null)
        {
            CollectibleSO collectibleData = GetCollectibleDataByType(collectibleType);

            collectible = new Collectible(collectibleData, new CollectibleProgress());
            colletiblesWithProgression.Add(collectible);
        }

        //TODO: review later, it would be best to remove this parameter
        if (openShardsReceivedMenu == true)
        {
            CanvasManager.Instance.OpenMenu(Menu.ShardsReceived, new MenuSetupOptions(collectible, shardsCount), false);
        }

        collectible.OnReceiveShards(shardsCount);
        GameManager.Instance.SaveGame();
    }

    public void GiveRandomShardToRandomCollectible(LootboxType lootboxType)
    {
        CollectibleType randomCollectibleType = (CollectibleType)UnityEngine.Random.Range(1, Enum.GetValues(typeof(CollectibleType)).Length);

        GiveRandomShardTo(randomCollectibleType, lootboxType);
    }

    public void GiveRandomShardTo(CollectibleType collectibleType, LootboxType lootboxType)
    {
        Collectible collectible = GetCollectibleByType(collectibleType);

        if (collectible == null)
        {
            CollectibleSO collectibleData = GetCollectibleDataByType(collectibleType);

            collectible = new Collectible(collectibleData, new CollectibleProgress());
            colletiblesWithProgression.Add(collectible);
        }

        int shardsCount = GetRandomShardCount(lootboxType); // TODO: Implement this method to correctly give correct shards(today is giving wrong for example charlotte shards) https://ocarinastudios.atlassian.net/browse/DQG-2026

        CanvasManager.Instance.OpenMenu(Menu.ShardsReceived, new MenuSetupOptions(collectible, shardsCount), false);

        collectible.OnReceiveShards(shardsCount);
        GameManager.Instance.SaveGame();
    }

    private int GetRandomShardCount(LootboxType lootboxType)
    {
        int minShards;
        int maxShards;

        switch (lootboxType)
        {
            case LootboxType.Common:
                minShards = MIN_COMMONBOX_SHARDS;
                maxShards = MAX_COMMONBOX_SHARDS;
                break;
            case LootboxType.Rare:
                minShards = MIN_RAREBOX_SHARDS;
                maxShards = MAX_RAREBOX_SHARDS;
                break;
            case LootboxType.Legendary:
                minShards = MIN_LEGENDARYBOX_SHARDS;
                maxShards = MAX_LEGENDARYBOX_SHARDS;
                break;
            case LootboxType.Arts:
                minShards = MIN_CATEGORYBOX_SHARDS;
                maxShards = MAX_CATEGORYBOX_SHARDS;
                break;
            default:
                minShards = MIN_COMMONBOX_SHARDS;
                maxShards = MAX_COMMONBOX_SHARDS;
                break;
        }

        return UnityEngine.Random.Range(minShards, maxShards);
    }


    public void SetupCollectibles()
    {
        colletiblesWithProgression.Clear();

        // TODO: Review how "first time setup" should work.
        // https://ocarinastudios.atlassian.net/browse/DQG-881?atlOrigin=eyJpIjoiNmQwYjZmNTU3YjlkNDIyNjgzMzA5MTM2OGE0NGJmOTIiLCJwIjoiaiJ9
        if (PlayerProgress.SaveState.hasSaveData)
        {
            SetupSavedCollectibles();
            // CheckForCollectibleLevelUp();
        }
        else
        {
            // Unlock Virgil in the first run of the game
            GiveShardsTo(CollectibleType.Virgil, 0);
        }

        // #if UNITY_EDITOR
        //         // Only for testing
        //         foreach (CollectibleSO collectibleData in CollectiblesData)
        //         {
        //             GiveShardsTo(collectibleData.Type, 20);
        //         }
        // #endif
    }

    // Load saved collectibles.
    private void SetupSavedCollectibles()
    {
        // FIXME: The behavior were changed for the MVP. Review after definitions about collectible and store flow.
        // https://ocarinastudios.atlassian.net/browse/DQG-896?atlOrigin=eyJpIjoiNjNlMGNhMWUyOGQ2NGJjNGI4ZGJkNzQ3MzJlYmY5YWMiLCJwIjoiaiJ9
        // List<CollectibleSO> collectiblesNotSaved = new List<CollectibleSO>(CollectiblesData);

        foreach (CollectibleSaveData savedCollectible in PlayerProgress.CollectiblesSaved)
        {
            CollectibleSO collectibleData = GetCollectibleDataByType(savedCollectible.type);
            Collectible collectible = new Collectible(collectibleData, savedCollectible.progress);
            colletiblesWithProgression.Add(collectible);

            // collectiblesNotSaved.Remove(collectibleData);
        }

        // Check if a given collectible should be unlocked without receiving shards.
        // CheckForFreeCollectibles(collectiblesNotSaved);
    }

    // private void CheckForFreeCollectibles(List<CollectibleSO> collectiblesData)
    // {
    //     foreach (CollectibleSO data in collectiblesData)
    //     {
    //         if (data.ShardsRequiredPerLevel[0] == 0)
    //         {
    //             GiveShardsTo(data.Type, 0);
    //         }
    //     }
    // }

    // Check if a given collectible should level up without receiving shards.
    // This will unlock a collectible in a scenario where level up values are changed and the player updates the game.
    // private void CheckForCollectibleLevelUp()
    // {
    //     // Force every collectible to execute receive shards logic. 
    //     // If the collectible should level up with the current number of shards, 
    //     // every related behavior will also correctly be updated. For instance, update abilities.
    //     foreach (Collectible collectible in PlayerCollectibles)
    //     {
    //         // Since we are not unlocking a new collectible, it's not necessary to call GiveShardsTo. We can 
    //         // directly call OnReceiveShards in the collectible's object.
    //         collectible.OnReceiveShards(0);
    //     }
    // }

    private void ChangeCollectibleEquippedState(CollectibleType type, bool equip)
    {
        if (equip)
        {
            // Equipped collectibles can't be equipped again, so no checks needed here.
            PlayerProgress.CurrentTeam.Add(type);
            return;
        }

        PlayerProgress.CurrentTeam.Remove(type);
    }
}
