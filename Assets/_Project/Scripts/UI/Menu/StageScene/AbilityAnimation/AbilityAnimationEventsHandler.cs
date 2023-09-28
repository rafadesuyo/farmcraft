using DreamQuiz.Player;
using UnityEngine;

public class AbilityAnimationEventsHandler : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private AbilityAnimationUI abilityAnimationUI;

    private PlayerStageInstance playerStageInstance;

    private void Awake()
    {
        playerStageInstance = GetComponentInParent<PlayerStageInstance>();

        playerStageInstance.OnInitialized += PlayerStageInstance_OnInitialized;
    }

    private void PlayerStageInstance_OnInitialized()
    {
        playerStageInstance.OnInitialized -= PlayerStageInstance_OnInitialized;
        playerStageInstance.PlayerStageAbility.OnAbilityUse += PlayAbilityAnimation;
    }

    private void OnEnable()
    {
        if (playerStageInstance.PlayerStageAbility != null)
        {
            playerStageInstance.PlayerStageAbility.OnAbilityUse += PlayAbilityAnimation;
        }
    }

    private void OnDisable()
    {
        if (playerStageInstance.PlayerStageAbility != null)
        {
            playerStageInstance.PlayerStageAbility.OnAbilityUse -= PlayAbilityAnimation;
        }
    }

    private void PlayAbilityAnimation(BaseAbilityBehaviour abilityBehaviour)
    {
        abilityAnimationUI.Setup(abilityBehaviour);
    }
}
