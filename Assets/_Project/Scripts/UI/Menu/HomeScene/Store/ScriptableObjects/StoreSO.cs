using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StoreData", menuName = "Store/StoreData")]
public class StoreSO : ScriptableObject
{
    public List<StoreItemSO> itemsToSell = new List<StoreItemSO>();
}
