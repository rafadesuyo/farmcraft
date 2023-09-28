using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Collectibles/Collectible List")]
public class CollectibleListSO : ScriptableObject
{
    //Variables
    [SerializeField] private List<CollectibleSO> collectibles = new List<CollectibleSO>();

    //Getters
    public List<CollectibleSO> Collectibles => collectibles;

    public CollectibleSO GetCollectibleByType(CollectibleType type)
    {
        foreach (CollectibleSO collectible in collectibles)
        {
            if (collectible.Type == type)
            {
                return collectible;
            }
        }

        return null;
    }
}
