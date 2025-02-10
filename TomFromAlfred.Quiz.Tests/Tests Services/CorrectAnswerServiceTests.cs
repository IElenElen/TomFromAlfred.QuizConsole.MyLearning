using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    public class CorrectAnswerServiceTests
    {
        private readonly CorrectAnswerService _correctAnswerService;

        public CorrectAnswerServiceTests() 
        {
            _correctAnswerService = new CorrectAnswerService();
        }

        [Fact]
        public void Add_ShouldAddCorrectAnswer_WhenValidEntityIsGiven()
        {
            // Arrange
            var correctAnswer = new CorrectAnswer(14, "Zima");

            // Act
            _correctAnswerService.Add(correctAnswer);
            var result = _correctAnswerService.GetAllActive();

            // Assert
            Assert.Contains(correctAnswer, result);
        }

        [Fact]
        public void Add_ShouldNotAddDuplicateCorrectAnswer_WhenSameIdIsGiven()  // Oblany
        {
            // Arrange
            var existingAnswer = new CorrectAnswer(11, "Jesień"); // Odpowiedź już istnieje

            // Act
            _correctAnswerService.Add(existingAnswer); // Próba dodania duplikatu
            var result = _correctAnswerService.GetAllActive();

            // Assert
            Assert.Equal(3, result.Count()); // Nie dodaję duplikatu, więc lista ma nadal 3 elementy
        }


        [Fact]
        public void Delete_ShouldRemoveCorrectAnswer_WhenValidEntityIsGiven()
        {
            // Arrange
            var correctAnswer = new CorrectAnswer(11, "Jesień");

            // Act
            _correctAnswerService.Delete(correctAnswer);
            var result = _correctAnswerService.GetAllActive();

            // Assert
            Assert.DoesNotContain(correctAnswer, result);
        }

        [Fact]
        public void Delete_ShouldNotRemoveCorrectAnswer_WhenEntityDoesNotExist()
        {
            // Arrange
            var nonExistingAnswer = new CorrectAnswer(999, "Brak");

            // Act
            _correctAnswerService.Delete(nonExistingAnswer); // Próba usunięcia nieistniejącej odpowiedzi
            var result = _correctAnswerService.GetAllActive();

            // Assert
            Assert.Equal(3, result.Count()); // Lista nie zmienia się, ponieważ odpowiedź nie istniała
        }

        [Fact]
        public void GetAll_ShouldReturnAllCorrectAnswers()
        {
            // Act
            var result = _correctAnswerService.GetAllActive();

            // Assert
            Assert.Equal(3, result.Count()); // Sprawdzam, czy są 3 poprawne odpowiedzi
        }

        [Fact]
        public void Update_ShouldUpdateCorrectAnswer_WhenValidEntityIsGiven()
        {
            // Arrange
            var updatedAnswer = new CorrectAnswer(11, "Zima");

            // Act
            _correctAnswerService.Update(updatedAnswer);
            var result = _correctAnswerService.GetCorrectAnswerForQuestion(11);

            // Assert
            Assert.Equal("Zima", result.CorrectAnswerContent);
        }

        [Fact]
        public void Update_ShouldNotUpdateCorrectAnswer_WhenEntityDoesNotExist()
        {
            // Arrange
            var nonExistingAnswer = new CorrectAnswer(999, "Brak");

            // Act
            _correctAnswerService.Update(nonExistingAnswer); // Próba aktualizacji nieistniejącej odpowiedzi
            var result = _correctAnswerService.GetCorrectAnswerForQuestion(999);

            // Assert
            Assert.Null(result); // Odpowiedź o takim Id nie istnieje, więc nie powinno być zmiany
        }


        [Fact]
        public void GetCorrectAnswerForQuestion_ShouldReturnCorrectAnswer_WhenValidQuestionIdIsGiven()
        {
            // Act
            var result = _correctAnswerService.GetCorrectAnswerForQuestion(12);

            // Assert
            Assert.Equal("Warszawa", result.CorrectAnswerContent);
        }

        [Fact]
        public void GetCorrectAnswerForQuestion_ShouldReturnNull_WhenInvalidQuestionIdIsGiven()
        {
            // Act
            var result = _correctAnswerService.GetCorrectAnswerForQuestion(999); // Id, które nie istnieje

            // Assert
            Assert.Null(result); // Zwrot null
        }

    }
}
