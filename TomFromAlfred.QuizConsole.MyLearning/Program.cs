using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;
using TomFromAlfred.Quiz.Tests;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    //Najmniejsze szczegoły również ważne :-), szczególnie przy szukaniu błędów
    public class Program //zmiana widoczności kolejnych klas
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Majowa wersja - bez opcji czasu: wersja gotowa do pracy z modułem 4: Testy.");

            // Poniżej daję info użytkownikowi
            Console.WriteLine("Pytania poniższego quizu są jednokrotnego wyboru. Po zapoznaniu się z treścią pytania naciśnij a, b lub c,");
            Console.WriteLine("wybierając odpowiedź według Ciebie poprawną.");
            Console.WriteLine("Za poprawną odpowiedż otrzymasz jeden punkt, za błędną brak punktu.");
            Console.WriteLine();

            // Inicjalizacja serwisów
            QuestionServiceApp questionServiceApp = new QuestionServiceApp();
            ChoicesArraysServiceApp choicesService = new ChoicesArraysServiceApp();
            AnswerVerifierServiceApp answerVerifierServiceApp = new AnswerVerifierServiceApp();
            QuestionsListServiceApp questionsListService = new QuestionsListServiceApp(questionServiceApp);

            // Inicjalizacja menadżerów
            QuizPresentationForUsersManagerApp quizPresentationManager = new QuizPresentationForUsersManagerApp(questionsListService, choicesService);
            ResultsAndUsersPointsManagerApp resultsManager = new ResultsAndUsersPointsManagerApp(answerVerifierServiceApp);
            UsersChoicesManagerApp userChoicesManager = new UsersChoicesManagerApp(new ConsoleInputReaderManagerApp());
            UsersExitManagerApp usersExitManager = new UsersExitManagerApp();

            // Prezentacja pytań
            quizPresentationManager.PresentAQuiz(); //poprawa nazwy metody

            // Pętla quizu
            foreach (var question in questionsListService.GetRandomQuestions())
            {
                // Pobieranie wyboru użytkownika
                char userChoice = userChoicesManager.GetUserChoice();

                // Weryfikacja odpowiedzi i przyznawanie punktów
                bool result = resultsManager.VerifyAnswer(question.QuestionNumber, userChoice);
                resultsManager.DisplayResult(result);

                // Sprawdzanie, czy użytkownik chce zakończyć quiz
                if (usersExitManager.CheckForExit())
                {
                    break;
                }
            }
            resultsManager.DisplayFinalScore();
        }
    }
}
