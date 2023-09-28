using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleShardsProgressHandler : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI shardsCountText;
    [SerializeField] private Image shardIcon;
    [SerializeField] private Image shardSliderFillImage;
    [SerializeField] private Slider shardProgressSlider;
    [SerializeField] private RectTransform canUpgradeIcon;

    //Variables
    [Header("Default Variables")]
    [SerializeField] private Color shardSliderColorNormal;
    [SerializeField] private Color shardSliderColorCanUpgrade;

    public void SetupShardsProgress(Collectible collectible)
    {
        shardIcon.sprite = ProjectAssetsDatabase.Instance.GetCollectibleShardIcon(collectible.Data.Type);

        if (collectible.IsMaxLevel == false)
        {
            shardsCountText.text = $"{collectible.CurrentShards}/{collectible.ShardsToNextLevel}";
            shardProgressSlider.value = Mathf.InverseLerp(0, collectible.ShardsToNextLevel, collectible.CurrentShards);
            canUpgradeIcon.gameObject.SetActive(collectible.HasEnoughShardsToLevelUp);

            if (collectible.HasEnoughShardsToLevelUp == true)
            {
                shardSliderFillImage.color = shardSliderColorCanUpgrade;
            }
            else
            {
                shardSliderFillImage.color = shardSliderColorNormal;
            }
        }
        else
        {
            shardsCountText.text = $"{collectible.CurrentShards}";
            shardProgressSlider.value = 1;
            canUpgradeIcon.gameObject.SetActive(false);

            shardSliderFillImage.color = shardSliderColorCanUpgrade;
        }
    }
}
