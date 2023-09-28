using UnityEngine;

[System.Serializable]
public class StoreContainer 
{
    public enum ContainerType
    {
        None,
        Hearts,
        Shards
    }

    public string containerName;
    public int itemId;
}
