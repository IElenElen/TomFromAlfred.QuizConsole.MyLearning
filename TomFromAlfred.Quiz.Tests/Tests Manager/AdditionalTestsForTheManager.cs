using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.ServiceSupport;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using TomFromAlfred.QuizConsole.Tests.SupportForTests;
using Xunit.Abstractions;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Manager
{
    // Oblane: 0 / 7

    // Klasa testów dla Helpera managerskiego

    public class AdditionalTestsForTheManager
    {
        private readonly ITestOutputHelper _output;

        private readonly Mock<IUserInterface> _mockUserInterface; 
        private readonly MockQuizService _mockQuizService;
        private readonly ScoreService _scoreService; // Rzeczywista instancja
        private readonly EndService _endService; // Rzeczywista instancja
        private readonly QuizManager _quizManager;

        public AdditionalTestsForTheManager(ITestOutputHelper output)
        {
            _output = output;

            // Tworzę rzeczywisty `QuestionService`, ale bez domyślnych pytań
            var questionService = new QuestionService(false);

            _mockQuizService = new MockQuizService(
           questionService,
           new Mock<ChoiceService>().Object,
           new Mock<CorrectAnswerService>().Object,
           new Mock<JsonCommonClass>().Object,
           new Mock<IFileWrapper>().Object
            );

            _scoreService = new ScoreService();
            _endService = new EndService(_scoreService);

            _quizManager = new QuizManager(_mockQuizService, _scoreService, _endService, _mockUserInterface.Object);
        }

        // 1 
        [Fact] // Zaliczony
        public void Constructor_ShouldThrowException_WhenQuestionsAreNull() // Konstruktor: podaje wyjątek, jeśli pytania = null
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ManagerHelper(null, _mockQuizService));
        }

        // 2
        [Fact] // Zaliczony
        public void HasNext_ShouldReturnTrue_WhenQuestionsAreAvailable() // Czy ma następne? : zwraca prawdę, gdy są kolejne pytania
        {
            // Arrange 
            var questions = new List<Question>
            {
                new Question(1, "Question 1"),
                new Question(2, "Question 2")
            };

            // Tworzę `QuizService`, ale nie ładuję domyślnych pytań
            var questionService = new QuestionService(false);

            var quizService = new MockQuizService(
                questionService,
                new Mock<ChoiceService>().Object,
                new Mock<CorrectAnswerService>().Object,
                new Mock<JsonCommonClass>().Object,
                new Mock<IFileWrapper>().Object
            );

            var managerHelper = new ManagerHelper(questions, quizService);

            // Act
            var result = managerHelper.HasNext();

            // Assert
            Assert.True(result);
        }

        // 3
        [Fact] // Zaliczony
        public void HasNext_ShouldReturnFalse_WhenNoMoreQuestions() // // Czy ma następne? : zwraca fałszę, gdy brak pytań
        {
            // Arrange
            var questions = new List<Question>
            {
                new Question(1, "Question 1"),
                new Question(2, "Question 2"),
                new Question(3, "Question 3")
            };


            var questionService = new QuestionService(false);

            var quizService = new MockQuizService(
                questionService,
                new Mock<ChoiceService>().Object,
                new Mock<CorrectAnswerService>().Object,
                new Mock<JsonCommonClass>().Object,
                new Mock<IFileWrapper>().Object
            );

            var managerHelper = new ManagerHelper(questions, quizService);

            // Sprawdzenie przed `NextQuestion()`
            Assert.True(managerHelper.HasNext()); // Powinno zwrócić true, bo mamy więcej niż 1 pytanie

            // Przechodzę przez wszystkie pytania
            managerHelper.NextQuestion(); // Teraz indeks = 1
            Assert.True(managerHelper.HasNext()); // Powinno nadal zwracać true

            managerHelper.NextQuestion(); // Teraz indeks = 2 (ostatnie pytanie)

            // `HasNext()` powinno teraz zwrócić `false`, bo nie ma więcej pytań
            Assert.False(managerHelper.HasNext());
        }

        // 4
        [Fact] // Zaliczony
        public void GetCurrentQuestion_ShouldReturnCurrentQuestion() // Podaje: bieżące pytanie
        {
            // Arrange
            var questions = new List<Question>
            {
                new Question(1, "Question 1"),
                new Question(2, "Question 2")
            };

            var questionService = new QuestionService(false);

            var quizService = new MockQuizService(
                questionService,
                new Mock<ChoiceService>().Object,
                new Mock<CorrectAnswerService>().Object,
                new Mock<JsonCommonClass>().Object,
                new Mock<IFileWrapper>().Object
            );

            var managerHelper = new ManagerHelper(questions, quizService);

            // Sprawdzenie przed `GetCurrentQuestion()`
            Assert.True(managerHelper.HasNext());

            // Pobieram pierwsze pytanie
            var currentQuestion = managerHelper.GetCurrentQuestion();
            Assert.Equal(1, currentQuestion.QuestionId);
            Assert.Equal("Question 1", currentQuestion.QuestionContent);

            // Sprawdzam `NextQuestion()`
            managerHelper.NextQuestion();
            var nextQuestion = managerHelper.GetCurrentQuestion();
            Assert.Equal(2, nextQuestion.QuestionId);
            Assert.Equal("Question 2", nextQuestion.QuestionContent);

            // `HasNext()` powinno teraz zwrócić `false`
            Assert.False(managerHelper.HasNext());
        }

        // 5
        [Fact] // Zaliczony
        public void GetCurrentQuestion_ShouldThrowException_WhenQuestionListIsEmpty() // Podaje pytanie: wyrzuca wyjątek jeśli lista pytań pusta
        {
            // Arrange
            var emptyQuestions = new List<Question>(); // Pusta lista pytań

            var questionService = new QuestionService(false);
            var quizService = new MockQuizService(
                questionService,
                new Mock<ChoiceService>().Object,
                new Mock<CorrectAnswerService>().Object,
                new Mock<JsonCommonClass>().Object,
                new Mock<IFileWrapper>().Object
            );

            var managerHelper = new ManagerHelper(emptyQuestions, quizService);

            // Act & Assert - od razu powinien być wyjątek, bo nie ma pytań
            Assert.Throws<InvalidOperationException>(() => managerHelper.GetCurrentQuestion());
        }

        // 6
        [Fact] // Zaliczony
        public void NextQuestion_ShouldMoveToNextQuestion() // Następne: powinien przejść do kolejnego
        {
            // Arrange
            var questions = new List<Question>
            {
                new Question(1, "Question 1"),
                new Question(2, "Question 2")
            };

            var questionService = new QuestionService(false);

            var quizService = new MockQuizService(
                questionService,
                new Mock<ChoiceService>().Object,
                new Mock<CorrectAnswerService>().Object,
                new Mock<JsonCommonClass>().Object,
                new Mock<IFileWrapper>().Object
            );

            var managerHelper = new ManagerHelper(questions, quizService);

            // Sprawdzenie początkowego pytania przed NextQuestion()
            var firstQuestion = managerHelper.GetCurrentQuestion();
            Assert.Equal(1, firstQuestion.QuestionId);
            Assert.Equal("Question 1", firstQuestion.QuestionContent);

            // Sprawdzenie `HasNext()` przed przejściem do kolejnego pytania
            Assert.True(managerHelper.HasNext());

            // Act - przejście do kolejnego pytania
            managerHelper.NextQuestion();

            // Sprawdzenie, czy indeks został zwiększony
            var currentQuestion = managerHelper.GetCurrentQuestion();
            Assert.Equal(2, currentQuestion.QuestionId);
            Assert.Equal("Question 2", currentQuestion.QuestionContent);

            // `HasNext()` powinno teraz zwrócić `false`
            Assert.False(managerHelper.HasNext());
        }

        // 7
        [Fact] // Zaliczony
        public void Shuffle_ShouldRandomizeListOrder() // Losowanie: losuje pytania
        {
            // Arrange
            var questions = new List<int> { 1, 2, 3, 4, 5 };
            var originalOrder = new List<int>(questions); // Kopia oryginalnej listy

            // Act
            var shuffledQuestions = ManagerHelper.Shuffle(questions);

            // Assert
            Assert.NotEqual(originalOrder, shuffledQuestions); // Powinna być inna kolejność
            Assert.Equal(originalOrder.Count, shuffledQuestions.Count); // Ta sama liczba elementów
            Assert.True(originalOrder.All(q => shuffledQuestions.Contains(q))); // Te same elementy
        }
    }
}
