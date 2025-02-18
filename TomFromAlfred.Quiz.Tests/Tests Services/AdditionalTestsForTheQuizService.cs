using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.ServiceSupport;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Oblane: ? / ?
    public class AdditionalTestsForTheQuizService
    {
        private readonly Mock<QuestionService> _mockQuestionService;
        private readonly Mock<ChoiceService> _mockChoiceService;
        private readonly Mock<CorrectAnswerService> _mockCorrectAnswerService;
        private readonly Mock<JsonCommonClass> _mockJsonCommonClass;
        private readonly Mock<IFileWrapper> _mockFileWrapper;

        private readonly QuizService _quizService;

        public AdditionalTestsForTheQuizService()
        {
            // Tworzę mocki dla klas serwisowych
            _mockQuestionService = new Mock<QuestionService>();
            _mockChoiceService = new Mock<ChoiceService>();
            _mockCorrectAnswerService = new Mock<CorrectAnswerService>();
            _mockJsonCommonClass = new Mock<JsonCommonClass>();
            _mockFileWrapper = new Mock<IFileWrapper>();


            // Inicjalizacja QuizService z zamockowanymi obiektami
            _quizService = new QuizService(
                _mockQuestionService.Object,
                _mockChoiceService.Object,
                _mockCorrectAnswerService.Object,
                _mockJsonCommonClass.Object,
                _mockFileWrapper.Object
            );
        }
        // 28 // dodatek
        // Losowanie odpowiedzi nie powinno się dziać podczas ładowania. Powinno się odbywać tylko podczas pobierania odpowiedzi dla pytania.
        [Fact]
        public void LoadChoicesFromJson_ShouldLoadAndShuffleChoices_WhenFileIsValid() // Oblany
        {
            // Arrange
            var choices = new List<Choice>
            {
                new Choice(1, ' ', "Choice 1"),
                new Choice(2, ' ', "Choice 2"),
                new Choice(3, ' ', "Choice 3")
            };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<Choice>>(It.IsAny<string>())).Returns(choices);

            // Act
            _quizService.LoadChoicesFromJson();

            // Assert
            var shuffledChoices = _quizService.GetChoicesForQuestion(1).ToList();

            // Sprawdzam, czy wybory zostały przetasowane
            Assert.NotEqual(choices[0].ChoiceLetter, shuffledChoices[0].ChoiceLetter);
            Assert.NotEqual(choices[1].ChoiceLetter, shuffledChoices[1].ChoiceLetter);
            Assert.NotEqual(choices[2].ChoiceLetter, shuffledChoices[2].ChoiceLetter);

            // Sprawdzam, czy litery zostały przypisane poprawnie
            Assert.Equal('A', shuffledChoices[0].ChoiceLetter);
            Assert.Equal('B', shuffledChoices[1].ChoiceLetter);
            Assert.Equal('C', shuffledChoices[2].ChoiceLetter);
        }

        // 31 // dodatek - wyjątek w metodzie zrobić
        [Fact]
        public void LoadCorrectSetFromJson_ShouldHandleInvalidJson() // Oblany
        {
            // Arrange
            var incorrectSet = new List<JsonHelper>
            {
                new JsonHelper { QuestionNumber = -1, LetterCorrectAnswer = "Z", ContentCorrectAnswer = "" }
            };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<JsonHelper>>(It.IsAny<string>())).Returns(incorrectSet);

            // Act
            _quizService.LoadCorrectSetFromJson();

            // Assert

            // Testuję, czy po załadowaniu błędnych danych, metoda GetCorrectAnswerForQuestion nie zwróci odpowiedzi dla pytania o id -1
            var correctAnswer = _mockCorrectAnswerService.Object.GetCorrectAnswerForQuestion(-1);
            Assert.Null(correctAnswer); // Oczekuję, że nie będzie poprawnej odpowiedzi dla tego pytania
        }

        // 32 // dodatek - zrobić obsługę wyjątku w metodzie
        [Fact]
        public void LoadCorrectSetFromJson_ShouldHandleEmptyCorrectAnswers() // Oblany
        {
            // Arrange
            var emptyCorrectAnswers = new List<JsonHelper> // Pusty zestaw poprawnych odpowiedzi
            {
                // Zawiera dane, ale bez odpowiedzi poprawnych
                new JsonHelper { QuestionNumber = 1, LetterCorrectAnswer = null, ContentCorrectAnswer = null }
            };

            _mockJsonCommonClass.Setup(x => x.ReadFromFile<List<JsonHelper>>(It.IsAny<string>())).Returns(emptyCorrectAnswers);

            // Setup mock dla GetCorrectAnswerForQuestion, zwracający null
            _mockCorrectAnswerService.Setup(x => x.GetCorrectAnswerForQuestion(It.IsAny<int>())).Returns((CorrectAnswer)null);

            // Act
            _quizService.LoadCorrectSetFromJson();

            // Assert
            var correctAnswer = _mockCorrectAnswerService.Object.GetCorrectAnswerForQuestion(1); // Sprawdzam wywołanie mocka
            Assert.Null(correctAnswer); // Spodziewam się, że nie będzie poprawnej odpowiedzi
        }


        // Próba zastosowania refleksji
        // 33 // dodatek
        [Fact]
        public void IsChoiceRelatedToQuestion_ShouldReturnTrue_WhenChoiceIsRelatedToQuestion() // Oblany
        {
            // Arrange
            var choice = new Choice(15, 'A', "próbny wybór");
            var question = new Question(1, "próbna treść");

            // Używam refleksji, aby uzyskać dostęp do prywatnej metody
            var methodInfo = typeof(QuizService).GetMethod("IsChoiceRelatedToQuestion", BindingFlags.NonPublic | BindingFlags.Static);
            var result = (bool)methodInfo.Invoke(null, new object[] { choice, question });

            // Assert
            Assert.True(result); // Oczekuję, że wynik będzie True
        }

        // 34 // dodatek 
        [Fact]
        public void IsChoiceRelatedToQuestion_ShouldReturnFalse_WhenChoiceIsNotRelatedToQuestion() // Oblany
        {
            // Arrange
            var choice = new Choice(25, 'A', "próbny wybór");
            var question = new Question(1, "próbna treść");

            // Używam refleksji, aby uzyskać dostęp do prywatnej metody
            var methodInfo = typeof(QuizService).GetMethod("IsChoiceRelatedToQuestion", BindingFlags.NonPublic | BindingFlags.Static);
            var result = (bool)methodInfo.Invoke(null, new object[] { choice, question });

            // Assert
            Assert.False(result); // Oczekuję, że wynik będzie False
        }
    }
}
