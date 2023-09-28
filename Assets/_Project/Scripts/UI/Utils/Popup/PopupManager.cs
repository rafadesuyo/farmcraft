using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupManager : LocalSingleton<PopupManager>
{
    [SerializeField] private GameObject popupObj = null;

    // AUX FOR MVP
    [SerializeField] private GameObject achievementPopup;
    [SerializeField] private Image achievementIcon = null;
    [SerializeField] private TextMeshProUGUI achievementName = null;
    [SerializeField] private TextMeshProUGUI achievementGoldReward = null;

    private PopupUI popup = null;

    private void Start()
    {
        popup = popupObj.GetComponent<PopupUI>();
    }

    public void Open(string title, System.Action confirmCallback = null)
    {
        confirmCallback += () => popupObj.SetActive(false);
        popup.Setup(title, confirmCallback);
        popupObj.SetActive(true);
    }

    public void OpenAchievementPopup(AchievementSO achievementData)
    {
        achievementPopup.SetActive(true);
        achievementIcon.sprite = achievementData.Icon;
        achievementName.text = achievementData.Id;
        achievementGoldReward.text = $"+{achievementData.GoldRewardValue}";
    }

    public void ClosePopup()
    {
        achievementPopup.SetActive(false);
    }
}