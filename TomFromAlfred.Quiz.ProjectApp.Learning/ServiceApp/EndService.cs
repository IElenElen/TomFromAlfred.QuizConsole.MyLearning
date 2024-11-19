using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class EndService
    {
        public bool ShouldEnd(string userInput)
        {
            // Jeśli użytkownik wpisze 'k', zakończ quiz
            return userInput.Trim().ToLower() == "k";
        }

        public void EndQuiz()
        {
            Console.WriteLine("Quiz zakończony przez użytkownika.");
            Environment.Exit(0); // Kończy program
        }
    }
}
