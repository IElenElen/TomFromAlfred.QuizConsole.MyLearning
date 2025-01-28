using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    /*
     Projekt: Quiz.

     Funkcjonalności:

     Wyświetlenie zestawów quizu, zestawy losowane. OK

     Numeruję zestaw jako całość, który wyświetla się użytkwnikowi. Czy to dobry pomysł??? TAK

     Odliczanie czasu???  Odpuszczam po kolejnych nieudanych próbach.

     Wynik: zliczanie poprawnych odpowiedzi + podanie procentowe poprawności. 
     Wynik na koniec quizu, użytkownik musi przejść cały quiz, jeśli chce zobaczyć punktację. OK  

     Wyjście z quiz w każdym momencie. OK

     Budowa:

     Interfejs jaki? dla serwisu Crud 
     
     Klasa wspólna: dla pracy na plikach json - klasa serwisowa
     
     Menadżery: dla Quizu i pomocnika.

     Serwisy: podserwis dla Entity, serwisy dla Quizu, Punktacji i Zakończenia oraz dla pomocnika. 

     Testy w xunit: jednostkowe i integracyjne
     */

    public class Program // Zmiana widoczności kolejnych klas
    {
        static void Main(string[] args)
        {
            // Utworzenie instancji wymaganych serwisów
            var questionService = new QuestionService();
            var choiceService = new ChoiceService();
            var correctAnswerService = new CorrectAnswerService();
            var jsonService = new JsonCommonClass(); // Serwis JSON

            // Instancja QuizService
            var quizService = new QuizService(
                questionService,
                choiceService,
                correctAnswerService,
                jsonService
            );

            // Inicjalizacja serwisów pomocniczych
            var scoreService = new ScoreService();
            var endService = new EndService(scoreService);

            // Utworzenie i uruchomienie QuizManager
            var quizManager = new QuizManager(quizService, scoreService, endService);
            quizManager.ConductQuiz();
        }
    }
}
