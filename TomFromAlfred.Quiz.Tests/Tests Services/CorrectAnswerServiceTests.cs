using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Oblane: 3 / 9

    public class CorrectAnswerServiceTests
    {
        private readonly CorrectAnswerService _correctAnswerService;

        public CorrectAnswerServiceTests() 
        {
            _correctAnswerService = new CorrectAnswerService();
        }

        // 1
        [Fact] // Zaliczony
        public void Add_ShouldAddCorrectAnswer_WhenValidEntityIsGiven() // Dodaje poprawną odpowiedź, jeśli poprawne entity podane
        {
            // Arrange
            var correctAnswerToAdd = new CorrectAnswer(14, "Zima", true);

            // Act
            _correctAnswerService.Add(correctAnswerToAdd);
            var result = _correctAnswerService.GetAllActive();

            // Assert
            Assert.Contains(correctAnswerToAdd, result);
        }

        // 2
        [Fact] // Oblany, widzi 4
        public void Add_ShouldNotAddDuplicateCorrectAnswer_WhenSameIdIsGiven()  // Nie dodaje duplikatu poprawnej odpowiedzi
        {
            // Arrange
            var alreadyExistingCorrectAnswer = new CorrectAnswer(11, "Jesień", true); // Odpowiedź już istnieje

            // Act
            _correctAnswerService.Add(alreadyExistingCorrectAnswer); // Próba dodania duplikatu
            var result = _correctAnswerService.GetAllActive();

            // Assert
            Assert.Equal(8, result.Count()); // Nie dodaję duplikatu, więc lista ma nadal 8 elementów
        }

        // 3
        [Fact] // Zaliczony
        public void Delete_ShouldRemoveCorrectAnswer_WhenValidEntityIsGiven() // Usuwa, jeśli poprawne entity jest podane
        {
            // Arrange
            var existingCorrectAnswerToDelete = new CorrectAnswer(11, "Jesień", true);

            // Act
            _correctAnswerService.Delete(existingCorrectAnswerToDelete);
            var result = _correctAnswerService.GetAllActive();

            // Assert
            Assert.DoesNotContain(existingCorrectAnswerToDelete, result);
        }

        // 4
        [Fact] // Oblany, widzi 3 zamiast 8
        public void Delete_ShouldNotRemoveCorrectAnswer_WhenEntityDoesNotExist() // Bez poprawnego entity nic nie usuwa
        {
            // Arrange
            var nonExistingCorrectAnswer = new CorrectAnswer(999, "Brak", true);

            // Act
            _correctAnswerService.Delete(nonExistingCorrectAnswer); // Próba usunięcia nieistniejącej odpowiedzi
            var result = _correctAnswerService.GetAllActive().ToList();

            // Assert
            Assert.Equal(8, result.Count()); // Lista nie zmienia się, ponieważ odpowiedź nie istniała
        }

        // 5
        [Fact] // Oblany
        public void GetAll_ShouldReturnAllCorrectActiveAnswers() // Podaje wszystkie poprawne aktywne odpowiedzi
        {
            // Act
            var result = _correctAnswerService.GetAllActive().ToList();

            // Debug - wyświetl liczbę odpowiedzi
            Console.WriteLine($"Liczba odpowiedzi w GetAllActive(): {result.Count}");

            foreach (var correctAnswer in result)
            {
                Console.WriteLine($"Poprawna odpowiedź Id: {correctAnswer.CorrectAnswerId}, treść: {correctAnswer.CorrectAnswerContent}, aktywna: {correctAnswer.IsActive}");
            }

            // Assert
            Assert.Equal(8, result.Count()); // Sprawdzam, czy jest 8 poprawnych aktywnych odpowiedzi
        }

        // 6
        [Fact] // Zaliczony
        public void Update_ShouldUpdateCorrectAnswer_WhenValidEntityIsGiven() // Zmiana poprawnej odpowiedzi, jeśli entity poprawne dostarczone
        {
            // Arrange
            var correctAnswerToUpdate = new CorrectAnswer(11, "Zmieniona zima", true);

            // Act
            _correctAnswerService.Update(correctAnswerToUpdate);
            var result = _correctAnswerService.GetCorrectAnswerForQuestion(11);

            // Assert
            Assert.Equal("Zmieniona zima", result.CorrectAnswerContent);
        }

        // 7
        [Fact] // Zaliczony
        public void Update_ShouldNotUpdateCorrectAnswer_WhenEntityDoesNotExist() // Bez zmiany, jeśli entity nie istnieje
        {
            // Arrange
            var nonExistingCorrectAnswer = new CorrectAnswer(999, "Brak", true);

            // Act
            _correctAnswerService.Update(nonExistingCorrectAnswer); // Próba aktualizacji nieistniejącej odpowiedzi
            var result = _correctAnswerService.GetCorrectAnswerForQuestion(999);

            // Assert
            Assert.Null(result); // Odpowiedź o takim Id nie istnieje, więc nie powinno być zmiany
        }

        // 8
        [Fact] // Zaliczony
        public void GetCorrectAnswerForQuestion_ShouldReturnCorrectAnswer_WhenValidQuestionIdIsGiven() // Zwraca poprawną odpowiedź, jesli pytanie ma właściwe Id
        {
            // Act
            var result = _correctAnswerService.GetCorrectAnswerForQuestion(12);

            // Assert
            Assert.Equal("Warszawa", result.CorrectAnswerContent);
        }

        // 9
        [Fact] // Zaliczony
        public void GetCorrectAnswerForQuestion_ShouldReturnNull_WhenInvalidQuestionIdIsGiven() // Zwraca null, jeśli Id pytania jest niepoprawne
        {
            // Act
            var result = _correctAnswerService.GetCorrectAnswerForQuestion(999); // Id, które nie istnieje

            // Assert
            Assert.Null(result); // Zwrot null
        }
    }
}
