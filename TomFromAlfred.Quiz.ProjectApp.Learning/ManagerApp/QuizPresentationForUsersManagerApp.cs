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
            Console.WriteLine("Rozpoczynamy quiz...");
            List<Question> randomQuestions = _questionsListService.GetRandomQuestionsWithUserNumbering();

            if (randomQuestions.Count == 0)
            {
                Console.WriteLine("Brak pytań w quizie.");
                return;
            }

            foreach (var question in randomQuestions)
            {
                // Wyświetlanie treści pytania
                Console.WriteLine($"Pytanie {question.QuestionNumber}: {question.QuestionContent}");

                // Pobieranie wyborów związanych z tym pytaniem
                var choices = _choicesService.GetAllChoices().Where(c => c.ChoiceId == question.QuestionId).ToList();

                foreach (var choice in choices)
                {
                    // Wyświetlanie opcji wyboru (np. A: ..., B: ...)
                    Console.WriteLine($"{choice.OptionLetter}: {choice.ChoiceContent}");
                }

                // Pobieranie wyboru użytkownika
                char userChoice = _usersChoicesManager.GetUserChoice();

                // Weryfikacja odpowiedzi
                bool result = _resultsManager.VerifyAnswer(question.QuestionId, userChoice);
                _resultsManager.DisplayResult(result);

                // Sprawdzanie, czy użytkownik chce zakończyć quiz
                if (_exitManager.CheckForExit())
                {
                    break;
                }
            }

            // Wyświetlenie wyniku końcowego
            _resultsManager.DisplayFinalScore();
        }
    }
}

