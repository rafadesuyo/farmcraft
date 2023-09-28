using NUnit.Framework;
using UnityEngine;

namespace DreamQuiz.Tests
{
    public class StoreItemSOTests
    {
        [TestCase(0, 5, true)]
        [TestCase(3, 5, true)]
        [TestCase(5, 5, false)]
        [TestCase(7, 5, false)]
        public void CanBePurchased_PurchaseCountAndLimit_ReturnsCorrectValue(int purchaseCount, int purchaseLimit, bool expectedResult)
        {
            StoreItemSO storeItem = ScriptableObject.CreateInstance<StoreItemSO>();
            storeItem.PurchaseCount = purchaseCount;
            storeItem.PurchaseLimit = purchaseLimit;

            Assert.AreEqual(expectedResult, storeItem.CanBePurchased);
        }
    }
}