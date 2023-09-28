using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Image abilityImage;

    //Variables
    private AbilityDataSO currentAbility;

    public void SetAbility(AbilityDataSO abilitySO)
    {
        currentAbility = abilitySO;

        UpdateVariables();

        abilityImage.gameObject.SetActive(true);
    }

    public void EmptyAbility()
    {
        currentAbility = null;

        ResetVariables();

        abilityImage.gameObject.SetActive(false);
    }

    private void UpdateVariables()
    {
        abilityImage.sprite = currentAbility.Icon;
    }

    public void ResetVariables()
    {
        abilityImage.sprite = null;
    }
}
