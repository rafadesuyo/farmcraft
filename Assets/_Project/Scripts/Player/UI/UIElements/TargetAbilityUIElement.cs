using DreamQuiz.Player;
using UnityEngine;

public class TargetAbilityUIElement<T> : PlayerUIElement where T : ITargetable
{
    [SerializeField] private CanvasGroup canvasGroup;

    protected override void InitializeUI()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        var allAbilities = playerStageInstance.PlayerStageAbility.GetAllAbilities();
        foreach (var abilityBehaviour in allAbilities)
        {
            var targetAbilityBehaviour = abilityBehaviour as BaseTargetableAbilityBehaviour<T>;

            if (targetAbilityBehaviour != null)
            {
                playerStageInstance.PlayerStageAbility.RegisterAbilityListener(targetAbilityBehaviour.GetAbilityId(), TargetableAbilityBehaviour_OnUpdate);
            }
        }
    }

    private void TargetableAbilityBehaviour_OnUpdate(BaseAbilityBehaviour abilityBehaviour)
    {
        var targetAbilityBehaviour = abilityBehaviour as BaseTargetableAbilityBehaviour<T>;

        if (targetAbilityBehaviour != null)
        {
            UpdateUI(targetAbilityBehaviour.TargetState);
        }
    }

    protected void UpdateUI(TargetState targetState)
    {
        if (targetState == TargetState.Searching)
        {
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.alpha = 0;
        }
    }
}
