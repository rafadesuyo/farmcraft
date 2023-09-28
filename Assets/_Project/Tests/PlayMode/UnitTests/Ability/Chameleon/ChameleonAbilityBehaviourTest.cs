using System.Collections;
using System.Collections.Generic;
using DreamQuiz.Player;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DreamQuiz.Tests
{
    public class ChameleonAbilityBehaviourTest
    {
        [SerializeField] AbilityDataSO abilityDataSO;

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void Initialize_HappyPath(int lvl)
        {
            //Arrange
            var playerData = new PlayerData()
            {
                MaxSleepingTime = 100
            };
            var playerStageData = new PlayerStageData(playerData);
            var collectibleAbility = new CollectibleAbility(abilityDataSO, lvl);
            var chameleonAbility = new ChameleonAbilityBehaviour();

            //Act
            chameleonAbility.Initialize(playerStageData, collectibleAbility, null);

            //Assert
            Assert.IsTrue(chameleonAbility.CurrentLevel == lvl);
        }
    }
}