using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamPreviewAbilityItem : MonoBehaviour
{
    [SerializeField] private Image abilityImage = null;
    [SerializeField] private TextMeshProUGUI abilityNameText = null;

    public void Setup(CollectibleAbility ability)
    {
        if (ability != null)
        {
            abilityImage.sprite = ability.AbilityDataSO.Icon;
            abilityNameText.text = ability.AbilityDataSO.Name;
        }
    }
}