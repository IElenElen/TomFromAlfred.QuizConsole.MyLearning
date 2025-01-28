using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class QuizManager // Komunikacja z użytkownikiem
    {
        private readonly QuizService _quizService;
        private readonly ScoreService _scoreService;
        private readonly EndService _endService;

        public QuizManager(QuizService quizService, ScoreService scoreService, EndService endService)
        {
            _quizService = quizService ?? throw new ArgumentNullException(nameof(quizService));
            _scoreService = scoreService ?? throw new ArgumentNullException(nameof(scoreService));
            _endService = endService ?? throw new ArgumentNullException(nameof(endService));
        }

        public void ConductQuiz()
        {
            Console.WriteLine();
            Console.WriteLine("Witamy w Quiz. Jedna odpowiedź tylko poprawna. Do wyboru: A, B lub C.");
            Console.WriteLine("Jeżeli chcesz zakończyć quiz nacisnij K.");
            Console.WriteLine();
            Console.WriteLine("Jeśli przejdziesz cały Quiz, otrzymasz informację o ilości i procencie uzyskanych punktów.");
            Console.WriteLine();
            Console.WriteLine("Opcje: 1 - Odpowiedź na pytanie, 2 - Przejście do następnego pytania.");
            Console.WriteLine();

            var questions = ManagerHelper.Shuffle(_quizService.GetAllQuestions().ToList());
            var managerHelper = new ManagerHelper(questions);

            _scoreService.StartNewQuiz(questions.Count);
            int displayNumber = 1;
            bool completedAllQuestions = false; 

            while (managerHelper.HasNext())
            {
                var currentQuestion = managerHelper.GetCurrentQuestion();

                Console.WriteLine($"{displayNumber}, {currentQuestion.QuestionContent}");

                var choices = _quizService.GetShuffledChoicesForQuestion(currentQuestion.QuestionId, out var letterMapping).ToList();

                foreach (var choice in choices)
                {
                    Console.WriteLine($"{choice.ChoiceLetter}: {choice.ChoiceContent}");
                }

                bool hasAnswered = false;

                Console.WriteLine("1 - Odpowiedź na pytanie, 2 - Przejście do następnego pytania.");

                while (!hasAnswered)
                {
                    // Wymuszanie poprawnej akcji (1, 2)
                    string userInput = string.Empty; // Inicjalizacja na pusty ciąg znaków
                    do
                    {
                        Console.Write("Twoja akcja (1, 2, K): ");
                        userInput = Console.ReadLine()?.Trim() ?? string.Empty; // Jeśli null, przypisz pusty ciąg znaków

                        // Sprawdzenie czy użytkownik chce zakończyć quiz
                        if (_endService.ShouldEnd(userInput))
                        {
                            // Jeśli nie ma więcej pytań, quiz uznany za ukończony
                            _endService.EndQuiz(managerHelper.HasNext() == false);
                        }

                        if (userInput != "1" && userInput != "2")
                        {
                            Console.WriteLine("Nieprawidłowa opcja. Wybierz poprawną akcję: 1, 2 lub K.");
                        }
                    } while (userInput != "1" && userInput != "2");

                    switch (userInput)
                    {
                        case "1":
                            // Wymuszanie poprawnej odpowiedzi (A, B, C)
                            char userChoiceLetter;
                            bool validAnswer = false;
                            do
                            {
                                Console.Write("Twoja odpowiedź (A, B, C): ");
                                var userResponse = Console.ReadLine()?.Trim().ToUpper();

                                if (!string.IsNullOrEmpty(userResponse) && _endService.ShouldEnd(userResponse))
                                {
                                    // Użytkownik kończy quiz (przed ukończeniem)
                                    _endService.EndQuiz(false);
                                }

                                if (!string.IsNullOrEmpty(userResponse) && userResponse.Length == 1 && "ABC".Contains(userResponse))
                                {
                                    userChoiceLetter = userResponse[0];
                                    validAnswer = true;

                                    if (_quizService.CheckAnswer(currentQuestion.QuestionId, userChoiceLetter, letterMapping))
                                    {
                                        Console.WriteLine("Poprawna odpowiedź!");
                                        _scoreService.IncrementScore();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Zła odpowiedź.");
                                    }
                                    hasAnswered = true;
                                }
                                else
                                {
                                    Console.WriteLine("Nieprawidłowy wybór. Wprowadź literę A, B, C.");
                                }
                            } while (!validAnswer);

                            break;

                        case "2":
                            // Pominięcie pytania
                            Console.WriteLine("Pytanie pominięte.");
                            hasAnswered = true;
                            break;
                    }
                }

                displayNumber++;
                managerHelper.NextQuestion();
            }

            // Użytkownik dotarł do końca quizu
            completedAllQuestions = true;

            Console.WriteLine("Koniec pytań.");
            _endService.EndQuiz(completedAllQuestions);
        }
        

        public void AddQuestion() // Metoda dodawania pytania
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

