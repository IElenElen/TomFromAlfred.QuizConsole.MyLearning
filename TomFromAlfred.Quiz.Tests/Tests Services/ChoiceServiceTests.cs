using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    public class ChoiceServiceTests
    {
        private readonly ChoiceService _choiceService;

        public ChoiceServiceTests()
        { 
            _choiceService = new ChoiceService();
        }

        [Fact]

        public void Add_ShouldAddNewChoice_WhenChoiceDoesNotExist()
        {
            //Arrange
            var newChoice0 = new Choice(22, 'A', "Opcja testowa A");
            var newChoice1 = new Choice(22, 'B', "Opcja testowa B");
            var newChoice2 = new Choice(22, 'C', "Ocpja testowa C");

            //Act
            _choiceService.Add(newChoice0);
            _choiceService.Add(newChoice1);
            _choiceService.Add(newChoice2);

            //Assert
            Assert.Contains(newChoice0, _choiceService.GetAll());
            Assert.Contains(newChoice1, _choiceService.GetAll());
            Assert.Contains(newChoice2, _choiceService.GetAll());
        }

        [Fact]
        public void Add_ShouldNotAdd_WhenChoiceExist() // Oblany
        {
            //Arrange
            var existingChoice = new Choice(22, 'A', "Opcja A");

            //Act
            _choiceService.Add(existingChoice);
            var allChoices = _choiceService.GetAll();

            //Assert
            Assert.Single(allChoices);
        }

        [Fact]
        public void Delete_ShouldRemoveExistingChoice()
        {
            //Arrange
            var choiceToDelete = new Choice(22, 'A', "Opcja A");

            //Act
            _choiceService.Delete(choiceToDelete);
            var allChoices = _choiceService.GetAll();


            //Assert
            Assert.DoesNotContain(choiceToDelete, allChoices);
        }

        [Fact]
        public void Delete_ShouldNotRemoveChoice_WhenChoiceDoesNotExist() // Oblany
        {
            var choiceToDelete = new Choice(89, 'A', "Opcja nieistniejąca");

            _choiceService.Delete(choiceToDelete);
            var allChoices = _choiceService.GetAll();

            Assert.Equal(8, allChoices.Count()); // Nic w liście nie zmieniam
        }

        [Fact]

        public void GetAll_ShouldReturnAllChoices() // Oblany
        {
            var allChoices = _choiceService.GetAll();
            Assert.Equal(8, allChoices.Count()); 
        }

        [Fact]
        public void GetChoicesForQuestion_ShouldReturnFilteredChoices_WhenQuestionIdExists()
        {
            var choicesForQuestion = _choiceService.GetChoicesForQuestion(11);
            Assert.Equal(3, choicesForQuestion.Count()); // Trzy wybory dla pytania nr 11
        }

        [Fact]
        public void GetChoicesForQuestion_ShouldReturnEmpty_WhenQuestionIdDoesNotExist()
        {
            var choicesForQuestion = _choiceService.GetChoicesForQuestion(100);
            Assert.Empty(choicesForQuestion); // Nie ma wyboru dla pytania 100
        }

        [Fact]
        public void Update_ShouldUpdateChoice_WhenChoiceExists() // Oblany
        {
            var choiceToUpdate = new Choice(3, 'A', "Jesień");
            choiceToUpdate.ChoiceContent = "Zmieniona Jesień";

            _choiceService.Update(choiceToUpdate);

            var updatedChoice = _choiceService.GetChoicesForQuestion(3).First(c => c.ChoiceLetter == 'A');
            Assert.Equal("Zmieniona Jesień", updatedChoice.ChoiceContent);
        }

        [Fact]
        public void Update_ShouldNotUpdate_WhenChoiceDoesNotExist() // Oblany
        {
            var choiceToUpdate = new Choice(99, 'D', "Nieistniejąca opcja");

            _choiceService.Update(choiceToUpdate);

            var allChoices = _choiceService.GetAll();
            Assert.Equal(8, allChoices.Count()); 
        }
    }
}
