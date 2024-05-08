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
        public char GetUserChoice() //zostawiam ten styl kodu
        {
            Console.WriteLine();
            Console.Write("Twój wybór (wpisz a, b lub c): ");
            char userChoice = char.ToLower(Console.ReadKey().KeyChar);

            // Dodatkowa walidacja wyboru użytkownika
            while (userChoice != 'a' && userChoice != 'b' && userChoice != 'c')
            {
                Console.WriteLine();
                Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                Console.WriteLine();
                Console.Write("Twój wybór (wpisz a, b lub c): ");
                userChoice = char.ToLower(Console.ReadKey().KeyChar);
            }
            Console.WriteLine(); // Nowa linia po wprowadzeniu wyboru
            return userChoice;
        }
    }
}
    
