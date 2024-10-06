using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.DataServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    /* Gdy system wyświetla pytania, używa np. metody DisplayQuestions,
           która generuje losowe pytania i ustawia ich numerację od 1 do n,
           tak aby były zrozumiałe dla użytkownika. */

    public class QuizPresentationForUsersManagerApp //klasa prezentująca quiz
    {
        private readonly MappingServiceApp _mappingServiceApp; //zakres, możliwość działania, typ obiketu, pole
        private readonly QuestionsRaffleServiceApp _questionsListService;
        private readonly ChoiceServiceApp _choicesService;
        private readonly UsersChoicesManagerApp _usersChoicesManager;
        private readonly ResultsAndUsersPointsManagerApp _resultsManager;
        private readonly UsersExitManagerApp _exitManager;
        private readonly int _questionNumber;
        private EntitySupport _entitySupport; 

        public QuizPresentationForUsersManagerApp(
            MappingServiceApp mappingServiceApp, //typ, parametr
            QuestionsRaffleServiceApp questionsListService,
            ChoiceServiceApp choicesService,
            UsersChoicesManagerApp usersChoicesManager,
            ResultsAndUsersPointsManagerApp resultsManager,
            UsersExitManagerApp exitManager,
            EntitySupport entitySupport)
        {
            _mappingServiceApp = mappingServiceApp ?? throw new ArgumentNullException(nameof(mappingServiceApp)); //pole, wartość do pola przypisana
                                                   //?? sprawdzam czy jest null, Argument... = typ wyjątku
            _questionsListService = questionsListService ?? throw new ArgumentNullException(nameof(questionsListService));
            _choicesService = choicesService ?? throw new ArgumentNullException(nameof(choicesService));
            _usersChoicesManager = usersChoicesManager ?? throw new ArgumentNullException(nameof(usersChoicesManager));
            _resultsManager = resultsManager ?? throw new ArgumentNullException(nameof(resultsManager));
            _exitManager = exitManager ?? throw new ArgumentNullException(nameof(exitManager));
            _entitySupport = entitySupport ?? throw new ArgumentNullException(nameof(entitySupport));
        }

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
                Console.WriteLine($"Pytanie {question.QuestionNumber}: {question.QuestionContent}");

                var choices = _choicesService.GetAllChoices().Where(c => c.ChoiceId == question.QuestionId).ToList();
                Console.WriteLine($"Dostępne wybory ({choices.Count}):");

                foreach (var choice in choices)
                {
                    Console.WriteLine($"{choice.OptionLetter}: {choice.ChoiceContent}");
                }

                char userChoice = _usersChoicesManager.GetUserChoice();
                Console.WriteLine($"Użytkownik wybrał opcję: {userChoice}");

                bool result = _resultsManager.VerifyAnswer(question.QuestionId, userChoice);
                _resultsManager.DisplayResult(result);

                if (_exitManager.CheckForExit())
                {
                    Console.WriteLine("Użytkownik zakończył quiz.");
                    break;
                }
            }

            _resultsManager.DisplayFinalScore();
        }
    }
}

