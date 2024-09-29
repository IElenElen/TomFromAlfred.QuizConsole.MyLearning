using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.DataServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class QuizPresentationForUsersManagerApp //klasa prezentująca quiz
    {
        private readonly MappingServiceApp _mappingServiceApp;
        private readonly QuestionsRaffleServiceApp _questionsListService;
        private readonly ChoiceServiceApp _choicesService;
        private readonly UsersChoicesManagerApp _usersChoicesManager;
        private readonly ResultsAndUsersPointsManagerApp _resultsManager;
        private readonly UsersExitManagerApp _exitManager;
        private readonly int _questionNumber;
        private readonly object _entitySupport;

        public QuizPresentationForUsersManagerApp(
            MappingServiceApp mappingServiceApp,
            QuestionsRaffleServiceApp questionsListService,
            ChoiceServiceApp choicesService,
            UsersChoicesManagerApp usersChoicesManager,
            ResultsAndUsersPointsManagerApp resultsManager,
            UsersExitManagerApp exitManager)
        {
            //tu też coś z mappingu?
            _questionsListService = questionsListService ?? throw new ArgumentNullException(nameof(questionsListService));
            _choicesService = choicesService ?? throw new ArgumentNullException(nameof(choicesService));
            _usersChoicesManager = usersChoicesManager ?? throw new ArgumentNullException(nameof(usersChoicesManager));
            _resultsManager = resultsManager ?? throw new ArgumentNullException(nameof(resultsManager));
            _exitManager = exitManager ?? throw new ArgumentNullException(nameof(exitManager));
        }

        /* Gdy system wyświetla pytania, używa np. metody DisplayQuestions,
           która generuje losowe pytania i ustawia ich numerację od 1 do n,
           tak aby były zrozumiałe dla użytkownika. */

        public void PresentAQuiz()
        {
            Console.WriteLine("Rozpoczynamy quiz..."); // Debug
            List<Question> randomQuestions = _questionsListService.GetRandomQuestionsWithUserNumbering();

            if (randomQuestions.Count == 0)
            {
                Console.WriteLine("Brak pytań w quizie."); // Debug
                return;
            }

            HashSet<int> shownQuestions = new HashSet<int>();
            int displayNumber = 1;
            Random random = new Random();

            
                // Obsługa wyboru użytkownika
                char userChoice = _usersChoicesManager.GetUserChoice();

                Console.WriteLine();
            
            // Wyświetl wynik końcowy
            _resultsManager.DisplayFinalScore();
        }
    }
}

