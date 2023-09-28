using System;

[Serializable]
public class CollectibleSaveData
{
    public CollectibleType type;
    public CollectibleProgress progress;

    public CollectibleSaveData(CollectibleType newType, CollectibleProgress newProgress)
    {
        type = newType;
        progress = newProgress;
    }
}
