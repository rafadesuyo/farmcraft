using DreamQuiz.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StageCollectibleItem : MonoBehaviour
{
    [SerializeField] private Image collectionIconImg = null;
    [SerializeField] private StageCollectibleAbilityItem stageCollectibleAbilityItemList;
    private List<CollectibleAbility> collectibleAbilities;

    public void Setup(Collectible collectible, PlayerStageAbility playerStageAbility)
    {
        // This is a safe check, most likely will never trigger.
        if (collectible == null)
        {
            return;
        }

        collectionIconImg.sprite = collectible.Data.Icon;

        PopulateAbility(collectible.SelectableAbility, playerStageAbility);
    }

    public void ShowPassiveAbilitiesPreview()
    {
        List<CollectibleAbility> abilities = new List<CollectibleAbility>();

        foreach (var collectibleAbility in collectibleAbilities)
        {
            if (collectibleAbility.AbilityDataSO.IsPassiveUse)
            {
                abilities.Add(collectibleAbility);
            }
        }

        EventsManager.Publish(EventsManager.onSelectCollectibleAbility, new OnSelectCollectibleAbilityPreviewEvent(abilities));
    }

    public void ClosePassiveAbilitiesPreview()
    {
        EventsManager.Publish(EventsManager.onDeselectCollectibleAbility);
    }

    private void PopulateAbility(CollectibleAbility collectibleAbilities, PlayerStageAbility playerStageAbility)
    {
        stageCollectibleAbilityItemList.Setup(collectibleAbilities, playerStageAbility);
        stageCollectibleAbilityItemList.gameObject.SetActive(true);
    }
}
