using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class UsersExitManagerApp
    {
        public bool CheckForExit()
        {
        string? userInputX = Console.ReadLine(); //czyli ten mechanizm do managera?

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
