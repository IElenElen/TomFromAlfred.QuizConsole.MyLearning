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
        public void Constructor_AddsAllQuestionsFromQuestionService() //test 3xA z mock i fake
        {
            // Arrange
            var fakeQuestions = new List<Question>
                {
                new Question(9, "xyz"),
                new Question(11, "Example question 11"),
                };

            var mockQuestionService = new Mock<QuestionServiceApp>();

            mockQuestionService.Setup(x => x.AllQuestions).Returns(fakeQuestions);

            // Act
            var managerApp = new QuestionsRaffleServiceApp(mockQuestionService.Object);
            var actualQuestions = managerApp.Questions;

            // Assert
            Assert.Equal(fakeQuestions.Count, actualQuestions.Count); // sprawdzenie, czy liczba pytań zgadza się
                                                                      
            // Czy wszystkie pytania z QuestionServiceApp zostały dodane do listy Questions?
            foreach (var expectedQuestion in fakeQuestions)
            {
                Assert.Contains(expectedQuestion, actualQuestions);
            }
        }

        [Fact]
        public void Constructor_HandlesEmptyQuestionService()
        {
            // Arrange
            var mockQuestionService = new Mock<QuestionServiceApp>();
            mockQuestionService.Setup(x => x.AllQuestions).Returns(new List<Question>());

            // Act
            var managerApp = new QuestionsRaffleServiceApp(mockQuestionService.Object);

            // Assert
            Assert.Empty(managerApp.Questions);
        }

        [Fact]
        public void GetRandomQuestions_QuestionsAreRandomAndRenumbered()
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
            var uniqueNumbers = new HashSet<int>(randomQuestions.Select(q => q.QuestionContent));
            Assert.Equal(3, uniqueNumbers.Count);
            Assert.All(randomQuestions, q => Assert.InRange(q.QuestionContent, 1, 3));
        }

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
