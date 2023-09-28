using DreamQuiz.Player;
using NUnit.Framework;

namespace DreamQuiz.Tests.Player
{
    public class PlayerStageDataTest
    {
        [Test]
        public void PlayerStageData_Constructor_HappyPath()
        {
            int maxSleepingTime = 1;
            var playerData = new PlayerData();
            playerData.MaxSleepingTime = maxSleepingTime;
            var playerStageData = new PlayerStageData(playerData);

            Assert.IsTrue(
                playerStageData.SleepingTime.MaxValue == maxSleepingTime,
                $"[Constructor] Expected MaxSleepingTime: {maxSleepingTime}. Received {playerStageData.SleepingTime.MaxValue}");

            Assert.IsTrue(
                playerStageData.SleepingTime.CurrentValue == maxSleepingTime,
                $"[Constructor] Expected CurrentSleepingTime: {maxSleepingTime}. Received {playerStageData.SleepingTime.CurrentValue}");
        }

        [Test]
        public void PlayerStageData_Constructor_NullReferences()
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //assert
            Assert.IsNotNull(
                playerData, $"{playerData} is Null");

            Assert.IsNotNull(
                playerStageData, $"{playerStageData} is Null");
        }

        [Test]
        public void PlayerStageData_SetSleepingTime_HappyPath()
        {
            int maxSleepingTime = 1;
            var playerData = new PlayerData();
            playerData.MaxSleepingTime = maxSleepingTime;
            var playerStageData = new PlayerStageData(playerData);
            int expectedSleepingTime = 2;

            playerStageData.SleepingTime.OnSleepingTimeChanged += (value) =>
            {
                AssertSleepingTimeCallback(value, expectedSleepingTime, "SetSleepingTime");
            };

            playerStageData.SleepingTime.SetValue(expectedSleepingTime);

            playerStageData.SleepingTime.OnSleepingTimeChanged -= (value) =>
            {
                AssertSleepingTimeCallback(value, expectedSleepingTime, "SetSleepingTime");
            };
        }

        [Test]
        public void PlayerStageData_AddSleepingTime_HappyPath()
        {
            int maxSleepingTime = 1;
            var playerData = new PlayerData();
            playerData.MaxSleepingTime = maxSleepingTime;
            var playerStageData = new PlayerStageData(playerData);
            int sleepingTimeToAdd = 1;
            int expectedSleepingTime = maxSleepingTime + sleepingTimeToAdd;

            playerStageData.SleepingTime.OnSleepingTimeChanged += (value) =>
            {
                AssertSleepingTimeCallback(value, expectedSleepingTime, "AddSleepingTime");
            };

            playerStageData.SleepingTime.Add(sleepingTimeToAdd);

            playerStageData.SleepingTime.OnSleepingTimeChanged -= (value) =>
            {
                AssertSleepingTimeCallback(value, expectedSleepingTime, "AddSleepingTime");
            };
        }

        [Test]
        public void PlayerStageData_UseSleepingTime_HappyPath()
        {
            int maxSleepingTime = 1;
            var playerData = new PlayerData();
            playerData.MaxSleepingTime = maxSleepingTime;
            var playerStageData = new PlayerStageData(playerData);
            int sleepingTimeToConsume = 1;
            int expectedSleepingTime = maxSleepingTime - sleepingTimeToConsume;

            playerStageData.SleepingTime.OnSleepingTimeChanged += (value) =>
            {
                AssertSleepingTimeCallback(value, expectedSleepingTime, "UseSleepingTime");
            };

            playerStageData.SleepingTime.Use(sleepingTimeToConsume);

            playerStageData.SleepingTime.OnSleepingTimeChanged -= (value) =>
            {
                AssertSleepingTimeCallback(value, expectedSleepingTime, "UseSleepingTime");
            };
        }

        [Test]
        [TestCase(1f)]
        [TestCase(0f)]
        [TestCase(50f)]
        public void AddPoison_HappyPath(float valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            float expectedAmount = playerStageData.PoisonMultiplier + valueToAdd;
            playerStageData.AddPoison(valueToAdd);

            //assert
            Assert.IsTrue(
               expectedAmount == playerStageData.PoisonMultiplier,
               $"Poison value is: [{playerStageData.PoisonMultiplier}], Expected value was:[{expectedAmount}]");
        }

        [Test]
        [TestCase(1f)]
        [TestCase(0f)]
        [TestCase(50f)]
        public void RemovePoison_HappyPath(float valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            playerStageData.AddPoison(valueToAdd);
            playerStageData.RemovePoison();

            //assert
            Assert.IsTrue(
               playerStageData.PoisonMultiplier == 0,
               $"Poison value is: [{playerStageData.PoisonMultiplier}], Expected value was:[0]");
        }

        [Test]
        [TestCase(1f)]
        [TestCase(0f)]
        [TestCase(50f)]
        public void RemovePoisonAmount_HappyPath(float valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);
            float expectedAmount = playerStageData.PoisonMultiplier - valueToAdd;
            float startingAmount = playerStageData.PoisonMultiplier;

            //act
            playerStageData.AddPoison(valueToAdd);
            playerStageData.RemovePoison(valueToAdd);

            //assert
            Assert.IsTrue(
               expectedAmount == startingAmount - valueToAdd,
               $"Poison value is: [{playerStageData.PoisonMultiplier}], Expected value was:[{expectedAmount}]");
        }

        [Test]
        [TestCase(5.0f)]
        [TestCase(0f)]
        public void AddArmor_HappyPath(float valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            float expectedValue = playerStageData.ArmorMultiplier + valueToAdd;
            playerStageData.AddArmor(valueToAdd);

            //assert
            Assert.IsTrue(
               expectedValue == playerStageData.ArmorMultiplier,
               $"Armor value is: [{playerStageData.ArmorMultiplier}], Expected value was:[{expectedValue}]");
        }

        [Test]
        [TestCase(5.0f)]
        [TestCase(0f)]
        public void RemoveArmor_HappyPath(float valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            playerStageData.AddArmor(valueToAdd);
            playerStageData.RemoveArmor();

            //assert
            Assert.IsTrue(
               playerStageData.ArmorMultiplier == 0,
               $"Armor value is: [{playerStageData.ArmorMultiplier}], Expected value was:[0]");
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(0)]
        [TestCase(100)]
        public void AddSheepsCollected_HappyPath(int valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            int expectedValue = playerStageData.SheepsCollected + valueToAdd;
            playerStageData.AddSheepsCollected(valueToAdd);

            //assert
            Assert.IsTrue(
               expectedValue == playerStageData.SheepsCollected,
               $"Added value was: [{valueToAdd}], Expected value was:[{expectedValue}]");
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(0)]
        [TestCase(100)]
        public void SetSheepsCollected_HappyPath(int valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            playerStageData.SetSheepsCollected(valueToAdd);

            //assert
            Assert.IsTrue(
               valueToAdd == playerStageData.SheepsCollected,
               $"New value is: [{playerStageData.SheepsCollected}], Expected value was:[{valueToAdd}]");
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(100)]
        public void Check_If_SheepsCollected_IsSetNegative(int valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            playerStageData.SetSheepsCollected(-valueToAdd);

            //assert
            Assert.IsTrue(
               valueToAdd == playerStageData.SheepsCollected,
               $"Player SheepsCollected was set to a negative value: [ {playerStageData.SheepsCollected} ]");
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(0)]
        [TestCase(100)]
        public void SetWolvesCollected_HappyPath(int valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            playerStageData.SetWolvesCollected(valueToAdd);

            //assert
            Assert.IsTrue(
               valueToAdd == playerStageData.WolvesCollected,
               $"New value is: [{playerStageData.WolvesCollected}], Expected value was:[{valueToAdd}]");
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(100)]
        public void Check_If_WolvesCollected_IsSetNegative(int valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            playerStageData.SetWolvesCollected(-valueToAdd);

            //asserts
            Assert.IsFalse(
                playerStageData.WolvesCollected < 0,
                $"Player WolvesCollected was set to a negative value: [ {playerStageData.WolvesCollected} ]");
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(0)]
        [TestCase(100)]
        public void AddWolvesCollected_HappyPath(int valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            int expectedValue = playerStageData.WolvesCollected + valueToAdd;
            playerStageData.AddWolvesCollected(valueToAdd);

            //assert
            Assert.IsTrue(
               expectedValue == playerStageData.WolvesCollected,
               $"Added value was: [{valueToAdd}], Expected value was:[{expectedValue}]");
        }

        [Test]
        [TestCase(1)]
        [TestCase(0)]
        [TestCase(150)]
        public void SetPowerCount_HappyPath(int valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            playerStageData.SetPowerCount(valueToAdd);

            //assert
            Assert.IsTrue(
               valueToAdd == playerStageData.PowerCount,
               $"New value is: [{playerStageData.PowerCount}], Expected value was:[{valueToAdd}]");
        }

        [Test]
        [TestCase(1)]
        [TestCase(24)]
        [TestCase(0)]
        public void AddPowerCount_HappyPath(int valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            int expectedValue = playerStageData.PowerCount + valueToAdd;
            playerStageData.AddPowerCount(valueToAdd);
            //assert
            Assert.IsTrue(
               expectedValue == playerStageData.PowerCount,
               $"Added value was: [{valueToAdd}], Expected value was:[{expectedValue}]");
        }

        [Test]
        [TestCase(1)]
        [TestCase(24)]
        [TestCase(0)]
        public void RemovePowerCount_HappyPath(int valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            int expectedValue = playerStageData.PowerCount;
            playerStageData.AddPowerCount(valueToAdd);
            playerStageData.RemovePowerCount(valueToAdd);
            //assert
            Assert.IsTrue(
               expectedValue == playerStageData.PowerCount,
               $"Added value was: [{valueToAdd}], Expected value was:[{expectedValue}]");
        }

        [Test]
        [TestCase(1)]
        [TestCase(50)]
        [TestCase(0)]
        public void SetKeyCount_HappyPath(int valueToChange)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            playerStageData.SetKeyCount(valueToChange);

            //assert
            Assert.IsTrue(
               valueToChange == playerStageData.KeyCount,
               $"New value is: [{playerStageData.KeyCount}], Expected value was:[{valueToChange}]");
        }

        [Test]
        [TestCase(1)]
        [TestCase(0)]
        [TestCase(27)]
        public void AddKeyCount_HappyPath(int valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            int expectedValue = playerStageData.KeyCount + valueToAdd;

            playerStageData.AddKeyCount(valueToAdd);

            //assert
            Assert.IsTrue(
                expectedValue == playerStageData.KeyCount,
                $"Added value was: [{valueToAdd}], Expected value was:[{expectedValue}]");
        }

        [Test]
        [TestCase(1)]
        [TestCase(0)]
        [TestCase(50)]
        public void RemoveKeyCount_HappyPath(int valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            int expectedValue = playerStageData.KeyCount + valueToAdd;

            playerStageData.AddKeyCount(valueToAdd);
            playerStageData.RemoveKeyCount(valueToAdd);

            //assert
            Assert.IsTrue(
                expectedValue == playerStageData.KeyCount + valueToAdd,
                $"Added value was: [{valueToAdd}], Expected value was:[{expectedValue}]");
        }

        [Test]
        [TestCase(1)]
        [TestCase(0)]
        [TestCase(100)]
        public void SetGoldCount_HappyPath(int valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            playerStageData.SetGoldCount(valueToAdd);
            //assert
            Assert.IsTrue(
                valueToAdd == playerStageData.GoldCount,
                $"Added value was: [{valueToAdd}], Expected value was:[{valueToAdd}]");
        }

        [Test]
        [TestCase(1)]
        [TestCase(0)]
        [TestCase(100)]
        public void AddGoldCount_HappyPath(int valueToAdd)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);
            int expectedValue = playerStageData.GoldCount + valueToAdd;

            //act
            playerStageData.AddGoldCount(valueToAdd);

            //assert
            Assert.IsTrue(
                expectedValue == playerStageData.GoldCount,
                $"Added value was: [{valueToAdd}], Expected value was:[{expectedValue}]");
        }

        [Test]
        [TestCase(1)]
        [TestCase(50)]
        public void Check_If_PlayerGold_IsSetNegative(int valueToChange)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            playerStageData.SetGoldCount(-valueToChange);

            //assert
            Assert.IsFalse(
                playerStageData.GoldCount < 0,
                $"Player gold was set to a negative value: [ {playerStageData.GoldCount} ]");
        }

        [Test]
        [TestCase(100)]
        [TestCase(1)]
        public void Check_If_PlayerGold_IsAddedNegative(int valueToChange)
        {
            //arrange
            var playerData = new PlayerData();
            var playerStageData = new PlayerStageData(playerData);

            //act
            playerStageData.AddGoldCount(-valueToChange);

            //assert
            Assert.IsFalse(
                playerStageData.GoldCount < 0,
                $"Player gold was added to a negative value: [ {playerStageData.GoldCount} ]");
        }

        private void AssertSleepingTimeCallback(float value, float expectedValue, string testName)
        {
            Assert.IsTrue(
                expectedValue == value,
                $"[{testName}] Expected CurrentSleepingTime: {expectedValue}. Received {value}");
        }
    }
}