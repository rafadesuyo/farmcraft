using UnityEngine;
using UnityEngine.UI;

public class DailyContentButton : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Image canCollectRewardIcon;

    private void OnEnable()
    {
        DailyContentView.OnUIIsClosed += UpdateDailyContentButton;

        UpdateDailyContentButton();
    }

    private void OnDisable()
    {
        DailyContentView.OnUIIsClosed -= UpdateDailyContentButton;
    }

    public void UpdateDailyContentButton()
    {
        if(DailyMissionsManager.Instance.CanAnyCurrentFixedMissionBeCollected() == true || DailyMissionsManager.Instance.CanAnyCurrentRandomMissionBeCollected() == true)
        {
            canCollectRewardIcon.gameObject.SetActive(true);
            return;
        }

        foreach (DailyRewardsListSO dailyRewardsList in DailyRewardsManager.Instance.Rewards)
        {
            if (dailyRewardsList.State != DailyRewardsListSO.DailyRewardsListState.Enabled)
            {
                continue;
            }

            if(dailyRewardsList.CanRewardsBeCollected() == true)
            {
                canCollectRewardIcon.gameObject.SetActive(true);
                return;
            }
        }

        canCollectRewardIcon.gameObject.SetActive(false);
    }

    public void OpenDailyContentView()
    {
        CanvasManager.Instance.OpenMenu(Menu.DailyContent);
    }
}
