using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;

namespace TomFromAlfred.QuizConsole.Tests.Integration_Tests
{
    // Oblane: 0 / 1

    public class TestForDisplayScoreSummary
    {
        // 1
        [Fact] // Zaliczony
        public void DisplayScoreSummary_ShouldDisplayCorrectSummary() // Wyświetla: zdobyte punkty
        {
            // Arrange
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            var scoreService = new ScoreService();
            scoreService.StartNewQuiz(10);
            scoreService.IncrementScore();

            // Act
            scoreService.DisplayScoreSummary();

            // Assert
            var output = stringWriter.ToString();

            output.Should().Contain("Zdobyte punkty: 1/10")
                  .And.Contain("Procent poprawnych odpowiedzi:")
                  .And.MatchRegex(@"\d{1,3}(\.\d{1,2})?%");
        }
    }
}
