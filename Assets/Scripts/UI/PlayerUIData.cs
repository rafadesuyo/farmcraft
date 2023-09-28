using TMPro;
using UnityEngine;

public class PlayerUIData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerGold;

    [SerializeField] private TextMeshProUGUI _playerGoldenWood;

    [SerializeField] private CurrencyManager _currencyManager;

    private void Start()
    {
        _playerGold.text = $"{_currencyManager.PlayerGold}/{_currencyManager.PlayerGoldLimit}";
        _playerGoldenWood.text = $"{_currencyManager.PlayerWood}/{_currencyManager.PlayerWoodLimit}";
        _currencyManager.OnGoldUpdate += CurrencyManager_OnGoldUpdate;
    }

    private void CurrencyManager_OnGoldUpdate(object sender, System.EventArgs e)
    {
        _playerGold.text = $"{_currencyManager.PlayerGold}/{_currencyManager.PlayerGoldLimit}";
    }
}