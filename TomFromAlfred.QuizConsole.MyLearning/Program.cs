﻿
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    /*
     Projekt: Quiz.

     Funkcjonalności:

     Wyświetlenie zestawów quizu, po kolei. Najpierw nr 1, czekanie na odpowiedź, weryfikacja odpowiedzi, potem zestaw nr 2 itd.

     Numeruję zestaw jako całość, który wyświetla się użytkwnikowi. Czy to dobry pomysł???

     Odliczanie czasu??? Może się nie udać...

     Wynik: zliczanie poprawnych odpowiedzi + podanie procentowe poprawności. Wynik na koniec quizu.

     Losowanie zestawów?

     Wyjście z quiz w każdym momencie.

     Budowa:

     Interfejs jaki? dla serwisu Crud 
     
     Klasa wspólna: dla pracy na plikach json - klasa serwisowa
     
     Menadżery

     Serwisy: 

     Testy w xunit: jednostkowe i integracyjne
     */

    public class Program //zmiana widoczności kolejnych klas
    {
        static void Main(string[] args)
        {
            QuestionService questionService = new QuestionService(); //inicjalizacja nowego obiektu 
            questionService.DisplayAllQuestions(); //wywołuję metodę Display, dla wyświetlenia przykładowych pytań
            ChoiceService choiceService = new ChoiceService();
            choiceService.DisplayAllChoices();
        }
    }
}
