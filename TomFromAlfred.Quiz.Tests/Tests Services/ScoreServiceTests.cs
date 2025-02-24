using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Oblane: 0 / 18
    public class ScoreServiceTests 
    {
        private readonly ITestOutputHelper _output;
        private ScoreService _scoreService; // Testuję tę klasę

        public ScoreServiceTests(ITestOutputHelper output) 
        {
            _output = output;
            _scoreService = new ScoreService(); // Nowy obiekt na potrzeby testu
        }

        // 1
        [Theory] // Zaliczony
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(62)]
        [InlineData(100)]
        public void StartNewQuiz_ShouldResetScore_And_UpdateQuizSets(int newActiveQuestionsTotal) // Uruchamia nowy Quiz: resetuje punktację i ustawia nowy zestaw Quizu
        {
            // Arrange
            for (int i = 0; i < 50; i++)
            {
                _scoreService.IncrementScore(); // Wcześniejszy wynik (po poprzednim uruchomieniu Quizu)
            }

            // Act
            _scoreService.StartNewQuiz(newActiveQuestionsTotal);

            // Assert
            Assert.Equal(0, _scoreService.GetScore());
            Assert.Equal(newActiveQuestionsTotal, _scoreService.GetTotalActiveSets());
        }

        // 2 
        [Fact] // Zaliczony
        public void StartNewQuiz_ShouldThrowException_WhenSetInQuizIsZero() // Uruchamia nowy Quiz: wyrzuca wyjątek, jeśli brak zestawu w Quizie
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _scoreService.StartNewQuiz(0));
        }

        // 3
        [Fact] // Zaliczony
        public void StartNewQuiz_ShouldThrowException_WhenQuizContainsNegativeQuestionCount() // Uruchamia nowy Quiz: wyrzuca wyjątek, jesli liczba zestawu minusowa
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _scoreService.StartNewQuiz(-5));
        }

        // 4
        [Fact] // Zaliczony
        public void StartNewQuiz_ShouldHandleLargeQuizSets() // Wyświetla Quiz nawet z dużą liczbą zestawów
        {
            // Arrange
            int newTotalSet = 1000;

            // Act
            _scoreService.StartNewQuiz(newTotalSet);

            // Assert
            Assert.Equal(0, _scoreService.GetScore()); // Punkty nadal powinny wynosić 0
            Assert.Equal(1000, newTotalSet); // Liczba pytań powinna być 1000
        }

        // 5 
        [Fact] // Zaliczony
        public void StartNewQuiz_ShouldHandleMaxQuizSets() // Uruchamia Quiz: zachowanie przy max ilości zestawów Quizu
        {
            _scoreService.StartNewQuiz(int.MaxValue);
            Assert.Equal(int.MaxValue, _scoreService.GetTotalActiveSets());
        }

        // 6
        [Fact] // Zaliczony
        public void StartNewQuiz_ShouldResetScoreAndUpdateQuiz_WithEachCall() // Uruchamia Quiz: resetuje punkty i ustawia nowy Quiz przy każdym nowym wywołaniu
        {
            // Arrange
            int firstQuizActiveQuestions = 10;
            int secondQuizActiveQuestions = 30;
            int thirdQuizActiveQuestions = 20;

            // Act
            _scoreService.StartNewQuiz(firstQuizActiveQuestions);
            _scoreService.StartNewQuiz(secondQuizActiveQuestions);
            _scoreService.StartNewQuiz(thirdQuizActiveQuestions);

            // Assert
            Assert.Equal(0, _scoreService.GetScore()); // Po każdym wywołaniu punkty powinny być zresetowane
            Assert.Equal(thirdQuizActiveQuestions, _scoreService.GetTotalActiveSets()); // Liczba zestawów powinna wynosić 30 w trzecim wywołaniu
        }

        // 7
        [Fact] // Zaliczony
        public void IncrementScore_ShouldIncreaseScoreByOne() // Zlicza punktację o 1
        {
            // Arrange
            _scoreService.StartNewQuiz(10); // Inicjalizuję Quiz z 10 pytaniami
            var initialScore = _scoreService.GetScore(); // Początkowy wynik

            // Act
            _scoreService.IncrementScore(); 

            // Assert
            Assert.Equal(initialScore + 1, _scoreService.GetScore()); // Sprawdzam, czy wynik się zwiększył o 1
        }

        // 8
        [Fact] // Zaliczony
        public void IncrementScore_ShouldWorkMultipleTimes() // Zlicza punktację po każdym zestawie
        {
            // Arrange
            _scoreService.StartNewQuiz(10); // Inicjalizuję quiz z 10 pytaniami
            var initialScore = _scoreService.GetScore(); // Początkowy wynik

            // Act
            _scoreService.IncrementScore(); // Zwiększam wynik o 1
            _scoreService.IncrementScore(); // Zwiększam wynik o 1
            _scoreService.IncrementScore(); // Zwiększam wynik o 1

            // Assert
            Assert.Equal(initialScore + 3, _scoreService.GetScore()); // Sprawdzam, czy wynik został zwiększony 3 razy
        }

        // 9
        [Fact] // Zaliczony
        public void IncrementScore_ShouldNotAffectTotalQuestions() // Zlicza punkty, liczba pytań pozostaje ta sama
        {
            // Arrange
            _scoreService.StartNewQuiz(15); // Inicjalizuję quiz z 15 pytaniami

            // Act
            _scoreService.IncrementScore(); // Zwiększam wynik o 1
            _scoreService.IncrementScore(); // Drugie zwiększenie wyniku
            _scoreService.IncrementScore(); // Kolejne zwiększenie wyniku

            // Assert
            Assert.Equal(15, _scoreService.GetTotalActiveSets()); // Sprawdzam, czy liczba pytań pozostała niezmieniona
        }

        // 10
        [Fact] // Zaliczony
        public void GetScore_ShouldReturnZero_WhenQuizNotStarted() // Podaje 0, jeśli Quiz nie został uruchomiony
        {
            // Act
            var score = _scoreService.GetScore();

            // Assert
            Assert.Equal(0, score); // Sprawdzam, czy wynik wynosi 0, jeśli Quiz nie został rozpoczęty
        }

        // 11
        [Fact] // Zaliczony
        public void GetScore_ShouldReturnCorrectScore_AfterMultipleIncrements() // Podaje wynik: poprawna punktacja, po zliczeniu poprawnych odpowiedzi
        {
            // Arrange
            _scoreService.StartNewQuiz(25); // Inicjalizuję Quiz z 25 pytaniami

            // Act
            _scoreService.IncrementScore();
            _scoreService.IncrementScore();
            _scoreService.IncrementScore();
            _scoreService.IncrementScore();

            var score = _scoreService.GetScore(); // Pobieram wynik

            // Assert
            Assert.Equal(4, score); // Sprawdzam, czy wynik wynosi 4 po kilkukrotnej inkrementacji
        }

        // 12
        [Fact] // Zaliczony
        public void GetScore_ShouldReturnZero_AfterReset() // Podaje wynik: zwraca zero po resecie
        {
            // Arrange
            _scoreService.StartNewQuiz(10); // Inicjalizuję quiz z 10 pytaniami
            _scoreService.IncrementScore(); // Zwiększam wynik o 1

            // Act
            _scoreService.ResetScore(); // Resetuję wynik

            // Assert
            Assert.Equal(0, _scoreService.GetScore()); // Sprawdzam, czy wynik został zresetowany do 0
        }

        // 13
        [Fact] // Zaliczony
        public void GetScore_ShouldReturnZero_AfterQuizStartedAndNoIncrement() // Podaje punktację: podaje 0, jeśli nie było inkrementacji
        {
            // Arrange
            _scoreService.StartNewQuiz(5); // Inicjalizuję quiz

            // Act
            var scoreNoIncrement = _scoreService.GetScore(); // Pobieram wynik

            // Assert
            Assert.Equal(0, scoreNoIncrement); // Sprawdzam, czy wynik wynosi 0, jeśli nie było inkrementacji
        } 

        // 14
        [Fact] // Zaliczony
        public void ResetScore_ShouldSetScoreToZero_AfterMultipleIncrements_QuizBreak() // Podaje wynik: zwraca 0, po kilku odpowiedziach - przerwanie Quizu
        {
            // Arrange
            _output.WriteLine("=== Start test ===");
            _output.WriteLine($"Calling StartNewQuiz(8)");
            _scoreService.StartNewQuiz(8); // Inicjalizuję Quiz

            _output.WriteLine("StartNewQuiz() wywołane.");
            _output.WriteLine($"Instance ID after StartNewQuiz: {_scoreService.GetHashCode()}");

            // Inkrementacja wyniku
            _scoreService.IncrementScore();
            _scoreService.IncrementScore();
            _scoreService.IncrementScore();

            // Pobranie wyniku przed resetem
            var scoreBeforeReset = _scoreService.GetScore();
            _output.WriteLine($"Score before reset: {scoreBeforeReset}");

            // **🔹 Asercja przed resetem!**
            Assert.True(scoreBeforeReset > 0, "Score should be greater than 0 before reset.");

            // Act - reset wyniku
            _scoreService.ResetScore();
            var scoreAfterReset = _scoreService.GetScore();
            _output.WriteLine($"Score after reset: {scoreAfterReset}");

            // Assert - wynik powinien być 0 po resecie
            Assert.Equal(0, scoreAfterReset);

            _output.WriteLine("=== End test ===");
        }

        // 15
        [Fact] // Zaliczony
        public void GetPercentage_ShouldReturn100_WhenAllAnswersAreCorrect() // Podaje procenty: zwraca 100, jeśli wszystkie odpowiedzi użytkownika są dobre
        {
            // Arrange
            _scoreService.StartNewQuiz(10); // Inicjalizuję Quiz z 10 pytaniami
            for (int i = 0; i < 10; i++)
            {
                _scoreService.IncrementScore(); // Inkrementacja wyniku dla każdej odpowiedzi
            }

            // Act
            var percentage = _scoreService.GetPercentage(); // Pobieram procent

            // Assert
            Assert.Equal(100, percentage); // Oczekiwany wynik to 100%
        }

        // 16
        [Fact] // Zaliczony
        public void GetPercentage_ShouldReturn0_WhenNoAnswersAreCorrect() // Podaje procenty: zwraca 0, bo nie było poprawnej odpowiedzi od użytkownika lub nastąpił reset
        {
            // Arrange
            _scoreService.StartNewQuiz(9); // Inicjalizuję Quiz z 9 pytaniami

            // Act
            var percentage = _scoreService.GetPercentage(); // Pobieram procent

            // Assert
            Assert.Equal(0, percentage); // Oczekiwany wynik to 0%, ponieważ nie zwiększałam wyniku
        }

        // 17
        [Fact] // Zaliczony
        public void GetPercentage_ShouldReturnZero_AfterScoreReset() // Podaje procenty: zwraca 0, jeśli nastąpiło przerwanie Quizu
        {
            // Arrange
            _scoreService.StartNewQuiz(9); // Quiz z 9 pytaniami
            _scoreService.IncrementScore(); // Poprawna odpowiedź, wynik powinien się zwiększyć
            _scoreService.ResetScore(); // Resetuję wynik

            // Act
            var percentage = _scoreService.GetPercentage(); // Pobieram procent

            // Assert
            Assert.Equal(0, percentage); // Powinno zwrócić 0%, bo wynik został zresetowany
        }

        // 18
        [Fact] // Zaliczony
        public void GetPercentage_ShouldReturnCorrectPercentage_WhenSomeAnswersAreCorrect() // Podaje procent: zwraca poprawne %, jeśli kilka odpowiedzi użytkownika było dobrych
        {
            // Arrange
            _scoreService.StartNewQuiz(10); // Powinno ustawić AllActiveQuizSets = 10
            _scoreService.IncrementScore(); // Powinno zwiększyć Score o 1

            // Debugowanie
            Assert.Equal(10, _scoreService.AllActiveQuizSets); // Sprawdzenie liczby pytań
            Assert.Equal(1, _scoreService.Score); // Sprawdzenie, czy wynik się zwiększył

            // Act
            var percentage = _scoreService.GetPercentage();

            // Assert
            Assert.Equal(10, percentage); // Powinno zwrócić 10%
        }
    }
}
