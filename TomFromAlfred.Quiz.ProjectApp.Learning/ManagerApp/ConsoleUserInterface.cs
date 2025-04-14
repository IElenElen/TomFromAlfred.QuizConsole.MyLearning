using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForManager;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class ConsoleUserInterface : IConsoleUserInterface
    {
        public virtual string ReadInputLine()
        {
            string? userInput;

            do
            {
                Console.Write("Wprowadź dane: "); 
                userInput = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(userInput));

            return userInput;
        }

        public virtual void WriteOutputLine(string message) => Console.WriteLine(message);
    }
}
