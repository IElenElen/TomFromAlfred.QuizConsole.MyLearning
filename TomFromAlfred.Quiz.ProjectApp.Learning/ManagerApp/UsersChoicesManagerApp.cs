using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class UsersChoicesManagerApp // klasa zarządza interakcjami z użytkownikiem //tę klasę z metodami zostawiłabym już jako managera...
    {
        private readonly IUserInputReader _inputReader; //pole przechowujące obiekt implementujący interfejs IUserInputReader,
                                                        //używany do odczytywania danych wejściowych od użytkownika

        public UsersChoicesManagerApp(IUserInputReader inputReader)
        {
            _inputReader = inputReader;
        }

        public char GetUserChoice() //zostawiam ten styl kodu, metoda GetUserChoice pobiera wybór użytkownika i waliduje go
        {
            Console.WriteLine();
            Console.Write("Twój wybór (wpisz a, b lub c): ");  // wyświetlenie komunikatu z prośbą o dokonanie wyboru
            ConsoleKeyInfo keyInfo = _inputReader.ReadKey(); 
            Console.WriteLine($"\nNaciśnięty klawisz: {keyInfo.KeyChar}");
            char userChoice = char.ToLower(keyInfo.KeyChar); //konwersja do małej litery

            while (!char.IsLetter(userChoice) || (userChoice != 'a' && userChoice != 'b' && userChoice != 'c')) //walidacja wyboru użytkownika
            {
                Console.WriteLine();
                Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                Console.WriteLine();
                Console.Write("Twój wybór (wpisz a, b lub c): ");
                keyInfo = _inputReader.ReadKey();
                userChoice = char.ToLower(keyInfo.KeyChar); //ponowne odczytanie znaku
                Console.WriteLine($"\nNaciśnięty klawisz: {userChoice}");
            }
            Console.WriteLine(); // nowa linia po wprowadzeniu wyboru
            return userChoice;  // zwrócenie prawidłowego wyboru użytkownika
        }
    }
}
    
