using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class UsersExitManagerApp //skoro wyjście z quizu, to raczej spokojnie ogarnie to manager
    {
        public bool CheckForExit()
        {
            Console.WriteLine("Czy chcesz zakończyć quiz? (wpisz 'k' aby zakończyć, 'n' aby kontynuować)");

            char userInputX = char.ToLower(Console.ReadKey().KeyChar);
            Console.WriteLine();

                    if (userInputX == 'k' || userInputX == 'K')
                    {
                        Console.WriteLine("Quiz został zatrzymany.");
                        return true;
                    }

            return false;
        }
    }
}
