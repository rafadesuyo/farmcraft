using NUnit.Framework;

namespace DreamQuiz.Tests.Quiz
{
    public class QuizCategoryFormatterTest
    {
        [TestCase("general knowledge", QuizCategory.GeneralKnowledge)]
        [TestCase("arts and entertainment", QuizCategory.ArtsAndEntertainment)]
        [TestCase("science", QuizCategory.Science)]
        [TestCase("puzzles", QuizCategory.Puzzles)]
        [TestCase("human sciences", QuizCategory.HumanSciences)]
        [TestCase("sports", QuizCategory.Sports)]
        public void GetQuizCategoryByCategoryName_HappyPath_ShouldReturnCorrectCategory(string categoryName, QuizCategory expectedQuizCategory)
        {
            // Act
            QuizCategory category = QuizCategoryMaps.GetQuizCategoryByCategoryName(categoryName);

            // Assert
            Assert.AreEqual(expectedQuizCategory, category);
        }

        [Test]
        public void GetQuizCategoryByCategoryName_InvalidCategory_ShouldReturnRandomCategoryForInvalidName()
        {
            // Arrange
            string categoryName = "invalid category";

            // Act
            QuizCategory category = QuizCategoryMaps.GetQuizCategoryByCategoryName(categoryName);

            // Assert
            Assert.AreEqual(QuizCategory.Random, category);
        }

        [TestCase("general knowledge", QuizCategory.GeneralKnowledge)]
        [TestCase("arts and entertainment", QuizCategory.ArtsAndEntertainment)]
        [TestCase("science", QuizCategory.Science)]
        [TestCase("puzzles", QuizCategory.Puzzles)]
        [TestCase("human sciences", QuizCategory.HumanSciences)]
        [TestCase("sports", QuizCategory.Sports)]
        public void GetCategoryNameByQuizCategory_HappyPath_ShouldReturnCorrectName(string categoryName, QuizCategory expectedQuizCategory)
        {
            // Act
            QuizCategory category = QuizCategoryMaps.GetQuizCategoryByCategoryName(categoryName);

            // Assert
            Assert.AreEqual(expectedQuizCategory, category);
        }
    }
}

