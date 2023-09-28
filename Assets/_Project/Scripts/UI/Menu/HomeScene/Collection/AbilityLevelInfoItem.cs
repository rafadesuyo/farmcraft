using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityLevelInfoItem : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Image abilityLockedImage;
    [SerializeField] private Image selectedOutline;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private TextMeshProUGUI levelInfoText;
    [SerializeField] private CollectibleLevelProgressHandler collectibleLevelHandler;

    public void Setup(CollectibleAbility ability, int level)
    {
        abilityIcon.sprite = ability.AbilityDataSO.Icon;

        if (level >= ability.AbilityDataSO.UnlockLevel)
        {
            levelInfoText.text = ability.AbilityDataSO.GetModifierText(level);
        }
        else
        {
            levelInfoText.text = string.Empty;
        }

        collectibleLevelHandler.SetupLevel(level);

        abilityLockedImage.gameObject.SetActive(level < ability.AbilityDataSO.UnlockLevel);

        selectedOutline.gameObject.SetActive(level == ability.RawLevel);
    }

    public void ResetVariables()
    {
        abilityIcon.sprite = null;
        levelInfoText.text = string.Empty;
    }
}
