using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForService;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.EntityService;
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

    public class QuizService : IQuizService
    {
        public const string QuestionsFilePath = @"C:\Users\Ilka\Desktop\.net\Quiz Tomek Konsola\questions.Tomek.json";
        public const string ChoicesFilePath = @"C:\Users\Ilka\Desktop\.net\Quiz Tomek Konsola\choices.Tomek.json";
        public const string CorrectSetFilePath = @"C:\Users\Ilka\Desktop\.net\Quiz Tomek Konsola\correctSet.Tomek.json";

        private readonly IFileWrapper _fileWrapper;

        private JsonCommonClass _jsonService;
        private readonly QuestionService _questionService;
        private readonly ChoiceService _choiceService;
        private readonly CorrectAnswerService _correctAnswerService;

        protected List<Question> _jsonQuestions = [];
        protected Dictionary<int, List<Choice>> _jsonChoices = [];
        protected Dictionary<int, string> _correctAnswers = [];

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
        public virtual void AddQuestionToJson(Question question)
        {
            ArgumentNullException.ThrowIfNull(question);

            _jsonQuestions.Add(question);
            SaveQuestionsToJson();
        }

        public virtual bool CheckAnswer(int questionId, char userChoiceLetter, Dictionary<char, char> letterMapping)
        {
            foreach (var mapping in letterMapping)
            {
                Console.WriteLine($"Mapowanie liter: Nowa: {mapping.Key}, Oryginalna: {mapping.Value}");
            }

            // Inlinowanie przypisania oryginalnej litery
            char originalLetter = letterMapping != null && letterMapping.TryGetValue(userChoiceLetter, out var mapped)
                ? mapped
                : userChoiceLetter;

            Console.WriteLine($"Sprawdzanie odpowiedzi: Użytkownik: {userChoiceLetter}, Zamapowana: {originalLetter}");

            // Pobierz zamapowaną literę użytkownika
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

            if (choices.Count == 0)
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

        public virtual IEnumerable<Question> GetAllQuestions()
        {
            if (_jsonQuestions.Count > 0)
            {
                Console.WriteLine($"Priorytet JSON: {_jsonQuestions.Count} pytań wczytano z JSON.");
                Console.WriteLine("Łączenie pytań z JSON i serwisu. Priorytet: JSON.");

                var jsonQuestionIds = _jsonQuestions.Select(q => q.QuestionId).ToHashSet();
                var remainingServiceQuestions = _questionService.GetAllActive()
                    .Where(q => !jsonQuestionIds.Contains(q.QuestionId));

                Console.WriteLine($"Liczba pytań z serwisu dodanych do JSON: {remainingServiceQuestions.Count()}");
                return _jsonQuestions.Concat(remainingServiceQuestions);
            }

            Console.WriteLine("Zwracane pytania z serwisu.");
            return _questionService.GetAllActive();
        }

        public virtual IEnumerable<Choice> GetChoicesForQuestion(int questionId)
        {
            // Najpierw sprawdź dane z JSON
            if (_jsonChoices.TryGetValue(questionId, out var choicesFromJson)) // Nie upraszczać
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
            return Enumerable.Empty<Choice>(); // Nie upraszczać
        }

        public IEnumerable<Choice> GetShuffledChoicesForQuestion(int questionId, out Dictionary<char, char> letterMapping)
        {
            var choices = GetChoicesForQuestion(questionId).ToList();
            var random = new Random();
            List<Choice> shuffledChoices;

            // Upewniam się, że lista się zmieniła (ograniczam liczbę prób do uniknięcia nieskończonej pętli)
            int maxAttempts = 10;
            int attempts = 0;
            do
            {
                shuffledChoices = choices.OrderBy(_ => random.Next()).ToList(); // Nie upraszczać
                attempts++;
            } while (attempts < maxAttempts && shuffledChoices.SequenceEqual(choices));

            letterMapping = new Dictionary<char, char>(); // Nie upraszczać
            char newLetter = 'A';

            var finalChoices = new List<Choice>();

            foreach (var choice in shuffledChoices)
            {
                letterMapping[newLetter] = choice.ChoiceLetter; // Nowa → Oryginalna litera
                Console.WriteLine($"Mapowanie: {newLetter} → {choice.ChoiceLetter}");

                // Tworzę nowy obiekt zamiast modyfikować oryginał
                finalChoices.Add(new Choice
                {
                    ChoiceId = choice.ChoiceId,
                    ChoiceLetter = newLetter,
                    ChoiceContent = choice.ChoiceContent,
                    IsActive = true
                });
                newLetter++;
            }

            return finalChoices;
        }

        private static List<Choice> ShuffleChoices(IEnumerable<Choice> choices)
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

        public void InitializeJsonService(JsonCommonClass jsonService, string jsonFilePath)
        {
               ArgumentNullException.ThrowIfNull(jsonService);

            if (!_fileWrapper.Exists(jsonFilePath))
                throw new FileNotFoundException($"Plik {jsonFilePath} nie istnieje.");

            _jsonService = jsonService;
        }


        public virtual void LoadChoicesFromJson()
        {
            if (!File.Exists(ChoicesFilePath))
            {
                Console.WriteLine($"Plik {ChoicesFilePath} nie istnieje.");
                return;
            }

            var jsonChoices = _jsonService.ReadFromFile<List<Choice>>(ChoicesFilePath) ?? new List<Choice>(); // Nie upraszczać
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

                var correctSet = _jsonService.ReadFromFile<List<JsonHelper>>(CorrectSetFilePath) ?? new List<JsonHelper>(); // Nie upraszczać

                foreach (var correct in correctSet)
                {
                    // Mapowanie QuestionNumber → CorrectAnswerId
                    int questionId = correct.QuestionNumber;

                    if (string.IsNullOrWhiteSpace(correct.ContentCorrectAnswer))
                        throw new InvalidOperationException("Brak treści poprawnej odpowiedzi.");

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
                        if (correct.LetterCorrectAnswer is null)
                            throw new InvalidOperationException("Brak litery poprawnej odpowiedzi.");

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
                _jsonQuestions = _jsonService.ReadFromFile<List<Question>>(filePath) ?? new List<Question>(); // Nie upraszczać

                // Jeżeli lista jest pusta, rzucam wyjątek
                if (_jsonQuestions.Count == 0)
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

        public virtual void SaveQuestionsToJson()
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
