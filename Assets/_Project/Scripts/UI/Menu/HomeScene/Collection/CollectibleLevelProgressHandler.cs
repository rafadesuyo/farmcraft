using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

public class CollectibleLevelProgressHandler : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private CollectibleLevelProgressHandler_StarIcon[] starIcons;

    //Variables
    [Header("Default Variables")]
    [SerializeField] private Color upgradeStarNormalColor = Color.white;
    [SerializeField] private Color upgradeStarFadeColor = new Color(1, 1, 1, 0);
    [SerializeField] private float upgradeStarFadeDuration = 0.5f;

    [Header("Options")]
    [SerializeField] private bool hideUnlockedStars;

    private TweenerCore<Color, Color, DG.Tweening.Plugins.Options.ColorOptions> currentStarTween;

    public void SetupLevel(int collectibleLevel)
    {
        for (int i = 0; i < starIcons.Length; i++)
        {
            starIcons[i].Star_Background.gameObject.SetActive((i < collectibleLevel) || !(hideUnlockedStars));

            starIcons[i].Star_Filled.gameObject.SetActive(i < collectibleLevel);
            starIcons[i].Star_Filled.color = upgradeStarNormalColor;
        }
    }

    public void PlayUpgradeAnimation(int collectibleLevel)
    {
        if(collectibleLevel <= 0 || collectibleLevel > starIcons.Length)
        {
            Debug.LogError($"The collectible level to play the upgrade animation is invalid!\nCollectible Level: {collectibleLevel}. Number of stars in the handler: {starIcons.Length}.");
            return;
        }

        int levelStarIndex = collectibleLevel - 1;

        starIcons[levelStarIndex].Star_Filled.color = upgradeStarNormalColor;

        StopUpgradeAnimation();

        currentStarTween = starIcons[levelStarIndex].Star_Filled.DOColor(upgradeStarFadeColor, upgradeStarFadeDuration).SetLoops(-1, LoopType.Yoyo);
    }

    public void StopUpgradeAnimation()
    {
        if (currentStarTween != null)
        {
            currentStarTween.Kill();
            currentStarTween = null;
        }
    }
}
