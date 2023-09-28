using UnityEngine;

[CreateAssetMenu(fileName = "SleepyAbilityEnhancementLvl0", menuName = "Ability/Enhancement/Sleepy Ability")]
public class SleepyAbilityEnhancementSO : AbilityEnhancementSO
{
    [SerializeField] int sleepingTimeToAdd = 5;

    public int SleepingTimeToAdd
    {
        get
        {
            return sleepingTimeToAdd;
        }
    }

    public override string GetModifierText()
    {
        return $"+{sleepingTimeToAdd}";
    }
}