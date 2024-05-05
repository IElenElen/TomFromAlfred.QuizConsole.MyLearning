using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class QuestionsManagerApp //to kod z serwisu przeniesiony do managera app
    {
        public List<Question> Questions { get; set; } = new List<Question>();
        public QuestionsManagerApp()
        {
            QuestionServiceApp questionServiceApp = new QuestionServiceApp();
            Questions.AddRange(questionServiceApp.AllQuestions);
        }
    }
}
