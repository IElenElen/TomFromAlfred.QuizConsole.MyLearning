using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class QuizManager
    {
        private readonly QuizService _quizService;
        private readonly ScoreService _scoreService;
        private readonly EndService _endService;

        public QuizManager(QuizService quizService, ChoiceService choiceService, ScoreService scoreService, EndService endService)
        {
            _quizService = quizService;
            _scoreService = scoreService;
            _endService = endService;
        }

        public void ConductQuiz()
        {
            Console.WriteLine("Witamy w Quiz. Jedna odpowiedź tylko poprawna. Do wyboru: A, B lub C.");
            Console.WriteLine("Jeżeli chcesz zakończyć quiz nacisnij K.");
            Console.WriteLine();
            Console.WriteLine("Jeśli przejdziesz cały Quiz, otrzymasz informację o ilości i procencie uzyskanych punktów.");
            Console.WriteLine();

            // Przekształcenie IEnumerable na List i losowanie
            var random = new Random();
            var questions = _quizService.GetQuestions()
                .ToList() // Zamiana IEnumerable na List
                .OrderBy(_ => random.Next()) // Losowanie kolejności
                .ToList();

            _scoreService.StartNewQuiz(questions.Count); // Inicjalizacja punktacji

            int displayNumber = 1; // Numeracja wyświetlana dla użytkownika zaczyna się od 1

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

                    // Sprawdzenie, czy użytkownik chce zakończyć quiz
                    if (_endService.ShouldEnd(userInput))
                    {
                        _endService.EndQuiz(); // Obsługa zakończenia quizu
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
                    _scoreService.IncrementScore(); // Zwiększa punktację
                }
                else
                {
                    var correctAnswer = _quizService.GetCorrectAnswerForQuestion(question.QuestionId);
                    Console.WriteLine();
                    Console.WriteLine($"Zła odpowiedź. Poprawna odpowiedź to: {correctAnswer}\n");
                }

                displayNumber++; // Zwiększanie numeracji wyświetlanej użytkownikowi
            }

            Console.WriteLine("Koniec quizu!");
            Console.WriteLine();
            _scoreService.DisplayScoreSummary(); // Wyświetlenie wyników
        }
    }
}

