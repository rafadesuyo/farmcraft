using System;

[Serializable]
public class CollectibleProgress
{
    public int level = 0;
    public int shards = 0;
    public bool isUnlocked = false;

    // This can be done using only shards as parameter, but requires some structure changes
    // Since the currentShards are the value between 0 and shardsToNextLevel, not the total amout of shards.
    public CollectibleProgress(int currentLevel, int currentShards, bool isUnlocked)
    {
        level = currentLevel;
        shards = currentShards;
        this.isUnlocked = isUnlocked;
    }

    public CollectibleProgress() { }
}
