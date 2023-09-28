using DreamQuiz;
using TMPro;
using UnityEngine;

public class HomeView : MenuView
{
    [Header("Header outputs")]
    [SerializeField] private TextMeshProUGUI totalHeartsTxt = null;
    [SerializeField] private TextMeshProUGUI totalGoldTxt = null;

    public override Menu Type => Menu.Home;

    public override void Initialize()
    { 
        //Enable Input
        PlayerInput.EnableInput = true;
    }

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        UpdateInfo();
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

    public void StartGame()
    {
        CanvasManager.Instance.OpenMenu(Menu.WorldMap, null, true);
        AudioManager.Instance.Play("Button");
    }

    public void StartTrialMode()
    {
        StageLoadManager.Instance.LoadStage(new TrialModeStageInitializer());
    }

    public void OpenProfile()
    {
        CanvasManager.Instance.OpenMenu(Menu.Profile);
        AudioManager.Instance.Play("Button");
    }

    public void OpenHeartPopup()
    {
        CanvasManager.Instance.OpenMenu(Menu.HeartPopup);
        AudioManager.Instance.Play("Heart");
    }

    public void OpenGoldPopup()
    {
        CanvasManager.Instance.OpenMenu(Menu.GoldPopup);
        AudioManager.Instance.Play("Gold");
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
