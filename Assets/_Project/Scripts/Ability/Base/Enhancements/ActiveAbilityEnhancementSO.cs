using UnityEngine;

[CreateAssetMenu(fileName = "ActiveAbilityEnhancementLvl0", menuName = "Ability/Enhancement/Active Ability")]
public class ActiveAbilityEnhancementSO : AbilityEnhancementSO
{
    [SerializeField] int usePerStage = 1;

    public int UsePerStage
    {
        get
        {
            return usePerStage;
        }
    }

    public override string GetModifierText()
    {
        return $"x{usePerStage}";
    }
}