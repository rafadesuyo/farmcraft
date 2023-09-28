using UnityEngine;
using System.Collections.Generic;

public class StageAbilityPreviewUI : MonoBehaviour
{
    [SerializeField] private RectTransform abilitiesPreviewContainer = null;
    [SerializeField] private GameObject abilityPreviewPrefab = null;

    public void Setup(List<CollectibleAbility> collectibleAbilityList)
    {
        GenericPool.CreatePool<AbilityInfoItem>(abilityPreviewPrefab, abilitiesPreviewContainer);
        PopulateAbilites(collectibleAbilityList);
    }

    private void PopulateAbilites(List<CollectibleAbility> collectibleAbilityList)
    {
        ReleaseAbilityInfos();

        foreach (CollectibleAbility collectibleAbility in collectibleAbilityList)
        {
            var abilityItem = GenericPool.GetItem<AbilityInfoItem>();
            abilityItem.Setup(collectibleAbility);
        }
    }

    private void ReleaseAbilityInfos()
    {
        AbilityInfoItem[] abilityInfoItems = abilitiesPreviewContainer.GetComponentsInChildren<AbilityInfoItem>();

        foreach (AbilityInfoItem abilityInfoItem in abilityInfoItems)
        {
            abilityInfoItem.Release();
        }
    }
}
