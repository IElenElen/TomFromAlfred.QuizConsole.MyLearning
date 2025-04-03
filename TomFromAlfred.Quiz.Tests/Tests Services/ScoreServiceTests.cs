using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForService;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Oblane: 0 / 19
 
    public class ScoreServiceTests 
    {
        #region  Start ScoreS. Tests
        // 1
        [Theory] // Zaliczony
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(62)]
        [InlineData(100)]
        public void StartNewQuiz_ShouldResetScore_And_UpdateQuizSets(int newActiveQuestionsTotal) // Uruchamia nowy Quiz: resetuje punktację i ustawia nowy zestaw Quizu
        {
            // Arrange
            var scoreService = new ScoreService();

            for (int i = 0; i < 50; i++) scoreService.IncrementScore();

            // Act
            scoreService.StartNewQuiz(newActiveQuestionsTotal);

            // Assert
            scoreService.GetScore().Should().Be(0);

            scoreService.GetTotalActiveSets().Should().Be(newActiveQuestionsTotal);
        }

        // 2 
        [Fact] // Zaliczony
        public void StartNewQuiz_ShouldThrowException_WhenSetInQuizIsZero() // Uruchamia nowy Quiz: wyrzuca wyjątek, jeśli brak zestawu w Quizie
        {
            // Arrange
            var scoreService = new ScoreService();

            // Act
            Action act = () => scoreService.StartNewQuiz(0);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
               .WithMessage("*Liczba pytań musi być większa niż 0.*");
        }

        // 3
        [Fact] // Zaliczony
        public void StartNewQuiz_ShouldThrowException_WhenQuizContainsNegativeQuestionCount() // Uruchamia nowy Quiz: wyrzuca wyjątek, jesli liczba zestawu minusowa
        {
            // Arrange
            var scoreService = new ScoreService();

            // Act
            Action act = () => scoreService.StartNewQuiz(-5);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
               .WithMessage("*Liczba pytań musi być większa niż 0.*");
        }

        // 4
        [Fact] // Zaliczony
        public void StartNewQuiz_ShouldHandleLargeQuizSets() // Wyświetla Quiz nawet z dużą liczbą zestawów
        {
            // Arrange
            var scoreService = new ScoreService();
            int newTotalSet = 1000;

            // Act
            scoreService.StartNewQuiz(newTotalSet);

            // Assert
            scoreService.GetScore().Should().Be(0);

            scoreService.GetTotalActiveSets().Should().Be(newTotalSet);
        }

        // 5 
        [Fact] // Zaliczony
        public void StartNewQuiz_ShouldHandleMaxIntQuestionsWithoutOverflow() // Uruchamia Quiz: zachowanie przy max ilości zestawów Quizu
        {
            // Arrange
            var scoreService = new ScoreService();

            // Act
            scoreService.StartNewQuiz(int.MaxValue);

            // Assert
            scoreService.GetTotalActiveSets().Should().Be(int.MaxValue);

            scoreService.GetScore().Should().Be(0);
        }

        // 6
        [Fact] // Zaliczony
        public void StartNewQuiz_ShouldResetScoreAndUpdateQuiz_WithEachCall() // Uruchamia Quiz: resetuje punkty i ustawia nowy Quiz przy każdym nowym wywołaniu
        {
            // Arrange
            var scoreService = new ScoreService();

            // Act
            scoreService.StartNewQuiz(10);

            scoreService.StartNewQuiz(30);

            scoreService.StartNewQuiz(20);

            // Assert
            scoreService.GetScore().Should().Be(0);

            scoreService.GetTotalActiveSets().Should().Be(20);
        }
#endregion Start ScoreS. Tests

        #region IncrementS. ScoreS. Tests
        // 7
        [Fact] // Zaliczony
        public void IncrementScore_ShouldIncreaseScoreByOne() // Zlicza punktację o 1
        {
            // Arrange
            var scoreService = new ScoreService();

            scoreService.StartNewQuiz(10);

            var initialScore = scoreService.GetScore();

            // Act
            scoreService.IncrementScore();

            // Assert
            scoreService.GetScore().Should().Be(initialScore + 1);
        }

        // 8
        [Fact] // Zaliczony
        public void IncrementScore_ShouldWorkMultipleTimes() // Zlicza punktację po każdym zestawie
        {
            // Arrange
            var scoreService = new ScoreService();

            scoreService.StartNewQuiz(10);

            var initialScore = scoreService.GetScore();

            // Act
            scoreService.IncrementScore();

            scoreService.IncrementScore();

            scoreService.IncrementScore();

            // Assert
            scoreService.GetScore().Should().Be(initialScore + 3);
        }

        // 9
        [Fact] // Zaliczony
        public void IncrementScore_ShouldNotAffectTotalQuestions() // Zlicza punkty, liczba pytań pozostaje ta sama
        {
            // Arrange
            var scoreService = new ScoreService();

            scoreService.StartNewQuiz(15);

            // Act
            scoreService.IncrementScore();

            scoreService.IncrementScore();

            scoreService.IncrementScore();

            // Assert
            scoreService.GetTotalActiveSets().Should().Be(15);
        }
#endregion IncrementS. ScoreS. Tests

        #region GetScore ScoreS. Tests
        // 10
        [Fact] // Zaliczony
        public void GetScore_ShouldReturnZero_WhenQuizNotStarted() // Podaje 0, jeśli Quiz nie został uruchomiony
        {
            // Arrange
            var scoreService = new ScoreService();

            // Act
            var score = scoreService.GetScore();

            // Assert
            score.Should().Be(0);
        }

        // 11
        [Fact] // Zaliczony
        public void GetScore_ShouldReturnCorrectScore_AfterMultipleIncrements() // Podaje wynik: poprawna punktacja, po zliczeniu poprawnych odpowiedzi
        {
            // Arrange
            var scoreService = new ScoreService();

            scoreService.StartNewQuiz(25);

            // Act
            scoreService.IncrementScore();

            scoreService.IncrementScore();

            scoreService.IncrementScore();

            scoreService.IncrementScore();

            // Assert
            scoreService.GetScore().Should().Be(4);
        }

        // 12
        [Fact] // Zaliczony
        public void GetScore_ShouldReturnZero_AfterReset() // Podaje wynik: zwraca zero po resecie
        {
            // Arrange
            var scoreService = new ScoreService();

            scoreService.StartNewQuiz(10);

            scoreService.IncrementScore();

            // Act
            scoreService.ResetScore();

            // Assert
            scoreService.GetScore().Should().Be(0);
        }

        // 13
        [Fact] // Zaliczony
        public void GetScore_ShouldReturnZero_AfterQuizStartedAndNoIncrement() // Podaje punktację: podaje 0, jeśli nie było inkrementacji
        {
            // Arrange
            var scoreService = new ScoreService();

            scoreService.StartNewQuiz(5);

            // Act
            var scoreNoIncrement = scoreService.GetScore();

            // Assert
            scoreNoIncrement.Should().Be(0);
        }
#endregion GetScore ScoreS. Tests

        // 14
        [Fact] // Zaliczony
        public void ResetScore_ShouldSetScoreToZero_AfterMultipleIncrements_QuizBreak() // Podaje wynik: zwraca 0, po kilku odpowiedziach - przerwanie Quizu
        {
            // Arrange
            var scoreService = new ScoreService();

            scoreService.StartNewQuiz(8);

            scoreService.IncrementScore();

            scoreService.IncrementScore();

            scoreService.IncrementScore();

            var scoreBeforeReset = scoreService.GetScore();

            scoreBeforeReset.Should().BeGreaterThan(0);

            // Act
            scoreService.ResetScore();

            // Assert
            scoreService.GetScore().Should().Be(0);
        }

        #region GetPercentage ScoreS.Tests
        // 15
        [Fact] // Zaliczony
        public void GetPercentage_ShouldReturn100_WhenAllAnswersAreCorrect() // Podaje procenty: zwraca 100, jeśli wszystkie odpowiedzi użytkownika są dobre
        {
            // Arrange
            var scoreService = new ScoreService();

            scoreService.StartNewQuiz(10);

            for (int i = 0; i < 10; i++)
            {
                scoreService.IncrementScore();
            }

            // Act
            var percentage = scoreService.GetPercentage();

            // Assert
            percentage.Should().Be(100);
        }

        // 16
        [Fact] // Zaliczony
        public void GetPercentage_ShouldReturn0_WhenNoAnswersAreCorrect() // Podaje procenty: zwraca 0, bo nie było poprawnej odpowiedzi od użytkownika lub nastąpił reset
        {
            // Arrange
            var scoreService = new ScoreService();

            scoreService.StartNewQuiz(9);

            // Act
            var percentage = scoreService.GetPercentage();

            // Assert
            percentage.Should().Be(0);
        }

        // 17
        [Fact] // Zaliczony
        public void GetPercentage_ShouldReturnZero_AfterScoreReset() // Podaje procenty: zwraca 0, jeśli nastąpiło przerwanie Quizu
        {
            // Arrange
            var scoreService = new ScoreService();

            scoreService.StartNewQuiz(9);

            scoreService.IncrementScore();

            scoreService.ResetScore();

            // Act
            var percentage = scoreService.GetPercentage();

            // Assert
            percentage.Should().Be(0);
        }

        // 18
        [Fact] // Zaliczony
        public void GetPercentage_ShouldReturnCorrectPercentage_WhenSomeAnswersAreCorrect() // Podaje procent: zwraca poprawne %, jeśli kilka odpowiedzi użytkownika było dobrych
        {
            // Arrange
            var scoreService = new ScoreService();

            scoreService.StartNewQuiz(10);

            scoreService.IncrementScore();

            // Act
            var percentage = scoreService.GetPercentage();

            // Assert
            percentage.Should().Be(10);
        }
#endregion GetPercentage ScoreS.Tests

        // 19
        [Fact] // Zaliczony
        public void DisplayScoreSummary_ShouldPrintCorrectSummary() // Wyświetla: podaje poprawny wynik użytkownika
        {
            // Arrange
            var scoreService = new ScoreService();

            scoreService.StartNewQuiz(10);

            scoreService.IncrementScore();

            using var sw = new StringWriter();

            var originalOut = Console.Out;
            Console.SetOut(sw);

            // Act
            scoreService.DisplayScoreSummary();

            // Restore output
            Console.SetOut(originalOut);

            // Assert
            var output = sw.ToString();

            output.Should().Contain("Zdobyte punkty: 1/10");

            output.Should().Contain("Procent poprawnych odpowiedzi: 10.00%");
        }
    }
}
