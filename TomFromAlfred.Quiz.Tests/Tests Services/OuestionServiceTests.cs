using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Oblane: 2 / 11

    public class OuestionServiceTests
    {
        private readonly QuestionService _questionService; 

        public OuestionServiceTests()
        {
            // Inicjalizacja przed każdym testem (dodaję tu - tylko raz)
            _questionService = new QuestionService();
        }

        // 1
        [Fact] // Zaliczony
        public void Add_ShouldAddNewQuestion() // Dodaje: nowe pytanie
        {
            //Arrange
            var questionService = new QuestionService();
            var newQuestion = new Question(14, "Kim był Alfred Szklarski?");

            //Act
            questionService.Add(newQuestion);

            //Assert
            Assert.Contains(newQuestion, questionService.GetAllActive().ToList());
        }

        // 2
        [Fact] // Zaliczony
        public void Add_ShouldAddNewQuestionToEmptyList() // Dodaje: pytanie do pustej listy
        {
            // Arrange
            var questionService = new QuestionService();
            var newQuestion = new Question(1, "Pytanie testowe");

            // Act
            questionService.Add(newQuestion);

            // Assert
            Assert.Contains(newQuestion, questionService.GetAllActive().ToList());
        }

        // 3
        [Fact] // Oblany
        public void Add_ShouldNotAddDuplicateQuestion() // Dodaje: nie dodaje duplikatu
        {
            // Arrange
            var questionService = new QuestionService();
            var existingQuestion = new Question(1, "Pytanie testowe");
            questionService.Add(existingQuestion);  // Dodaję pytanie

            var duplicateQuestion = new Question(1, "Pytanie testowe (duplikat)");

            // Act
            questionService.Add(duplicateQuestion);  // Próba dodania pytania z tym samym Id

            // Assert
            var allQuestions = questionService.GetAllActive().ToList();
            Assert.Single(allQuestions);  // Lista powinna zawierać tylko jedno pytanie o Id = 1
        }

        // 4
        [Fact] // Oblany ???
        public void Add_ShouldNotAddNullQuestion() // Dodaje: nie dodaje, kiedy Question jest null
        {
            // Arrange
            var questionService = new QuestionService();

            // Act
            questionService.Add(null);

            // Assert
            Assert.Empty(questionService.GetAllActive());  // Lista powinna pozostać pusta
        }

            // 5
            [Fact] // Zaliczony
        public void Delete_ShouldDeleteExistingQuestion() // Usuwa: pytanie, które istnieje
        {
            // Arrange
            var questionService = new QuestionService();
            var questionToDelete = new Question(15, "Pytanie do usuwania.");

            questionService.Add(questionToDelete);

            // Sprawdzam, czy pytanie faktycznie zostało dodane przed usunięciem
            Assert.Contains(questionToDelete, questionService.GetAllActive());

            // Act
            questionService.Delete(questionToDelete);

            // Assert
            Assert.DoesNotContain(questionToDelete, questionService.GetAllActive());
        }

        // 6
        [Fact] // Zaliczony
        public void Delete_ShouldNotThrowExceptionWhenDeletingNonExistingQuestion() // Usuwa: nie usuwa pytania, które nie istnieje, system to "spokojnie" akceptuje
        {
            // Arrange
            var questionService = new QuestionService();
            var nonExistentQuestion = new Question(999, "Pytanie, które nie istnieje");

            // Act & Assert
            var exception = Record.Exception(() => questionService.Delete(nonExistentQuestion));
            Assert.Null(exception);  // Nie oczekuję żadnego wyjątku
        }

        // 7
        [Fact] // Zaliczony
        public void Delete_ShouldNotThrowExceptionWhenListIsEmpty() // Usuwa: nie wyrzuca wyjątku, jeśli lista jest pusta
        {
            // Arrange
            var questionService = new QuestionService(); // Pusta lista
            var nonExistentQuestion = new Question(998, "Pytanie, które nie istnieje");

            // Act
            var exception = Record.Exception(() => questionService.Delete(nonExistentQuestion));

            // Assert
            Assert.Null(exception);  // Oczekuję, że metoda nie rzuci wyjątku
        }

        // 8
        [Fact] // Zaliczony
        public void Delete_ShouldNotThrowExceptionWhenDeletingNull() // Usuwa: nie wyrzuca wyjątku, jeśli jest null
        {
            // Arrange
            var questionService = new QuestionService();

            // Act & Assert
            var exception = Record.Exception(() => questionService.Delete(null));

            Assert.Null(exception);  // Nie oczekuję żadnego wyjątku
        }

        // 9
        [Fact] // Zaliczony
        public void Update_ShouldUpdateExistingQuestion() // Aktualizuje: zmienia istniejące pytanie
        {
            // Arrange
            var questionToUpdate = new Question(12, "Pytanie do zmiany.");
            _questionService.Add(questionToUpdate);

            var updatedQuestion = new Question(12, "Pytanie zmienione.");

            // Act - Aktualizuję pytanie z nową treścią
            _questionService.Update(updatedQuestion);

            // Assert - Pobieram pytanie po aktualizacji i sprawdzam, czy treść się zmieniła
            var result = _questionService.GetAllActive().First(q => q.QuestionId == 12);
            Assert.Equal("Pytanie zmienione.", result.QuestionContent);
        }

        // 10
        [Fact] // Zaliczony
        public void Update_ShouldOnlyUpdateIfQuestionContentChanges() // Aktualzuje: zmienia tylko, jeśli zmieniła się treść pytania
        {
            // Arrange
            var existingQuestionToUpdate = new Question(13, "Pytanie do aktualizacji.");
            _questionService.Add(existingQuestionToUpdate);

            // Act - Aktualizuję to samo pytanie z tą samą treścią (nie zmienia się)
            var sameContentQuestion = new Question(13, "Pytanie do aktualizacji.");
            _questionService.Update(sameContentQuestion);

            // Assert - Pobieram pytanie po aktualizacji
            var updatedQuestion = _questionService.GetAllActive().First(q => q.QuestionId == 13);

            // Sprawdzam, czy treść pytania jest taka sama (nie została zmieniona)
            Assert.Equal("Pytanie do aktualizacji.", updatedQuestion.QuestionContent);

            // Dodatkowy test: Aktualizuję z nową treścią
            var modifiedQuestion = new Question(13, "Zmieniona treść pytania.");
            _questionService.Update(modifiedQuestion);

            // Sprawdzam, czy treść została zaktualizowana tylko w przypadku zmiany
            var updatedAfterChange = _questionService.GetAllActive().First(q => q.QuestionId == 13);
            Assert.Equal("Zmieniona treść pytania.", updatedAfterChange.QuestionContent);
        }

        // 11
        [Fact] // Zaliczony
        public void Update_ShouldNotUpdateNonExistingQuestion() // Aktualizuje: nic nie zmienia, jeśli pytanie nie istnieje
        {
            // Arrange
            int initialCount = _questionService.GetAllActive().Count(); // Zapisuję początkową liczbę pytań

            var nonExistentQuestion = new Question(99, "To pytanie nie istnieje.");

            // Act
            _questionService.Update(nonExistentQuestion);

            // Assert
            var result = _questionService.GetAllActive().FirstOrDefault(q => q.QuestionId == 99);
            Assert.Null(result); // Sprawdzam, czy pytania dalej nie ma
            Assert.Equal(initialCount, _questionService.GetAllActive().Count()); // Liczba pytań nie powinna zmienić się
        }
    }
}
