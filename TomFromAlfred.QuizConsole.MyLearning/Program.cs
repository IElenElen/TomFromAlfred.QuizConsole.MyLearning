using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.DataServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using TomFromAlfred.Quiz.Tests;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    /* Konsola założenia: Quiz.
     * Pytania połączone z wyborem za pomocą id, które jest stałe i ukryte dla użytkownika. 
     * Wybór ma m.in. id, do tego trzy opcje A B C wraz z treściami.
     * Quiz jednokrotnego wyboru. 
     * Quiz w zwykłej kolejności - losowanie mi na razie nie wychodzi.
     * Entity: Question - budowa pojedynczego pytania; Choice - budowa pojedynczego wyboru; CorrectSet - budowa poprawnej odpowiedzi;
     * Nr - jedna z właściwosci pytania i wyboru, pokazuje się użytkownikowi po kolei.
     * EntitySupport - wsparcie dla Entity.
     * Abstrakt - interfejsy (mega ubogie = podstawowe), część wspólna - klasa bazowa (bardzo prosta).
     * Serwisy App: DataServiceApp (m.in. inicjalizacja danych, obsługa plików) oraz zwykłe Service (obsługa pytań, wyborów, weryfikacji odpowiedzi
     * oraz mapowanie pytań z ich wyborami).
     * Menadżery: odczytu, prezentacji quizu, rezultatu i punktacji, wyboru użytkownika, wyjścia.
     * Program - klasa główna - inicjalizuje poszczególne elementy i wyświetla quiz.
     * Testy: Serwisów oraz Integracyjne. */

    //Komentarze w quiz dokończyć i jeszcze raz analiza mojego rozumowania!!!
    
    /* 08.10.24 - dziś nadal sprawdzanie id pytania
     * naprawdę cofam się w rozwoju... :-( */


    //Najmniejsze szczegoły również ważne :-), szczególnie przy szukaniu błędów

    public class Program //zmiana widoczności kolejnych klas
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cofam się w rozwoju..."); //komentarze 

            // Poniżej daję info użytkownikowi

            Console.WriteLine("Pytania poniższego quizu są jednokrotnego wyboru. Po zapoznaniu się z treścią pytania naciśnij a, b lub c,");
            Console.WriteLine("wybierając odpowiedź według Ciebie poprawną.");
            Console.WriteLine("Za poprawną odpowiedż otrzymasz jeden punkt, za błędną brak punktu.");
            Console.WriteLine();

            // Inicjalizacja serwisów i menedżerów
            Console.WriteLine("Inicjalizacja serwisów...");

            var entitySupport = new EntitySupport();

            IEnumerable<Question>? questions = null;
            var questionServiceApp = new QuestionServiceApp(entitySupport, questions); 
            var questionsDataService = new QuestionsDataService(questionServiceApp); //przekazanie instancji

            var choicesDataService = new ChoicesDataService();
            var choiceService = new ChoiceServiceApp(choicesDataService);

            var mappingServiceApp = new MappingServiceApp(choiceService, entitySupport);

            var inputReader = new ConsoleInputReaderManagerApp();
            var usersChoicesManager = new UsersChoicesManagerApp(inputReader);

            var contentCorrectSets = new List<ContentCorrectSet>(); 
            var answerVerifierServiceApp = new AnswerVerifierServiceApp(contentCorrectSets);
            var resultsManager = new ResultsAndUsersPointsManagerApp(answerVerifierServiceApp);

            var exitManager = new UsersExitManagerApp();

            var quizManager = new QuizPresentationForUsersManagerApp
            (
                mappingServiceApp,
                choiceService,
                usersChoicesManager,
                resultsManager,
                exitManager,
                entitySupport
            );

            // wyświetlenie quizu
            Console.WriteLine("Rozpoczynamy quiz...");
            int questionId = 0;
            quizManager.PresentAQuiz(questionId);
            Console.WriteLine("Quiz zakończony.");
        }
    }
}
