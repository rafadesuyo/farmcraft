using TMPro;
using UnityEngine;

public class WorldMapView : MenuView
{
    // Components
    [Header("Components")]
    [SerializeField] private WorldMap worldMap;

    [Header("Header outputs")]
    [SerializeField] private TextMeshProUGUI totalHeartsTxt = null;
    [SerializeField] private TextMeshProUGUI totalGoldTxt = null;
    [Space(10)]

    [SerializeField] private RectTransform worldLockedScreen;
    [SerializeField] private TextMeshProUGUI stageNameText;
    [SerializeField] private TextMeshProUGUI stageNumberText;

    [Space(10)]

    [SerializeField] private NeedToUnlockCollectibleUI needToUnlockCollectibleUI;

    public override Menu Type => Menu.WorldMap;

    public override void Initialize()
    {
        needToUnlockCollectibleUI.CloseUI();
        WorldLockedScreenActive(false);

        worldMap.Init();

        worldMap.OnStageSelected += UpdateCurrentStageInfo;
        worldMap.OnNeedCollectibleToPlayStage += OpenNeedToUnlockCollectibleUI;
    }

    private void OnEnable()
    {
        EventsManager.AddListener(EventsManager.onGoldChange, OnChangeGold);
        EventsManager.AddListener(EventsManager.onHeartChange, OnChangeHeart);
    }

    private void OnDisable()
    {
        EventsManager.RemoveListener(EventsManager.onGoldChange, OnChangeGold);
        EventsManager.RemoveListener(EventsManager.onHeartChange, OnChangeHeart);
    }

    private void FixedUpdate()
    {
        CheckIfCameraIsInUnlockedWorld();
    }

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        EventsManager.Publish(EventsManager.onOpenWorldMapView);
        UpdateInfo();
        worldMap.Open();
    }

    protected override void OnClose()
    {
        worldMap.Close();

        WorldLockedScreenActive(false);
    }

    private void CheckIfCameraIsInUnlockedWorld()
    {
        WorldLockedScreenActive(!worldMap.CheckIfCameraIsInUnlockedWorld());
    }

    private void WorldLockedScreenActive(bool value)
    {
        worldLockedScreen.gameObject.SetActive(value);
    }

    private void OpenNeedToUnlockCollectibleUI(StageInfoSO stageInfo)
    {
        needToUnlockCollectibleUI.Init(stageInfo);
    }

    private void UpdateCurrentStageInfo(StageInfoSO stageInfo)
    {
        stageNameText.text = stageInfo.Name;
        stageNumberText.text = $"Stage {stageInfo.Id}";
    }

    private void UpdateInfo()
    {
        UpdateTotalHearts();
        UpdateTotalGold();
    }

    private void OnChangeGold(IGameEvent gameEvent)
    {
        UpdateTotalGold();
    }

    private void OnChangeHeart(IGameEvent gameEvent)
    {
        UpdateTotalHearts();
    }

    private void UpdateTotalHearts()
    {
        totalHeartsTxt.text = $"{HeartManager.Instance.CurrentHeartCount}/{HeartManager.Instance.MaxHeartCount}";
    }

    private void UpdateTotalGold()
    {
        totalGoldTxt.text = $"{PlayerProgress.TotalGold}";
    }
}
