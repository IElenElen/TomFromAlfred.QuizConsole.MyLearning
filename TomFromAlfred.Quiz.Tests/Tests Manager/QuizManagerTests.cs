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
    public class QuizManagerTests
    {
        private readonly Mock<QuizService> _mockQuizService;
        private readonly Mock<ScoreService> _mockScoreService;
        private readonly Mock<EndService> _mockEndService;
        private readonly QuizManager _quizManager;

        public QuizManagerTests()
        {
            _mockQuizService = new Mock<QuizService>();
            _mockScoreService = new Mock<ScoreService>();
            _mockEndService = new Mock<EndService>();

            _quizManager = new QuizManager(_mockQuizService.Object, _mockScoreService.Object, _mockEndService.Object);
        }

        [Fact]
        public void ConductQuiz_ShouldHandleUserAnsweringCorrectly() // Oblany
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

            // Act
            _quizManager.ConductQuiz();

            // Assert
            _mockScoreService.Verify(s => s.IncrementScore(), Times.Once);
            _mockEndService.Verify(e => e.EndQuiz(false), Times.Never); // Sprawdzam, czy quiz się nie kończy, jeśli użytkownik odpowiedział
        }

        [Fact]
        public void ConductQuiz_ShouldHandleNoQuestionsAvailable() // Oblany
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

        [Fact] //Oblany
        public void AddQuestion_ShouldNotAddQuestion_WhenCorrectAnswerIsInvalid() // Inaczej sformułować, bo po prostu nie przechodzę do kolejnego pytania
        {
            // Arrange
            var questionContent = "What is the capital of France?";
            var choices = new List<string> { "Paris", "London", "Berlin" };
            var correctAnswer = "Z"; // Niepoprawna odpowiedź (powinna być A, B lub C)

            _mockQuizService.Setup(q => q.GetAllQuestions()).Returns(new List<Question> { new Question(1, "Dummy question") });
            _mockQuizService.Setup(q => q.AddQuestionToJson(It.IsAny<Question>())).Verifiable();

            // Symuluję dodawanie pytania z nieprawidłową odpowiedzią
            Console.SetIn(new StringReader($"{questionContent}\n{choices[0]}\n{choices[1]}\n{choices[2]}\n{correctAnswer}\n"));

            // Act
            _quizManager.AddQuestion();

            // Assert
            _mockQuizService.Verify(q => q.AddQuestionToJson(It.IsAny<Question>()), Times.Never); // Upewniam się, że pytanie nie zostało dodane
        }

        [Fact]
        public void ConductQuiz_ShouldNotIncrementScore_WhenAnswerIsIncorrect() // Oblany
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

        [Fact]
        public void ConductQuiz_ShouldSkipQuestion_WhenUserSelectsOption2() // Oblany
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

        [Fact]
        public void ConductQuiz_ShouldEndQuiz_WhenUserInputsK() // Oblany
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
        }


    }
}
