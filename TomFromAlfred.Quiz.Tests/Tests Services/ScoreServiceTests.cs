using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    public class ScoreServiceTests
    {
        private ScoreService _scoreService; // Testujemy tę klasę

        public ScoreServiceTests()
        {
            _scoreService = new ScoreService();
        }

        [Fact]
        public void StartNewQuiz_ShouldResetScoreAndSetTotalQuestions()
        {
            // Arrange
            int totalQuestions = 10;

            // Act
            _scoreService.StartNewQuiz(totalQuestions);

            // Assert
            Assert.Equal(0, _scoreService.GetScore()); // Punkty powinny być zresetowane
            Assert.Equal(10, totalQuestions); // Liczba pytań ustawiona poprawnie
        }

        [Fact]
        public void StartNewQuiz_ShouldHandleZeroTotalQuestions()
        {
            //Arrange
            int totalQuestions = 0;

            // Act
            _scoreService.StartNewQuiz(totalQuestions);

            // Assert
            Assert.Equal(0, _scoreService.GetScore()); // Punkty nadal powinny wynosić 0
            Assert.Equal(0, totalQuestions); // Liczba pytań powinna być 0
        }

        [Fact]
        public void StartNewQuiz_ShouldHandleLargeTotalQuestions()
        {
            // Arrange
            int totalQuestions = 1000;

            // Act
            _scoreService.StartNewQuiz(totalQuestions);

            // Assert
            Assert.Equal(0, _scoreService.GetScore()); // Punkty nadal powinny wynosić 0
            Assert.Equal(1000, totalQuestions); // Liczba pytań powinna być 1000
        }

        [Fact]
        public void StartNewQuiz_ShouldHandleNegativeTotalQuestions()
        {
            // Arrange
            int totalQuestions = -10; // Liczba pytań ujemna

            // Act
            _scoreService.StartNewQuiz(totalQuestions);

            // Assert
            Assert.Equal(0, _scoreService.GetScore()); // Punkty powinny być 0
            Assert.Equal(-10, totalQuestions); // Liczba pytań powinna być -10
        }

        [Fact]
        public void StartNewQuiz_ShouldResetScoreAndSetTotalQuestions_WhenCalledOnce()
        {
            // Arrange
            int totalQuestions = 10;

            // Act
            _scoreService.StartNewQuiz(totalQuestions);

            // Assert
            Assert.Equal(0, _scoreService.GetScore()); // Punktacja powinna być zresetowana
            Assert.Equal(10, totalQuestions); // Liczba pytań powinna być ustawiona na 10
        }

        [Fact]
        public void StartNewQuiz_ShouldResetScoreAndSetTotalQuestions_WhenCalledMultipleTimes()
        {
            // Arrange
            int firstQuizQuestions = 10;
            int secondQuizQuestions = 20;

            // Act
            _scoreService.StartNewQuiz(firstQuizQuestions);
            _scoreService.StartNewQuiz(secondQuizQuestions);

            // Assert
            Assert.Equal(0, _scoreService.GetScore()); // Po pierwszym wywołaniu punkty powinny być zresetowane
            Assert.Equal(0, _scoreService.GetScore()); // Po drugim wywołaniu punkty również powinny być zresetowane
        }

        [Fact]
        public void StartNewQuiz_ShouldUpdateTotalQuestionsWithEachCall()
        {
            // Arrange
            int firstQuizQuestions = 5;
            int secondQuizQuestions = 15;
            int thirdQuizQuestions = 30;

            // Act
            _scoreService.StartNewQuiz(firstQuizQuestions);
            _scoreService.StartNewQuiz(secondQuizQuestions);
            _scoreService.StartNewQuiz(thirdQuizQuestions);

            // Assert
            Assert.Equal(0, _scoreService.GetScore()); // Punkty powinny być 0 po każdym wywołaniu
            Assert.Equal(30, thirdQuizQuestions); // Ostateczna liczba pytań powinna wynosić 30
        }
        [Fact]
        public void IncrementScore_ShouldIncreaseScoreByOne()
        {
            // Arrange
            _scoreService.StartNewQuiz(10); // Inicjalizujemy quiz z 10 pytaniami
            var initialScore = _scoreService.GetScore(); // Początkowy wynik

            // Act
            _scoreService.IncrementScore(); // Zwiększamy wynik o 1

            // Assert
            Assert.Equal(initialScore + 1, _scoreService.GetScore()); // Sprawdzamy, czy wynik się zwiększył o 1
        }

        [Fact]
        public void IncrementScore_ShouldNotAffectTotalQuestions()
        {
            // Arrange
            _scoreService.StartNewQuiz(10); // Inicjalizujemy quiz z 10 pytaniami

            // Act
            _scoreService.IncrementScore(); // Zwiększamy wynik o 1

            // Assert
            Assert.Equal(10, _scoreService.GetTotalQuestions()); // Sprawdzamy, czy liczba pytań pozostała niezmieniona
        }

        [Fact]
        public void IncrementScore_ShouldWorkMultipleTimes()
        {
            // Arrange
            _scoreService.StartNewQuiz(10); // Inicjalizujemy quiz z 10 pytaniami
            var initialScore = _scoreService.GetScore(); // Początkowy wynik

            // Act
            _scoreService.IncrementScore(); // Zwiększamy wynik o 1
            _scoreService.IncrementScore(); // Zwiększamy wynik o 1
            _scoreService.IncrementScore(); // Zwiększamy wynik o 1

            // Assert
            Assert.Equal(initialScore + 3, _scoreService.GetScore()); // Sprawdzamy, czy wynik został zwiększony 3 razy
        }

        [Fact]
        public void GetScore_ShouldReturnZero_WhenQuizNotStarted()
        {
            // Act
            var score = _scoreService.GetScore();

            // Assert
            Assert.Equal(0, score); // Sprawdzamy, czy wynik wynosi 0, jeśli quiz nie został rozpoczęty
        }

        [Fact]
        public void GetScore_ShouldReturnCorrectScore_AfterIncrement()
        {
            // Arrange
            _scoreService.StartNewQuiz(10); // Inicjalizujemy quiz z 10 pytaniami

            // Act
            _scoreService.IncrementScore(); // Zwiększamy wynik o 1
            var score = _scoreService.GetScore(); // Pobieramy wynik

            // Assert
            Assert.Equal(1, score); // Sprawdzamy, czy wynik wynosi 1 po inkrementacji
        }

        [Fact]
        public void GetScore_ShouldReturnZero_AfterReset()
        {
            // Arrange
            _scoreService.StartNewQuiz(10); // Inicjalizujemy quiz z 10 pytaniami
            _scoreService.IncrementScore(); // Zwiększamy wynik o 1

            // Act
            _scoreService.ResetScore(); // Resetujemy wynik

            // Assert
            Assert.Equal(0, _scoreService.GetScore()); // Sprawdzamy, czy wynik został zresetowany do 0
        }

        [Fact]
        public void GetScore_ShouldReturnZero_AfterQuizStartedAndNoIncrement()
        {
            // Arrange
            _scoreService.StartNewQuiz(10); // Inicjalizujemy quiz

            // Act
            var score = _scoreService.GetScore(); // Pobieramy wynik

            // Assert
            Assert.Equal(0, score); // Sprawdzamy, czy wynik wynosi 0, jeśli nie było inkrementacji
        }

        [Fact]
        public void GetScore_ShouldReturnCorrectValue_AfterMultipleIncrements()
        {
            // Arrange
            _scoreService.StartNewQuiz(10); // Inicjalizujemy quiz
            _scoreService.IncrementScore(); // Pierwsza inkrementacja
            _scoreService.IncrementScore(); // Druga inkrementacja

            // Act
            var score = _scoreService.GetScore(); // Pobieramy wynik

            // Assert
            Assert.Equal(2, score); // Sprawdzamy, czy wynik wynosi 2 po dwóch inkrementacjach
        }

        [Fact]
        public void GetScore_ShouldReturnZero_AfterResetAfterMultipleIncrements()
        {
            // Arrange
            _scoreService.StartNewQuiz(10); // Inicjalizujemy quiz
            _scoreService.IncrementScore(); // Pierwsza inkrementacja
            _scoreService.IncrementScore(); // Druga inkrementacja
            _scoreService.ResetScore(); // Resetowanie wyniku

            // Act
            var score = _scoreService.GetScore(); // Pobieramy wynik

            // Assert
            Assert.Equal(0, score); // Sprawdzamy, czy wynik został zresetowany do 0
        }

        [Fact]
        public void GetPercentage_ShouldReturn100_WhenAllAnswersAreCorrect()
        {
            // Arrange
            _scoreService.StartNewQuiz(10); // Inicjalizujemy quiz z 10 pytaniami
            for (int i = 0; i < 10; i++)
            {
                _scoreService.IncrementScore(); // Inkrementujemy wynik dla każdej odpowiedzi
            }

            // Act
            var percentage = _scoreService.GetPercentage(); // Pobieramy procent

            // Assert
            Assert.Equal(100, percentage); // Oczekiwany wynik to 100%
        }

        [Fact]
        public void GetPercentage_ShouldReturn0_WhenNoAnswersAreCorrect()
        {
            // Arrange
            _scoreService.StartNewQuiz(10); // Inicjalizujemy quiz z 10 pytaniami

            // Act
            var percentage = _scoreService.GetPercentage(); // Pobieramy procent

            // Assert
            Assert.Equal(0, percentage); // Oczekiwany wynik to 0%, ponieważ nie zwiększaliśmy wyniku
        }

        [Fact]
        public void GetPercentage_ShouldReturnCorrectPercentage_WhenSomeAnswersAreCorrect()
        {
            // Arrange
            _scoreService.StartNewQuiz(10); // Inicjalizujemy quiz z 10 pytaniami
            _scoreService.IncrementScore(); // Zwiększamy wynik o 1

            // Act
            var percentage = _scoreService.GetPercentage(); // Pobieramy procent

            // Assert
            Assert.Equal(10, percentage); // Oczekiwany wynik to 10% (1 poprawna odpowiedź z 10 pytań)
        }

        [Fact]
        public void GetPercentage_ShouldReturn0_WhenTotalQuestionsIsZero()
        {
            // Arrange
            _scoreService.StartNewQuiz(0); // Inicjalizujemy quiz z 0 pytaniami

            // Act
            var percentage = _scoreService.GetPercentage(); // Pobieramy procent

            // Assert
            Assert.Equal(0, percentage); // Oczekiwany wynik to 0%, ponieważ nie ma pytań
        }

        [Fact]
        public void GetPercentage_ShouldReturn0_AfterResetScore()
        {
            // Arrange
            _scoreService.StartNewQuiz(10); // Inicjalizujemy quiz z 10 pytaniami
            _scoreService.IncrementScore(); // Zwiększamy wynik o 1
            _scoreService.ResetScore(); // Resetujemy wynik

            // Act
            var percentage = _scoreService.GetPercentage(); // Pobieramy procent

            // Assert
            Assert.Equal(0, percentage); // Oczekiwany wynik to 0%, ponieważ wynik został zresetowany
        }
    }
}
