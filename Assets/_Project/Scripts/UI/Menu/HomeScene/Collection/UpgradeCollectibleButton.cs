using System.Text;
using TMPro;
using UnityEngine;

public class UpgradeCollectibleButton : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI upgradeText;

    //Variables
    [Header("Default Variables")]
    [SerializeField] private string textToReplaceWithValue = "{value}";
    [SerializeField] private string textToReplaceWithColor = "{color}";
    [SerializeField][TextArea] private string textUpgrade = "Upgrade\n<size=75%><color=#{color}>{value}</color></size>";

    [Space(10)]

    [SerializeField] private Color upgradeValueTextColor;

    public void Setup(int upgradeValue)
    {
        StringBuilder text = new StringBuilder(textUpgrade);
        text.Replace(textToReplaceWithColor, ColorUtility.ToHtmlStringRGB(upgradeValueTextColor));
        text.Replace(textToReplaceWithValue, upgradeValue.ToString());

        upgradeText.text = text.ToString();
    }
}
