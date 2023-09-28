using UnityEngine;

[CreateAssetMenu(fileName = "SleepyTouchAbilityEnhancementLvl0", menuName = "Ability/Enhancement/Sleepy Touch Ability")]
public class SleepyTouchAbilityEnhancementSO : ActiveAbilityEnhancementSO
{
    [SerializeField] int goldAmount = 50;

    public int GoldAmount
    {
        get
        {
            return goldAmount;
        }
    }
}