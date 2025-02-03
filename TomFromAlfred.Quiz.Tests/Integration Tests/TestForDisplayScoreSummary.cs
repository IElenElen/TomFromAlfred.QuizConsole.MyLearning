using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;

namespace TomFromAlfred.QuizConsole.Tests.Integration_Tests
{
    public class TestForDisplayScoreSummary
    {
        private readonly ScoreService _scoreService;

        public TestForDisplayScoreSummary()
        {
            _scoreService = new ScoreService(); // Inicjalizacja obiektu ScoreService
        }

        [Fact]
        public void DisplayScoreSummary_ShouldDisplayCorrectSummary()
        {
            // Arrange
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter); // Przechwycenie danych wypisywanych na konsolę

            _scoreService.StartNewQuiz(10); // Inicjalizujemy quiz z 10 pytaniami
            _scoreService.IncrementScore(); // Zwiększamy wynik o 1

            // Act
            _scoreService.DisplayScoreSummary(); // Wywołanie metody, która wypisuje wynik

            // Assert
            var output = stringWriter.ToString().Trim(); // Pobranie przechwyconego tekstu i usunięcie zbędnych białych znaków
            Assert.Contains("Zdobyte punkty: 1/10", output); // Sprawdzenie, czy wynik zawiera poprawny tekst
            Assert.Contains("Procent poprawnych odpowiedzi:", output); // Sprawdzenie, czy tekst zawiera frazę procentową
            Assert.Contains("10", output); // Sprawdzamy, czy wynik zawiera procent (elastyczność w formacie)
        }
    }
}
