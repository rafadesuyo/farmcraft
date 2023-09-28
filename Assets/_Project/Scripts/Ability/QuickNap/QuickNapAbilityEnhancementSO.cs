using UnityEngine;

[CreateAssetMenu(fileName = "QuickNapAbilityEnhancementLvl0", menuName = "Ability/Enhancement/Quick Nap Ability")]
public class QuickNapAbilityEnhancementSO : ActiveAbilityEnhancementSO
{
    [SerializeField] int sleepingTimeRecoveryAmount = 15;

    public int SleepingTimeRecoveryAmount
    {
        get
        {
            return sleepingTimeRecoveryAmount;
        }
    }
}
