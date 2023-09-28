using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeartPopupView : MenuView
{
    private const int heartsCount= 4;

    //Components
    [Header("Components")]
    [SerializeField] private TMP_Text youHaveHeartsText;
    [SerializeField] private TMP_Text totalHeartsText;
    [SerializeField] private TMP_Text timeToGetHeartsText;
    [SerializeField] private Image heartIcon;

    [Space(10)]

    [SerializeField] private RectTransform totalHeartsPanel;
    [SerializeField] private RectTransform buyHeartsPanel;

    //Variables
    [Header("Default Variables")]
    [SerializeField] private string textToReplaceWithValue = "{value}";

    [Space(10)]

    [SerializeField] private string textYouHaveHearts = "You have";
    [SerializeField] private string textZeroHearts = "You don't have any hearts";
    [SerializeField] private string textMaxHearts = "Play more and explore the amazing Dream Quizzz world";
    [SerializeField] private string textWaitToGetHearts = "Wait {value} to get +1";

    //Getters
    public override Menu Type => Menu.HeartPopup;

    public void UpdateHearts()
    {
        int hearts = HeartManager.Instance.CurrentHeartCount;

        if (hearts > 0)
        {
            int maxHearts = HeartManager.Instance.MaxHeartCount;
            youHaveHeartsText.text = textYouHaveHearts;
            totalHeartsText.text = $"{hearts}/{maxHearts}";
        }
        else
        {
            youHaveHeartsText.text = textZeroHearts;
        }

        totalHeartsPanel.gameObject.SetActive(hearts > heartsCount);
        buyHeartsPanel.gameObject.SetActive(hearts <= heartsCount);
    }

    public void UpdateTimeToGetHearts(float time)
    {
        heartIcon.gameObject.SetActive(HeartManager.Instance.CanEarnFreeHeart);

        if (HeartManager.Instance.CanEarnFreeHeart == false)
        {
            timeToGetHeartsText.text = textMaxHearts;
            return;
        }

        TimeSpan timeToGetHeart = TimeSpan.FromSeconds(time);
        string timeToGetHeartString = $"{timeToGetHeart.Minutes}:{timeToGetHeart.Seconds}";

        timeToGetHeartsText.text = textWaitToGetHearts.Replace(textToReplaceWithValue, timeToGetHeartString);
    }

    public void BuyHearts()
    {
        // Close HeartPopup
        CanvasManager.Instance.ReturnMenu();
        // Open store in the heart section
        CanvasManager.Instance.OpenMenu(Menu.Store);
    }

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        UpdateVariables();
    }

    protected override void OnClose()
    {
        ResetVariables();
    }

    private void UpdateVariables()
    {
        UpdateHearts();
        UpdateTimeToGetHearts(HeartManager.Instance.TimeToNextHeart);
    }

    private void ResetVariables()
    {
        youHaveHeartsText.text = string.Empty;
        totalHeartsText.text = string.Empty;
        timeToGetHeartsText.text = string.Empty;
    }
}
