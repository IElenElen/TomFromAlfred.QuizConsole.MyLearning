using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.Tests
{
    public class ChoicesManagerTests
    {
        [Fact]
        public void Constructor_InitializesChoiceServiceApp()
        {
            //Arrange
            ChoicesManagerApp choicesManager;

            //Act
            choicesManager = new ChoicesManagerApp();

            //Assert
            Assert.NotNull(choicesManager);
            Assert.NotNull(choicesManager.Choice0Array);
        }
        
        [Fact]
        public void Choice0Array_DoesNotContainNulls()
        {
            // Arrange
            ChoicesManagerApp choicesManager = new ChoicesManagerApp();

            // Act
            Choice[] choice0Array = choicesManager.Choice0Array;

            // Assert
            Assert.All(choice0Array, choice => Assert.NotNull(choice));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void GetChoicesForQuestion_ReturnsCorrectChoices(int questionNumber)
        {
            // Arrange
            ChoicesManagerApp choicesManager = new ChoicesManagerApp();

            // Act
            Choice[] choices = choicesManager.GetChoicesForQuestion(questionNumber);

            // Assert
            Assert.NotNull(choices);
            Assert.NotEmpty(choices); // czy tablica nie jest pusta?
        }

        [Fact]
        public void GetChoicesForQuestion_ReturnsEmptyArrayForInvalidQuestionNumber()
        {
            //Arrange
            ChoicesManagerApp choicesManager = new ChoicesManagerApp();
            //Act
            Choice[] choices = choicesManager.GetChoicesForQuestion(-3);
            //Assert
            Assert.NotNull(choices);
            Assert.Empty(choices); //czy tablica jest pusta?
        }
    }
}
