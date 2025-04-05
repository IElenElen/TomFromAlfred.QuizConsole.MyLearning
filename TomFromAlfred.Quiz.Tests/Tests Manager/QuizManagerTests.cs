using FluentAssertions;
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
        private readonly Mock<IQuizService> _mockQuizService = new();
        private readonly Mock<IScoreService> _mockScoreService = new();
        private readonly Mock<IEndService> _mockEndService = new();
        private readonly Mock<IUserInterface> _mockUserInterface = new();
        private readonly QuizManager _quizManager;

        public QuizManagerTests()
        {
            _mockEndService.Setup(e => e.ShouldEnd(It.IsAny<string>())).Returns(false);
            _mockEndService.Setup(e => e.EndQuiz(It.IsAny<bool>()));

            _quizManager = new QuizManager(
                _mockQuizService.Object,
                _mockScoreService.Object,
                _mockEndService.Object,
                _mockUserInterface.Object
            );
        }

        [Fact]
        public void ConductQuiz_ShouldHandleUserAnsweringCorrectly()
        {
            var question = new Question(1, "Kto był bohaterem?");
            var letterMapping = new Dictionary<char, char> { { 'A', 'A' }, { 'B', 'B' }, { 'C', 'C' } };

            _mockQuizService.Setup(x => x.GetAllQuestions()).Returns([question]);
            _mockQuizService.Setup(x => x.GetShuffledChoicesForQuestion(1, out letterMapping))
                .Returns([
                new Choice(1, 'A', "Tomek"),
                new Choice(1, 'B', "Staś"),
                new Choice(1, 'C', "Michał")
                ]);
            _mockQuizService.Setup(x => x.CheckAnswer(1, 'A', letterMapping)).Returns(true);

            SetupUserInputSequence("1", "A", "K");

            _quizManager.ConductQuiz();

            _mockScoreService.Invocations.Should().ContainSingle(x => x.Method.Name == "IncrementScore");
            _mockUserInterface.Invocations.Should().Contain(i => i.Arguments.Contains("Poprawna odpowiedź!"));
        }

        [Fact]
        public void ConductQuiz_ShouldHandleNoQuestionsAvailable()
        {
            _mockQuizService.Setup(q => q.GetAllQuestions()).Returns(new List<Question>());

            _quizManager.ConductQuiz();

            _mockUserInterface.Invocations.Should().Contain(i => i.Arguments.Contains("Koniec pytań."));
        }

        [Fact]
        public void ConductQuiz_ShouldNotIncrementScore_WhenAnswerIsIncorrect()
        {
            var question = new Question(1, "Pytanie?");
            var letterMapping = new Dictionary<char, char> { { 'A', 'A' }, { 'B', 'B' }, { 'C', 'C' } };

            _mockQuizService.Setup(q => q.GetAllQuestions()).Returns([question]);
            _mockQuizService.Setup(q => q.GetShuffledChoicesForQuestion(1, out letterMapping))
                .Returns([
                new Choice(1, 'A', "A"),
                new Choice(1, 'B', "B"),
                new Choice(1, 'C', "C")
                ]);
            _mockQuizService.Setup(q => q.CheckAnswer(1, 'B', letterMapping)).Returns(false);

            SetupUserInputSequence("1", "B", "K");

            _quizManager.ConductQuiz();

            _mockScoreService.Invocations.Should().NotContain(i => i.Method.Name == "IncrementScore");
            _mockUserInterface.Invocations.Should().Contain(i => i.Arguments.Contains("Zła odpowiedź."));
        }

        [Fact]
        public void ConductQuiz_ShouldSkipQuestion_WhenUserSelectsOption2()
        {
            _mockQuizService.Setup(q => q.GetAllQuestions()).Returns([new Question(1, "Pytanie?")]);

            SetupUserInputSequence("2", "K");

            _quizManager.ConductQuiz();

            _mockUserInterface.Invocations.Should().Contain(i => i.Arguments.Contains("Pytanie pominięte."));
        }

        [Fact]
        public void ConductQuiz_ShouldEndQuiz_WhenUserInputsK()
        {
            _mockEndService.Setup(e => e.ShouldEnd("K")).Returns(true);
            SetupUserInputSequence("K");

            _quizManager.ConductQuiz();

            _mockEndService.Invocations.Should().ContainSingle(i => i.Method.Name == "EndQuiz");
        }

        private void SetupUserInputSequence(params string[] inputs)
        {
            var sequence = _mockUserInterface.SetupSequence(x => x.ReadLine());
            foreach (var input in inputs)
                sequence = sequence.Returns(input);
        }
    }
}
