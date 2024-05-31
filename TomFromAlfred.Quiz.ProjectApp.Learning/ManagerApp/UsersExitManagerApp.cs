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
        string? userInputX = Console.ReadLine(); 

                    if (userInputX == "k" || userInputX == "K")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Quiz został zatrzymany.");
                    return true;
                    }

            return false;
        }
    }
}
