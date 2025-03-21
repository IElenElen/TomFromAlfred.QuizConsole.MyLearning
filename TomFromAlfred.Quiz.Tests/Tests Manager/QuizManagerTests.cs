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
        private readonly Mock<IQuizService> _mockQuizService;
        private readonly Mock<IScoreService> _mockScoreService;
        private readonly Mock<IEndService> _mockEndService;
        private readonly Mock<IUserInterface> _mockUserInterface;
        private readonly QuizManager _quizManager;

        public QuizManagerTests()
        {
            _mockQuizService = new Mock<IQuizService>();
            _mockScoreService = new Mock<IScoreService>();
            _mockEndService = new Mock<IEndService>();
            _mockUserInterface = new Mock<IUserInterface>();

            // Setup mocka `IEndService`
            _mockEndService.Setup(e => e.ShouldEnd(It.IsAny<string>())).Returns(false);
            _mockEndService.Setup(e => e.EndQuiz(It.IsAny<bool>())).Verifiable();

            // Setup testowych pytań
            var testQuestions = new List<Question>
            {
                new Question(1, "Jak nazywał się główny bohater książki?")
            };

            var testChoices = new List<Choice>
            {
                new Choice(1, 'A', "Tomek Wilmowski"),
                new Choice(1, 'B', "Staś Tarkowski"),
                new Choice(1, 'C', "Michał Wołodyjowski")
            };

            var letterMapping = new Dictionary<char, char> { { 'A', 'A' }, { 'B', 'B' }, { 'C', 'C' } };

            _mockQuizService.Setup(q => q.GetAllQuestions()).Returns(testQuestions);
            _mockQuizService.Setup(q => q.GetShuffledChoicesForQuestion(1, out letterMapping)).Returns(testChoices);
            _mockQuizService.Setup(q => q.CheckAnswer(1, 'A', letterMapping)).Returns(true);

            // Symulacja wejścia użytkownika
            _mockUserInterface.SetupSequence(ui => ui.ReadLine())
                .Returns("1")  // Wybór odpowiedzi
                .Returns("A")  // Odpowiedź użytkownika
                .Returns("K"); // Wyjście z quizu

            _quizManager = new QuizManager(_mockQuizService.Object, _mockScoreService.Object, _mockEndService.Object, _mockUserInterface.Object);
        }

        // 1
        [Fact] // Oblany
        public void ConductQuiz_ShouldHandleUserAnsweringCorrectly() // Wyświetla: przyjmuje poprawnie odpowiedź użytkownika i dodaje punkt
        {
            // Arrange
            var testQuestions = new List<Question>
    {
        new Question(1, "Jak nazywał się główny bohater książki?")
    };

            var letterMapping = new Dictionary<char, char> { { 'A', 'A' }, { 'B', 'B' }, { 'C', 'C' } };

            _mockQuizService.Setup(q => q.GetAllQuestions()).Returns(testQuestions);
            _mockQuizService.Setup(q => q.GetShuffledChoicesForQuestion(1, out letterMapping))
                .Returns(new List<Choice>
                {
            new Choice(1, 'A', "Tomek Wilmowski"),
            new Choice(1, 'B', "Staś Tarkowski"),
            new Choice(1, 'C', "Michał Wołodyjowski")
                });

            // Upewniamy się, że CheckAnswer zwraca true dla A
            _mockQuizService.Setup(q => q.CheckAnswer(1, 'A', letterMapping))
                .Returns(true)
                .Callback(() => Console.WriteLine("CheckAnswer został wywołany!"));

            // Logowanie, aby sprawdzić, jakie wartości zwraca ReadLine()
            _mockUserInterface.SetupSequence(ui => ui.ReadLine())
                .Returns(() => { Console.WriteLine("User Input: 1"); return "1"; })  // Użytkownik wybiera odpowiedź
                .Returns(() => { Console.WriteLine("User Input: A"); return "A"; })  // Odpowiedź użytkownika
                .Returns(() => { Console.WriteLine("User Input: K"); return "K"; }); // Wyjście

            // Act
            _quizManager.ConductQuiz();

            // Assert
            _mockQuizService.Verify(q => q.CheckAnswer(1, 'A', letterMapping), Times.Once, "CheckAnswer() nie został wywołany!");

            _mockScoreService.Verify(s => s.IncrementScore(), Times.Once, "IncrementScore() nie zostało wywołane!");

            _mockUserInterface.Verify(ui => ui.WriteLine("Poprawna odpowiedź!"), Times.Once, "Nie wyświetlono komunikatu o poprawnej odpowiedzi!");
        } 
        
        // 2
        [Fact] // 
        public void ConductQuiz_ShouldHandleNoQuestionsAvailable() // Wyświetla: brak pytań
        {
            // Arrange
            _mockQuizService.Setup(q => q.GetAllQuestions()).Returns(new List<Question>());

            // Act
            _quizManager.ConductQuiz();

            // Assert
            _mockUserInterface.Verify(ui => ui.WriteLine("Koniec pytań."), Times.Once);
        }

        // 3
        [Fact] // Oblany
        public void ConductQuiz_ShouldNotIncrementScore_WhenAnswerIsIncorrect() // Wyświetla: nie daje punktu, jęśli odpowiedź błędna
        {
            // Arrange
            var testQuestions = new List<Question>
            {
                new Question(1, "Jak nazywał się główny bohater książki?")
            };

            var letterMapping = new Dictionary<char, char> { { 'A', 'A' }, { 'B', 'B' }, { 'C', 'C' } };

            _mockQuizService.Setup(q => q.GetAllQuestions()).Returns(testQuestions);
            _mockQuizService.Setup(q => q.GetShuffledChoicesForQuestion(1, out letterMapping))
                .Returns(new List<Choice>
                {
                    new Choice(1, 'A', "Tomek Wilmowski"),
                    new Choice(1, 'B', "Staś Tarkowski"),
                    new Choice(1, 'C', "Michał Wołodyjowski")
                });

            _mockQuizService.Setup(q => q.CheckAnswer(1, 'B', letterMapping)).Returns(false);

            _mockUserInterface.SetupSequence(ui => ui.ReadLine())
                .Returns("1")  // Użytkownik wybiera odpowiedź
                .Returns("B")  // Zła odpowiedź
                .Returns("K"); // Wyjście

            // Act
            _quizManager.ConductQuiz();

            // Assert
            _mockScoreService.Verify(s => s.IncrementScore(), Times.Never);
            _mockUserInterface.Verify(ui => ui.WriteLine("Zła odpowiedź."), Times.Once);
        }

        // 4
        [Fact] // Oblany
        public void ConductQuiz_ShouldSkipQuestion_WhenUserSelectsOption2() // Wyświetla: pomija pytanie na żądanie użytkownika
        {
            // Arrange
            var testQuestions = new List<Question>
            {
                new Question(1, "Jak nazywał się główny bohater książki?")
            };

            _mockQuizService.Setup(q => q.GetAllQuestions()).Returns(testQuestions);

            _mockUserInterface.SetupSequence(ui => ui.ReadLine())
                .Returns("2")  // Pominięcie pytania
                .Returns("K"); // Wyjście

            // Act
            _quizManager.ConductQuiz();

            // Assert
            _mockUserInterface.Verify(ui => ui.WriteLine("Pytanie pominięte."), Times.Once);
        }

        // 5
        [Fact] // 
        public void ConductQuiz_ShouldEndQuiz_WhenUserInputsK() // Wyświetla: przerywa Quiz na żądanie użytkownika
        {
            // Arrange
            _mockEndService.Setup(e => e.ShouldEnd("K")).Returns(true);

            _mockUserInterface.SetupSequence(ui => ui.ReadLine())
                .Returns("K"); // Wyjście

            // Act
            _quizManager.ConductQuiz();

            // Assert
            _mockEndService.Verify(e => e.EndQuiz(It.IsAny<bool>()), Times.Once);
        } 
    }
}
