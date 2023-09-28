using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityListItem : MonoBehaviour
{
    [SerializeField] private Image abilityIcon = null;
    [SerializeField] private TextMeshProUGUI nameTxt = null;
    [SerializeField] private TextMeshProUGUI descriptionTxt = null;
    [SerializeField] private TextMeshProUGUI manaCostTxt = null;

    public void Setup(AbilityDataSO ability)
    {
        abilityIcon.sprite = ability.Icon;
        nameTxt.text = ability.Name;
        //descriptionTxt.text = ability.Description;

        manaCostTxt.text = ability.IsPassiveUse ? "Active" : "Passive";
    }
}
