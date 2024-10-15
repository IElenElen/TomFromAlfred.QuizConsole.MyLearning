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
           
        }
    }
}
