using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using TomFromAlfred.QuizConsole.Tests.Z___SupportForTests;
using Xunit.Abstractions;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Oblane: 0 / 14

    public class QuestionServiceTests
    {
        // Testowanie zachowania metod w klasie serwisowej pytań (co powinno się zadziać i kiedy)

        #region Add QuestionServiceTests
        // 1
        [Fact] // Zaliczony
        public void Add_ShouldAddNewQuestion_ToActiveList() // Dodaje (powinien): nowe pytanie do aktywnej listy
        {
            //Arrange
            var questionService = new QuestionService();

            DataClearingCommonClass.ClearQuestions(questionService);


            var newQuestion = new Question(20, "Nowe pytanie: Kim był Alfred Szklarski?");

            //Act
            questionService.Add(newQuestion);

            //Assert
            var activeQuestions = questionService.GetAllActive();

            activeQuestions.Should().Contain(newQuestion);
        }

        // 2
        [Fact] // Zaliczony
        public void Add_ShouldAddNewQuestionToEmptyList() // Dodaje: nowe pytanie do pustej listy
        {
            // Arrange
            var questionService = new QuestionService();

            DataClearingCommonClass.ClearQuestions(questionService);

            var newQuestion = new Question(1, "Pytanie testowe dodawane do pustej listy.");

            // Act
            questionService.Add(newQuestion);

            // Assert
            var activeQuestions = questionService.GetAllActive();

            activeQuestions.Should().Contain(newQuestion);
        }

        // 3
        [Fact] // Zaliczony
        public void Add_ShouldNotAddDuplicateQuestion() // Dodaje: nie dodaje duplikatu
        {
            // Arrange
            var questionService = new QuestionService();

            DataClearingCommonClass.ClearQuestions(questionService);

            var existingQuestion = new Question(1, "Istniejące pytanie x.");

            questionService.Add(existingQuestion);

            var duplicateQuestion = new Question(1, "Istniejące pytanie x (duplikat).");

            // Act
            questionService.Add(duplicateQuestion);

            // Assert
            var allActiveQuestions = questionService.GetAllActive();

            allActiveQuestions.Should().ContainSingle(q => q.QuestionId == 1); // Czy Id = 1 pojawia się tylko raz
        }

        // 4
        [Fact] // Zaliczony
        public void Add_ShouldNotAddNullToQuestionList() // Dodaje: nie dodaje do listy, kiedy pytanie jest null
        {
            // Arrange
            var questionService = new QuestionService();

            DataClearingCommonClass.ClearQuestions(questionService);

            var countQuestionsBefore = questionService.GetAllActive().Count();

            // Act
            questionService.Add(null);

            var countQuestionsAfter = questionService.GetAllActive().Count();

            // Assert
            countQuestionsAfter.Should().Be(countQuestionsBefore);
        }
        #endregion Add QuestionServiceTests

        #region Delete QuestionServiceTests
        // 5
        [Fact] // Zaliczony
        public void Delete_ShouldDeleteExistingQuestionById() // Usuwa: pytanie z listy, które istnieje (po Id)
        {
            // Arrange
            var questionService = new QuestionService();

            DataClearingCommonClass.ClearQuestions(questionService);


            var questionToDelete = new Question(20, "Czy Tomek jest chłopcem?");

            questionService.Add(questionToDelete);

            // Tworzę nową instancję o tym samym Id (ale inny obiekt)
            var questionWithSameId = new Question(20, "Czy Tomek to podróżnik?");

            // Act
            questionService.Delete(questionWithSameId);

            // Assert
            var activeQuestions = questionService.GetAllActive();

            activeQuestions.Should().NotContain(questionToDelete); // Nie powinien zawierać po usunięciu w aktywnych
        }

        // 6
        [Fact] // Zaliczony
        public void Delete_ShouldNotThrowExceptionWhenDeletingNonExistingQuestion() // Usuwa: nie usuwa pytania, które nie istnieje, system to "spokojnie" akceptuje
        {
            // Arrange
            var questionService = new QuestionService();

            DataClearingCommonClass.ClearQuestions(questionService);


            var nonExistentQuestion = new Question(999, "Pytanie, które nie istnieje.");

            // Act 
            var exception = Record.Exception(() => questionService.Delete(nonExistentQuestion));

            // Assert
            exception.Should().BeNull("Usunięcie nieistniejącego pytania nie powinno rzucać wyjątku."); // Nie oczekuję żadnego wyjątku
        }

        // 7
        [Fact] // Zaliczony
        public void Delete_ShouldNotThrow_WhenDeletingNonExistentQuestionFromEmptyList() // Usuwa: nie wyrzuca wyjątku przy próbie usunięcia pytania, jeśli lista pytań jest pusta
        {
            // Arrange
            var questionService = new QuestionService();

            DataClearingCommonClass.ClearQuestions(questionService);


            var nonExistentQuestion = new Question(998, "Nieistniejące pytanie.");

            // Act
            var exception = Record.Exception(() => questionService.Delete(nonExistentQuestion));

            // Assert
            exception.Should().BeNull("lista może być pusta i operacja nadal powinna być bezpieczna."); // Oczekuję, że metoda nie rzuci wyjątku
        }

        // 8
        [Fact] // Zaliczony
        public void Delete_ShouldNotThrow_WhenNullQuestionIsPassed() // Usuwa: nic nie robi, nie wyrzuca wyjątku, jeśli pytanie to null
        {
            // Arrange 
            var questionService = new QuestionService();

            DataClearingCommonClass.ClearQuestions(questionService);

            // Act
            var exception = Record.Exception(() => questionService.Delete(null));

            // Assert
            exception.Should().BeNull("Operacja usuwania nulla powinna być tolerowana."); // Nie oczekuję żadnego wyjątku
        }
        #endregion Delete QuestionServiceTests

        #region Update QuestionServiceTests
        // 9
        [Fact] // Zaliczony
        public void Update_ShouldUpdate_ChangeContentOfExistingQuestion_ByIdInList() // Aktualizuje: zmienia istniejące pytanie
        {
            // Arrange
            var questionService = new QuestionService();

            DataClearingCommonClass.ClearQuestions(questionService);


            var questionOrginal = new Question(12, "Pytanie do zmiany.");

            questionService.Add(questionOrginal);

            var updatedQuestion = new Question(12, "Pytanie zmienione.");

            // Act - Aktualizuję pytanie z nową treścią
            questionService.Update(updatedQuestion);

            // Assert
            questionService.GetAllActive()
                           .Single(q => q.QuestionId == 12)
                           .QuestionContent.Should().Be("Pytanie zmienione.");
        }

        // 10
        [Fact] // Zaliczony
        public void Update_ShouldOnlyUpdateIfQuestionContentChanges() // Aktualzuje: zmienia tylko, jeśli zmieniła się treść pytania
        {
            // Arrange
            var questionService = new QuestionService();

            DataClearingCommonClass.ClearQuestions(questionService);


            var existingQuestionToUpdate = new Question(13, "Pytanie do aktualizacji.");

            questionService.Add(existingQuestionToUpdate);

            // Act 1 – aktualizacja bez zmian treści
            var sameContentQuestion = new Question(13, "Pytanie do aktualizacji.");

            questionService.Update(sameContentQuestion);

            // Assert 1 – nic się nie zmieniło
            var activeQuestions = questionService.GetAllActive();

            activeQuestions.First(q => q.QuestionId == 13)
                           .QuestionContent.Should().Be("Pytanie do aktualizacji.");

            // Act 2 – zmiana treści
            var modifiedQuestion = new Question(13, "Zmieniona treść pytania.");

            questionService.Update(modifiedQuestion);

            // Assert 2 – treść została zmieniona
            activeQuestions = questionService.GetAllActive();

            activeQuestions.First(q => q.QuestionId == 13)
                           .QuestionContent.Should().Be("Zmieniona treść pytania.");
        }

        // 11
        [Fact] // Zaliczony
        public void Update_ShouldNotThrowExceptionWhenNullPassed() // Aktualizuje: nic nie robi => tolerancja nulla, nie zgłasza błędu, sprawdza bezpieczeństwo
        {
            // Arrange
            var questionService = new QuestionService();

            DataClearingCommonClass.ClearQuestions(questionService);


            // Act
            var exception = Record.Exception(() => questionService.Update(null));

            // Assert
            exception.Should().BeNull("Operacja aktualizacji (null) powinna być bezpieczna."); // Nie powinien rzucać wyjątku
        }

        // 12
        [Fact] // Zaliczony 
        public void Update_ShouldDoNothing_WhenQuestionDoesNotExist() // Aktualizuje: nic nie zmienia, jeśli pytanie nie istnieje
        {
            // Arrange
            var questionService = new QuestionService();

            DataClearingCommonClass.ClearQuestions(questionService);


            var nonExistentQuestion = new Question(99, "To pytanie nie istnieje.");

            // Act
            questionService.Update(nonExistentQuestion);

            // Assert
            var activeQuestions = questionService.GetAllActive();

            activeQuestions.FirstOrDefault(q => q.QuestionId == 99).Should().BeNull(); // Sprawdzam, czy pytania dalej nie ma
        }

        // 13
        [Fact] // Zaliczony 
        public void Update_ShouldNotChangeQuestionsCount_ForNonExistingQuestion() // Aktualizuje: nic nie zmienia, podaje info
        {
            // Arrange
            var questionService = new QuestionService();

            DataClearingCommonClass.ClearQuestions(questionService);


            int initialQuestionsCount = questionService.GetAllActive().Count(); // Zapisuję początkową liczbę pytań

            var nonExistentQuestion = new Question(99, "To pytanie na pewno nie istnieje.");

            // Act
            questionService.Update(nonExistentQuestion);

            // Assert
            questionService.GetAllActive().Should().HaveCount(initialQuestionsCount); // Liczba pytań nie powinna zmienić się
        }
        #endregion Update QuestionServiceTests

        #region GetAllActive for Questions
        // Dodatkowo:

        // 14
        [Fact] // Zaliczony
        public void GetAllActive_ShouldReturnAllAddedActiveQuestions() // Podaje wszystkie aktywne: zwraca wszystkie dodane aktywne pytania
        {
            // Arrange
            var questionService = new QuestionService();

            DataClearingCommonClass.ClearQuestions(questionService);

            var question1 = new Question(1, "Pytanie nr 1.");
            var question2 = new Question(2, "Pytanie nr 2.");

            questionService.Add(question1);
            questionService.Add(question2);

            // Act
            var activeQuestions = questionService.GetAllActive();

            // Assert
            activeQuestions.Should().HaveCount(2);
            activeQuestions.Should().Contain(question1);
            activeQuestions.Should().Contain(question2);
        }
        #endregion GetAllActive for Questions
    }
}
