using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.Tests.Tests_Services
{
    public class QuestionServiceTests
    {
        [Fact]
        public void GetQuestionByNumber_ReturnCorrectQuestion() //test 3xA
        {
            //Arrange
            QuestionServiceApp questionServiceApp = new QuestionServiceApp();

            //Act
            Question result = questionServiceApp.GetQuestionByNumber(2);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Tomek przed pierwszą przygodą mieszka w: ", result.QuestionContent);
            Assert.Equal(2, result.QuestionContent);
        }

        [Fact]
        public void GetQuestionByNumber_ReturnsNullWhenQuestionNotFound() //test 3xA
        {
            // Arrange
            QuestionServiceApp questionServiceApp = new QuestionServiceApp();

            // Act
            Question result = questionServiceApp.GetQuestionByNumber(10);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void RemoveQuestionByNumber_RemovesQuestion_WhenNumberExists()
        {
            // Arrange
            var allQuestions = new List<Question>
            {
                new Question(0, "Example question 1"),
                new Question(1, "Example question 2"),
                new Question(2, "Example question 3")
            };
            var questionServiceApp = new QuestionServiceApp(allQuestions);
            int questionNumberToRemove = 1;

            // Act
            questionServiceApp.RemoveQuestionByNumber(questionNumberToRemove);

            // Assert
            Assert.Null(questionServiceApp.GetQuestionByNumber(questionNumberToRemove)); // czy pytanie zostało usunięte?
            Assert.Equal(2, questionServiceApp.AllQuestions.Count()); // czy liczba pytań została zmniejszona?
            Assert.DoesNotContain(questionServiceApp.AllQuestions, q => q.QuestionContent == questionNumberToRemove); // czy pytanie zostało usunięte z listy?
        }

        [Fact]
        public void RemoveQuestionByNumber_WritesMessage_WhenNumberDoesNotExist() //test 3xA z mock
        {
            // Arrange
            var allQuestions = new List<Question>
        {
            new Question(0, "Example question 1"),
            new Question(1, "Example question 2"),
            new Question(2, "Example question 3")
        };
            var questionServiceApp = new QuestionServiceApp(allQuestions);
            var mockConsole = new Mock<IConsole>();
            int nonExistingQuestionNumber = 999;

            // Act
            questionServiceApp.RemoveQuestionByNumber(nonExistingQuestionNumber);

            // Assert
            Assert.Null(questionServiceApp.GetQuestionByNumber(nonExistingQuestionNumber)); // czy żadne pytanie nie zostało usunięte?
        }

    }

    internal interface IConsole //na potrzeby testu
    {
    }
}
