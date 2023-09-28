using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPreviewAbilitySection : MonoBehaviour
{
    [SerializeField] private Transform abilityContainer = null;
    [SerializeField] private GameObject abilityItemPrefab = null;

    private List<TeamPreviewAbilityItem> currentAbilities = new List<TeamPreviewAbilityItem>();

    public void Setup(List<CollectibleAbility> collectibleAbilityList)
    {
        ClearList();
        GenericPool.CreatePool<TeamPreviewAbilityItem>(abilityItemPrefab, abilityContainer);

        foreach (CollectibleAbility collectibleAbility in collectibleAbilityList)
        {
            var abilityItem = GenericPool.GetItem<TeamPreviewAbilityItem>();
            abilityItem.transform.SetParent(abilityContainer);

            abilityItem.Setup(collectibleAbility);
            currentAbilities.Add(abilityItem);
        }
    }

    private void ClearList()
    {
        foreach (TeamPreviewAbilityItem abilityItem in currentAbilities)
        {
            this.ReleaseItem();
        }

        currentAbilities.Clear();
    }
}
