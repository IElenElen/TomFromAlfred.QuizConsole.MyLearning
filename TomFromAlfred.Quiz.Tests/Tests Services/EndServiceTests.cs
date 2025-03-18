using System;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForService;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Oblane: 0 / 4
     
    public class EndServiceTests
    {
        private readonly EndService _endService;
        private readonly Mock<IScoreService> _mockScoreService; // Mockowanie ScoreService

        public EndServiceTests()
        {
            _mockScoreService = new Mock<IScoreService>(); // Tworzenie mocka
            _endService = new EndService(_mockScoreService.Object); // Przekazanie mocka do EndService
        }

        // 1 
        [Theory] // Zaliczony
        [InlineData("k")] // mała litera
        [InlineData("K")] // wielka litera
        public void ShouldEnd_ShouldReturnTrue_WhenInputIsK(string input) // Kończy Quiz: jeśli użytkownik wpisze "k"
        {
            // Act
            bool result = _endService.ShouldEnd(input);

            // Assert
            Assert.True(result);
        }

        // 2
        [Theory] // Zaliczony
        [InlineData("  k  ", false)] 
        [InlineData("  K  ", false)] 
        [InlineData(" ", false)]  
        [InlineData("", false)]   
        public void ShouldEnd_ShouldReturnExpectedResult(string input, bool expected) // Kończy: NIE kończy, jeśli widzi inne zachowania niż wprowadzenie k
        {
            // Act
            bool result = _endService.ShouldEnd(input);

            // Assert
            Assert.Equal(expected, result);
        }

        // 3
        [Fact] // Zaliczony
        public void EndQuiz_ShouldReturnCompletionMessageAndDisplayScore_WhenQuizIsCompleted() // Kończy: zwraca info o uzyskanej punktacji, jeśli Quiz ukończony
        {
            // Act
            string result = _endService.EndQuiz(true);

            // Assert
            Assert.Equal("Ukończyłeś / aś Quiz. Dziękujemy za udział!", result);

            // Sprawdzenie, czy metoda `DisplayScoreSummary()` została wywołana raz
            _mockScoreService.Verify(s => s.DisplayScoreSummary(), Times.Once);
        }

        // 4
        [Fact] // Zaliczony
        public void EndQuiz_ShouldResetScore_WhenQuizIsNotCompleted() // Kończy: reset punktacji, jesli Quiz nie jest ukończony
        {
            // Act
            string result = _endService.EndQuiz(false);

            // Assert
            Assert.Equal("Quiz został przerwany. Brak punktów.", result);

            // Sprawdzenie, czy `ResetScore()` zostało wywołane
            _mockScoreService.Verify(s => s.ResetScore(), Times.Once);
        }
    }
}
