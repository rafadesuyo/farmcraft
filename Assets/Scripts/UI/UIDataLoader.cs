using TMPro;
using UnityEngine;

public class UIDataLoader : MonoBehaviour, IDataPersistence
{
    [SerializeField] private TextMeshProUGUI _playerGold;

    [SerializeField] private TextMeshProUGUI _playerWood;

    [SerializeField] private TextMeshProUGUI _playerName;

    [SerializeField] private CurrencyManager _currencyManager;
    public string PlayerName { get => _playerName.text; }

    private void Start()
    {
        _playerGold.text = $"{_currencyManager.PlayerGold}/{_currencyManager.PlayerGoldLimit}";

        _playerWood.text = $"{_currencyManager.PlayerWood}/{_currencyManager.PlayerWoodLimit}";

        _currencyManager.OnGoldUpdate += CurrencyManager_OnGoldUpdate;
    }

    private void CurrencyManager_OnGoldUpdate(object sender, System.EventArgs e)
    {
        _playerGold.text = $"{_currencyManager.PlayerGold}/{_currencyManager.PlayerGoldLimit}";
        _playerWood.text = $"{_currencyManager.PlayerWood}/{_currencyManager.PlayerWoodLimit}";
    }

    public void LoadData(GameData gameData)
    {
        _playerName.text = gameData.PlayerName;
    }

    public void SaveData(GameData gameData)
    {
        gameData.PlayerName = _playerName.text;
    }
}