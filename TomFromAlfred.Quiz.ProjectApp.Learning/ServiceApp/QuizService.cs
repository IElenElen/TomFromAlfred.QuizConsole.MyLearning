using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.ServiceSupport;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    /* 
     * Dla pytania: json - questions.Tomek.json ma QuestionId i QuestionContent. Tak jak entity Question.
     * Dla wyboru: json - choices.Tomek.json ma ChoiceId, ChoiceLetter i ChoiceContent. Tak jak entity Choice.
     * Dla poprawnego zestawu: json - correctSet.Tomek.json ma QuestionNumber, LetterCorrectAnswer, ContentCorrectAnswer. 
     * A entity CorrectAnswer ma CorrectAnswerId oraz CorrectAnswerContent.
     */


    // Próba zmapowania danych json JsonHelper i entity CorrectAnswer
    public class QuizService
    {
        public const string QuestionsFilePath = @"C:\Users\Ilka\Desktop\.net\Quiz Tomek Konsola\questions.Tomek.json";
        public const string ChoicesFilePath = @"C:\Users\Ilka\Desktop\.net\Quiz Tomek Konsola\choices.Tomek.json";
        public const string CorrectSetFilePath = @"C:\Users\Ilka\Desktop\.net\Quiz Tomek Konsola\correctSet.Tomek.json";

        private readonly IFileWrapper _fileWrapper; 

        private JsonCommonClass _jsonService;
        private readonly QuestionService _questionService;
        private readonly ChoiceService _choiceService;
        private readonly CorrectAnswerService _correctAnswerService;

        protected List<Question> _jsonQuestions = new();
        protected Dictionary<int, List<Choice>> _jsonChoices = new();
        protected Dictionary<int, string> _correctAnswers = new();

        public QuizService(
                QuestionService questionService,
                ChoiceService choiceService,
                CorrectAnswerService correctAnswerService,
                JsonCommonClass jsonService,
                IFileWrapper fileWrapper)
        {
            _questionService = questionService ?? throw new ArgumentNullException(nameof(questionService));
            _choiceService = choiceService ?? throw new ArgumentNullException(nameof(choiceService));
            _correctAnswerService = correctAnswerService ?? throw new ArgumentNullException(nameof(correctAnswerService));
            _jsonService = jsonService ?? throw new ArgumentNullException(nameof(jsonService));
            _fileWrapper = fileWrapper ?? throw new ArgumentNullException(nameof(fileWrapper));

            LoadQuestionsFromJson(QuestionsFilePath);
            LoadChoicesFromJson();
            LoadCorrectSetFromJson();
        }

        public void InitializeJsonService(JsonCommonClass jsonService, string jsonFilePath)
        {
            if (jsonService == null)
                throw new ArgumentNullException(nameof(jsonService));

            if (!_fileWrapper.Exists(jsonFilePath)) // Używam mockowalnej wersji File.Exists
                throw new FileNotFoundException($"Plik {jsonFilePath} nie istnieje.");

            _jsonService = jsonService;
        }

        public virtual IEnumerable<Question> GetAllQuestions()
        {
            if (_jsonQuestions.Any())
            {
                Console.WriteLine($"Priorytet JSON: {_jsonQuestions.Count} pytań wczytano z JSON.");
                Console.WriteLine("Łączenie pytań z JSON i serwisu. Priorytet: JSON.");

                // Łącz dane, ale unikaj duplikatów
                var jsonQuestionIds = _jsonQuestions.Select(q => q.QuestionId).ToHashSet();
                var remainingServiceQuestions = _questionService.GetAllActive().Where(q => !jsonQuestionIds.Contains(q.QuestionId));
                Console.WriteLine($"Liczba pytań z serwisu dodanych do JSON: {remainingServiceQuestions.Count()}");
                return _jsonQuestions.Concat(remainingServiceQuestions);
            }

            Console.WriteLine("Zwracane pytania z serwisu.");
            return _questionService.GetAllActive();
        }

        public virtual IEnumerable<Choice> GetChoicesForQuestion(int questionId)
        {
            // Najpierw sprawdź dane z JSON
            if (_jsonChoices.ContainsKey(questionId))
            {
                Console.WriteLine($"Pobrano wybory z JSON dla pytania {questionId}");
                return _jsonChoices[questionId];
            }

            // Jeśli brak danych w JSON, sprawdź dane twardo zakodowane
            var choicesFromService = _choiceService.GetChoicesForQuestion(questionId);
            if (choicesFromService.Any())
            {
                Console.WriteLine($"Pobrano wybory z serwisu dla pytania {questionId}");
                return choicesFromService;
            }

            Console.WriteLine($"Brak wyborów dla pytania o Id {questionId}.");
            return Enumerable.Empty<Choice>();
        }

        public virtual IEnumerable<Choice> GetShuffledChoicesForQuestion(int questionId, out Dictionary<char, char> letterMapping)
        {
            var choices = GetChoicesForQuestion(questionId).ToList();
            var random = new Random();
            List<Choice> shuffledChoices;

            // Upewniam się, że lista się zmieniła (ograniczam liczbę prób do uniknięcia nieskończonej pętli)
            int maxAttempts = 10;
            int attempts = 0;
            do
            {
                shuffledChoices = choices.OrderBy(_ => random.Next()).ToList();
                attempts++;
            } while (attempts < maxAttempts && shuffledChoices.SequenceEqual(choices));

            letterMapping = new Dictionary<char, char>();
            char newLetter = 'A';

            var finalChoices = new List<Choice>();

            foreach (var choice in shuffledChoices)
            {
                letterMapping[newLetter] = choice.ChoiceLetter; // Nowa → Oryginalna litera
                Console.WriteLine($"Mapowanie: {newLetter} → {choice.ChoiceLetter}");

                // Tworzę nowy obiekt zamiast modyfikować oryginał
                finalChoices.Add(new Choice(choice.ChoiceId, newLetter, choice.ChoiceContent));
                newLetter++;
            }

            return finalChoices;
        }

        public virtual bool CheckAnswer(int questionId, char userChoiceLetter, Dictionary<char, char> letterMapping)
        {
            foreach (var mapping in letterMapping)
            {
                Console.WriteLine($"Mapowanie liter: Nowa: {mapping.Key}, Oryginalna: {mapping.Value}");
            }

            // Pobierz zamapowaną literę użytkownika
            char originalLetter;
            if (letterMapping != null && letterMapping.TryGetValue(userChoiceLetter, out originalLetter))
            {
                Console.WriteLine($"Sprawdzanie odpowiedzi: Użytkownik: {userChoiceLetter}, Zamapowana: {originalLetter}");
            }
            else
            {
                originalLetter = userChoiceLetter;
                Console.WriteLine($"Brak mapowania dla {userChoiceLetter}, używam oryginalnej litery.");
            }

            // Pobranie treści odpowiedzi użytkownika (po mapowaniu!)
            var userAnswerContent = GetAnswerContentFromLetter(originalLetter, questionId);
            if (userAnswerContent == null)
            {
                Console.WriteLine("Nie znaleziono treści dla litery użytkownika. Zwracam fałsz.");
                return false;
            }

            // Pobranie treści poprawnej odpowiedzi dla danego pytania
            if (_correctAnswers.TryGetValue(questionId, out var correctLetter))
            {
                // Zamiana poprawnej litery na jej faktyczną treść
                var correctAnswerContent = GetAnswerContentFromLetter(correctLetter[0], questionId);
                if (correctAnswerContent == null)
                {
                    Console.WriteLine("Nie znaleziono poprawnej odpowiedzi w JSON. Zwracam fałsz.");
                    return false;
                }

                Console.WriteLine($"Z JSON: Użytkownik: \"{userAnswerContent}\", Poprawna: \"{correctAnswerContent}\"");

                if (string.Equals(userAnswerContent, correctAnswerContent, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Poprawna odpowiedź z JSON.");
                    return true;
                }
            }

            // Pobranie poprawnej odpowiedzi z serwisu, jeśli nie ma jej w JSON
            var correctAnswerFromService = _correctAnswerService.GetCorrectAnswerForQuestion(questionId);
            if (correctAnswerFromService != null)
            {
                Console.WriteLine($"Z ENCJI: Użytkownik: \"{userAnswerContent}\", Poprawna: \"{correctAnswerFromService.CorrectAnswerContent}\"");

                if (string.Equals(userAnswerContent, correctAnswerFromService.CorrectAnswerContent, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Poprawna odpowiedź z encji CorrectAnswer.");
                    return true;
                }
            }

            // Jeśli dotarłam tutaj, odpowiedź jest błędna
            Console.WriteLine("Zła odpowiedź.");
            return false;
        }

        public virtual string? GetAnswerContentFromLetter(char answerLetter, int questionId)
        {
            Console.WriteLine($"Szukam treści odpowiedzi dla litery: {answerLetter} w pytaniu Id {questionId}");

            var choices = GetChoicesForQuestion(questionId).ToList();

            if (!choices.Any())
            {
                Console.WriteLine($"Brak odpowiedzi dla pytania {questionId}.");
                return null;
            }

            var match = choices.FirstOrDefault(c => c.ChoiceLetter == answerLetter);
            if (match != null)
            {
                Console.WriteLine($"Znaleziono treść odpowiedzi: \"{match.ChoiceContent}\" dla litery {answerLetter}");
                return match.ChoiceContent;
            }

            Console.WriteLine("Nie znaleziono dopasowania dla litery.");
            return null;
        }

        public virtual void LoadQuestionsFromJson(string filePath)
        {
            Console.WriteLine("Wywołano LoadQuestionsFromJson");

            if (!File.Exists(filePath)) // Zmieniam z QuestionsFilePath na parametr filePath
            {
                Console.WriteLine($"Plik {filePath} nie istnieje.");
                return;
            }

            try
            {
                // Wczytanie JSON
                _jsonQuestions = _jsonService.ReadFromFile<List<Question>>(filePath) ?? new List<Question>();

                // Jeżeli lista jest pusta, rzucam wyjątek
                if (_jsonQuestions == null)
                {
                    Console.WriteLine("JsonQuestions jest NULL!");
                }

                else if (_jsonQuestions.Count == 0)
                {
                    Console.WriteLine("JSON pytania są puste - rzucam wyjątek!");
                    throw new JsonException("JSON does not contain any questions");
                }

                Console.WriteLine($"Wczytano {_jsonQuestions.Count} pytań z JSON.");
            }
            catch (JsonException ex)  // Chwytam tylko wyjątek JsonException
            {
                // Ponownie rzucam wyjątek, aby test mógł go przechwycić
                throw new JsonException("JSON does not contain any questions", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas ładowania pytań z JSON: {ex.Message}");
            }
        }

        public virtual void LoadChoicesFromJson()
        {
            if (!File.Exists(ChoicesFilePath))
            {
                Console.WriteLine($"Plik {ChoicesFilePath} nie istnieje.");
                return;
            }

            var jsonChoices = _jsonService.ReadFromFile<List<Choice>>(ChoicesFilePath) ?? new List<Choice>();
            _jsonChoices = jsonChoices
                .GroupBy(c => c.ChoiceId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var choice in jsonChoices)
            {
                Console.WriteLine($"Wczytano wybór: Id {choice.ChoiceId}, opcja {choice.ChoiceLetter}, treść: {choice.ChoiceContent}.");
            }
        }

        public virtual void LoadCorrectSetFromJson()
        {
            try
            {
                if (!File.Exists(CorrectSetFilePath))
                {
                    Console.WriteLine($"Plik {CorrectSetFilePath} nie istnieje.");
                    return;
                }

                var correctSet = _jsonService.ReadFromFile<List<JsonHelper>>(CorrectSetFilePath) ?? new List<JsonHelper>();

                foreach (var correct in correctSet)
                {
                    // Mapowanie QuestionNumber → CorrectAnswerId
                    int questionId = correct.QuestionNumber;

                    // Mapowanie poprawnej odpowiedzi do encji
                    var correctAnswer = new CorrectAnswer(
                        questionId,
                        correct.ContentCorrectAnswer,
                        true
                    );

                    Console.WriteLine($"Z JSON: Pytanie {correct.QuestionNumber}, Litera: {correct.LetterCorrectAnswer}, Treść: {correct.ContentCorrectAnswer}");

                    // Dodanie do słownika dla JSON
                    if (!_correctAnswers.ContainsKey(questionId))
                    {
                        _correctAnswers[questionId] = correct.LetterCorrectAnswer; // Tylko litera
                    }

                    Console.WriteLine($"Wczytano poprawną odpowiedź: {correctAnswer.CorrectAnswerId} → {correctAnswer.CorrectAnswerContent} ({correct.LetterCorrectAnswer})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas wczytywania poprawnych odpowiedzi z JSON: {ex.Message}");
            }
        }

        private IEnumerable<Choice> ShuffleChoices(IEnumerable<Choice> choices) 
        {
            var random = new Random();
            var shuffledChoices = choices.OrderBy(_ => random.Next()).ToList();

            char letter = 'A';
            foreach (var choice in shuffledChoices)
            {
                choice.ChoiceLetter = letter++;
            }

            return shuffledChoices;
        }

        private static bool IsChoiceRelatedToQuestion(Choice choice, Question question)
        {
            return choice.ChoiceId >= question.QuestionId * 10 && choice.ChoiceId < (question.QuestionId + 1) * 10;
        }

        public void AddQuestionToJson(Question question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            _jsonQuestions.Add(question);
            SaveQuestionsToJson();
        }

        private void SaveQuestionsToJson()
        {
            try
            {
                _jsonService.WriteToFile(QuestionsFilePath, _jsonQuestions);
                Console.WriteLine("Zapisano pytania do pliku JSON.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas zapisywania pytań do pliku JSON: {ex.Message}");

                #if DEBUG
                throw; // W trybie debug/testów propaguję wyjątek
                #endif
            }
        }
    }
}


