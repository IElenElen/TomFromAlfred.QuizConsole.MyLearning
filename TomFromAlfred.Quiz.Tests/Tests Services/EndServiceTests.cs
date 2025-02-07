using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    public class EndServiceTests
    {

        // Testy - część do analziy


        private readonly EndService _endService;

        public EndServiceTests()
        {
            var ScoreService = new ScoreService(); // Tworzę instancję ScoreService jako zależność
            _endService = new EndService(ScoreService);
        }

        [Fact]
        public void ShouldEnd_ShouldReturnTrue_WhenInputIsLowercaseK()
        {
            // Act
            bool result = _endService.ShouldEnd("k");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ShouldEnd_ShouldReturnTrue_WhenInputIsUppercaseK()
        {
            // Act
            bool result = _endService.ShouldEnd("K");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ShouldEnd_ShouldReturnTrue_WhenInputHasSpaces()
        {
            // Act
            bool result = _endService.ShouldEnd("  k  ");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ShouldEnd_ShouldReturnFalse_WhenInputIsNotK()
        {
            // Act
            bool result = _endService.ShouldEnd("nie");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ShouldEnd_ShouldReturnFalse_WhenInputIsNull()
        {
            // Act
            bool result = _endService.ShouldEnd(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EndQuiz_ShouldDisplayCompletionMessage_WhenQuizIsCompleted() // Do rozważenia
        {
            // Arrange
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter); // Ustawiam przechwytywanie wyjścia konsoli

            // Zastępuję Environment.Exit(0) metodą no-op (która nic nie robi)
            var originalExitMethod = typeof(Environment).GetMethod("Exit", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            var noOpExit = new Action<int>((exitCode) => { /* Nothing */ });
            originalExitMethod.Invoke(null, new object[] { 0 }); // Zastępuję metodę

            // Act
            try
            {
                _endService.EndQuiz(true); // Wykonaj metodę
            }
            catch (Exception) { /* Ignoruj wyjątek */ }

            // Assert
            var output = stringWriter.ToString();
            Assert.Contains("Ukończyłeś / aś Quiz. Dziękujemy za udział!", output); // Oczekiwany tekst
        }

        [Fact] // Do analizy
        public void EndQuiz_ShouldResetScore_WhenQuizIsNotCompleted() // Do rozważenia
        {
            // Arrange
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Zastępuję Environment.Exit(0) metodą no-op
            var originalExitMethod = typeof(Environment).GetMethod("Exit", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            var noOpExit = new Action<int>((exitCode) => { /* Nothing */ });
            originalExitMethod.Invoke(null, new object[] { 0 }); // Zastępuję metodę

            // Act
            try
            {
                _endService.EndQuiz(false); // Wykonaj metodę
            }
            catch (Exception) { }

            // Assert
            var output = stringWriter.ToString();
            Assert.Contains("Quiz został przerwany. Brak punktów.", output); // Oczekiwany tekst
        }
    }
}
