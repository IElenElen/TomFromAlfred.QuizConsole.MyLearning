using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Oblane: 1, przerwane: 2 = do analizy / 6
     
    public class EndServiceTests
    {
        private readonly EndService _endService;

        public EndServiceTests()
        {
            var ScoreService = new ScoreService(); // Tworzę instancję ScoreService jako zależność
            _endService = new EndService(ScoreService);
        }

        // 1 
        [Theory] // Zaliczony
        [InlineData("k")] // mała litera
        [InlineData("K")] // wielka litera
        public void ShouldEnd_ShouldReturnTrue_WhenInputIsK(string input) // System kończy Quiz, jesli użytkownik wpisze "k"
        {
            // Act
            bool result = _endService.ShouldEnd(input);

            // Assert
            Assert.True(result);
        }

        // 2
        [Fact] // Oblany
        public void ShouldEnd_ShouldReturnFalse_WhenInputHasSpaces() // Jeśli system dostaje spacje - czeka na właściwy sygnał od użytkownika
        {
            // Act
            bool result = _endService.ShouldEnd("  k  ");

            // Assert
            Assert.False(result);
        }

        // 3
        [Fact] // Zaliczony
        public void ShouldEnd_ShouldReturnFalse_WhenInputIsNotK() // Jeśli użytkownik nie wprowadzi K, system nie kończy Quizu
        {
            // Act
            bool result = _endService.ShouldEnd("nie");

            // Assert
            Assert.False(result);
        }

        // 4
        [Fact] // Zaliczony
        public void ShouldEnd_ShouldReturnFalse_WhenInputIsNull() // Brak danych od użytkownika, system czeka na interakcję użytkownika
        {
            // Act
            bool result = _endService.ShouldEnd(null);

            // Assert
            Assert.False(result);
        }

        // 5
        [Fact] // Test przerwany ???
        public void EndQuiz_ShouldDisplayCompletionMessage_WhenQuizIsCompleted() // Po ukończonym quizie daje oczekiwaną punktację
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

        // 6
        [Fact] // Do analizy, test przerwany ???
        public void EndQuiz_ShouldResetScore_WhenQuizIsNotCompleted() // Reset punktacji, jesli quiz nie zakończony
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
