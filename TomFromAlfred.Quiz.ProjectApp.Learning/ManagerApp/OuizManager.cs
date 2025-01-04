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

        private void InitializeJsonService()
        {
            var jsonService = new JsonCommonClass();
            _quizService.InitializeJsonService(jsonService, QuizService.QuestionsFilePath);
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
                .OrderBy(_ => random.Next())
                .ToList();

            var managerHelper = new ManagerHelper(questions);

            _scoreService.StartNewQuiz(questions.Count);
            int displayNumber = 1; // Zmienna do numerowania pytań

            while (managerHelper.HasNext())
            {
                var currentQuestion = managerHelper.GetCurrentQuestion();
                Console.WriteLine($"{displayNumber}, {currentQuestion.QuestionContent}");

                var choices = _quizService.GetChoicesForQuestion(currentQuestion.QuestionId);
                foreach (var choice in choices)
                {
                    Console.WriteLine($"{choice.ChoiceLetter}: {choice.ChoiceContent}");
                }

                Console.WriteLine();
                Console.WriteLine("Co chcesz zrobić?");
                Console.WriteLine("1. Odpowiedzieć na pytanie");
                Console.WriteLine("2. Przejść do następnego pytania");
                Console.WriteLine("3. Zakończyć quiz");

                var userInput = Console.ReadLine()?.Trim();

                switch (userInput)
                {
                    case "1":
                        // Odpowiedz na pytanie
                        Console.Write("Twoja odpowiedź: ");
                        var answer = Console.ReadLine()?.Trim().ToUpper();

                        if (string.IsNullOrEmpty(answer) || !choices.Any(c => c.ChoiceLetter.ToString() == answer))
                        {
                            Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                            continue;
                        }

                        if (_quizService.CheckAnswer(currentQuestion.QuestionId, answer[0]))
                        {
                            Console.WriteLine("Poprawna odpowiedź!");
                            _scoreService.IncrementScore();
                        }
                        else
                        {
                            var correctAnswer = _quizService.GetCorrectAnswerForQuestion(currentQuestion.QuestionId);
                            Console.WriteLine($"Zła odpowiedź. Poprawna odpowiedź to: {correctAnswer}");
                        }
                        break;

                    case "2":
                        // Pomiń pytanie
                        Console.WriteLine("Pytanie pominięte.");
                        break;

                    case "3":
                        // Zakończ quiz
                        Console.WriteLine("Zakończono quiz. Dziękujemy za udział!");
                        _scoreService.DisplayScoreSummary();
                        return;

                    default:
                        Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                        continue;
                }

                displayNumber++; // Zwiększ numer pytania
                managerHelper.NextQuestion();
            }

            Console.WriteLine("Koniec pytań.");
            _scoreService.DisplayScoreSummary();
        }

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

