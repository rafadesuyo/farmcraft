using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShardLocationItem : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI locationNameText;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private TextMeshProUGUI goToShardsText;
    [SerializeField] private Image locationIcon;
    [SerializeField] private Image resourceIcon;

    //Variables
    [Header("Default Variables")]
    [SerializeField] private string textToReplaceWithValue = "{value}";
    [SerializeField] private string textToReplaceWithColor = "{color}";

    [Space(10)]

    [SerializeField] private Sprite goldIcon;
    [SerializeField] private Sprite heartIcon;

    [Space(10)]

    [SerializeField] private string textStore = "Store";

    [Space(10)]

    [SerializeField][TextArea] private string textBuy = "Buy\n<size=75%><color=#{color}>{value}</color></size>";
    [SerializeField][TextArea] private string textPlay = "Play\n<size=75%><color=#{color}>{value}</color></size>";

    [Space(10)]

    [SerializeField] private Color goToShardsValueTextColor;

    private Action onGoToShardsButtonPressed;

    public void PressGoToShardsButton()
    {
        onGoToShardsButtonPressed?.Invoke();
    }

    public void Setup(StoreItemSO storeItem, Action onGoToShardsButtonPressed)
    {
        this.onGoToShardsButtonPressed = onGoToShardsButtonPressed;

        locationNameText.text = textStore;

        quantityText.gameObject.SetActive(true);
        quantityText.text = storeItem.Quantity.ToString();

        UpdateGoToShardsText(textBuy, storeItem.Price.ToString());

        locationIcon.sprite = storeItem.Icon;
        resourceIcon.sprite = goldIcon;
    }

    public void Setup(StageInfoSO stageInfo, GoalReward goalReward, Action onGoToShardsButtonPressed)
    {
        this.onGoToShardsButtonPressed = onGoToShardsButtonPressed;

        locationNameText.text = stageInfo.Name;

        quantityText.gameObject.SetActive(true);
        quantityText.text = goalReward.TotalReward.ToString();

        UpdateGoToShardsText(textPlay, "1");

        locationIcon.sprite = ProjectAssetsDatabase.Instance.GetCollectibleShardIcon(goalReward.CollectibleType);
        resourceIcon.sprite = heartIcon;
    }

    public void ResetVariables()
    {
        locationNameText.text = string.Empty;
        quantityText.text = string.Empty;
        goToShardsText.text = string.Empty;
        locationIcon.sprite = null;

        onGoToShardsButtonPressed = null;
    }

    private void UpdateGoToShardsText(string baseText, string value)
    {
        StringBuilder text = new StringBuilder(baseText);
        text.Replace(textToReplaceWithColor, ColorUtility.ToHtmlStringRGB(goToShardsValueTextColor));
        text.Replace(textToReplaceWithValue, value.ToString());

        goToShardsText.text = text.ToString();
    }

    public void Release()
    {
        ResetVariables();
        this.ReleaseItem();
    }
}
