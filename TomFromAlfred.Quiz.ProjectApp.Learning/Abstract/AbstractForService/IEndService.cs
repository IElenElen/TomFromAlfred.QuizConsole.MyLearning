using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForService
{
    public interface IEndService
    {
        bool ShouldEnd(string? userInput);
        string EndQuiz(bool quizCompleted);
    }
}
