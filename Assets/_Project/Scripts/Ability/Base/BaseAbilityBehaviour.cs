using DreamQuiz.Player;
using System;
using UnityEngine;

public abstract class BaseAbilityBehaviour
{
    protected Action<BaseAbilityBehaviour> abilityUsedCallback;

    public PlayerStageData PlayerStageData { get; protected set; }
    public CollectibleAbility CollectibleAbility { get; protected set; }
    public Collectible Collectible { get; protected set; }
    public int CurrentLevel
    {
        get
        {
            return CollectibleAbility.Level;
        }
    }
    public int MaxUsePerStage { get; protected set; }
    public int CurrentUsePerStage { get; protected set; }

    public event Action<BaseAbilityBehaviour> OnAbilityUpdate;

    public abstract AbilityId GetAbilityId();
    public abstract bool CanUseAbility();
    public abstract void UseAbility();
    protected virtual void ApplyEnhancements(AbilityEnhancementSO abilityEnhancementSO) { }
    public virtual void OnInitialize() { }

    public void Initialize(PlayerStageData playerStageData, CollectibleAbility collectibleAbility, Collectible collectible)
    {
        PlayerStageData = playerStageData;
        CollectibleAbility = collectibleAbility;
        Collectible = collectible;

        if (CollectibleAbility.AbilityDataSO.AbilityEnhancements.Count - 1 < CurrentLevel)
        {
            Debug.LogError($"[BaseAbility] Enhancement not found for level {CurrentLevel} in {GetAbilityId()}");
            return;
        }

        var enhancement = CollectibleAbility.AbilityDataSO.AbilityEnhancements[CurrentLevel];
        ApplyEnhancements(enhancement);

        if (collectibleAbility.AbilityDataSO.IsPassiveUse == false)
        {
            ApplyActiveEnhancements(enhancement);
            CurrentUsePerStage = MaxUsePerStage;
        }

        OnInitialize();

        if (CollectibleAbility.AbilityDataSO.IsPassiveUse)
        {
            UseAbility();
        }

        UpdateAbility();
    }

    public bool TryUseAbility(Action<BaseAbilityBehaviour> abilityUsedCallback)
    {
        if (CollectibleAbility.AbilityDataSO.IsPassiveUse == false && CheckIfHasUseCount() == false)
        {
            return false;
        }

        if (CanUseAbility() == false)
        {
            return false;
        }

        this.abilityUsedCallback = abilityUsedCallback;
        UseAbility();
        UpdateAbility();

        return true;
    }

    public bool CheckIfHasUseCount()
    {
        return CurrentUsePerStage > 0;
    }

    protected void UpdateAbility()
    {
        OnAbilityUpdate?.Invoke(this);
    }

    protected T GetEnhancementFromBase<T>(AbilityEnhancementSO abilityEnhancementSO) where T : AbilityEnhancementSO
    {
        var enhancementSO = abilityEnhancementSO as T;

        if (enhancementSO == null)
        {
            Debug.LogError($"[BaseAbilityBehaviour] Missing {typeof(T)} for {CurrentLevel} level");
        }

        return enhancementSO;
    }

    private void ApplyActiveEnhancements(AbilityEnhancementSO abilityEnhancementSO)
    {
        var activeAbilityEnhancementSO = GetEnhancementFromBase<ActiveAbilityEnhancementSO>(abilityEnhancementSO);
        MaxUsePerStage = activeAbilityEnhancementSO.UsePerStage;
        CurrentUsePerStage = MaxUsePerStage;
    }

    protected void ConsumeUsePerStage()
    {
        CurrentUsePerStage--;
        abilityUsedCallback?.Invoke(this);
        UpdateAbility();
    }
}
