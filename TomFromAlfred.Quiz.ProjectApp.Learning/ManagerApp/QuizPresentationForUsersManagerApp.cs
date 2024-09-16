using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.DataServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class QuizPresentationForUsersManagerApp //klasa prezentująca quiz
    {
        private readonly QuestionsRaffleServiceApp _questionsListService;
        private readonly ChoiceServiceApp _choicesService;
        private readonly UsersChoicesManagerApp _usersChoicesManager;
        private readonly ResultsAndUsersPointsManagerApp _resultsManager;
        private readonly UsersExitManagerApp _exitManager;
        private readonly int _questionNumber;

        public QuizPresentationForUsersManagerApp(
            QuestionsRaffleServiceApp questionsListService,
            ChoiceServiceApp choicesService,
            UsersChoicesManagerApp usersChoicesManager,
            ResultsAndUsersPointsManagerApp resultsManager,
            UsersExitManagerApp exitManager)
        {
            _questionsListService = questionsListService ?? throw new ArgumentNullException(nameof(questionsListService));
            _choicesService = choicesService ?? throw new ArgumentNullException(nameof(choicesService));
            _usersChoicesManager = usersChoicesManager ?? throw new ArgumentNullException(nameof(usersChoicesManager));
            _resultsManager = resultsManager ?? throw new ArgumentNullException(nameof(resultsManager));
            _exitManager = exitManager ?? throw new ArgumentNullException(nameof(exitManager));
        }

        public void PresentAQuiz() 
        {
            Console.WriteLine("Rozpoczynamy quiz..."); // Debug
            List<Question> randomQuestions = _questionsListService.GetRandomQuestions();

            if (randomQuestions.Count == 0)
            {
                Console.WriteLine("Brak pytań w quizie."); // Debug
                return;
            }


            HashSet<int> shownQuestions = new HashSet<int>();
            int displayNumber = 1;
            Random random = new Random(); 

            while (shownQuestions.Count < randomQuestions.Count)
            {
                int nextQuestionIndex;
                do
                {
                    nextQuestionIndex = random.Next(randomQuestions.Count);
                } while (shownQuestions.Contains(nextQuestionIndex));

                shownQuestions.Add(nextQuestionIndex);

                var question = randomQuestions[nextQuestionIndex];
                var choices = _choicesService.GetChoicesForQuestion(question.QuestionNumber.ToString());
                Console.WriteLine($"Pytanie {displayNumber}: {question.QuestionContent}");

                foreach (var choice in choices)
                {
                    Console.WriteLine($"{choice.OptionLetter}: {choice.ChoiceContent}");
                }

                // Obsługa wyboru użytkownika
                char userChoice = _usersChoicesManager.GetUserChoice();

                // Weryfikacja odpowiedzi
                bool isCorrect = _resultsManager.VerifyAnswer(question.QuestionContent, userChoice);
                _resultsManager.DisplayResult(isCorrect);

                // Sprawdź, czy użytkownik chce zakończyć quiz
                if (_exitManager.CheckForExit())
                {
                    Console.WriteLine("Zakończenie quizu na żądanie użytkownika."); // Debug
                    break;
                }

                displayNumber++;
                Console.WriteLine();
            }
            // Wyświetl wynik końcowy
            _resultsManager.DisplayFinalScore();
        }
    }
}

