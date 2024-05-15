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
        public void GetQuestionByNumber_ReturnCorrectQuestion()
        {
            //Arrange
            QuestionServiceApp questionServiceApp = new QuestionServiceApp();

            //Act

            //List<Question> allQuestionsList = questionServiceApp.AllQuestions.ToList(); // Konwersja IEnumerable<Question> na List<Question>
            //Question result = questionServiceApp.GetQuestionByNumber(questionServiceApp.AllQuestions, 2);

            Question result = questionServiceApp.GetQuestionByNumber(2);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Tomek przed pierwszą przygodą mieszka w: ", result.QuestionContent);
            Assert.Equal(2, result.QuestionNumber);
        }

        [Fact]
        public void GetQuestionByNumber_ReturnsNullWhenQuestionNotFound()
        {
            // Arrange
            QuestionServiceApp questionServiceApp = new QuestionServiceApp();

            // Act

            //Question result = questionServiceApp.GetQuestionByNumber(questionServiceApp.AllQuestions, 10);

            Question result = questionServiceApp.GetQuestionByNumber(10);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void RemoveQuestionByNumber_NothingRemovedWhenQuestionNotFound()
        {
            // Arrange
            var questionList = new List<Question>
        {
            new Question(0, "Question 1"),
            new Question(1, "Question 2"),
            new Question(2, "Question 3")
        };

            var mockQuestions = new Mock<IEnumerable<Question>>();
            mockQuestions.Setup(q => q.GetEnumerator()).Returns(questionList.GetEnumerator());

            var questionServiceApp = new QuestionServiceApp(mockQuestions.Object);

            // Act
            questionServiceApp.RemoveQuestionByNumber(10);

            // Assert
            Assert.Equal(3, questionServiceApp.AllQuestions.Count()); // Sprawdzamy, czy liczba pytań pozostała bez zmian

            // Wyświetlamy zawartość listy AllQuestions
            var questions = questionServiceApp.AllQuestions.ToList();
            foreach (var question in questions)
            {
                Console.WriteLine($"QuestionNumber: {question.QuestionNumber}, QuestionContent: {question.QuestionContent}");
            }
        }
    }
}
