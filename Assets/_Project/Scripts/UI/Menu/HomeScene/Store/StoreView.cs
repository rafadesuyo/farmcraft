using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ItemType
{
    Hearts,
    Shards,
    Lootboxes,
    Pillows
}

public class StoreView : MenuView
{
    [Header("Header outputs")]
    [SerializeField] private TextMeshProUGUI totalHeartsTxt = null;
    [SerializeField] private TextMeshProUGUI totalGoldTxt = null;
    [SerializeField] private TextMeshProUGUI totalOneekoinTxt = null;

    public override Menu Type => Menu.Store;

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        UpdateInfo(null);
    }

    private void OnEnable()
    {
        EventsManager.AddListener(EventsManager.onGoldChange, UpdateInfo);
        EventsManager.AddListener(EventsManager.onHeartChange, UpdateInfo);
        EventsManager.AddListener(EventsManager.onOneekoinChange, UpdateInfo);
    }

    private void OnDisable()
    {
        EventsManager.RemoveListener(EventsManager.onGoldChange, UpdateInfo);
        EventsManager.RemoveListener(EventsManager.onHeartChange, UpdateInfo);
    }

    public void UpdateInfo(IGameEvent gameEvent)
    {
        UpdateTotalHearts();
        UpdateTotalGold();
        UpdateTotalOneekoin();
    }

    private void UpdateTotalOneekoin()
    {
        totalOneekoinTxt.text = $"{PlayerProgress.TotalOneekoin}";
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
