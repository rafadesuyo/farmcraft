using UnityEngine;

public enum AchievementReward
{
    None,
    SleepingTime,
    DreamEnergy
}

public class AchievementSO : ScriptableObject
{
    [Header("Achievement infos")]
    [SerializeField] private string id = "";
    [SerializeField] private string description = "";
    [SerializeField] private Sprite icon = null;
    [SerializeField] private Sprite negativeIcon = null;

    [Header("Requirement")]
    [Tooltip("What value achievement is expecting? This is the 9 in \"Get 5 collectibles level 9")]
    [SerializeField] private int targetValue = 0;
    [Tooltip("How many times targetValue must trigger? This is the 5 in \"Get 5 collectibles level 9")]
    [SerializeField] private int progressNeeded = 0;

    // Better way to do it now, with in mind the time remaining and that it's locally.
    // This will change when using achievement API (ours or thirdparty)
    [Header("Event infos")]
    [SerializeField] private string eventToListen = "";
    public string EventToListen => eventToListen;

    [Header("Rewards")]
    [SerializeField] private AchievementReward rewardType;
    [SerializeField] private int rewardValue = 0;
    [SerializeField] private int goldRewardValue = 0;

    public string Id => id;
    public string Description => description;
    public Sprite Icon => icon;
    public Sprite NegativeIcon => negativeIcon;

    public AchievementReward RewardType => rewardType;
    public int RewardValue => rewardValue;
    public int GoldRewardValue => goldRewardValue;

    public int TargetValue => targetValue;
    public virtual int ProgressNeeded => progressNeeded;

    public virtual int GetProgress()
    { return 0; }
}
