using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Manager
{
    // Oblane: 11 / 11

    public class AdditionalTestsForTheManager
    {
        private readonly Mock<QuizService> _mockQuizService;
        private readonly Mock<ScoreService> _mockScoreService;
        private readonly Mock<EndService> _mockEndService;
        private readonly QuizManager _quizManager;

        public AdditionalTestsForTheManager()
        {
            _mockQuizService = new Mock<QuizService>();
            _mockScoreService = new Mock<ScoreService>();
            _mockEndService = new Mock<EndService>();

            _quizManager = new QuizManager(_mockQuizService.Object, _mockScoreService.Object, _mockEndService.Object);
        }

        // 1 
        [Fact] // Oblany
        public void Constructor_ShouldThrowException_WhenQuestionsAreNull() // ???
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ManagerHelper(null));
        }

        // 2
        [Fact] // Oblany
        public void HasNext_ShouldReturnTrue_WhenQuestionsAreAvailable() // ???
        {
            // Arrange
            var questions = new List<Question>
            {
                new Question(1, "Question 1"),
                new Question(2, "Question 2")
            };

            var managerHelper = new ManagerHelper(questions);

            // Act
            var result = managerHelper.HasNext();

            // Assert
            Assert.True(result);
        }

        // 3
        [Fact] // Oblany
        public void HasNext_ShouldReturnFalse_WhenNoMoreQuestions() // ???
        {
            // Arrange
            var questions = new List<Question>
            {
                new Question(1, "Question 1")
            };

            var managerHelper = new ManagerHelper(questions);
            managerHelper.NextQuestion(); // Przechodzę do końca listy

            // Act
            var result = managerHelper.HasNext();

            // Assert
            Assert.False(result);
        }

        // 4
        [Fact] // Oblany
        public void GetCurrentQuestion_ShouldReturnCurrentQuestion() // Podaje: bieżące pytanie
        {
            // Arrange
            var questions = new List<Question>
            {
                new Question(1, "Question 1"),
                new Question(2, "Question 2")
            };

            var managerHelper = new ManagerHelper(questions);

            // Act
            var currentQuestion = managerHelper.GetCurrentQuestion();

            // Assert
            Assert.Equal(1, currentQuestion.QuestionId);
            Assert.Equal("Question 1", currentQuestion.QuestionContent);
        }

        // 5
        [Fact] // Oblany
        public void GetCurrentQuestion_ShouldThrowException_WhenNoMoreQuestions() // Podaje: wyjątek, jeśli ???
        {
            // Arrange
            var questions = new List<Question>
            {
                new Question(1, "Question 1")
            };

            var managerHelper = new ManagerHelper(questions);
            managerHelper.NextQuestion(); // Przekroczenie listy

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => managerHelper.GetCurrentQuestion());
        }

        // 6
        [Fact] // Oblany
        public void NextQuestion_ShouldMoveToNextQuestion() // ???
        {
            // Arrange
            var questions = new List<Question>
            {
                new Question(1, "Question 1"),
                new Question(2, "Question 2")
            };

            var managerHelper = new ManagerHelper(questions);
            managerHelper.NextQuestion();

            // Act
            var currentQuestion = managerHelper.GetCurrentQuestion();

            // Assert
            Assert.Equal(2, currentQuestion.QuestionId);
            Assert.Equal("Question 2", currentQuestion.QuestionContent);
        }

        // 7 
        [Fact] // Oblany
        public void NextQuestion_ShouldNotExceedQuestionList() // ???
        {
            // Arrange
            var questions = new List<Question>
            {
                new Question(1, "Question 1")
            };

            var managerHelper = new ManagerHelper(questions);
            managerHelper.NextQuestion(); // Powinno dojść do końca

            // Act
            managerHelper.NextQuestion(); // Kolejne wywołanie nie powinno zmieniać stanu

            // Assert
            Assert.Throws<InvalidOperationException>(() => managerHelper.GetCurrentQuestion());
        }

        // 8
        [Fact] // Oblany
        public void Shuffle_ShouldRandomizeListOrder() // ???
        {
            // Arrange
            var questions = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            var shuffledQuestions = ManagerHelper.Shuffle(questions);

            // Assert
            Assert.NotEqual(questions, shuffledQuestions); // Lista powinna być przetasowana
            Assert.Equal(questions.Count, shuffledQuestions.Count); // Sprawdzam, czy liczba elementów się zgadza
            Assert.True(questions.All(q => shuffledQuestions.Contains(q))); // Sprawdzam, czy lista zawiera te same elementy
        }

        // 9
        [Fact] // Oblany
        public void AddQuestion_ShouldAddQuestionCorrectly() // Dodaje: poprawnie pytanie do listy
        {
            // Arrange
            var questionContent = "What is the capital of France?";
            var choices = new List<string> { "Paris", "London", "Berlin" };
            var correctAnswer = "A"; // Poprawna odpowiedź to "Paris"

            _mockQuizService.Setup(q => q.GetAllQuestions()).Returns(new List<Question> { new Question(1, "Dummy question") });
            _mockQuizService.Setup(q => q.AddQuestionToJson(It.IsAny<Question>())).Verifiable();

            // Symuluję dodawanie pytania
            Console.SetIn(new StringReader($"{questionContent}\n{choices[0]}\n{choices[1]}\n{choices[2]}\n{correctAnswer}\n"));

            // Act
            _quizManager.AddQuestion();

            // Assert
            _mockQuizService.Verify(q => q.AddQuestionToJson(It.Is<Question>(q => q.QuestionContent == questionContent)), Times.Once); // Upewniam się, że pytanie zostało dodane do systemu
        }

        // 10
        [Fact] // Oblany
        public void AddQuestion_ShouldNotAddQuestion_WhenQuestionContentIsEmpty() // Dodaje: Nie, jeśli treść pytania pusta
        {
            // Arrange
            string userInput = "\n"; // Użytkownik nie wpisuje pytania
            Console.SetIn(new StringReader(userInput));

            // Act
            _quizManager.AddQuestion();

            // Assert
            _mockQuizService.Verify(q => q.AddQuestionToJson(It.IsAny<Question>()), Times.Never); // Pytanie nie zostało dodane
        }

        // 11
        [Fact] // Oblany
        public void AddQuestion_ShouldNotAddQuestion_WhenCorrectAnswerIsInvalid() // Dodaje: Nie, jeśli poprawna odpowiedź jest poza zakresem
        {
            // Arrange
            string userInput = "What is the capital of France?\nParis\nLondon\nBerlin\nZ\n"; // Niepoprawna odpowiedź 'Z'
            Console.SetIn(new StringReader(userInput));

            // Act
            _quizManager.AddQuestion();

            // Assert
            _mockQuizService.Verify(q => q.AddQuestionToJson(It.IsAny<Question>()), Times.Never);
        }
    }
}
