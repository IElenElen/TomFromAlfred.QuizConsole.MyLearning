using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;

namespace TomFromAlfred.QuizConsole.Tests.Integration_Tests
{
    // Oblane: / 1

    public class TestForDisplayScoreSummary
    {
        private readonly ScoreService _scoreService;

        public TestForDisplayScoreSummary()
        {
            _scoreService = new ScoreService(); // Inicjalizacja obiektu ScoreService
        }

        // 1
        [Fact]
        public void DisplayScoreSummary_ShouldDisplayCorrectSummary()
        {
            // Arrange
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter); // Przechwycenie danych wypisywanych na konsolę

            _scoreService.StartNewQuiz(10); // Inicjalizuję quiz z 10 pytaniami
            _scoreService.IncrementScore(); // Zwiększam wynik o 1

            // Act
            _scoreService.DisplayScoreSummary(); // Wywołanie metody, która wypisuje wynik

            // Assert
            var output = stringWriter.ToString().Trim(); // Pobranie przechwyconego tekstu i usunięcie zbędnych białych znaków

            Assert.Contains("Zdobyte punkty: 1/10", output); // Sprawdzam, czy wynik zawiera poprawny tekst
            Assert.Contains("Procent poprawnych odpowiedzi:", output); // Sprawdzam, czy tekst zawiera frazę procentową
            Assert.Contains("10", output); // Sprawdzam, czy wynik zawiera procent (elastyczność w formacie)
        }
    }
}
