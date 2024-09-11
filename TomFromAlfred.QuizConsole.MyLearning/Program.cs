﻿using TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp;
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
            Console.WriteLine("Majowa wersja - bez opcji czasu: wersja gotowa do pracy z modułem 4: Testy.");

            // Poniżej daję info użytkownikowi
            Console.WriteLine("Pytania poniższego quizu są jednokrotnego wyboru. Po zapoznaniu się z treścią pytania naciśnij a, b lub c,");
            Console.WriteLine("wybierając odpowiedź według Ciebie poprawną.");
            Console.WriteLine("Za poprawną odpowiedż otrzymasz jeden punkt, za błędną brak punktu.");
            Console.WriteLine();

            // Inicjalizacja serwisów
            QuestionServiceApp questionServiceApp = new();
            ChoiceServiceApp choicesService = new();
            AnswerVerifierServiceApp answerVerifierServiceApp = new();
            CorrectDataService correctDataService = new();
            QuestionsRaffleServiceApp questionsListService = new(questionServiceApp);

            // Inicjalizacja menadżerów
            QuizPresentationForUsersManagerApp quizPresentationManager = new(questionsListService, choicesService);
            ResultsAndUsersPointsManagerApp resultsManager = new(answerVerifierServiceApp);
            UsersChoicesManagerApp userChoicesManager = new(new ConsoleInputReaderManagerApp());
            UsersExitManagerApp usersExitManager = new();

            // Prezentacja pytań
            quizPresentationManager.PresentAQuiz(); //poprawa nazwy metody

            // Pętla quizu
            foreach (var question in questionsListService.GetRandomQuestions())
            {
                // Pobieranie wyboru użytkownika
                char userChoice = userChoicesManager.GetUserChoice();

                // Weryfikacja odpowiedzi i przyznawanie punktów
                bool result = resultsManager.VerifyAnswer(question.QuestionContent, userChoice);
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
