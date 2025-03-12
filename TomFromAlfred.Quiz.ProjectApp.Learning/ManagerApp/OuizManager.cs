using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract;
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
        public readonly IUserInterface _userInterface;

        public QuizManager(QuizService quizService, ScoreService scoreService, EndService endService, IUserInterface userInterface)
        {
            _quizService = quizService ?? throw new ArgumentNullException(nameof(quizService));
            _scoreService = scoreService ?? throw new ArgumentNullException(nameof(scoreService));
            _endService = endService ?? throw new ArgumentNullException(nameof(endService));
            _userInterface = userInterface ?? throw new ArgumentNullException(nameof(userInterface)); 
        }

        public void ConductQuiz()
        {
            Console.WriteLine();
            _userInterface.WriteLine("Witamy w Quiz. Sprawdź, jak dobrze znasz Tomka Wilmowskiego i jego niezwykłe przygody :-).");
            _userInterface.WriteLine("Każdy zestaw ma tylko jedną poprawną odpowiedź. Do wyboru: A, B lub C.");
            _userInterface.WriteLine("Jeżeli chcesz zakończyć quiz nacisnij K.");
            Console.WriteLine();
            _userInterface.WriteLine("Jeśli przejdziesz cały Quiz (możesz pytanie pominąć), otrzymasz informację o ilości i procencie uzyskanych punktów.");
            Console.WriteLine();
            _userInterface.WriteLine("Opcje: 1 - Odpowiedź na pytanie, 2 - Przejście do następnego pytania.");
            Console.WriteLine();

            var questions = ManagerHelper.Shuffle(_quizService.GetAllQuestions().ToList());
            var managerHelper = new ManagerHelper(questions, _quizService);
            _scoreService.StartNewQuiz(questions.Count);
            int displayNumber = 1;
            bool completedAllQuestions = false;

            int iterationCount = 0;
            int maxIterations = 50; // Zapobiegam nieskończonej pętli

            while (managerHelper.HasNext())
            {

                iterationCount++;
                if (iterationCount > maxIterations)
                {
                    _userInterface.WriteLine("Przekroczono limit iteracji, przerywamy pętlę!");
                    break;
                }

                _userInterface.WriteLine("Iteracja {iterationCount}");

                var currentQuestion = managerHelper.GetCurrentQuestion();

                _userInterface.WriteLine($"{displayNumber}, {currentQuestion.QuestionContent}");

                var choices = _quizService.GetShuffledChoicesForQuestion(currentQuestion.QuestionId, out var letterMapping).ToList();

                foreach (var choice in choices)
                {
                    _userInterface.WriteLine($"{choice.ChoiceLetter}: {choice.ChoiceContent}");
                }

                bool hasAnswered = false;

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
                            // Jeśli nie ma więcej pytań, Quiz uznany za ukończony
                            _userInterface.WriteLine(_endService.EndQuiz(managerHelper.HasNext() == false));

                            return; // TERAZ NAPRAWDĘ KOŃCZĘ QUIZ!
                        }

                        if (userInput != "1" && userInput != "2")
                        {
                            _userInterface.WriteLine("Nieprawidłowa opcja. Wybierz poprawną akcję: 1, 2 lub K.");
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
                                        _userInterface.WriteLine("Poprawna odpowiedź!");
                                        _scoreService.IncrementScore();
                                    }
                                    else
                                    {
                                        _userInterface.WriteLine("Zła odpowiedź.");
                                    }
                                    hasAnswered = true;
                                }
                                else
                                {
                                    _userInterface.WriteLine("Nieprawidłowy wybór. Wprowadź literę A, B, C.");
                                }
                            } while (!validAnswer);

                            break;

                        case "2":
                            // Pominięcie pytania
                            _userInterface.WriteLine("Pytanie pominięte.");
                            hasAnswered = true;
                            break;
                    }
                }

                displayNumber++;
                managerHelper.NextQuestion();
            }

            // Użytkownik dotarł do końca quizu
            completedAllQuestions = true;

            _userInterface.WriteLine("Koniec pytań.");
            _endService.EndQuiz(completedAllQuestions);
        }
    }
}

