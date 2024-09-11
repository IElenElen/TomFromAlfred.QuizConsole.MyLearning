using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.Tests
{
    public class QuestionsListTests
    {
        [Fact]
        public void GetRandomQuestions_ReturnsAllQuestionsIfNumberExceedsAvailable()
        {
            // Arrange
            var fakeQuestions = new List<Question>
        {
            new Question(1, "Question 1"),
            new Question(2, "Question 2"),
            new Question(3, "Question 3")
        };

            var mockQuestionService = new Mock<QuestionServiceApp>();
            mockQuestionService.Setup(x => x.AllQuestions).Returns(fakeQuestions);

            var managerApp = new QuestionsRaffleServiceApp(mockQuestionService.Object);

            // Act
            var randomQuestions = managerApp.GetRandomQuestions();

            // Assert
            Assert.Equal(3, randomQuestions.Count);
        }
    }
}
