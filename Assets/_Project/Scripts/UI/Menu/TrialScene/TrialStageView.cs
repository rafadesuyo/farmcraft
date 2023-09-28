using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrialStageView : MonoBehaviour
{
    [SerializeField] Transform viewContent;
    [Header("CATEGORIES")]
    [SerializeField] GameObject trialCategoryItemPrefab;
    [SerializeField] Transform categoriesContainer;
    [Header("SCORES")]
    [SerializeField] GameObject trialScoreItemPrefab;
    [SerializeField] Transform scoresContainer;
    [SerializeField] GameObject retrievingDataSign;
    [Header("REWARD")]
    [SerializeField] GameObject rewardUI;
    [SerializeField] TextMeshProUGUI rewardAmountText;

    QuizSystem quizSystem;

    private const int firstValidCategoryIndexValue = 2;

    private void OnEnable()
    {
        TrialStageManager.OnDataLoaded += DisableRetrievingDataSign;
        TrialStageManager.OnTrialRewardHandled += ShowRewardPanel;
    }

    private void OnDisable()
    {
        TrialStageManager.OnDataLoaded -= DisableRetrievingDataSign;
        TrialStageManager.OnTrialRewardHandled -= ShowRewardPanel;
    }

    private void Start()
    {
        CreateLayoutItems(trialCategoryItemPrefab, categoriesContainer);
        CreateLayoutItems(trialScoreItemPrefab, scoresContainer, true);

        quizSystem = StageSystemLocator.GetSystem<QuizSystem>();
        quizSystem.OnQuizStateChange += SetViewVisibility;

        HideRewardPanel();
    }

    private void SetViewVisibility(QuizState quizState)
    {
        viewContent.gameObject.SetActive(quizState == QuizState.None);
    }

    private void CreateLayoutItems(GameObject itemPrefab, Transform parent, bool hasRetrievingDataImg = false)
    {
        int quizCategoriesCount = Enum.GetValues(typeof(QuizCategory)).Length;

        for (int i = firstValidCategoryIndexValue; i < quizCategoriesCount; i++)
        {
            GameObject item = Instantiate(itemPrefab, parent);
            QuizCategory category = (QuizCategory)i;
            item.GetComponent<CategoryUIBaseItem>().SetCategory(category);
        }

        if (hasRetrievingDataImg)
        {
            retrievingDataSign.SetActive(true);
            retrievingDataSign.transform.SetAsLastSibling();
        }
    }

    private void DisableRetrievingDataSign()
    {
        retrievingDataSign.SetActive(false);
    }

    private void ShowRewardPanel(int amount)
    {
        rewardAmountText.text = amount.ToString();
        rewardUI.SetActive(true);
    }

    public void HideRewardPanel()
    {
        rewardUI.SetActive(false);
    }
}
