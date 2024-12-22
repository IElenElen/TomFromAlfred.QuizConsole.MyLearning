using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class QuizService
    {
        private JsonCommonClass _jsonService;
        private string _jsonFilePath;

        private readonly QuestionService _questionService;
        private readonly ChoiceService _choiceService;
        private readonly CorrectAnswerService _correctAnswerService;

        private List<Question> _jsonQuestions = new(); // Bufor pytań z JSON

        public QuizService(
            QuestionService questionService,
            ChoiceService choiceService,
            CorrectAnswerService correctAnswerService,
            JsonCommonClass jsonService,
            string jsonFilePath)
        {
            _questionService = questionService ?? throw new ArgumentNullException(nameof(questionService));
            _choiceService = choiceService ?? throw new ArgumentNullException(nameof(choiceService));
            _correctAnswerService = correctAnswerService ?? throw new ArgumentNullException(nameof(correctAnswerService));
            _jsonService = jsonService ?? throw new ArgumentNullException(nameof(jsonService));
            _jsonFilePath = jsonFilePath ?? throw new ArgumentNullException(nameof(jsonFilePath));

            // Inicjalizacja pytań z JSON
            LoadQuestionsFromJson();
        }

        public void InitializeJsonService(JsonCommonClass jsonService, string jsonFilePath)
        { 

            _jsonService = jsonService ?? throw new ArgumentNullException(nameof(jsonService));
            _jsonFilePath = jsonFilePath ?? throw new ArgumentNullException(nameof(jsonFilePath));
            LoadQuestionsFromJson();
        }

        // Wczytanie pytań z JSON
        private void LoadQuestionsFromJson()
        {
            if (_jsonService == null || string.IsNullOrEmpty(_jsonFilePath))
            {
                throw new InvalidOperationException("JSON service is not initialized.");
            }

            try
            {
                _jsonQuestions = _jsonService.ReadFromFile<List<Question>>(_jsonFilePath) ?? new List<Question>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas wczytywania pytań z pliku JSON: {ex.Message}");
                _jsonQuestions = new List<Question>();
            }
        }

        // Pobranie wszystkich pytań z serwisów (dotychczasowa obsługa)
        public IEnumerable<Question> GetQuestionsFromServices()
        {
            return _questionService.GetAll();
        }

        // Pobranie wszystkich pytań z JSON
        public IEnumerable<Question> GetQuestionsFromJson()
        {
            return _jsonQuestions;
        }

        // Dodanie pytania do JSON
        public void AddQuestionToJson(Question question)
        {
            if (question == null) throw new ArgumentNullException(nameof(question));

            _jsonQuestions.Add(question);
            SaveQuestionsToJson();
        }

        // Zapis pytań do pliku JSON
        private void SaveQuestionsToJson()
        {
            try
            {
                _jsonService.WriteToFile(_jsonFilePath, _jsonQuestions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas zapisywania pytań do pliku JSON: {ex.Message}");
            }
        }

        // Pobranie wszystkich pytań (z serwisów + JSON)
        public IEnumerable<Question> GetAllQuestions()
        {
            var serviceQuestions = GetQuestionsFromServices();
            return serviceQuestions.Concat(_jsonQuestions).ToList(); // Łączenie pytań z serwisów i JSON
        }

        public IEnumerable<Choice> GetChoicesForQuestion(int questionId)
        {
            return _choiceService.GetChoicesForQuestion(questionId);
        }

        public CorrectAnswer GetCorrectAnswerForQuestion(int questionId)
        {
            return _correctAnswerService.GetCorrectAnswerForQuestion(questionId);
        }

        public bool CheckAnswer(int questionId, char userChoiceLetter)
        {
            // Pobierz poprawną odpowiedź dla danego pytania
            var correctAnswer = _correctAnswerService.GetCorrectAnswerForQuestion(questionId);
            if (correctAnswer == null)
            {
                Console.WriteLine($"Brak poprawnej odpowiedzi dla pytania o ID {questionId}.");
                return false; // Jeśli brak poprawnej odpowiedzi, uznaj odpowiedź za błędną
            }

            // Sprawdź, czy litera wyboru użytkownika odpowiada poprawnej odpowiedzi
            return string.Equals(correctAnswer.CorrectAnswerContent, userChoiceLetter.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
