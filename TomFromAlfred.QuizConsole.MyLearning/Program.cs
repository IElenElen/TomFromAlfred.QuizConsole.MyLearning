
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
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

     Interfejs jaki?
     
     Klasa wspólna: dla crud klasa serwisowa, dla pracy na plikach json - też klasa serwisowa
     
     Menadżery

     Serwisy: 

     Testy w xunit: jednostkowe i integracyjne
     */

    //15.10.24 Tworzę entity Question.

    public class Program //zmiana widoczności kolejnych klas
    {
        static void Main(string[] args)
        {
            GenericCommonPartCrud<Question> commonPartCrudApp = new GenericCommonPartCrud<Question>();
        }
    }
}
