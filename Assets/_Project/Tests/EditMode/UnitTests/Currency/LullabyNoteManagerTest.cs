using NUnit.Framework;
using UnityEngine;

public class LullabyNoteManagerTest : MonoBehaviour
{
    private LullabyNoteManager lullabyNoteManager;

    [SetUp]
    public void SetUp()
    {
        GameObject gameObject = new GameObject();
        lullabyNoteManager = gameObject.AddComponent<LullabyNoteManager>();
    }

    [Test]
    [TestCase(15000)]
    [TestCase(51)]
    public void AddLullabyNotesDontIgnoreLimit_HappyPath(int amountToAdd)
    {
        // Arrange
        int initialCount = lullabyNoteManager.CurrentLullabyNoteCount;
        // Act
        lullabyNoteManager.AddLullabyNotes(amountToAdd, false);

        // Assert
        Assert.AreNotEqual(initialCount + amountToAdd, lullabyNoteManager.CurrentLullabyNoteCount);
    }

    [Test]
    [TestCase(10)]
    [TestCase(0)]
    [TestCase(50)]
    [TestCase(15000)]
    public void AddLullabyNotesIgnoreLimit_HappyPath(int amountToAdd)
    {
        // Arrange
        int initialCount = lullabyNoteManager.CurrentLullabyNoteCount;

        // Act
        lullabyNoteManager.AddLullabyNotes(amountToAdd, true);
        int expectedAmount = initialCount + amountToAdd;

        // Assert
        Assert.AreEqual(initialCount + amountToAdd, expectedAmount);
    }

    [Test]
    [TestCase(10)]
    [TestCase(0)]
    [TestCase(50)]
    [TestCase(15000)]
    public void AddLullabyNotes_HappyPath(int amountToAdd)
    {
        // Arrange
        int initialCount = lullabyNoteManager.CurrentLullabyNoteCount;

        // Act
        lullabyNoteManager.AddLullabyNotes(amountToAdd, true);
        int expectedAmount = initialCount + amountToAdd;

        // Assert
        Assert.AreEqual(initialCount + amountToAdd, expectedAmount);
    }

    [Test]
    public void UseLullabyNote_HappyPath()
    {
        // Arrange
        lullabyNoteManager.AddLullabyNotes(10);
        int startAmount = lullabyNoteManager.CurrentLullabyNoteCount;

        // Act
        lullabyNoteManager.UseLullabyNote();
        int expectedAmount = startAmount - 1;
        int currentAmount = lullabyNoteManager.CurrentLullabyNoteCount;

        // Assert
        Assert.AreEqual(currentAmount, expectedAmount);
    }

    [Test]
    [TestCase(1)]
    [TestCase(50)]
    public void HasLullabyNote_HappyPath(int amountToAdd)
    {
        // Arrange
        lullabyNoteManager.AddLullabyNotes(amountToAdd);
        // Act
        bool hasLullabyNote = lullabyNoteManager.HasLullabyNote;

        // Assert
        Assert.IsTrue(hasLullabyNote);
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void HasLullabyNote_ShouldReturnFalse(int amountToAdd)
    {
        // Arrange
        lullabyNoteManager.AddLullabyNotes(amountToAdd);
        // Act
        bool hasLullabyNote = lullabyNoteManager.HasLullabyNote;

        // Assert
        Assert.IsFalse(hasLullabyNote);
    }
}