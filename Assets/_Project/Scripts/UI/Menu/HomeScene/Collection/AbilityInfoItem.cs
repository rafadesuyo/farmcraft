using UnityEngine;
using TMPro;

public class AbilityInfoItem : AbilityInfoItem_Base
{
    //Components
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private RectTransform levelToUnlockContainer;
    [SerializeField] private CollectibleLevelProgressHandler collectibleLevelHandler;

    //Events
    public delegate void AbilityEvent(CollectibleAbility ability);

    private AbilityEvent onInfoButtonPressed;

    //Variables
    [Header("Variables")]
    [SerializeField] private Color modifierTextColor;

    private CollectibleAbility currentAbility;

    //Getters
    public AbilityEvent OnInfoButtonPressed { get => onInfoButtonPressed; set => onInfoButtonPressed = value; }

    public void PressInfoButton()
    {
        onInfoButtonPressed?.Invoke(currentAbility);
    }

    public override void Setup(CollectibleAbility ability)
    {
        base.Setup(ability);

        currentAbility = ability;

        levelToUnlockContainer.gameObject.SetActive(!currentAbility.IsUnlocked);
        collectibleLevelHandler.SetupLevel(currentAbility.AbilityDataSO.UnlockLevel);

        descriptionText.text = ability.AbilityDataSO.GetDescription(ability.Level, modifierTextColor);
    }

    public override void ResetVariables()
    {
        base.ResetVariables();

        currentAbility = null;
        onInfoButtonPressed = null;
    }
}
