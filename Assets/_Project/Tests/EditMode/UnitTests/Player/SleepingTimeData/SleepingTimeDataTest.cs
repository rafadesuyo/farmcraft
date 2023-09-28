using DreamQuiz.Player;
using NUnit.Framework;
using static DreamQuiz.Player.SleepingTimeData;

namespace DreamQuiz.Tests.Player
{
    public class SleepingTimeDataTest
    {
        [Test]
        [TestCase(SleepingTimeModifier.Poison, 1)]
        [TestCase(SleepingTimeModifier.Armor, 2)]
        [TestCase(SleepingTimeModifier.Poison, 100)]
        [TestCase(SleepingTimeModifier.Armor, 0)]
        public void HasModifier_True_HappyPath(SleepingTimeModifier modifier, float value)
        {
            //arrange
            SleepingTimeData sleepingTimeData = new SleepingTimeData(100, 100);

            //act
            sleepingTimeData.AddModifier(modifier, value);
            bool hasModifier = sleepingTimeData.HasModifier(modifier);

            //assert
            Assert.IsTrue(hasModifier);
        }

        [Test]
        [TestCase(SleepingTimeModifier.Poison)]
        [TestCase(SleepingTimeModifier.Armor)]
        [TestCase(3)]
        [TestCase(-1)]
        public void HasModifier_ShouldReturnFalse(SleepingTimeModifier modifier)
        {
            //arrange
            SleepingTimeData sleepingTimeData = new SleepingTimeData(100, 100);

            //act
            bool hasModifier = sleepingTimeData.HasModifier(modifier);

            //assert
            Assert.IsFalse(hasModifier);
        }

        [Test]
        [TestCase(1f, SleepingTimeModifier.Poison, 100)]
        [TestCase(0.5f, SleepingTimeModifier.Poison, 50)]
        public void AddModifierOverride_HappyPath(float overrideModifier, SleepingTimeModifier modifier, int startingSleepingTime)
        {
            //arrange
            SleepingTimeData sleepingTimeData = new SleepingTimeData(startingSleepingTime, 100);

            //act
            sleepingTimeData.AddModifier(modifier, 1);
            sleepingTimeData.AddModifierOverride(overrideModifier);
            sleepingTimeData.Use(100);
            float expectedValue = startingSleepingTime - (100 * overrideModifier);

            //assert
            Assert.AreEqual(expectedValue, sleepingTimeData.CurrentValue);
        }

        [Test]
        [TestCase(-1f, SleepingTimeModifier.Poison, 100)]
        public void AddModifierOverride_ReturnInvalidValue(float overrideModifier, SleepingTimeModifier modifier, int startingSleepingTime)
        {
            //arrange
            SleepingTimeData sleepingTimeData = new SleepingTimeData(startingSleepingTime, 100);

            //act
            sleepingTimeData.AddModifier(modifier, 1);
            sleepingTimeData.AddModifierOverride(overrideModifier);
            sleepingTimeData.Use(100);
            float expectedValue = startingSleepingTime - (100 * overrideModifier);

            //assert
            Assert.AreNotEqual(expectedValue, sleepingTimeData.CurrentValue);
        }

        [Test]
        [TestCase(2f, SleepingTimeModifier.Poison)]
        [TestCase(0.4f, SleepingTimeModifier.Poison)]
        [TestCase(-0.4f, SleepingTimeModifier.Poison)]
        [TestCase(0, SleepingTimeModifier.Poison)]
        public void RemoveModifierOverride_HappyPath(float overrideModifier, SleepingTimeModifier modifier)
        {
            //arrange
            SleepingTimeData sleepingTimeData = new SleepingTimeData(100, 100);

            //act
            sleepingTimeData.AddModifier(modifier, 1);
            sleepingTimeData.AddModifierOverride(overrideModifier);
            sleepingTimeData.RemoveModifierOverride();
            sleepingTimeData.Use(100);
            float expectedValue = 0;

            //assert
            Assert.AreEqual(expectedValue, sleepingTimeData.CurrentValue);
        }

        [Test]
        [TestCase(SleepingTimeModifier.Poison, 1)]
        [TestCase(SleepingTimeModifier.Armor, 1)]
        public void AddModifier_HappyPath(SleepingTimeModifier modifier, float value)
        {
            //arrange
            SleepingTimeData sleepingTimeData = new SleepingTimeData(100, 100);

            //act
            sleepingTimeData.AddModifier(modifier, value);
            bool hasModifier = sleepingTimeData.HasModifier(modifier);

            //assert
            Assert.IsTrue(hasModifier);
        }

        [Test]
        [TestCase(SleepingTimeModifier.Poison, 1)]
        [TestCase(SleepingTimeModifier.Armor, 1)]
        public void RemoveModifier_HappyPath(SleepingTimeModifier modifier, float value)
        {
            //arrange
            SleepingTimeData sleepingTimeData = new SleepingTimeData(100, 100);

            //act
            sleepingTimeData.AddModifier(modifier, value);
            sleepingTimeData.RemoveModifier(modifier);
            bool hasModifier = sleepingTimeData.HasModifier(modifier);

            //assert
            Assert.IsFalse(hasModifier);
        }

        [Test]
        [TestCase(SleepingTimeModifier.Poison, 1.5f)]
        [TestCase(SleepingTimeModifier.Armor, 1.5f)]
        public void AddModifierNerf_HappyPath(SleepingTimeModifier modifier, float value)
        {
            //arrange
            SleepingTimeData sleepingTimeData = new SleepingTimeData(100, 100);
            int expectedValue = -30;

            //act
            sleepingTimeData.AddModifier(modifier, value);
            sleepingTimeData.AddModifierNerf(modifier, 0.2f);
            sleepingTimeData.Use(100);

            //assert
            Assert.AreEqual(expectedValue, sleepingTimeData.CurrentValue);
        }

        [Test]
        [TestCase(SleepingTimeModifier.Poison, 1.5f)]
        [TestCase(SleepingTimeModifier.Armor, 1.5f)]
        public void RemoveModifierNerf_HappyPath(SleepingTimeModifier modifier, float value)
        {
            //arrange
            SleepingTimeData sleepingTimeData = new SleepingTimeData(100, 100);
            int expectedValue = -50;

            //act
            sleepingTimeData.AddModifier(modifier, value);
            sleepingTimeData.AddModifierNerf(modifier, 0.2f);
            sleepingTimeData.RemoveModifierNerf(modifier, 0.2f);
            sleepingTimeData.Use(100);

            //assert
            Assert.AreEqual(expectedValue, sleepingTimeData.CurrentValue);
        }

        [Test]
        [TestCase(100, 100)]
        [TestCase(150, 150)]
        [TestCase(101, 101)]
        public void HasSleepingTimeToMove_HappyPath(int initialValue, int maxValue)
        {
            //arrange
            SleepingTimeData sleepingTimeData = new SleepingTimeData(initialValue, maxValue);

            //act
            var hasSleepingTimeToMove = sleepingTimeData.HasSleepingTimeToMove(100);

            //assert
            Assert.IsTrue(hasSleepingTimeToMove, $"The player currently has {initialValue} out of {maxValue} Sleeping Time");
        }

        [Test]
        [TestCase(99, 100)]
        [TestCase(-50, 100)]
        public void HasSleepingTimeToMove_ShouldReturnFalse(int initialValue, int maxValue)
        {
            //arrange
            SleepingTimeData sleepingTimeData = new SleepingTimeData(initialValue, maxValue);

            //act
            var canMove = sleepingTimeData.HasSleepingTimeToMove(100);

            //assert
            Assert.IsFalse(canMove, $"The player currently has {initialValue} out of {maxValue} Sleeping Time");
        }

        [Test]
        [TestCase(100)]
        [TestCase(1)]
        [TestCase(1500)]
        [TestCase(0)]
        public void SetValue_HappyPath(int newMinValue)
        {
            //arrange
            SleepingTimeData sleepingTimeData = new SleepingTimeData(50, 100);

            //act
            sleepingTimeData.SetValue(newMinValue);
            int expectedValue = newMinValue;

            //assert
            Assert.AreEqual(expectedValue, sleepingTimeData.CurrentValue);
        }

        [Test]
        [TestCase(50, 50)]
        [TestCase(1, 99)]
        public void Add_HappyPath(int valueToAdd, int startingValue)
        {
            //arrange
            SleepingTimeData sleepingTimeData = new SleepingTimeData(startingValue, 100);

            //act
            sleepingTimeData.Add(valueToAdd);
            int expectedValue = startingValue + valueToAdd;

            //assert
            Assert.AreEqual(sleepingTimeData.CurrentValue, expectedValue);
        }
    }
}