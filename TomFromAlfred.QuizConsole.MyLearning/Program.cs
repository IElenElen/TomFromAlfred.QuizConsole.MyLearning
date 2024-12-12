using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    /*
     Projekt: Quiz.

     Funkcjonalności:

     Wyświetlenie zestawów quizu, po kolei. Najpierw nr 1, czekanie na odpowiedź, weryfikacja odpowiedzi, potem zestaw nr 2 itd. OK

     Numeruję zestaw jako całość, który wyświetla się użytkwnikowi. Czy to dobry pomysł??? TAK

     Odliczanie czasu???  Odpuszczam po kolejnych nieudanych próbach

     Wynik: zliczanie poprawnych odpowiedzi + podanie procentowe poprawności. Wynik na koniec quizu. OK 

     Losowanie zestawów? OK

     Wyjście z quiz w każdym momencie. OK

     Budowa:

     Interfejs jaki? dla serwisu Crud 
     
     Klasa wspólna: dla pracy na plikach json - klasa serwisowa
     
     Menadżery: dla Quizu.

     Serwisy: podserwis dla Entity, serwisy dla Quizu, Punktacji i Zakończenia. 

     Testy w xunit: jednostkowe i integracyjne
     */

    public class Program // Zmiana widoczności kolejnych klas
    {
        static void Main(string[] args)
        {
            var questionService = new QuestionService();
            var choiceService = new ChoiceService();
            var correctAnswerService = new CorrectAnswerService();

            // Instancja JsonCommonClass i ścieżki pliku JSON
            var jsonService = new JsonCommonClass();
            string jsonFilePath = "questions.json"; // Ścieżka do pliku JSON

            // Instancja QuizService z kompletnymi argumentami
            var quizService = new QuizService(
                questionService,
                choiceService,
                correctAnswerService,
                jsonService,
                jsonFilePath
            );

            var scoreService = new ScoreService();
            var endService = new EndService();
            var quizManager = new QuizManager(quizService, choiceService, scoreService, endService);

            quizManager.ConductQuiz(); // Uruchomienie QuizManager
        }
    }
}
