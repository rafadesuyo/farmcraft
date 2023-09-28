using System;
using UnityEngine;

public class HouseManager : MonoBehaviour, IDataPersistence, IUpgradable
{
    [SerializeField] private AudioManager _audioManager;

    [SerializeField] private GameObject _houseLvl1;

    [SerializeField] private GameObject _houseLvl2;

    [SerializeField] private GameObject _houseLvl3;

    private int _overflownGold;

    private int _overflownWood;

    [HideInInspector]
    public int HouseGold = 0;

    [HideInInspector]
    public int HouseWood = 0;

    [HideInInspector]
    public int WoodUpgradeCost = 5;

    [HideInInspector]
    public int GoldUpgradeCost = 10;

    [HideInInspector]
    public int CurrentHouseLevel;

    public event EventHandler OnHouseUpgrade;

    private void ActivateUpgrade()
    {
        switch (CurrentHouseLevel)
        {
            case 1:
                _houseLvl1.SetActive(true);
                break;

            case 2:
                _houseLvl1.SetActive(true);
                _houseLvl2.SetActive(true);
                break;

            case 3:
                _houseLvl1.SetActive(true);
                _houseLvl2.SetActive(true);
                _houseLvl3.SetActive(true);
                break;
        }
    }

    private void Start()
    {
        ActivateUpgrade();
    }

    public void HouseGoldUpdate(int goldAmount, int woodAmount)
    {
        if (CurrentHouseLevel < 3)
        {
            OverflowCheck(goldAmount, woodAmount);

            HouseGold += goldAmount;

            HouseWood += woodAmount;

            if (HouseGold >= GoldUpgradeCost && HouseWood >= WoodUpgradeCost)
            {
                HouseLevelUp();
            }

            OnHouseUpgrade?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OverflowCheck(int goldAmount, int woodAmount)
    {
        if ((goldAmount + HouseGold) > GoldUpgradeCost)
        {
            _overflownGold = goldAmount + HouseGold - GoldUpgradeCost;
        }

        if ((woodAmount + HouseWood) > WoodUpgradeCost)
        {
            _overflownWood = woodAmount + HouseWood - WoodUpgradeCost;
        }
    }

    private void HouseLevelUp()
    {
        CurrentHouseLevel++;
        ActivateUpgrade();
        HouseGold = 0;
        HouseWood = 0;
        HouseGold += _overflownGold;
        HouseWood += _overflownWood;
        _overflownWood = 0;
        _overflownGold = 0;
        UpdateCostMultiplier();
        _audioManager.PlaySound(_audioManager.UpgradeHouse);
    }

    private void UpdateCostMultiplier()
    {
        GoldUpgradeCost *= 2;
        WoodUpgradeCost *= 2;
    }

    public void LoadData(GameData gameData)
    {
        GoldUpgradeCost = gameData.HouseGoldUpgradeCost;
        CurrentHouseLevel = gameData.HouseLevel;
        HouseGold = gameData.HouseGold;
        HouseWood = gameData.HouseWood;
        WoodUpgradeCost = gameData.HouseWoodUpgradeCost;
    }

    public void SaveData(GameData gameData)
    {
        gameData.HouseGoldUpgradeCost = GoldUpgradeCost;
        gameData.HouseLevel = CurrentHouseLevel;
        gameData.HouseGold = HouseGold;
        gameData.HouseWood = HouseWood;
        gameData.HouseWoodUpgradeCost = WoodUpgradeCost;
    }
}