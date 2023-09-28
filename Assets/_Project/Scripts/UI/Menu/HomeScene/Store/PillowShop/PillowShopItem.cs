using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PillowShopItem : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image iconImg = null;
    [SerializeField] private TextMeshProUGUI amountTxt = null;
    [SerializeField] private TextMeshProUGUI priceTxt;
    [SerializeField] private Button buyBtn = null;

    private StoreItemSO item;

    //TEMPORARY FIELDS. ART UI IS STILL NOT IMPLEMENTED.
    //https://ocarinastudios.atlassian.net/browse/DQG-2124?atlOrigin=eyJpIjoiNWE3ZjQ1OGRiOGFiNDk3MWFmNmQ5ZTM2ZjBlNzRhNmMiLCJwIjoiaiJ9
    private string pillowsTextPrefix = "PILLOWS x";
    private string priceTextSuffix = " GOLD";

    public StoreItemSO Item => item;

    public static event Action<int> OnPillowItemPurchased;

    public void Setup(StoreItemSO newItem, System.Action onBuyCallback = null)
    {
        item = newItem;
        iconImg.sprite = item.Icon;
        amountTxt.text = pillowsTextPrefix + item.Quantity;
        priceTxt.text = item.Price + priceTextSuffix;

        OnGoldChange(null);

        buyBtn.interactable = PlayerProgress.TotalGold >= item.Price;
        buyBtn.onClick.RemoveAllListeners();
        onBuyCallback += Buy;
        buyBtn.onClick.AddListener(() => onBuyCallback?.Invoke());
    }

    private void OnEnable()
    {
        EventsManager.AddListener(EventsManager.onGoldChange, OnGoldChange);
    }

    private void OnDisable()
    {
        EventsManager.RemoveListener(EventsManager.onGoldChange, OnGoldChange);
        this.ReleaseItem();
    }

    private void OnGoldChange(IGameEvent gameEvent)
    {
        buyBtn.interactable = PlayerProgress.TotalGold >= item.Price;
    }

    private void Buy()
    {
        PlayerProgress.TotalGold -= item.Price;
        Debug.LogWarning($"Bought {item.Quantity} pillow shop item for {item.Price}. Total gold is now {PlayerProgress.TotalGold}");
        AudioManager.Instance.Play("Button");

        OnPillowItemPurchased?.Invoke(item.Quantity);
    }
}
