using Spine.Unity;
using UnityEngine;

public class AbilityAnimationUI : UIControllerAnimated
{
    //Components
    [Header("Components")]
    [SerializeField] private SkeletonGraphic abilityBackgroundEffect;
    [SerializeField] private SkeletonGraphic abilityIcon;
    [SerializeField] private CollectibleLoopAnimationHandler collectibleLoopAnimationHandler;

    private TimeTrackingSystem timeTrackingSystem;
    private QuizSystem quizSystem;

    //Variables
    private BaseAbilityBehaviour currentAbility;

    private bool hasSuspendedQuizTimeout = false;

    private void Start()
    {
        timeTrackingSystem = StageSystemLocator.GetSystem<TimeTrackingSystem>();
        quizSystem = StageSystemLocator.GetSystem<QuizSystem>();
    }

    protected override void OnOpen()
    {
        PauseTimers();
    }

    protected override void OnAfterOpenAnimation()
    {
        PlayAbilityAnimation();
    }

    protected override void OnClose()
    {
        ResumeTimers();

        ResetVariables();
    }

    public void Setup(BaseAbilityBehaviour ability)
    {
        OpenUI();

        currentAbility = ability;

        SetupAnimations();
    }

    private void ResetVariables()
    {
        currentAbility = null;
        hasSuspendedQuizTimeout = false;
    }

    private void SetupAnimations()
    {
        UpdateAbilityBackgroundEffectAnimation(currentAbility.Collectible.Data.AbilityBackgroundEffectAnimation);
        abilityBackgroundEffect.timeScale = 0;

        UpdateAbilityIconAnimation(currentAbility.CollectibleAbility.AbilityDataSO.IconSkeletonData, currentAbility.CollectibleAbility.AbilityDataSO.IconAnimation);
        abilityIcon.timeScale = 0;

        collectibleLoopAnimationHandler.UpdateCollectible(currentAbility.Collectible.Data);
        collectibleLoopAnimationHandler.PlayAbilityAnimation(FinishAbilityAnimation);
        collectibleLoopAnimationHandler.SetAnimationSpeed(0);
    }

    private void PlayAbilityAnimation()
    {
        abilityBackgroundEffect.timeScale = 1;
        abilityIcon.timeScale = 1;
        collectibleLoopAnimationHandler.SetAnimationSpeed(1);
    }

    private void UpdateAbilityBackgroundEffectAnimation(AnimationReferenceAsset backgroundEffectAnimation)
    {
        if(backgroundEffectAnimation == null)
        {
            Debug.LogError($"The collectible \"{currentAbility.Collectible.Data}\" background effect animation is null!", currentAbility.Collectible.Data);
        }

        abilityBackgroundEffect.AnimationState.SetAnimation(0, backgroundEffectAnimation, false);
    }

    private void UpdateAbilityIconAnimation(SkeletonDataAsset iconSkeletonData, AnimationReferenceAsset iconAnimation)
    {
        if(iconSkeletonData == null || iconAnimation == null)
        {
            Debug.LogError($"The ability \"{currentAbility.CollectibleAbility.AbilityDataSO}\" IconSkeletonData or IconAnimation is null!", currentAbility.CollectibleAbility.AbilityDataSO);
            return;
        }

        abilityIcon.skeletonDataAsset = iconSkeletonData;
        abilityIcon.Initialize(true);

        abilityIcon.AnimationState.SetAnimation(0, iconAnimation, false);
    }

    private void FinishAbilityAnimation()
    {
        CloseUI();
    }

    private void PauseTimers()
    {
        timeTrackingSystem.PauseCountingTime();

        if(quizSystem.CurrentQuizState == QuizState.WaitingForAnswer && quizSystem.IsTimeoutSuspended == false)
        {
            quizSystem.IsTimeoutSuspended = true;
            hasSuspendedQuizTimeout = true;
        }
    }

    private void ResumeTimers()
    {
        timeTrackingSystem.ResumeCountingTime();

        if(hasSuspendedQuizTimeout == true)
        {
            quizSystem.IsTimeoutSuspended = false;
        }
    }
}
