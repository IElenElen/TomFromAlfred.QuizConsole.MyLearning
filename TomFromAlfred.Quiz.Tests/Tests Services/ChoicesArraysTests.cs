using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.Tests
{
    public class ChoicesArraysTests
    {
        [Fact]
        public void Constructor_InitializesChoiceServiceApp() //test 3xA
        {
            //Arrange
            ChoicesArraysServiceApp choicesManager;

            //Act
            choicesManager = new ChoicesArraysServiceApp();

            //Assert
            Assert.NotNull(choicesManager);
            Assert.NotNull(choicesManager.Choice0Array);
        }
        
        [Fact]
        public void Choice0Array_DoesNotContainNulls() //test 3xA
        {
            // Arrange
            ChoicesArraysServiceApp choicesManager = new ChoicesArraysServiceApp();

            // Act
            Choice[] choice0Array = choicesManager.Choice0Array;

            // Assert
            Assert.All(choice0Array, choice => Assert.NotNull(choice));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void GetChoicesForQuestion_ReturnsCorrectChoices(int questionNumber) //test 3xA
        {
            // Arrange
            ChoicesArraysServiceApp choicesManager = new ChoicesArraysServiceApp();

            // Act
            Choice[] choices = choicesManager.GetChoicesForQuestion(questionNumber);

            // Assert
            Assert.NotNull(choices);
            Assert.NotEmpty(choices); // czy tablica nie jest pusta?
        }

        [Fact]
        public void GetChoicesForQuestion_ReturnsEmptyArrayForInvalidQuestionNumber() //test 3xA
        {
            //Arrange
            ChoicesArraysServiceApp choicesManager = new ChoicesArraysServiceApp();
            //Act
            Choice[] choices = choicesManager.GetChoicesForQuestion(-3);
            //Assert
            Assert.NotNull(choices);
            Assert.Empty(choices); //czy tablica jest pusta?
        }
    }
}
