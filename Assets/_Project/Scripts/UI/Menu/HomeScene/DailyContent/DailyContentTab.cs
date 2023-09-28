using UnityEngine;

public abstract class DailyContentTab : MonoBehaviour
{
    //Components
    [Header("Base Components")]
    [SerializeField] protected RectTransform contentLayout;

    //Events
    public static event Reward.RewardsEvent OnRewardIsCollected;

    public abstract string GetTitleText();

    public abstract string GetRefreshTextValue();

    public abstract void Initialize();

    public abstract void Setup();

    public abstract void ResetVariables();

    public void ResetLayoutPosition()
    {
        contentLayout.anchoredPosition = Vector2.zero;
    }

    protected void InvokeOnRewardIsCollected(Reward[] rewards)
    {
        OnRewardIsCollected?.Invoke(rewards);
    }
}
