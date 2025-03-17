using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForService
{
    public interface IScoreService
    {
        int Score { get; } 
        int AllActiveQuizSets { get; } 

        void StartNewQuiz(int newActiveQuestions);
        void RestartQuiz(int newActiveQuestions);

        void IncrementScore();

        int GetScore();

        double GetPercentage();

        void ResetScore();

        void DisplayScoreSummary();

        int GetTotalActiveSets();
    }
}
