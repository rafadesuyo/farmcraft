using TMPro;
using UnityEngine;

public class RewardCalculationItem : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI valueTxt = null;

    public void SetupWin(float value)
    {
        valueTxt.text = $"{value}";
        TextFormatter.ChangeTextColor(HexColors.Maize, valueTxt);
    }

    public void SetupLose(float value)
    {
        valueTxt.text = $"{value}";
        TextFormatter.ChangeTextColor(HexColors.SunsetOrange, valueTxt);
    }

    public void DisableItem()
    {
        this.gameObject.SetActive(false);
    }
}