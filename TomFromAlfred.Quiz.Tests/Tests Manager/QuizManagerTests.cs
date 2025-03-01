using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.ServiceSupport;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using TomFromAlfred.QuizConsole.Tests.SupportForTests;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Manager
{
    // Oblane: 5 / 5
    public class QuizManagerTests
    {
        private readonly Mock<IFileWrapper> _mockFileWrapper;
        private readonly Mock<JsonCommonClass> _mockJsonService;
        private readonly Mock<ScoreService> _mockScoreService;
        private readonly EndService _endService;
        private readonly MockQuizService _quizService; // Używam MockQuizService zamiast QuizService
        private readonly QuizManager _quizManager;

        public QuizManagerTests()
        {
            // Mockuję pliki i JSON, żeby testy nie czytały rzeczywistych plików
            _mockFileWrapper = new Mock<IFileWrapper>();
            _mockJsonService = new Mock<JsonCommonClass>();
            _mockScoreService = new Mock<ScoreService>();

            _endService = new EndService(_mockScoreService.Object);

            // Symuluję, że pliki istnieją, aby uniknąć błędów dostępu do plików w testach
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);

            // Prawdziwe instancje serwisów
            var questionService = new QuestionService();
            var choiceService = new ChoiceService();
            var correctAnswerService = new CorrectAnswerService();

            // Tworzę `MockQuizService` zamiast `QuizService`, żeby nie ładował JSON
            _quizService = new MockQuizService(
                questionService,
                choiceService,
                correctAnswerService,
                _mockJsonService.Object,
                _mockFileWrapper.Object
            );

            // Ustawiam testowe pytania i odpowiedzi
            _quizService.SetTestData(
                new List<Question>
                {
                    new Question(1, "Jak nazywał się główny bohater książki?")
                },
                new Dictionary<int, List<Choice>>
                {
                    { 1, new List<Choice>
                        {
                            new Choice(1, 'A', "Tomek Wilmowski"),
                            new Choice(1, 'B', "Staś Tarkowski"),
                            new Choice(1, 'C', "Michał Wołodyjowski")
                        }
                    }
                },
                new Dictionary<int, string>
                {
                    { 1, "A" }
                }
            );

            _quizManager = new QuizManager(_quizService, _mockScoreService.Object, _endService);
        }
        /*
        // 1
        [Fact] // Oblany
        public void ConductQuiz_ShouldHandleUserAnsweringCorrectly() // Wyświetla: przyjmuje poprawnie odpowiedź użytkownika
        {
            // Arrange
            var input = new StringReader("1\nA\n\n"); // Ostatni `\n` kończy input
            Console.SetIn(input);

            // Zamiast `StringWriter`, przekierowuję `Console.Out` do pustego strumienia (nie gromadzę danych w pamięci)
            Console.SetOut(TextWriter.Null);

            // Ograniczam liczbę iteracji w `ConductQuiz()`, żeby test się nie zapętlił
            int maxIterations = 3;
            int iterationCount = 0;

            // Act: Sprawdzam tylko jedno pytanie
            while (iterationCount < maxIterations)
            {
                _quizManager.ConductQuiz();
                iterationCount++;

                // Jeśli za dużo iteracji – przerywam test
                if (iterationCount >= maxIterations)
                {
                    throw new Exception("Test zapętlił się i przekroczył limit iteracji!");
                }
            }

            // Assert
            _mockScoreService.Verify(s => s.IncrementScore(), Times.Once);
        } */


        /*
        // 2
        [Fact] // Oblany
        public void ConductQuiz_ShouldHandleNoQuestionsAvailable() // Wyświetla: brak pytań
        {
            // Arrange
            _mockQuizService.Setup(q => q.GetAllQuestions()).Returns(new List<Question>()); // Brak pytań

            // Act
            _quizManager.ConductQuiz();

            // Assert
            // Sprawdzam, czy wyświetlono komunikat o braku pytań
            _mockQuizService.Verify(q => q.GetAllQuestions(), Times.Once);
            // Mogę również zweryfikować, czy użytkownik otrzymał odpowiedni komunikat
        }

        // 3
        [Fact] // Oblany
        public void ConductQuiz_ShouldNotIncrementScore_WhenAnswerIsIncorrect() // Wyświetla: nie daje punktu, jęśli odpowiedź błędna
        {
            // Arrange
            var question = new Question(1, "Sample question");
            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Choice A"),
                new Choice (1, 'B', "Choice B"),
                new Choice (1, 'C', "Choice C")
            };

            _mockQuizService.Setup(q => q.GetAllQuestions()).Returns(new List<Question> { question });
            _mockQuizService.Setup(q => q.GetShuffledChoicesForQuestion(It.IsAny<int>(), out It.Ref<Dictionary<char, char>>.IsAny)).Returns(choices);
            _mockQuizService.Setup(q => q.CheckAnswer(It.IsAny<int>(), 'B', It.IsAny<Dictionary<char, char>>())).Returns(false); // Ustawiam złą odpowiedź

            // Act
            _quizManager.ConductQuiz();

            // Assert
            _mockScoreService.Verify(s => s.IncrementScore(), Times.Never); // Sprawdzam, czy punkty nie zostały inkrementowane
        }

        // 4
        [Fact] // Oblany
        public void ConductQuiz_ShouldSkipQuestion_WhenUserSelectsOption2() // Wyświetla: pomija pytanie na żądanie użytkownika
        {
            // Arrange
            var question = new Question(1, "Sample question");
            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Choice A"),
                new Choice (1, 'B', "Choice B"),
                new Choice (1, 'C', "Choice C")
            };

            _mockQuizService.Setup(q => q.GetAllQuestions()).Returns(new List<Question> { question });
            _mockQuizService.Setup(q => q.GetShuffledChoicesForQuestion(It.IsAny<int>(), out It.Ref<Dictionary<char, char>>.IsAny)).Returns(choices);

            // Symulujemy użytkownika wybierającego akcję "2" (pominięcie pytania)
            _mockEndService.Setup(e => e.ShouldEnd(It.IsAny<string>())).Returns(false); // Zwracam false, żeby nie kończyć quizu

            // Act
            _quizManager.ConductQuiz();

            // Assert
            _mockQuizService.Verify(q => q.GetAllQuestions(), Times.Once); // Upewniam się, że pytanie zostało odczytane
            _mockScoreService.Verify(s => s.IncrementScore(), Times.Never); // Upewniam się, że punkty nie zostały inkrementowane
        }

        // 5
        [Fact] // Oblany
        public void ConductQuiz_ShouldEndQuiz_WhenUserInputsK() // Wyświetla: przerywa Quzi na żądanie użytkownika
        {
            // Arrange
            var question = new Question(1, "Sample question");
            var choices = new List<Choice>
            {
                new Choice (1, 'A', "Choice A"),
                new Choice (1, 'B', "Choice B"),
                new Choice (1, 'C', "Choice C")
            };

            _mockQuizService.Setup(q => q.GetAllQuestions()).Returns(new List<Question> { question });
            _mockQuizService.Setup(q => q.GetShuffledChoicesForQuestion(It.IsAny<int>(), out It.Ref<Dictionary<char, char>>.IsAny)).Returns(choices);
            _mockQuizService.Setup(q => q.CheckAnswer(It.IsAny<int>(), 'A', It.IsAny<Dictionary<char, char>>())).Returns(true);

            // Symuluję wejście użytkownika "K" aby zakończyć quiz
            _mockEndService.Setup(e => e.ShouldEnd(It.IsAny<string>())).Returns(true);

            // Act
            _quizManager.ConductQuiz();

            // Assert
            _mockEndService.Verify(e => e.EndQuiz(true), Times.Once); // Sprawdzam, czy metoda EndQuiz została wywołana, gdy użytkownik zakończył quiz
        } */
    }
}
