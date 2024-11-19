using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class QuizManager
    {
        private readonly QuizService _quizService;
        private readonly ScoreService _scoreService;

        public QuizManager(QuizService quizService, ChoiceService choiceService, ScoreService scoreService)
        {
            _quizService = quizService;
            _scoreService = scoreService;
        }

        public void ConductQuiz()
        {
            var questions = _quizService.GetQuestions();
            _scoreService.StartNewQuiz(questions.Count()); // Inicjalizacja punktacji

            foreach (var question in questions)
            {
                Console.WriteLine($"{question.QuestionContent}");

                var choicesForQuestion = _quizService.GetChoicesForQuestion(question.QuestionId);
                foreach (var choice in choicesForQuestion)
                {
                    Console.WriteLine($"{choice.ChoiceLetter}: {choice.ChoiceContent}");
                }

                char userChoiceLetter;
                while (true)
                {
                    Console.Write("Twoja odpowiedź (Wybierz A, B lub C): ");
                    var userInput = Console.ReadLine()?.Trim().ToUpper();
                    if (string.IsNullOrEmpty(userInput) || userInput.Length != 1 ||
                        !choicesForQuestion.Any(c => c.ChoiceLetter == userInput[0]))
                    {
                        Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.\n");
                        continue;
                    }

                    userChoiceLetter = userInput[0];
                    break;
                }
                var isCorrect = _quizService.CheckAnswer(question.QuestionId, userChoiceLetter);
                if (isCorrect)
                {
                    Console.WriteLine("Poprawna odpowiedź!\n");
                    _scoreService.IncrementScore(); // Zwiększa punktację
                }
                else
                {
                    var correctAnswer = _quizService.GetCorrectAnswerForQuestion(question.QuestionId);
                    Console.WriteLine($"Zła odpowiedź. Poprawna odpowiedź to: {correctAnswer}\n");
                }
            }
            Console.WriteLine("Koniec quizu!");
            _scoreService.DisplayScoreSummary(); // Wyświetlenie wyników
        }
    }
}

