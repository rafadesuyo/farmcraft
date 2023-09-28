using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameDifficulty
{
    Normal,
    Hard
}

[Serializable]
public class DifficultyValues
{
    public GameDifficulty difficulty = GameDifficulty.Normal;
    public int heartTime = 0;
    public int shardCost = 0;
    public int heartCost = 0;
    public int lootboxesCost = 150;
    public int pillowCost = 0;
}

public class GameDifficultyHandler : LocalSingleton<GameDifficultyHandler>
{
    [SerializeField] private List<DifficultyValues> difficultyValues = new List<DifficultyValues>();
    private GameDifficulty currentDifficulty = GameDifficulty.Hard;

    public void ChangeDifficulty(GameDifficulty difficulty)
    {
        currentDifficulty = difficulty;
    }
    
    public int GetLootboxCost()
    {
        return difficultyValues.Find(values => values.difficulty == currentDifficulty).lootboxesCost;
    }
    
    public int GetHeartTime()
    {
        return difficultyValues.Find(values => values.difficulty == currentDifficulty).heartTime;
    }

    public int GetShardCost()
    {
        return difficultyValues.Find(values => values.difficulty == currentDifficulty).shardCost;
    }

    public int GetPillowCost()
    {
        return difficultyValues.Find(values => values.difficulty == currentDifficulty).pillowCost;
    }

    public int GetHeartCost()
    {
        return difficultyValues.Find(values => values.difficulty == currentDifficulty).heartCost;
    }

    public int GetItemPrice(ItemType section, int quantity)
    {
        int basePrice = 0;

        if (section == ItemType.Shards)
        {
            basePrice = GetShardCost();
        }
        else if (section == ItemType.Hearts)
        {
            basePrice = GetHeartCost();
        }
        else if (section == ItemType.Lootboxes)
        {
            basePrice = GetLootboxCost();
        }
        else if (section == ItemType.Pillows)
        {
            basePrice = GetPillowCost();
        }

        return basePrice * quantity;
    }
}
