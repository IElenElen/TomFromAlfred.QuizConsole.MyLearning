using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.EntityService;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.ServiceSupport;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    /*
    Na razie długa przerwa... 

     Projekt: Quiz.

     Funkcjonalności:

     Wyświetlenie zestawów quizu, zestawy losowane. OK

     Numeruję zestaw jako całość, który wyświetla się użytkwnikowi. Czy to dobry pomysł??? TAK

     Odliczanie czasu???  Odpuszczam po kolejnych nieudanych próbach.

     Wynik: zliczanie poprawnych odpowiedzi + podanie procentowe poprawności. 

     Wynik na koniec quizu, użytkownik musi przejść cały quiz, jeśli chce zobaczyć punktację. OK  

     Wyjście z quiz w każdym momencie. OK

     Ustawiłam aktywność - pracuję na aktywnych danych.

     Budowa:
    
     Interfejs jaki? dla serwisu Crud, aby lepiej się testowało - też dla główniejszych serwisów i dla managera
     
     Klasa wspólna: dla pracy na plikach json - klasa serwisowa
     
     Menadżery: dla Quizu i pomocnika.

     Serwisy: podserwis dla Entity, serwisy dla Quizu, Punktacji i Zakończenia oraz dla pomocnika. 

     Testy w xUnit: jednostkowe i integracyjne - otestowanie metod serwisowych. 

     Dodanie [assembly: CollectionBehavior(DisableTestParallelization = true)] przed namespace w klasie testowej: ScoreServiceTests.

     Analiza testów: oblane: 62 / suma: 142 
     */

    public static class Program // Zmiana widoczności kolejnych klas
    {
        public static void Main(string[] args) // Nie upraszczać
        {
            var fileSupportWrapper = new FileSupportWrapper();
            var jsonFileCommon = new JsonCommonUtility(fileSupportWrapper); 

            var questionService = new QuestionService();
            var choiceService = new ChoiceService();
            var correctAnswerService = new CorrectAnswerService(choiceService);

            var quizService = new QuizService(
                questionService,
                choiceService,
                correctAnswerService,
                jsonFileCommon,
                fileSupportWrapper
            );

            var scoreService = new ScoreService();
            var endService = new EndService(scoreService);
            var userInterface = new ConsoleUserInterface();

            var quizManager = new QuizManager(quizService, scoreService, endService, userInterface);
            quizManager.ConductQuiz();
        }
    }
}
