using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class UsersChoicesManagerApp // klasa zarządza interakcjami z użytkownikiem //tę klasę z metodami zostawiłabym już jako managera...
    {
        private readonly IUserInputReader _inputReader; //na potrzeby testu - pole przechowujące obiekt implementujący interfejs IUserInputReader, używany do odczytywania danych wejściowych od użytkownika

        public UsersChoicesManagerApp(IUserInputReader inputReader)
        {
            _inputReader = inputReader; // konstruktor klasy UsersManagerApp, który inicjalizuje pole _inputReader za pomocą przekazanego obiektu implementującego IUserInputReader
        }

        public char GetUserChoice() //zostawiam ten styl kodu, metoda GetUserChoice pobiera wybór użytkownika i waliduje go
        {
            Console.WriteLine();
            Console.Write("Twój wybór (wpisz a, b lub c): ");  // wyświetlenie komunikatu z prośbą o dokonanie wyboru
            char userChoice = char.ToLower(_inputReader.ReadKey().KeyChar); // odczytanie znaku wprowadzonego przez użytkownika i konwersja do małej litery

            // Dodatkowa walidacja wyboru użytkownika
            while (userChoice != 'a' && userChoice != 'b' && userChoice != 'c')
            {
                Console.WriteLine();
                Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                Console.WriteLine();
                Console.Write("Twój wybór (wpisz a, b lub c): ");
                userChoice = char.ToLower(_inputReader.ReadKey().KeyChar); //ponowne odczytanie znaku
            }
            Console.WriteLine(); // nowa linia po wprowadzeniu wyboru
            return userChoice;  // zwrócenie prawidłowego wyboru użytkownika.
        }
    }
    // Interfejs IUserInputReader definiuje metodę do odczytywania znaku kluczowego z konsoli
    public interface IUserInputReader //dodany interfejs dla testu
    {
        ConsoleKeyInfo ReadKey(); // metoda ReadKey zwraca obiekt ConsoleKeyInfo reprezentujący klucz wprowadzony przez użytkownika
    }
}
    
