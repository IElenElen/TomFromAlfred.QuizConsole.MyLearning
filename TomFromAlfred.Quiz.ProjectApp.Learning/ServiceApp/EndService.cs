using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForService;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class EndService : IEndService
    {
        private readonly IScoreService _scoreService;

        public EndService(IScoreService scoreService) // Nie upraszczać
        {
            _scoreService = scoreService ?? throw new ArgumentNullException(nameof(scoreService));
        }

        public virtual string EndQuiz(bool quizCompleted)
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

        public virtual bool ShouldEnd(string? userInput)
        {
            return userInput == "k" || userInput == "K";
        }
    }
}
