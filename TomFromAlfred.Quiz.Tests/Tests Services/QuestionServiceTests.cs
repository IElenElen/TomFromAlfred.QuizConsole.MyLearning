using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using Xunit.Abstractions;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Oblane: 0 / 13

    // Czy tu już mam testy integracyjne na poziomie klasy???

    public class QuestionServiceTests
    {
        private readonly QuestionService _questionService;
        private readonly ITestOutputHelper _output;

        public QuestionServiceTests(ITestOutputHelper output)
        {
            // Inicjalizacja przed każdym testem (dodaję tu - tylko raz)
            _questionService = new QuestionService();
            _output = output;
        }

        // Testowanie zachowania metod w klasie serwisowej pytań (co powinno się zadziać)

        // 1
        [Fact] // Zaliczony
        public void Add_ShouldAddNewQuestion_ToActiveList() // Dodaje (powinien): nowe pytanie do aktywnej listy
        {
            //Arrange
            var newQuestion = new Question(20, "Kim był Alfred Szklarski?");

            //Act
            _questionService.Add(newQuestion);

            //Assert
            Assert.Contains(newQuestion, _questionService.GetAllActive().ToList());
        }

        // 2
        [Fact] // Zaliczony
        public void Add_ShouldAddNewQuestionToEmptyList() // Dodaje: nowe pytanie do pustej listy
        {
            // Arrange
            var newQuestion = new Question(1, "Pytanie testowe.");

            // Act
            _questionService.Add(newQuestion);

            // Assert
            Assert.Contains(newQuestion, _questionService.GetAllActive().ToList());
        }

        // 3
        [Fact] // Zaliczony
        public void Add_ShouldNotAddDuplicateQuestion() // Dodaje: nie dodaje duplikatu
        {
            // Arrange
            var allQuestionsBefore = _questionService.GetAllActive().ToList();
            _output.WriteLine($"Pytania przed testem: {allQuestionsBefore.Count}");

            var existingQuestion = new Question(1, "Pytanie testowe x.");
            _questionService.Add(existingQuestion);

            var duplicateQuestion = new Question(1, "Pytanie testowe x (duplikat).");

            // Act
            _questionService.Add(duplicateQuestion);

            // Assert
            var allQuestions = _questionService.GetAllActive().ToList();
            var countOfQuestionsWithId1 = allQuestions.Count(q => q.QuestionId == 1);

            _output.WriteLine($"Pytania po teście: {allQuestions.Count}");
            _output.WriteLine($"Ilość pytań z Id = 1: {countOfQuestionsWithId1}");

            Assert.Equal(1, countOfQuestionsWithId1); // Czy Id = 1 pojawia się tylko raz
        }

        // 4
        [Fact] // Zaliczony
        public void Add_ShouldNotAddNullToQuestionList() // Dodaje: nie dodaje do listy, kiedy pytanie jest null
        {
            // Arrange
            var countQuestionsBefore = _questionService.GetAllActive().Count();

            // Act
            _questionService.Add(null);

            // Assert
            var countQuestionsAfter = _questionService.GetAllActive().Count();
            Assert.Equal(countQuestionsBefore, countQuestionsAfter);  
        }

         // 5
        [Fact] // Zaliczony
        public void Delete_ShouldDeleteExistingQuestionById() // Usuwa: pytanie z listy, które istnieje (po Id)
        {
            // Arrange
            var questionToDelete = new Question(20, "Czy Tomek jest chłopcem?");

            _questionService.Add(questionToDelete);

            // Tworzę nową instancję o tym samym Id (ale inny obiekt)
            var questionWithSameId = new Question(20, "Czy Tomek to podróżnik?");

            // Act
            _questionService.Delete(questionWithSameId);

            // Assert
            Assert.DoesNotContain(questionToDelete, _questionService.GetAllActive()); // Nie powinien zawierać po usunięciu w aktywnych
        }

        // 6
        [Fact] // Zaliczony
        public void Delete_ShouldNotThrowExceptionWhenDeletingNonExistingQuestion() // Usuwa: nie usuwa pytania, które nie istnieje, system to "spokojnie" akceptuje
        {
            // Arrange
            var nonExistentQuestion = new Question(999, "Pytanie, które nie istnieje.");

            // Act & Assert
            var exception = Record.Exception(() => _questionService.Delete(nonExistentQuestion));
            Assert.Null(exception);  // Nie oczekuję żadnego wyjątku
        }

        // 7
        [Fact] // Zaliczony
        public void Delete_ShouldNotThrow_WhenDeletingNonExistentQuestionFromEmptyList() // Usuwa: nie wyrzuca wyjątku przy próbie usunięcia pytania, jeśli lista pytań jest pusta
        {
            // Arrange
            _output.WriteLine($"Liczba pytań przed testem: {_questionService.GetAllActive().Count()}");
            var nonExistentQuestion = new Question(998, "Pytanie, które nie istnieje.");

            // Act
            var exception = Record.Exception(() => _questionService.Delete(nonExistentQuestion));

            // Assert
            Assert.Null(exception);  // Oczekuję, że metoda nie rzuci wyjątku
        }

        // 8
        [Fact] // Zaliczony
        public void Delete_ShouldNotThrow_WhenNullQuestionIsPassed() // Usuwa: nic nie robi, nie wyrzuca wyjątku, jeśli pytanie to null
        {
            // Arrange & Act & Assert
            var exception = Record.Exception(() => _questionService.Delete(null));

            Assert.Null(exception);  // Nie oczekuję żadnego wyjątku
        }

        // 9
        [Fact] // Zaliczony
        public void Update_ShouldUpdate_ChangeContentOfExistingQuestion_ByIdInList() // Aktualizuje: zmienia istniejące pytanie
        {
            // Arrange
            var questionToUpdate = new Question(12, "Pytanie do zmiany.");
            _questionService.Add(questionToUpdate);

            var updatedQuestion = new Question(12, "Pytanie zmienione.");

            // Act - Aktualizuję pytanie z nową treścią
            _questionService.Update(updatedQuestion);

            // Assert:

            // Upewniam się, że jest tylko jedno pytanie o Id = 12
            var allQuestionsWithId = _questionService.GetAllActive().Where(q => q.QuestionId == 12);
            Assert.Single(allQuestionsWithId); // Chroni przed duplikatem

            // Sprawdzam, czy treść się zmieniła
            var result = allQuestionsWithId.First();
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
            var updatedAfterChangeQuestionContent = _questionService.GetAllActive().First(q => q.QuestionId == 13);
            Assert.Equal("Zmieniona treść pytania.", updatedAfterChangeQuestionContent.QuestionContent);
        }

        // 11
        [Fact] // Zaliczony // Czy warto go rozbić na dwa osobne testy???
        public void Update_ShouldNotUpdateNonExistingQuestion_AndShouldLogMessage() // Aktualizuje: nic nie zmienia, jeśli pytanie nie istnieje
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

        // Dodatkowo:

        // 12
        [Fact] // Zaliczony
        public void GetAllActive_ShouldReturnAllAddedActiveQuestions() // Podaje wszystkie aktywne: zwraca wszystkie dodane aktywne pytania
        {
            // Arrange
            var question1 = new Question(1, "Pytanie nr 1.");
            var question2 = new Question(2, "Pytanie nr 2.");

            _questionService.Add(question1);
            _questionService.Add(question2);

            // Act
            var allActiveQuestions = _questionService.GetAllActive().ToList();

            // Assert
            Assert.Equal(2, allActiveQuestions.Count);
            Assert.Contains(question1, allActiveQuestions);
            Assert.Contains(question2, allActiveQuestions);
        }

        // 13
        [Fact] // Zaliczony
        public void Update_ShouldNotThrowExceptionWhenNullPassed() // Aktualizuje: nic nie robi => tolerancja nulla, nie zgłasza błędu, sprawdza bezpieczeństwo
        {
            // Act
            var exception = Record.Exception(() => _questionService.Update(null));

            // Assert
            Assert.Null(exception); // Nie powinien rzucać wyjątku
        }
    }
}
