using NUnit.Framework;
using UnityEngine;

namespace DreamQuiz.Tests
{
    public class GoalRewardTest : MonoBehaviour
    {
        [Test]
        public void GoalReward_Constructor_HappyPath()
        {
            int rewardValue = 1;
            CurrencyType currencyType = CurrencyType.Gold;
            CollectibleType collectibleType = CollectibleType.None;
            bool givePartialReward = true;

            var goalReward = new GoalReward(rewardValue, currencyType, collectibleType, givePartialReward);

            Assert.IsTrue(goalReward.TotalReward == rewardValue,
                $"[GoalReward] Expected total reward {rewardValue} recieved {goalReward.TotalReward}");

            Assert.IsTrue(goalReward.CurrencyType == currencyType,
                $"[GoalReward] Expected currency type {currencyType} recieved {goalReward.CurrencyType}");

            Assert.IsTrue(goalReward.CollectibleType == collectibleType,
                $"[GoalReward] Expected collectible type {collectibleType} recieved {goalReward.CollectibleType}");

            Assert.IsTrue(goalReward.GivePartialReward == givePartialReward,
                $"[GoalReward] Expected give partial reward {givePartialReward} recieved {goalReward.GivePartialReward}");
        }
    }
}