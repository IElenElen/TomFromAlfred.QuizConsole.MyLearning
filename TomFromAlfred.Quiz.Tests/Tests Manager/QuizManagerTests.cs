using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForManager;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForService;
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
        private readonly Mock<IScoreService> _mockIScoreService;
        private readonly Mock<IEndService> _mockIEndService;
        private readonly Mock<IUserInterface> _mockUserInterface; 
        private readonly Mock<IQuizService> _mockQuizService;
        private readonly QuizManager _quizManager;

        public QuizManagerTests()
        {
            // Mockuję pliki i JSON, żeby testy nie czytały rzeczywistych plików
            _mockFileWrapper = new Mock<IFileWrapper>();
            _mockJsonService = new Mock<JsonCommonClass>();
            _mockIScoreService = new Mock<IScoreService>();
            _mockIEndService = new Mock<IEndService>();

            _mockUserInterface = new Mock<IUserInterface>(); // Inicjalizacja mocka IUserInterface

            _mockIEndService.Setup(e => e.ShouldEnd("K")).Returns(true);
            _mockIEndService.Setup(e => e.EndQuiz(It.IsAny<bool>())).Verifiable();

            // Symuluję, że pliki istnieją, aby uniknąć błędów dostępu do plików w testach
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);

            // Prawdziwe instancje serwisów
            var questionService = new QuestionService();
            var choiceService = new ChoiceService();
            var correctAnswerService = new CorrectAnswerService();

           // Tworzę mock `IQuizService` bez argumentów
                _mockQuizService = new Mock<IQuizService>(); // ✅ Poprawiony zapis

            // Setup mockowanych metod
            _mockQuizService.Setup(q => q.GetAllQuestions()).Returns(new List<Question>
            {
                new Question(1, "Jak nazywał się główny bohater książki?")
            });

            // Tworzę mapping literowy dla `out` parameter
            var letterMapping = new Dictionary<char, char> { { 'A', 'A' }, { 'B', 'B' }, { 'C', 'C' } };

            _mockQuizService.Setup(q => q.GetShuffledChoicesForQuestion(It.IsAny<int>(), out letterMapping))
                .Returns(new List<Choice>
                {
                    new Choice(1, 'A', "Tomek Wilmowski"),
                    new Choice(1, 'B', "Staś Tarkowski"),
                    new Choice(1, 'C', "Michał Wołodyjowski")
                });

            _mockQuizService.Setup(q => q.CheckAnswer(It.IsAny<int>(), 'A', It.IsAny<Dictionary<char, char>>()))
                .Returns(true);

            _quizManager = new QuizManager(_mockQuizService.Object, _mockIScoreService.Object, _mockIEndService.Object, _mockUserInterface.Object);
        }
        
        // 1
        [Fact] // Oblany
        public void ConductQuiz_ShouldHandleUserAnsweringCorrectly() // Wyświetla: przyjmuje poprawnie odpowiedź użytkownika
        {
            // Arrange
            var fakeQuizService = new FakeQuizService();

            // Tworzenie pytań bezpośrednio w teście
            var testQuestions = new List<Question>
            {
                new Question(1, "Jak nazywał się główny bohater książki?")
            };

            var testChoices = new Dictionary<int, List<Choice>>
            {
                { 1, new List<Choice>
                    {
                        new Choice(1, 'A', "Tomek Wilmowski"),
                        new Choice(1, 'B', "Staś Tarkowski"),
                        new Choice(1, 'C', "Michał Wołodyjowski")
                    }
                }
            };

            var testCorrectAnswers = new Dictionary<int, string>
            {
                { 1, "A" } // Poprawna odpowiedź
            };

            // Ustawienie testowych danych na fakeQuizService
            fakeQuizService.SetTestData(testQuestions, testChoices, testCorrectAnswers);

            var quizManager = new QuizManager(fakeQuizService, _mockIScoreService.Object, _mockIEndService.Object, _mockUserInterface.Object);

            _mockUserInterface.Setup(ui => ui.WriteLine(It.IsAny<string>()));

            _mockUserInterface.SetupSequence(ui => ui.ReadLine())
                .Returns("1")  // Wybór odpowiedzi
                .Returns("A")  // Odpowiedź użytkownika
                .Returns("K"); // Wyjście z quizu

            // Act
            quizManager.ConductQuiz();

            // Assert
            _mockIScoreService.Verify(s => s.IncrementScore(), Times.Once);
        } 
        /*
        // 2
        [Fact] // Oblany
        public void ConductQuiz_ShouldHandleNoQuestionsAvailable() // Wyświetla: brak pytań
        {
            // Arrange
           _mockQuizService.SetTestData(new List<Question>(), new Dictionary<int, List<Choice>>(), new Dictionary<int, string>());

            // Act
            _quizManager.ConductQuiz();

            // Assert
            _mockUserInterface.Verify(ui => ui.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);
        }

        // 3
        [Fact] // Oblany
        public void ConductQuiz_ShouldNotIncrementScore_WhenAnswerIsIncorrect() // Wyświetla: nie daje punktu, jęśli odpowiedź błędna
        {
            // Arrange
            var input = new StringReader("1\nB\n\n");
            Console.SetIn(input);

            _mockUserInterface.Setup(ui => ui.WriteLine(It.IsAny<string>()));
            _mockQuizService.SetTestData(
                new List<Question> { new Question(1, "Pytanie") },
                new Dictionary<int, List<Choice>> { { 1, new List<Choice> { new Choice(1, 'A', "Poprawna") } } },
                new Dictionary<int, string> { { 1, "A" } }
            );

            // Act
            _quizManager.ConductQuiz();

            // Assert
            _mockScoreService.Verify(s => s.IncrementScore(), Times.Never);
        }

        // 4
        [Fact] // Oblany
        public void ConductQuiz_ShouldSkipQuestion_WhenUserSelectsOption2() // Wyświetla: pomija pytanie na żądanie użytkownika
        {
            // Arrange
            var input = new StringReader("2\n\n");
            Console.SetIn(input);

            _mockUserInterface.Setup(ui => ui.WriteLine(It.IsAny<string>()));

            // Act
            _quizManager.ConductQuiz();

            // Assert
            _mockUserInterface.Verify(ui => ui.WriteLine("Pytanie pominięte."), Times.Once);
        }

        // 5
        [Fact] // Oblany
        public void ConductQuiz_ShouldEndQuiz_WhenUserInputsK() // Wyświetla: przerywa Quzi na żądanie użytkownika
        {
            // Arrange
            var input = new StringReader("K\n\n");
            Console.SetIn(input);

            _mockUserInterface.Setup(ui => ui.WriteLine(It.IsAny<string>()));

            // Zamockowanie _mockEndService i jego metody ShouldEnd
            _mockEndService.Setup(e => e.ShouldEnd(It.IsAny<string>())).Returns(true);

            // Act
            _quizManager.ConductQuiz();

            // Assert
            _mockEndService.Verify(e => e.EndQuiz(It.IsAny<bool>()), Times.Once);
        } */
    }
}
