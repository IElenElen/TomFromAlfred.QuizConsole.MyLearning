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
    public class QuestionsManagerTests
    {
        [Fact]
        public void Constructor_AddsAllQuestionsFromQuestionService()
        {
            // Arrange
            var fakeQuestions = new List<Question>
                {
                new Question(9, "xyz"),
                new Question(11, "Example question 11"),
                };

            var mockQuestionService = new Mock<QuestionServiceApp>();

            // Konfiguracja zachowania fałszywego obiektu
            mockQuestionService.Setup(x => x.AllQuestions).Returns(fakeQuestions);

            // Act
            var managerApp = new QuestionsManagerApp(mockQuestionService.Object);
            var actualQuestions = managerApp.Questions;

            // Assert
            Assert.Equal(fakeQuestions.Count, actualQuestions.Count); // Sprawdzenie, czy liczba pytań zgadza się
                                                                      
            // Czy wszystkie pytania z QuestionServiceApp zostały dodane do listy Questions
            foreach (var expectedQuestion in fakeQuestions)
            {
                Assert.Contains(expectedQuestion, actualQuestions);
            }
        }
    }
}
