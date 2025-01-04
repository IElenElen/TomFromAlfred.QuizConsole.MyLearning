using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class EndService
    {
        private readonly ScoreService _scoreService;

        public EndService(ScoreService scoreService)
        {
            _scoreService = scoreService ?? throw new ArgumentNullException(nameof(scoreService));
        }

        public bool ShouldEnd(string userInput)
        {
            // Jeśli użytkownik wpisze 'k', kończę quiz
            return userInput.Trim().ToLower() == "k";
        }

        public void EndQuiz()
        {
            Console.WriteLine("Quiz został przerwany. Dziękujemy za udział!");
            _scoreService.DisplayScoreSummary();
            Environment.Exit(0); 
        }
    }
}
