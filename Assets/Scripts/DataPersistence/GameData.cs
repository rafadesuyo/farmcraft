using System;

[Serializable]
public class GameData
{
    public DateTime LastSave { get; set; }

    public float TimeSpent { get; set; }

    public int PlayerGold { get; set; }

    public string PlayerName { get; set; }

    public int HouseLevel { get; set; }

    public int HouseGold { get; set; }

    public int HouseGoldUpgradeCost { get; set; }

    public int PlayerWood { get; set; }

    public int HouseWood { get; set; }

    public int HouseWoodUpgradeCost { get; set; }

    public bool PlayerCreated { get => !string.IsNullOrEmpty(PlayerName); }

    // This constructor is used to define the default data when a new game starts
    public GameData()
    {
        TimeSpent = 0f;
        PlayerGold = 0;
        PlayerName = string.Empty;
        HouseLevel = 0;
        HouseGold = 0;
        HouseGoldUpgradeCost = 10;
        HouseWoodUpgradeCost = 5;
        PlayerWood = 0;
        HouseWood = 0;
    }
}