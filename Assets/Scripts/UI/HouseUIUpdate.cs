using TMPro;
using UnityEngine;

public class HouseUIUpdate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _houseLevel;

    [SerializeField] private TextMeshProUGUI _houseWood;

    [SerializeField] private TextMeshProUGUI _houseStoredGold;

    [SerializeField] private HouseManager _houseManager;

    private void Start()
    {
        UpdateHouseUIStatus();
        _houseManager.OnHouseUpgrade += HouseUpgrade_OnHouseUpgrade;
    }

    private void HouseUpgrade_OnHouseUpgrade(object sender, System.EventArgs e)
    {
        UpdateHouseUIStatus();
    }

    private void UpdateHouseUIStatus()
    {
        if (_houseManager.CurrentHouseLevel < 3)
        {
            _houseLevel.text = $"Lvl {_houseManager.CurrentHouseLevel}";
            _houseStoredGold.text = $"{_houseManager.HouseGold} / {_houseManager.GoldUpgradeCost}";
            _houseWood.text = $"{_houseManager.HouseWood} / {_houseManager.WoodUpgradeCost}";
        }
        else if (_houseManager.CurrentHouseLevel >= 3)
        {
            _houseLevel.text = "MAX";
            _houseStoredGold.text = "MAX";
            _houseWood.text = "MAX";
        }
    }
}