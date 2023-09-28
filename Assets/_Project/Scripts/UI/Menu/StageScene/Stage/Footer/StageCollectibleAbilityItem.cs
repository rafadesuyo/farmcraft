using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace DreamQuiz.Player
{
    public class StageCollectibleAbilityItem : MonoBehaviour
    {
        //Enums
        public enum AbilitySelection { None, Holding, ShowingInfo }

        [Header("Visual Feedbacks")]
        // Defined on Figma.
        [SerializeField] private CanvasGroup abilityGroup = null;

        [Header("Elements")]
        [SerializeField] private Image icon = null;
        [SerializeField] private TextMeshProUGUI useCountText = null;
        [SerializeField] private ClickAndHoldButton useButton = null;
        [SerializeField] private float disabledCanvasGroupAlpha = 0.6f;
        [SerializeField] private float enabledCanvasGroupAlpha = 1f;

        private AbilityDataSO abilityDataSO;

        public void Setup(CollectibleAbility collectibleAbility, PlayerStageAbility playerStageAbility)
        {
            abilityDataSO = collectibleAbility.AbilityDataSO;
            playerStageAbility.RegisterAbilityListener(abilityDataSO.AbilityId, PlayerStageAbility_OnAbilityUpdate);
            var ability = playerStageAbility.GetAbility(abilityDataSO.AbilityId);
            PlayerStageAbility_OnAbilityUpdate(ability);

            if (collectibleAbility.AbilityDataSO.IsPassiveUse)
            {
                return;
            }

            useButton.OnPointerDownEvent.AddListener(OnSelectAbility);
        }

        private void PlayerStageAbility_OnAbilityUpdate(BaseAbilityBehaviour baseAbilityBehaviour)
        {
            useCountText.text = $"<size=50%>x</size>{baseAbilityBehaviour.CurrentUsePerStage}";

            if (abilityDataSO.IsPassiveUse || !baseAbilityBehaviour.CanUseAbility())
            {
                abilityGroup.alpha = disabledCanvasGroupAlpha;
                abilityGroup.interactable = false;
                return;
            }

            abilityGroup.alpha = enabledCanvasGroupAlpha;
            abilityGroup.interactable = true;
        }

        // TODO: work on the Stage UI refactoring a way to show the ability info if the player holds the ability button
        // https://ocarinastudios.atlassian.net/browse/DQG-1865
        private void OnSelectAbility()
        {
            UseAbility();
        }

        private void UseAbility()
        {
            if (PlayerManager.CurrentPlayerInstance.PlayerStageAbility.UseAbility(abilityDataSO.AbilityId))
            {
                AudioManager.Instance.Play("UseAbility");
            }
        }
    }
}