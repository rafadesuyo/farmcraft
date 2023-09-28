using TMPro;
using UnityEngine;

public class AbilityInfoItem_Mini : AbilityInfoItem_Base
{
    //Components
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI abilityMiniDescription;

    //Variables
    [Header("Default Variables")]
    [SerializeField] private string textToReplaceWithValue = "{value}";
    [SerializeField] private string textStarsToUnlock = "{value} stars to unlock";

    public override void Setup(CollectibleAbility ability)
    {
        base.Setup(ability);

        UpdateAbilityInfoInLevel(ability, ability.Level);

        if(ability.IsUnlocked == false)
        {
            abilityMiniDescription.text = textStarsToUnlock.Replace(textToReplaceWithValue, ability.AbilityDataSO.UnlockLevel.ToString());
        }
    }

    public void SetupAbilityInSpecificLevel(CollectibleAbility ability, int level)
    {
        base.Setup(ability);

        UpdateAbilityInfoInLevel(ability, level);

        abilityLockedImage.gameObject.SetActive(false);
    }

    private void UpdateAbilityInfoInLevel(CollectibleAbility ability, int level)
    {
        abilityMiniDescription.text = ability.AbilityDataSO.GetMiniDescription(level);
    }

    public override void ResetVariables()
    {
        base.ResetVariables();

        abilityMiniDescription.text = string.Empty;
    }
}
