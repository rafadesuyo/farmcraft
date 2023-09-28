using UnityEngine;

public class AbilityInfoUI : UIControllerAnimated
{
    //Components
    [Header("Components")]
    [SerializeField] private AbilityInfoItem abilityInfoItem;
    [SerializeField] private AbilityLevelInfoItem[] abilityLevelInfoItems;

    public void Setup(CollectibleAbility ability)
    {
        OpenUI();

        abilityInfoItem.Setup(ability);

        for (int i = 0; i < abilityLevelInfoItems.Length; i++)
        {
            abilityLevelInfoItems[i].Setup(ability, i + 1);
        }
    }

    protected override void OnClose()
    {
        ResetVariables();
    }

    private void ResetVariables()
    {
        abilityInfoItem.ResetVariables();

        foreach (AbilityLevelInfoItem abilityLevelInfoItem in abilityLevelInfoItems)
        {
            abilityLevelInfoItem.ResetVariables();
        }
    }
}
