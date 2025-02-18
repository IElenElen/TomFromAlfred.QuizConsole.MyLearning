using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using Moq;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.ServiceSupport;

namespace TomFromAlfred.QuizConsole.Tests.Integration_Tests
{
    /*
    // Oblane: 2 / 2
    public class QuizIntegrationTests
    {
        private readonly QuizService _quizService;
        private readonly QuizManager _quizManager;
        private ScoreService _scoreService;
        private CorrectAnswerService _correctAnswerService;
        private readonly EndService _endService;
        private readonly IFileWrapper _fileWrapper; // Nowa zależność

        // Mockuję File.Exists, aby test nie zależał od rzeczywistych plików
        _mockFileWrapper.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);

        public QuizIntegrationTests()
        {
            // Inicjalizacja serwisów z rzeczywistymi instancjami
            var questionService = new QuestionService();
            var choiceService = new ChoiceService();
            var correctAnswerService = new CorrectAnswerService();
            var jsonCommonClass = new JsonCommonClass();  // Założenie, że używam realnej implementacji klasy JsonCommonClass
            _scoreService = new ScoreService();
            _correctAnswerService = new CorrectAnswerService();
            _endService = new EndService(_scoreService);

            // Inicjalizacja QuizService z rzeczywistymi zależnościami
            _quizService = new QuizService(
                questionService,
                choiceService,
                correctAnswerService,
                jsonCommonClass
                );

            // Inicjalizacja QuizManager
            _quizManager = new QuizManager(_quizService, _scoreService, _endService);
        }

        // 1
        [Fact]
        public void ShouldLoadAndProcessQuizDataCorrectly_QuickIntegrationTest() // Za wolny
        {
            // Przygotowanie danych testowych bez użycia mocków
            var questions = new List<Question>
            {
                new Question (1, "2 + 2?"),
                new Question (2, "Stolica Francji")
            };

            var choices = new List<Choice>
            {
                new Choice ( 1,'A', "4" ),
                new Choice (1, 'B', "5"),
                new Choice (2, 'A', "Berlin"),
                new Choice (2, 'B', "Paryż")
            };

            var correctAnswers = new Dictionary<int, char>
            {
                { 1, 'A' },  // Correct answer for question 1 is 'A'
                { 2, 'B' }   // Correct answer for question 2 is 'B'
            };

            string QuestionsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "questions.json"); // Rzeczywista implementacja serwisu quizowego

            var questionService = new QuestionService();
            var choiceService = new ChoiceService();
            var correctAnswerService = new CorrectAnswerService();
            var jsonCommonClass = new JsonCommonClass();


            var quizService = new QuizService(questionService, choiceService, correctAnswerService, jsonCommonClass); quizService.LoadQuestionsFromJson(QuestionsFilePath);

            quizService.LoadChoicesFromJson();
            quizService.LoadCorrectSetFromJson();

            var quizManager = new QuizManager(quizService, _scoreService, _endService);

            // Symulowanie wejścia użytkownika
            var userInputs = new Queue<string>();
            userInputs.Enqueue("1"); // Wybór 1 - odpowiedź na pytanie
            userInputs.Enqueue("A"); // Wybór odpowiedzi A
            userInputs.Enqueue("2"); // Wybór 2 - przejście do następnego pytania
            userInputs.Enqueue("1"); // Wybór 1 - odpowiedź na pytanie
            userInputs.Enqueue("B"); // Wybór odpowiedzi B

            var reader = new StringReader(string.Join(Environment.NewLine, userInputs));
            Console.SetIn(reader); // Podmiana wejścia użytkownika na symulowane dane

            // Uruchomienie quizu
            quizManager.ConductQuiz();

            // Weryfikacja wyników: Sprawdzanie, czy odpowiedzi użytkownika są poprawne
            var userAnswers = new Dictionary<int, char>
            {
                { 1, 'A' },  // Odpowiedź użytkownika na pytanie 1
                { 2, 'B' }   // Odpowiedź użytkownika na pytanie 2
            };

            // Sprawdzam, czy odpowiedzi są poprawne w porównaniu do poprawnych odpowiedzi
            foreach (var (questionId, userAnswer) in userAnswers)
            {
                var correctAnswer = correctAnswers[questionId];
                Assert.Equal(correctAnswer, userAnswer); // Sprawdzenie poprawności odpowiedzi użytkownika
            }

            // Dodatkowe asercje dla danych
            var loadedQuestions = quizService.GetAllQuestions();
            var loadedChoicesQ1 = quizService.GetChoicesForQuestion(1);
            var loadedChoicesQ2 = quizService.GetChoicesForQuestion(2);

            // Czy pytania i odpowiedzi zostały prawidłowo załadowane
            Assert.Equal(2, loadedQuestions.Count());
            Assert.Contains(loadedChoicesQ1, c => c.ChoiceLetter == 'A' && c.ChoiceContent == "4");
            Assert.Contains(loadedChoicesQ1, c => c.ChoiceLetter == 'B' && c.ChoiceContent == "5");
            Assert.Contains(loadedChoicesQ2, c => c.ChoiceLetter == 'A' && c.ChoiceContent == "Berlin");
            Assert.Contains(loadedChoicesQ2, c => c.ChoiceLetter == 'B' && c.ChoiceContent == "Paris");
        }

        // 2
        [Fact]
        public void ShouldHandleMultipleQuestionsAndAnswersCorrectly() // Za wolny
        {

            _quizService.LoadQuestionsFromJson("dummyQuestionsPath");
            _quizService.LoadChoicesFromJson();
            _quizService.LoadCorrectSetFromJson();

            _quizManager.ConductQuiz();

            // Symulowanie wejścia użytkownika
            var userInputs = new Queue<string>();
            userInputs.Enqueue("1"); // Wybór 1 - odpowiedź na pytanie 1
            userInputs.Enqueue("A"); // Użytkownik wybiera odpowiedź A dla pytania 1
            userInputs.Enqueue("2"); // Wybór 2 - przejście do następnego pytania
            userInputs.Enqueue("1"); // Wybór 1 - odpowiedź na pytanie 2
            userInputs.Enqueue("B"); // Użytkownik wybiera odpowiedź B dla pytania 2

            // Mockowanie wejścia użytkownika (Console.ReadLine)
            var reader = new StringReader(string.Join(Environment.NewLine, userInputs));
            Console.SetIn(reader); // Podmiana wejścia użytkownika na symulowane dane

            // Uruchomienie quizu
            _quizManager.ConductQuiz();

            // Assert - Sprawdzam, czy pytania i odpowiedzi zostały prawidłowo załadowane
            var questions = _quizService.GetAllQuestions();
            var choicesQ1 = _quizService.GetChoicesForQuestion(1);
            var choicesQ2 = _quizService.GetChoicesForQuestion(2);

            Assert.Equal(2, questions.Count());
            Assert.Contains(choicesQ1, c => c.ChoiceLetter == 'A' && c.ChoiceContent == "2");
            Assert.Contains(choicesQ1, c => c.ChoiceLetter == 'B' && c.ChoiceContent == "3");
            Assert.Contains(choicesQ2, c => c.ChoiceLetter == 'A' && c.ChoiceContent == "Atlantyk");
            Assert.Contains(choicesQ2, c => c.ChoiceLetter == 'B' && c.ChoiceContent == "Pacifik");
        }
    } */
}
