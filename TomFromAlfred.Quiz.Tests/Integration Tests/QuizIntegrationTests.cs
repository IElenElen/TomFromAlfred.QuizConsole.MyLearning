using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.EntityService;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using Moq;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.ServiceSupport;
using FluentAssertions;
using NSubstitute;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForManager;
using TomFromAlfred.QuizConsole.Tests.Z___SupportForTests;

namespace TomFromAlfred.QuizConsole.Tests.Integration_Tests
{
    
    // Oblane: 2 / 2
    public class QuizIntegrationTests
    {
        private readonly Mock<IConsoleUserInterface> _mockUserInterface = new();
        private readonly Mock<IFileWrapper> _mockFileWrapper = new();
        private readonly Mock<JsonCommonClass> _mockJsonCommon = new();
        private readonly ScoreService _scoreService = new();
        private readonly CorrectAnswerService _correctAnswerService = new CorrectAnswerService(new ChoiceService()); // Nie upraszczać
        private QuizService CreateQuizServiceWithData(List<Question> questions, List<Choice> choices, List<JsonHelper> corrects)
        {
            _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
            _mockJsonCommon.Setup(x => x.ReadFromFile<List<Question>>(It.IsAny<string>())).Returns(questions);
            _mockJsonCommon.Setup(x => x.ReadFromFile<List<Choice>>(It.IsAny<string>())).Returns(choices);
            _mockJsonCommon.Setup(x => x.ReadFromFile<List<JsonHelper>>(It.IsAny<string>())).Returns(corrects);

            var quizService = new QuizService(
                new QuestionService(false),
                new ChoiceService(),
                _correctAnswerService,
                _mockJsonCommon.Object,
                _mockFileWrapper.Object
            );

            quizService.LoadQuestionsFromJson("fake.json");
            quizService.LoadChoicesFromJson();
            quizService.LoadCorrectSetFromJson();

            return quizService;
        }

        [Fact]
        public void ConductQuiz_ShouldEvaluateCorrectAnswersProperly()
        {
            // Arrange
            DataClearingCommonClass.ClearAll(null, null, _correctAnswerService, _scoreService);

            var questions = new List<Question>
            {
                new(1, "2+2=?"),
                new(2, "Stolica Francji")
            };

            var choices = new List<Choice>
            {
                new(1, 'A', "4"),
                new(1, 'B', "5"),
                new(2, 'A', "Berlin"),
                new(2, 'B', "Paryz")
            };

            var correctAnswers = new List<JsonHelper>
            {
                new() { QuestionNumber = 1, LetterCorrectAnswer = "A", ContentCorrectAnswer = "4" },
                new() { QuestionNumber = 2, LetterCorrectAnswer = "B", ContentCorrectAnswer = "Paryz" }
            };

            var quizService = CreateQuizServiceWithData(questions, choices, correctAnswers);
            var endService = new EndService(_scoreService);

            _mockUserInterface.SetupSequence(ui => ui.ReadInputLine())
                .Returns("1")
                .Returns("A")
                .Returns("1")
                .Returns("B");

            var quizManager = new QuizManager(quizService, _scoreService, endService, _mockUserInterface.Object);

            // Act
            quizManager.ConductQuiz();

            // Assert
            _scoreService.Score.Should().Be(2);
            _scoreService.GetPercentage().Should().Be(100);
        }

        [Fact]
        public void ConductQuiz_ShouldEvaluateIncorrectAnswersProperly()
        {
            // Arrange
            DataClearingCommonClass.ClearAll(null, null, _correctAnswerService, _scoreService);

            var questions = new List<Question>
            {
                new(1, "2+2=?"),
                new(2, "Stolica Francji")
            };

            var choices = new List<Choice>
            {
                new(1, 'A', "4"),
                new(1, 'B', "5"),
                new(2, 'A', "Berlin"),
                new(2, 'B', "Paryz")
            };

            var correctAnswers = new List<JsonHelper>
            {
                new() { QuestionNumber = 1, LetterCorrectAnswer = "A", ContentCorrectAnswer = "4" },
                new() { QuestionNumber = 2, LetterCorrectAnswer = "B", ContentCorrectAnswer = "Paryz" }
            };

            var quizService = CreateQuizServiceWithData(questions, choices, correctAnswers);
            var endService = new EndService(_scoreService);

            _mockUserInterface.SetupSequence(ui => ui.ReadInputLine())
                .Returns("1")
                .Returns("B") // błędna
                .Returns("1")
                .Returns("A"); // błędna

            var quizManager = new QuizManager(quizService, _scoreService, endService, _mockUserInterface.Object);

            // Act
            quizManager.ConductQuiz();

            // Assert
            _scoreService.Score.Should().Be(0);
            _scoreService.GetPercentage().Should().Be(0);
        }
    } 
}
