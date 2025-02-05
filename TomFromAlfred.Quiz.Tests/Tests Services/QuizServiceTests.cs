using Moq;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract;
using System.Reflection;


namespace TomFromAlfred.QuizConsole.Tests.Tests_Services
{
    // Wybieram zamianę metod w klasie na wirtualne

    // Przetestować kolejne metody
    public class QuizServiceTests
    {
        private readonly Mock<QuestionService> _mockQuestionService;
        private readonly Mock<ChoiceService> _mockChoiceService;
        private readonly Mock<CorrectAnswerService> _mockCorrectAnswerService;
        private readonly Mock<JsonCommonClass> _mockJsonService;
        private readonly QuizService _quizService;

        public QuizServiceTests()
        {
            // Tworzę mocki dla klas serwisowych
            _mockQuestionService = new Mock<QuestionService>();
            _mockChoiceService = new Mock<ChoiceService>();
            _mockCorrectAnswerService = new Mock<CorrectAnswerService>();
            _mockJsonService = new Mock<JsonCommonClass>();

            // Inicjalizacja QuizService z zamockowanymi obiektami
            _quizService = new QuizService(
                _mockQuestionService.Object,
                _mockChoiceService.Object,
                _mockCorrectAnswerService.Object,
                _mockJsonService.Object
            );
        }

        [Fact]
        public void GetAllQuestions_ShouldReturnQuestionsFromJson_WhenJsonHasQuestions()
        {
            // Arrange
            var jsonQuestions = new List<Question>
            {
                new Question(1, "What is 2+2?"), // Pozmieniać pytania na zblizone do quizu
                new Question(2, "What is 3+3?")
            };

            var serviceQuestions = new List<Question>
            {
                new Question(3, "What is 5+5?"),
                new Question(4, "What is 6+6?")
            };

            // Mock JsonCommonClass ReadFromFile
            _mockJsonService.Setup(js => js.ReadFromFile<List<Question>>(It.IsAny<string>()))
                            .Returns(jsonQuestions);

            // Mock QuestionService GetAll
            _mockQuestionService.Setup(qs => qs.GetAll())
                                .Returns(serviceQuestions);

            // Wstrzyknięcie mocków do QuizService
            var quizService = new QuizService(
                _mockQuestionService.Object,
                _mockChoiceService.Object,
                _mockCorrectAnswerService.Object,
                _mockJsonService.Object
            );

            // Act
            var result = quizService.GetAllQuestions().ToList();

            // Assert
            Assert.Equal(4, result.Count); // Sprawdzam, czy zwrócono 4 pytania
            Assert.Contains(result, q => q.QuestionId == 1); // Sprawdzam, czy pytanie z JSON jest w wynikach
            Assert.Contains(result, q => q.QuestionId == 3); // Sprawdzam, czy pytanie z serwisu jest w wynikach

            _mockJsonService.Verify();  // Weryfikacja, czy ReadFromFile() zostało wywołane
            _mockQuestionService.Verify();  // Weryfikacja, czy GetAll() zostało wywołane
        }
    }
}
