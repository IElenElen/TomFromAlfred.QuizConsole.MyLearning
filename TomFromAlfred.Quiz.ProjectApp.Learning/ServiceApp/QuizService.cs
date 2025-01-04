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
        // Stałe ścieżki plików JSON
        public const string QuestionsFilePath = @"C:\Users\Ilka\Desktop\.net\Quiz Tomek Konsola\question.Tomek.json";
        public const string ChoicesFilePath = @"C:\Users\Ilka\Desktop\.net\Quiz Tomek Konsola\choices.Tomek.json";
        public const string CorrectSetFilePath = @"C:\Users\Ilka\Desktop\.net\Quiz Tomek Konsola\correctSet.Tomek.json";

        private JsonCommonClass _jsonService;
        private readonly QuestionService _questionService;
        private readonly ChoiceService _choiceService;
        private readonly CorrectAnswerService _correctAnswerService;

        private List<Question> _jsonQuestions = new(); // Bufor pytań z JSON

        public QuizService(
            QuestionService questionService,
            ChoiceService choiceService,
            CorrectAnswerService correctAnswerService,
            JsonCommonClass jsonService)
        {
            _questionService = questionService ?? throw new ArgumentNullException(nameof(questionService));
            _choiceService = choiceService ?? throw new ArgumentNullException(nameof(choiceService));
            _correctAnswerService = correctAnswerService ?? throw new ArgumentNullException(nameof(correctAnswerService));
            _jsonService = jsonService ?? throw new ArgumentNullException(nameof(jsonService));

            LoadQuestionsFromJson();
            LoadChoicesFromJson();
            LoadCorrectSetFromJson();
        }

        public void InitializeJsonService(JsonCommonClass jsonService, string jsonFilePath)
        {
            _jsonService = jsonService ?? throw new ArgumentNullException(nameof(jsonService));
            if (!File.Exists(jsonFilePath))
            {
                throw new FileNotFoundException($"Plik {jsonFilePath} nie istnieje.");
            }
        }

        // Metoda wczytania pytań z pliku JSON
        private void LoadQuestionsFromJson()
        {
            try
            {
                if (!File.Exists(QuestionsFilePath))
                {
                    Console.WriteLine($"Plik {QuestionsFilePath} nie istnieje. Tworzę domyślny plik.");
                    var defaultQuestions = new List<Question>
                    {
                        new Question(18, "Pytanie 18?"),
                        new Question(19, "Pytanie 19?"),
                        new Question(20, "Pytanie 20?")
                    };
                    _jsonService.WriteToFile(QuestionsFilePath, defaultQuestions);
                }

                _jsonQuestions = _jsonService.ReadFromFile<List<Question>>(QuestionsFilePath) ?? new List<Question>();
                Console.WriteLine($"Wczytano pytań: {_jsonQuestions.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas wczytywania pytań z pliku JSON: {ex.Message}");
                _jsonQuestions = new List<Question>();
            }
        }

        // Dodanie pytania do pliku JSON
        public void AddQuestionToJson(Question question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            _jsonQuestions.Add(question);
            SaveQuestionsToJson();
        }

        // Zapis pytań do pliku JSON
        private void SaveQuestionsToJson()
        {
            try
            {
                _jsonService.WriteToFile(QuestionsFilePath, _jsonQuestions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas zapisywania pytań do pliku JSON: {ex.Message}");
            }
        }

        // Pobranie wszystkich pytań (z serwisów + JSON)
        public IEnumerable<Question> GetAllQuestions()
        {
            var filteredServiceQuestions = _questionService.GetAll()
                .Where(sq => !_jsonQuestions.Any(jq => jq.QuestionId == sq.QuestionId));

            return _jsonQuestions.Concat(filteredServiceQuestions).ToList();
        }

        // Wczytanie wyborów z pliku JSON
        private void LoadChoicesFromJson()
        {
            try
            {
                if (!File.Exists(ChoicesFilePath))
                {
                    Console.WriteLine($"Plik {ChoicesFilePath} nie istnieje. Tworzę domyślny plik.");
                    var defaultChoices = new List<Choice>
                    {
                        new Choice(18, 'A', "Opcja A"),
                        new Choice(18, 'B', "Opcja B"),
                        new Choice(18, 'C', "Opcja C")
                    };
                    _jsonService.WriteToFile(ChoicesFilePath, defaultChoices);
                }

                var jsonChoices = _jsonService.ReadFromFile<List<Choice>>(ChoicesFilePath);
                foreach (var choice in jsonChoices)
                {
                    _choiceService.Add(choice);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas wczytywania odpowiedzi z pliku JSON: {ex.Message}");
            }
        }

        public IEnumerable<Choice> GetChoicesForQuestion(int questionId)
        {
            var jsonChoices = _choiceService.GetAll()
                .Where(choice => _jsonQuestions.Any(q => q.QuestionId == questionId && IsChoiceRelatedToQuestion(choice, q)))
                .ToList();

            if (jsonChoices.Any())
            {
                return jsonChoices;
            }

            return _choiceService.GetChoicesForQuestion(questionId);
        }

        private bool IsChoiceRelatedToQuestion(Choice choice, Question question) //Relacja wyboru do jego pytania
        {
            return choice.ChoiceId >= question.QuestionId * 10 && choice.ChoiceId < (question.QuestionId + 1) * 10;
        }

        private void LoadCorrectSetFromJson()
        {
            try
            {
                if (!File.Exists(CorrectSetFilePath))
                {
                    Console.WriteLine($"Plik {CorrectSetFilePath} nie istnieje. Tworzę domyślny plik.");
                    var defaultCorrectSets = new List<JsonHelper>
                    {
                        new JsonHelper { QuestionNumber = 18, LetterCorrectAnswer = "B", ContentCorrectAnswer = "Odpowiedź B" }
                    };
                    _jsonService.WriteToFile(CorrectSetFilePath, defaultCorrectSets);
                }

                var jsonCorrectSets = _jsonService.ReadFromFile<List<JsonHelper>>(CorrectSetFilePath);
                foreach (var jsonSet in jsonCorrectSets)
                {
                    var correctAnswer = new CorrectAnswer(
                        jsonSet.QuestionNumber,
                        jsonSet.ContentCorrectAnswer
                    );
                    _correctAnswerService.Add(correctAnswer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas wczytywania poprawnych odpowiedzi z pliku JSON: {ex.Message}");
            }
        }

        public CorrectAnswer GetCorrectAnswerForQuestion(int questionId)
        {
            var jsonCorrectAnswer = _correctAnswerService.GetAll()
                .FirstOrDefault(answer => answer.CorrectAnswerId == questionId);

            if (jsonCorrectAnswer != null)
            {
                return jsonCorrectAnswer;
            }

            return _correctAnswerService.GetAll()
                .FirstOrDefault(answer => answer.CorrectAnswerId == questionId);
        }

        public bool CheckAnswer(int questionId, char userChoiceLetter)
        {
            var correctAnswer = GetCorrectAnswerForQuestion(questionId);
            if (correctAnswer == null)
            {
                Console.WriteLine($"Brak poprawnej odpowiedzi dla pytania o Id {questionId}.");
                return false;
            }

            var correctChoice = GetChoicesForQuestion(questionId)
                .FirstOrDefault(c => c.ChoiceLetter == userChoiceLetter);

            return correctChoice != null;
        }
    }
}

