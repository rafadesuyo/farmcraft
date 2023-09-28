using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeedToUnlockCollectibleUI : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Image categoryIconImage;
    [SerializeField] private Image collectibleIconImage;
    [SerializeField] private TextMeshProUGUI categoryNameText;
    [SerializeField] private TextMeshProUGUI collectibleNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Space(10)]

    [SerializeField] private RectTransform mainContainer;
    [SerializeField] private RectTransform oneCollectibleContainer;
    [SerializeField] private RectTransform nineCollectiblesContainer;

    [Space(10)]

    [SerializeField] private RectTransform size_OneCollectible;
    [SerializeField] private RectTransform size_NineCollectibles;

    //Variables
    [Header("Default Variables")]
    [SerializeField][TextArea] private string textYouNeedToUnlockACollectible = "You need to unlock an category collectible to play this stage.";
    [SerializeField][TextArea] private string textUnlock9Collectibles = "You need to collect 9 Oneerikos to play this stage.";

    public void OpenUI()
    {
        gameObject.SetActive(true);
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);

        ResetVariables();
    }

    public void Init(StageInfoSO stageInfo)
    {
        OpenUI();
        UpdateVariables(stageInfo);
    }

    private void UpdateVariables(StageInfoSO stageInfo)
    {
        if(stageInfo.UnlockWith9Collectibles == true)
        {
            oneCollectibleContainer.gameObject.SetActive(false);
            nineCollectiblesContainer.gameObject.SetActive(true);

            mainContainer.sizeDelta = size_NineCollectibles.sizeDelta;

            descriptionText.text = textUnlock9Collectibles;
        }
        else
        {
            oneCollectibleContainer.gameObject.SetActive(true);
            nineCollectiblesContainer.gameObject.SetActive(false);

            mainContainer.sizeDelta = size_OneCollectible.sizeDelta;

            CollectibleSO collectible = CollectibleManager.Instance.GetCollectibleDataByType(stageInfo.CollectibleNeededToPlay);

            categoryIconImage.sprite = ProjectAssetsDatabase.Instance.GetCategoryIcon(collectible.Category);
            collectibleIconImage.sprite = collectible.PortraitColored;

            categoryNameText.text = QuizCategoryMaps.GetCategoryNameByQuizCategory(collectible.Category);
            collectibleNameText.text = collectible.Name;

            descriptionText.text = textYouNeedToUnlockACollectible;
        }
    }

    private void ResetVariables()
    {
        categoryIconImage.sprite = null;
        collectibleIconImage.sprite = null;
        categoryNameText.text = string.Empty;
        collectibleNameText.text = string.Empty;
    }
}
