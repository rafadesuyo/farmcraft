using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbilityInfoItem_Base : MonoBehaviour
{
    //Components
    [Header("Base Components")]
    [SerializeField] protected Image abilityLockedImage;
    [SerializeField] protected Image abilityIcon;
    [SerializeField] protected TextMeshProUGUI abilityName;
    [SerializeField] protected TextMeshProUGUI abilityTypeText;

    //Variables
    [Header("Base Variables")]
    [SerializeField] protected string textPassive = "Passive";
    [SerializeField] protected string textActive = "Active";

    [Space(10)]

    [SerializeField] protected Color colorPassive;
    [SerializeField] protected Color colorActive;

    public virtual void Setup(CollectibleAbility collectibleAbility)
    {
        abilityIcon.sprite = collectibleAbility.AbilityDataSO.Icon;
        abilityName.text = collectibleAbility.AbilityDataSO.Name;

        if (collectibleAbility.AbilityDataSO.IsPassiveUse)
        {
            abilityTypeText.text = textPassive;
            abilityTypeText.color = colorPassive;
        }
        else
        {
            abilityTypeText.text = textActive;
            abilityTypeText.color = colorActive;
        }

        abilityLockedImage.gameObject.SetActive(!collectibleAbility.IsUnlocked);
    }

    public virtual void ResetVariables()
    {
        abilityIcon.sprite = null;
        abilityName.text = string.Empty;
        abilityTypeText.text = string.Empty;
    }

    public void Release()
    {
        ResetVariables();
        this.ReleaseItem();
    }
}
