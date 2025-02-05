using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    public class OuestionServiceTests
    {

        //private readonly QuestionService _questionService; // Dodam tutaj, aby nie powtarzać w każdym teście

        /*public QuestionServiceTests()
        {
            // Inicjalizacja przed każdym testem (dodaję tu - tylko raz)
            _questionService = new QuestionService();
        }*/

        [Fact]

        public void Add_ShouldAddNewQuestion() 
        {
            //Arrange
            var questionService = new QuestionService();
            var newQuestion = new Question(14, "Co to jest c#?");

            //Act
            questionService.Add(newQuestion);

            //Assert
            Assert.Contains(newQuestion, questionService.GetAll().ToList());
        }

        [Fact]
        public void Add_ShouldAddNewQuestionToEmptyList()
        {
            // Arrange
            var questionService = new QuestionService();
            var newQuestion = new Question(1, "Pytanie testowe");

            // Act
            questionService.Add(newQuestion);

            // Assert
            Assert.Contains(newQuestion, questionService.GetAll().ToList());
        }

        [Fact] // Oblany
        public void Add_ShouldNotAddDuplicateQuestion()
        {
            // Arrange
            var questionService = new QuestionService();
            var existingQuestion = new Question(1, "Pytanie testowe");
            questionService.Add(existingQuestion);  // Dodaję pytanie

            var duplicateQuestion = new Question(1, "Pytanie testowe (duplikat)");

            // Act
            questionService.Add(duplicateQuestion);  // Próba dodania pytania z tym samym Id

            // Assert
            var allQuestions = questionService.GetAll().ToList();
            Assert.Single(allQuestions);  // Lista powinna zawierać tylko jedno pytanie o Id = 1
        }

        [Fact] // Oblany
        public void Add_ShouldNotAddNullQuestion()
        {
            // Arrange
            var questionService = new QuestionService();

            // Act
            questionService.Add(null);

            // Assert
            Assert.Empty(questionService.GetAll().ToList());  // Lista powinna pozostać pusta
        }

        [Fact]
        public void Delete_ShouldDeleteExistingQuestion() 
        {
            //Arrange
            var questionService = new QuestionService();
            var questionToDelete = new Question(15, "Pytanie do usuwania.");
            questionService.Add(questionToDelete);

            //Act
            questionService.Delete(questionToDelete);

            //Assert
            Assert.DoesNotContain(questionToDelete, questionService.GetAll().ToList());
        }

        [Fact]
        public void Delete_ShouldNotThrowExceptionWhenDeletingNonExistingQuestion()
        {
            // Arrange
            var questionService = new QuestionService();
            var nonExistentQuestion = new Question(999, "Pytanie, które nie istnieje");

            // Act & Assert
            var exception = Record.Exception(() => questionService.Delete(nonExistentQuestion));
            Assert.Null(exception);  // Nie oczekuję żadnego wyjątku
        }

        [Fact]
        public void Delete_ShouldNotThrowExceptionWhenListIsEmpty()
        {
            // Arrange
            var questionService = new QuestionService();
            var nonExistentQuestion = new Question(998, "Pytanie, które nie istnieje");

            // Act & Assert
            var exception = Record.Exception(() => questionService.Delete(nonExistentQuestion));
            Assert.Null(exception);  // Nie oczekuję żadnego wyjątku
        }

        [Fact] //Oblany
        public void Delete_ShouldNotThrowExceptionWhenDeletingNull()
        {
            // Arrange
            var questionService = new QuestionService();

            // Act & Assert
            var exception = Record.Exception(() => questionService.Delete(null));
            Assert.Null(exception);  // Nie oczekuję żadnego wyjątku
        }

        [Fact] //Oblany
        public void Update_ShouldUpdateExistingQuestion() 
        {
            //Arrange
            var questionService = new QuestionService();
            var questionToUpdate = new Question(16, "Pytanie do zmiany.");
            questionService.Add(questionToUpdate);

            var updatedQuestion = new Question(16, "Pytanie zmienione.");

            //Act
            questionService.Update(questionToUpdate);

            //Assert
            var result = questionService.GetAll().First(q => q.QuestionId == 16);
            Assert.Equal("Pytanie zmienione", result.QuestionContent);
        }

        [Fact] // Oblany
        public void Update_ShouldOnlyUpdateIfQuestionContentChanges()
        {
            // Arrange
            var questionService = new QuestionService();
            var questionToUpdate = new Question(20, "Pytanie do aktualizacji.");
            questionService.Add(questionToUpdate);

            // Act - Aktualizuję to samo pytanie z tą samą treścią (nie zmienia się)
            var sameContentQuestion = new Question(20, "Pytanie do aktualizacji.");
            questionService.Update(sameContentQuestion);

            // Assert - Upewniam się, że treść pytania się nie zmieniła
            var updatedQuestion = questionService.GetAll().First(q => q.QuestionId == 16);
            Assert.Equal("Pytanie do aktualizacji.", updatedQuestion.QuestionContent);
        }

        [Fact]
        public void Update_ShouldNotUpdateNonExistingQuestion()
        {  
            //Arrange
            var questionService = new QuestionService();
            var nonExistentQuestion = new Question(99, "To pytanie nie istnieje.");

            //Act
            questionService.Update(nonExistentQuestion);

            //Assert
            var result = questionService.GetAll().FirstOrDefault(q => q.QuestionId == 99);
            Assert.Null(result);
        }
    }
}
