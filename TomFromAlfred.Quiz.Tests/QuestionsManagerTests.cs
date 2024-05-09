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
            var mockQuestionService = new Mock<QuestionServiceApp>(); // Tworzenie fałszywego obiektu QuestionServiceApp
            var fakeQuestions = new List<Question>
                {
            new Question { QuestionNumber = 9, QuestionContent = "xyz"}
            //new Question { QuestionNumber = 11, QuestionContent = "Example question 11" },
        };
            mockQuestionService.Setup(x => x.AllQuestions).Returns(fakeQuestions); // Konfiguracja zachowania fałszywego obiektu

            // Act
            var managerApp = new QuestionsManagerApp();
            var actualQuestions = managerApp.Questions;

            // Assert
            Assert.Equal(fakeQuestions.Count, actualQuestions.Count); // Sprawdzenie, czy liczba pytań zgadza się
                                                                      
            // Sprawdź, czy wszystkie pytania z QuestionServiceApp zostały dodane do listy Questions
            foreach (var expectedQuestion in fakeQuestions)
            {
                Assert.Contains(expectedQuestion, actualQuestions);
            }
        }
    }
}
