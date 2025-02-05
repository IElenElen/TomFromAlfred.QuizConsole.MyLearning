using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class EndService // Klasa dla zakończenia Quizu w każdym momencie
    {
        private readonly ScoreService _scoreService;

        public EndService(ScoreService scoreService)
        {
            _scoreService = scoreService ?? throw new ArgumentNullException(nameof(scoreService));
        }

        public bool ShouldEnd(string? userInput)
        {
            return userInput?.Trim().ToLower() == "k";
        }

        public void EndQuiz(bool quizCompleted)
        {
            if (quizCompleted)
            {
                Console.WriteLine("Ukończyłeś / aś Quiz. Dziękujemy za udział!");
                _scoreService.DisplayScoreSummary();
            }
            else
            {
                Console.WriteLine("Quiz został przerwany. Brak punktów.");
                _scoreService.ResetScore();
            }
            // Zastąpienie Exit(0) metodą, która nic nie robi 
            // Można tutaj użyć no-op metody:
            NoOpMethod();

            // Natychmiastowe zakończenie aplikacji
            //Environment.Exit(0);
        }

        private void NoOpMethod()
        {
            // Pusta metoda, która nie robi nic
        }
    }
}
