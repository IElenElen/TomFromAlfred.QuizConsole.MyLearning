using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class UsersManagerApp
    {
        private readonly IUserInputReader _inputReader; //na potrzeby testu

        public UsersManagerApp(IUserInputReader inputReader)
        {
            _inputReader = inputReader;
        }

    public char GetUserChoice() //zostawiam ten styl kodu
        {
            Console.WriteLine();
            Console.Write("Twój wybór (wpisz a, b lub c): ");
            char userChoice = char.ToLower(_inputReader.ReadKey().KeyChar);

            // Dodatkowa walidacja wyboru użytkownika
            while (userChoice != 'a' && userChoice != 'b' && userChoice != 'c')
            {
                Console.WriteLine();
                Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                Console.WriteLine();
                Console.Write("Twój wybór (wpisz a, b lub c): ");
                userChoice = char.ToLower(_inputReader.ReadKey().KeyChar);
            }
            Console.WriteLine(); // nowa linia po wprowadzeniu wyboru
            return userChoice;
        }
    }
    public interface IUserInputReader //dodany interfejs dla testu
    {
        ConsoleKeyInfo ReadKey();
    }
}
    
