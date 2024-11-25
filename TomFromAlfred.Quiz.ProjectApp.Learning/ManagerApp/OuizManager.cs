using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class QuizManager
    {
        private readonly QuizService _quizService;
        private readonly ScoreService _scoreService;
        private readonly EndService _endService;

        public QuizManager(QuizService quizService, ChoiceService choiceService, ScoreService scoreService, EndService endService)
        {
            _quizService = quizService ?? throw new ArgumentNullException(nameof(quizService));
            _scoreService = scoreService ?? throw new ArgumentNullException(nameof(scoreService));
            _endService = endService ?? throw new ArgumentNullException(nameof(endService));

            InitializeJsonService();
        }

        // Metoda inicjalizująca obsługę JSON
        private void InitializeJsonService()
        {
            const string jsonFilePath = @"C:\Users\Ilka\Desktop\.net\Quiz Tomek Konsola\questions.Tomek.json";

            var jsonService = new JsonCommonClass();
            _quizService.InitializeJsonService(jsonService, jsonFilePath); // Przekazujemy ścieżkę do QuizService
        }

        public void ConductQuiz()
        {
            Console.WriteLine("Witamy w Quiz. Jedna odpowiedź tylko poprawna. Do wyboru: A, B lub C.");
            Console.WriteLine("Jeżeli chcesz zakończyć quiz nacisnij K.");
            Console.WriteLine();
            Console.WriteLine("Jeśli przejdziesz cały Quiz, otrzymasz informację o ilości i procencie uzyskanych punktów.");
            Console.WriteLine();

            var random = new Random();
            var questions = _quizService.GetAllQuestions()
                .ToList()
                .OrderBy(_ => random.Next())
                .ToList();

            _scoreService.StartNewQuiz(questions.Count);

            int displayNumber = 1;

            foreach (var question in questions)
            {
                Console.WriteLine($"{displayNumber}, {question.QuestionContent}");

                var choicesForQuestion = _quizService.GetChoicesForQuestion(question.QuestionId);
                foreach (var choice in choicesForQuestion)
                {
                    Console.WriteLine($"{choice.ChoiceLetter}: {choice.ChoiceContent}");
                }

                char userChoiceLetter;

                while (true)
                {
                    Console.Write("Twoja odpowiedź: ");
                    var userInput = Console.ReadLine()?.Trim().ToUpper();

                    if (_endService.ShouldEnd(userInput))
                    {
                        _endService.EndQuiz();
                    }

                    if (string.IsNullOrEmpty(userInput) || userInput.Length != 1 ||
                        !choicesForQuestion.Any(c => c.ChoiceLetter == userInput[0]))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.\n");
                        continue;
                    }

                    userChoiceLetter = userInput[0];
                    break;
                }

                var isCorrect = _quizService.CheckAnswer(question.QuestionId, userChoiceLetter);
                if (isCorrect)
                {
                    Console.WriteLine();
                    Console.WriteLine("Poprawna odpowiedź!\n");
                    _scoreService.IncrementScore();
                }
                else
                {
                    var correctAnswer = _quizService.GetCorrectAnswerForQuestion(question.QuestionId);
                    Console.WriteLine();
                    Console.WriteLine($"Zła odpowiedź. Poprawna odpowiedź to: {correctAnswer}\n");
                }

                displayNumber++;
            }

            Console.WriteLine("Koniec quizu!");
            Console.WriteLine();
            _scoreService.DisplayScoreSummary();
        }

        // Dodanie nowego pytania
        public void AddQuestion()
        {
            Console.Write("Podaj treść pytania: ");
            var content = Console.ReadLine();

            Console.WriteLine("Dodaj odpowiedzi (wpisz po jednej, kończąc ENTER):");
            var choices = new List<string>();
            char choiceLetter = 'A';

            while (true)
            {
                Console.Write($"{choiceLetter}: ");
                var choiceContent = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(choiceContent))
                    break;

                choices.Add(choiceContent);
                choiceLetter++;
            }

            Console.Write("Podaj poprawną odpowiedź (literę A, B, itd.): ");
            var correctAnswerLetter = Console.ReadLine()?.ToUpper();

            if (correctAnswerLetter == null || correctAnswerLetter.Length != 1 || correctAnswerLetter[0] < 'A' || correctAnswerLetter[0] >= 'A' + choices.Count)
            {
                Console.WriteLine("Nieprawidłowa odpowiedź. Dodawanie pytania anulowane.");
                return;
            }

            var question = new Question(_quizService.GetAllQuestions().Max(q => q.QuestionId) + 1, content);
            _quizService.AddQuestionToJson(question);
            Console.WriteLine("Dodano nowe pytanie!");
        }
    }
}

