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
            return userInput == "k" || userInput == "K"; 
        }

        public string EndQuiz(bool quizCompleted)
        {
            if (quizCompleted)
            {
                _scoreService.DisplayScoreSummary();
                return "Ukończyłeś / aś Quiz. Dziękujemy za udział!";
            }
            else
            {
                _scoreService.ResetScore();
                return "Quiz został przerwany. Brak punktów.";
            }
        }
    }
}
