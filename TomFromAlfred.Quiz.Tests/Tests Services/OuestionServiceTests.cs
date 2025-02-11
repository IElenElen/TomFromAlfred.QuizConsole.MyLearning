using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Oblane: 4 / 11

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
        public void Add_ShouldAddNewQuestion() // Dodaje nowe pytanie
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
        public void Add_ShouldAddNewQuestionToEmptyList() // Dodaje pytanie do pustej listy
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
        public void Add_ShouldNotAddDuplicateQuestion() // Nie dodaje duplikatu
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
        public void Add_ShouldNotAddNullQuestion() // Nie dodaje nulla
        {
            // Arrange
            var questionService = new QuestionService();

            // Act
            questionService.Add(null);

            // Assert
            Assert.Empty(questionService.GetAllActive().ToList());  // Lista powinna pozostać pusta
        }

        // 5
        [Fact] // Zaliczony
        public void Delete_ShouldDeleteExistingQuestion() // Usuwa istniejące pytanie
        {
            //Arrange
            var questionService = new QuestionService();
            var questionToDelete = new Question(15, "Pytanie do usuwania.");
            questionService.Add(questionToDelete);

            //Act
            questionService.Delete(questionToDelete);

            //Assert
            Assert.DoesNotContain(questionToDelete, questionService.GetAllActive().ToList());
        }

        // 6
        [Fact] // Zaliczony
        public void Delete_ShouldNotThrowExceptionWhenDeletingNonExistingQuestion() // Jeżeli nie usuwam pytania, które nie istnieje, system to "spokojnie" akceptuje
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
        public void Delete_ShouldNotThrowExceptionWhenListIsEmpty() // System nie pokazuje wyjątku, jeśli lista jest pusta
        {
            // Arrange
            var questionService = new QuestionService();
            var nonExistentQuestion = new Question(998, "Pytanie, które nie istnieje");

            // Act & Assert
            var exception = Record.Exception(() => questionService.Delete(nonExistentQuestion));
            Assert.Null(exception);  // Nie oczekuję żadnego wyjątku
        }

        // 8
        [Fact] // Oblany
        public void Delete_ShouldNotThrowExceptionWhenDeletingNull() // Nie rzuca wyjątku, jeśli usuwam null
        {
            // Arrange
            var questionService = new QuestionService();

            // Act & Assert
            var exception = Record.Exception(() => questionService.Delete(null));
            Assert.Null(exception);  // Nie oczekuję żadnego wyjątku
        }

        // 9
        [Fact] // Oblany
        public void Update_ShouldUpdateExistingQuestion() // Zmienia istniejące pytanie
        {
            //Arrange
            var questionService = new QuestionService();
            var questionToUpdate = new Question(12, "Pytanie do zmiany.");
            questionService.Add(questionToUpdate);

            var updatedQuestion = new Question(12, "Pytanie zmienione.");

            //Act
            questionService.Update(questionToUpdate);

            //Assert
            var result = questionService.GetAllActive().First(q => q.QuestionId == 12);
            Assert.Equal("Pytanie zmienione", result.QuestionContent);
        }

        // 10
        [Fact] // Zaliczony
        public void Update_ShouldOnlyUpdateIfQuestionContentChanges() // Powinien zmienić treść, tylko jesli się ona zmienia ???
        {
            // Arrange
            var questionService = new QuestionService();
            var existingQuestionToUpdate = new Question(13, "Pytanie do aktualizacji.");
            questionService.Add(existingQuestionToUpdate);

            // Act - Aktualizuję to samo pytanie z tą samą treścią (nie zmienia się)
            var sameContentQuestion = new Question(13, "Pytanie do aktualizacji.");
            questionService.Update(sameContentQuestion);

            // Assert - Upewniam się, że treść pytania się nie zmieniła
            var updatedQuestion = questionService.GetAllActive().First(q => q.QuestionId == 13);
            Assert.Equal("Pytanie do aktualizacji.", updatedQuestion.QuestionContent);
        }

        // 11
        [Fact] // Zaliczony
        public void Update_ShouldNotUpdateNonExistingQuestion() // Nie zmienia pytania, które nie istnieje
        {  
            //Arrange
            var questionService = new QuestionService();
            var nonExistentQuestion = new Question(99, "To pytanie nie istnieje.");

            //Act
            questionService.Update(nonExistentQuestion);

            //Assert
            var result = questionService.GetAllActive().FirstOrDefault(q => q.QuestionId == 99);
            Assert.Null(result);
        }
    }
}
