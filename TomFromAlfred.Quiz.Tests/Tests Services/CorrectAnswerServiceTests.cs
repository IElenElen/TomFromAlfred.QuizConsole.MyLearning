using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Oblane: 0 / 11

    public class CorrectAnswerServiceTests
    {
        private readonly CorrectAnswerService _correctAnswerService;

        public CorrectAnswerServiceTests()
        {
            _correctAnswerService = new CorrectAnswerService();
        }

        // 1 
        [Fact] // Zaliczony
        public void Add_ShouldAddCorrectAnswer_WhenValidEntityIsGiven() // Dodaje: poprawną odpowiedź, jeśli entity jest poprawnie podane
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
        [Fact] // Zaliczony
        public void Add_ShouldNotAddDuplicateCorrectAnswer_WhenSameIdIsGiven() // Dodaje: nie dodaje duplikatów, kiedy Id jest takie samo
        {
            // Arrange
            var initialCount = _correctAnswerService.GetAllActive().Count();
            var alreadyExistingCorrectAnswer = new CorrectAnswer(11, "Jesień", true);

            // Act
            _correctAnswerService.Add(alreadyExistingCorrectAnswer);
            var result = _correctAnswerService.GetAllActive();

            // Assert - Liczba elementów powinna być taka sama
            Assert.Equal(initialCount, result.Count());
        }

        // 3 
        [Fact] //Zaliczony
        public void Delete_ShouldRemoveCorrectAnswerById_WhenValidEntityIsGiven() // Usuwa: poprawną odpowiedź, jeśli poprawnie podane jest entity
        {
            // Arrange
            var correctAnswerService = new CorrectAnswerService();
            var correctAnswerToDelete = new CorrectAnswer(12, "Warszawa", true);

            // Nowa instancja z tym samym CorrectAnswerId
            var sameIdCorrectAnswer = new CorrectAnswer(12, "Zamość", true);

            // Act
            correctAnswerService.Delete(sameIdCorrectAnswer);

            // Assert
            var result = correctAnswerService.GetCorrectAnswerForQuestion(12);
            Assert.Null(result);
        }

        // 4 
        [Fact] // Zaliczony
        public void Delete_ShouldNotRemoveCorrectAnswer_WhenEntityDoesNotExist() // Usuwa: nic, jeżeli entity nie istnieje
        {
            // Arrange
            var initialCount = _correctAnswerService.GetAllActive().Count();
            var nonExistingCorrectAnswer = new CorrectAnswer(999, "Brak", true);

            // Act
            _correctAnswerService.Delete(nonExistingCorrectAnswer);
            var result = _correctAnswerService.GetAllActive().Count();

            // Assert - Liczba odpowiedzi nie powinna się zmienić
            Assert.Equal(initialCount, result);
        }

        // 5
        [Fact] // Zaliczony
        public void FindCorrectAnswerContent_ShouldReturnErrorMessage_WhenChoiceServiceIsNull() // Znajduje: Jeśli serwis dla poprawności jest null - daje komunikat
        {
            // Arrange
            var correctAnswerService = new CorrectAnswerService();

            // Act
            var result = correctAnswerService.FindCorrectAnswerContent(11, 'A');

            // Assert
            Assert.Equal("Przykładowa odpowiedź", result);
        }

        // 6
        [Fact] // Zaliczony
        public void GetAll_ShouldReturnAllCorrectActiveAnswers() // Podaje: wszystkie poprawne aktywne odpowiedzi
        {
            // Arrange
            var initialActiveCount = _correctAnswerService.GetAllActive().Count();

            // Act
            var result = _correctAnswerService.GetAllActive().ToList();

            // Debug - wyświetl liczbę odpowiedzi
            Console.WriteLine($"Liczba odpowiedzi w GetAllActive(): {result.Count}");

            foreach (var correctAnswer in result)
            {
                Console.WriteLine($"Poprawna odpowiedź Id: {correctAnswer.CorrectAnswerId}, treść: {correctAnswer.CorrectAnswerContent}, aktywna: {correctAnswer.IsActive}");
            }

            // Assert - Sprawdzam, czy liczba się zgadza
            Assert.Equal(initialActiveCount, result.Count());
        }

        // 7 
        [Fact] // Zaliczony
        public void Update_ShouldUpdateCorrectAnswer_WhenValidEntityIsGiven() // Aktualizuje: poprawną odpowiedź jeśli entity jest poprawnie podane
        {
            // Arrange
            var correctAnswerToUpdate = new CorrectAnswer(11, "Zmieniona zima", true);

            // Act
            _correctAnswerService.Update(correctAnswerToUpdate);
            var result = _correctAnswerService.GetCorrectAnswerForQuestion(11);

            // Assert
            Assert.Equal("Zmieniona zima", result.CorrectAnswerContent);
        }

        // 8
        [Fact] // Zaliczony
        public void Update_ShouldNotUpdateCorrectAnswer_WhenEntityDoesNotExist() // Aktualizuje: nic nie robi, jeśli entity nie istnieje
        {
            // Arrange
            var nonExistingCorrectAnswer = new CorrectAnswer(999, "Brak", true);

            // Act
            _correctAnswerService.Update(nonExistingCorrectAnswer);
            var result = _correctAnswerService.GetCorrectAnswerForQuestion(999);

            // Assert
            Assert.Null(result);
        }

        // 9
        [Fact] // Zaliczony
        public void Update_ShouldNotChangeCorrectAnswerById_WhenContentIsSame() // Aktualizuje: pomija aktualizację, jeśli treść ta sama dla tego samego Id
        {
            // Arrange
            var correctAnswerService = new CorrectAnswerService();
            var existingAnswer = new CorrectAnswer(11, "Jesień", true);

            // Act
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                correctAnswerService.Update(existingAnswer);

                // Pobieram komunikaty z konsoli
                var logOutput = sw.ToString();
                Assert.Contains("Brak zmian: treść dla Id 11 jest już taka sama.", logOutput);
            }
            // Assert
            var result = correctAnswerService.GetCorrectAnswerForQuestion(11);
            Assert.Equal("Jesień", result.CorrectAnswerContent);
        }

        // 10
        [Fact] // Zaliczony
        public void GetCorrectAnswerForQuestion_ShouldReturnCorrectAnswer_WhenValidQuestionIdIsGiven() // Daje: poprawną odpowiedź, jeśli Id pytania jest poprawne
        {
            // Act
            var result = _correctAnswerService.GetCorrectAnswerForQuestion(12);

            // Assert
            Assert.Equal("Warszawa", result.CorrectAnswerContent);
        }

        // 11
        [Fact] // Zaliczony
        public void GetCorrectAnswerForQuestion_ShouldReturnNull_WhenInvalidQuestionIdIsGiven() // Daje: zwrot nulla, jeśli Id pytania jest niepoprawne
        {
            // Act
            var result = _correctAnswerService.GetCorrectAnswerForQuestion(999);

            // Assert
            Assert.Null(result);
        }
    }
}
