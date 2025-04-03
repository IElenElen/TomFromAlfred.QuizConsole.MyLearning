using System;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForService;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using FluentAssertions;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Oblane: 0 / 4
     
    public class EndServiceTests
    {
        // 1 
        [Theory] // Zaliczony
        [InlineData("k")] // mała litera
        [InlineData("K")] // wielka litera
        public void ShouldEnd_ShouldReturnTrue_WhenInputIsK(string input) // Kończy Quiz: jeśli użytkownik wpisze "k"
        {
            // Arrange
            var mockScore = new Mock<IScoreService>();

            var endService = new EndService(mockScore.Object);

            // Act
            var result = endService.ShouldEnd(input);

            // Assert
            result.Should().BeTrue();
        }

        // 2
        [Theory] // Zaliczony
        [InlineData("  k  ", false)] 
        [InlineData("  K  ", false)] 
        [InlineData(" ", false)]  
        [InlineData("", false)]   
        public void ShouldEnd_ShouldReturnExpectedResult(string input, bool expected) // Kończy: NIE kończy, jeśli widzi inne zachowania niż wprowadzenie k
        {
            // Arrange
            var mockScore = new Mock<IScoreService>();

            var endService = new EndService(mockScore.Object);

            // Act
            var result = endService.ShouldEnd(input);

            // Assert
            result.Should().Be(expected);
        }

        // 3
        [Fact] // Zaliczony
        public void EndQuiz_ShouldReturnCompletionMessageAndDisplayScore_WhenQuizIsCompleted() // Kończy: zwraca info o uzyskanej punktacji, jeśli Quiz ukończony
        {
            // Arrange
            var mockScore = new Mock<IScoreService>();

            var endService = new EndService(mockScore.Object);

            // Act
            var result = endService.EndQuiz(true);

            // Assert
            result.Should().Be("Ukończyłeś / aś Quiz. Dziękujemy za udział!");

            mockScore.Verify(s => s.DisplayScoreSummary(), Times.Once);
        }

        // 4
        [Fact] // Zaliczony
        public void EndQuiz_ShouldResetScore_WhenQuizIsNotCompleted() // Kończy: reset punktacji, jesli Quiz nie jest ukończony
        {
            // Arrange
            var mockScore = new Mock<IScoreService>();

            var endService = new EndService(mockScore.Object);

            // Act
            var result = endService.EndQuiz(false);

            // Assert
            result.Should().Be("Quiz został przerwany. Brak punktów.");

            mockScore.Verify(s => s.ResetScore(), Times.Once);
        }
    }
}
