using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.DataServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using TomFromAlfred.Quiz.Tests;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    //Najmniejsze szczegoły również ważne :-), szczególnie przy szukaniu błędów
    public class Program //zmiana widoczności kolejnych klas
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cofam się w rozwoju...");

            // Poniżej daję info użytkownikowi

            Console.WriteLine("Pytania poniższego quizu są jednokrotnego wyboru. Po zapoznaniu się z treścią pytania naciśnij a, b lub c,");
            Console.WriteLine("wybierając odpowiedź według Ciebie poprawną.");
            Console.WriteLine("Za poprawną odpowiedż otrzymasz jeden punkt, za błędną brak punktu.");
            Console.WriteLine();

            // Inicjalizacja serwisów i menedżerów
            
            var questionServiceApp = new QuestionServiceApp(); 
            var questionsDataService = new QuestionsDataService(questionServiceApp); //przekazanie instancji
            var questionsRaffleService = new QuestionsRaffleServiceApp(questionServiceApp); //używanie tej samej instancji

            var choiceService = new ChoiceServiceApp();
            var mappingServiceApp = new MappingServiceApp();
            var inputReader = new ConsoleInputReaderManagerApp();
            var usersChoicesManager = new UsersChoicesManagerApp(inputReader);

            var contentCorrectSets = new List<ContentCorrectSet>(); 
            var answerVerifierServiceApp = new AnswerVerifierServiceApp(contentCorrectSets);
            var resultsManager = new ResultsAndUsersPointsManagerApp(answerVerifierServiceApp);

            var exitManager = new UsersExitManagerApp();

            var quizManager = new QuizPresentationForUsersManagerApp
            (
                mappingServiceApp,
                questionsRaffleService, 
                choiceService,
                usersChoicesManager,
                resultsManager,
                exitManager
            );

            // wyświetlenie quizu
            quizManager.PresentAQuiz();
        }
    }
}
