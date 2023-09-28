using UnityEngine;

[CreateAssetMenu(fileName = "ImmunizerAbilityEnhancementLvl0", menuName = "Ability/Enhancement/Immunizer Ability")]
public class ImmunizerAbilityEnhancementSO : ActiveAbilityEnhancementSO
{
    [SerializeField] float poisonReduceAmount = 0.25f;
    [SerializeField] int moveCountToDeactivate = 1;

    public float PoisonReduceAmount
    {
        get
        {
            return poisonReduceAmount;
        }
    }

    public int MoveCountToDeactivate
    {
        get
        {
            return moveCountToDeactivate;
        }
    }
}
