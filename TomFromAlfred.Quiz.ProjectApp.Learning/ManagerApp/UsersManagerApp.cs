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
        // Metoda do pobierania odpowiedzi użytkownika
        public char GetUserChoiceWithTimeout(TimeMeasuringServiceApp timeService)
        {
            char userInput;

            do
            {
                Console.WriteLine();
                Console.Write("Twój wybór (wpisz a, b lub c): ");
                userInput = GetUserChoiceOrTimeout(timeService);

                if (userInput != 'a' && userInput != 'b' && userInput != 'c')
                {
                    Console.WriteLine();
                    Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                }
            } while (userInput != 'a' && userInput != 'b' && userInput != 'c');

            Console.WriteLine(); // Nowa linia po wprowadzeniu wyboru
            return userInput;
        }

        // Metoda do pobierania odpowiedzi użytkownika z uwzględnieniem czasu
        private char GetUserChoiceOrTimeout(TimeMeasuringServiceApp timeService)
        {
            var initialTime = DateTime.Now;
            while (DateTime.Now - initialTime < TimeSpan.FromSeconds(TimeMeasuringServiceApp.TIME_PER_QUESTION_IN_FULL_SECONDS))
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).KeyChar;
                    if (key == 'a' || key == 'b' || key == 'c')
                    {
                        Console.WriteLine(); // Nowa linia po wprowadzeniu wyboru
                        return key;
                    }
                }
                // Wyświetlanie aktualnego czasu i pozostawionego czasu w tej samej linii
                Console.Write($"\rCzas pozostały: {TimeMeasuringServiceApp.TIME_PER_QUESTION_IN_FULL_SECONDS - (DateTime.Now - initialTime).Seconds} sekund  ");
            }
            Console.WriteLine(); // Nowa linia po przekroczeniu czasu
            return '\0'; // Zwracamy pusty znak, jeśli użytkownik nie zdążył odpowiedzieć
        }
    }
}
