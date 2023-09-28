using NUnit.Framework;


public class PlayerProgressTest
{
    [TestCase (0,50)]
    [TestCase(15,40)]
    [TestCase(0,-229)]
    public void IncreaseXP_NoLevelIncrease(int startingXP, int amountToIncrease)
    {
        // Arrange
        GameData testGameData = new GameData();
        testGameData.totalXP = startingXP;
        testGameData.currentPlayerLevel = 1;
        int startingLevel = testGameData.currentPlayerLevel;
        PlayerProgress.EvaluateLoad(testGameData);
        int absAmountToIncrease = UnityEngine.Mathf.Abs(amountToIncrease);

        // Act
        PlayerProgress.IncreaseXP(amountToIncrease);

        // Assert
        Assert.IsTrue(PlayerProgress.SaveState.totalXP == (startingXP + absAmountToIncrease),
            $"Expected totalXP : {startingXP + absAmountToIncrease}, received {PlayerProgress.SaveState.totalXP}");

        Assert.IsFalse(PlayerProgress.SaveState.currentPlayerLevel > startingLevel,
            $"Expected {PlayerProgress.SaveState.currentPlayerLevel} to be EQUAL to {startingLevel}");
    }

    [TestCase(500,3000)]
    [TestCase(0,10000)]
    [TestCase(100,-2300)]
    public void IncreaseXP_LevelUp(int startingXP, int amountToIncrease)
    {
        // Arrange
        GameData testGameData = new GameData();
        testGameData.totalXP = startingXP;
        testGameData.currentPlayerLevel = 1;
        int startingLevel = testGameData.currentPlayerLevel;
        PlayerProgress.EvaluateLoad(testGameData);
        int absAmountToIncrease = UnityEngine.Mathf.Abs(amountToIncrease);

        // Act
        PlayerProgress.IncreaseXP(amountToIncrease);

        // Assert
        Assert.IsTrue(PlayerProgress.SaveState.totalXP == (startingXP + absAmountToIncrease),
            $"Expected totalXP : {startingXP + absAmountToIncrease}, received {PlayerProgress.SaveState.totalXP}");
        Assert.IsTrue(PlayerProgress.SaveState.currentPlayerLevel > startingLevel,
            $"Expected {PlayerProgress.SaveState.currentPlayerLevel} to be HIGHER THAN to {startingLevel}");
    }

    [TestCase(0,125)]
    [TestCase(90,400)]
    [TestCase(10,-200)]
    public void SetXP_HappyPath(int startingXP, int xpToSet)
    {
        //Arrange
        GameData testGameData = new GameData();
        testGameData.totalXP = startingXP;
        testGameData.currentPlayerLevel = 1;
        PlayerProgress.EvaluateLoad(testGameData);
        int absXPToSet = UnityEngine.Mathf.Abs(xpToSet);

        //Act
        PlayerProgress.SetXP(xpToSet);

        // Assert
        Assert.IsTrue(PlayerProgress.SaveState.totalXP == absXPToSet,
            $"Expected totalXPValue = {absXPToSet}, received {PlayerProgress.SaveState.totalXP}");
    }
}
