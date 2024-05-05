using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Managers;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.Services
{
    public class QuestionsService //kod z poprzedniego QuestionsSet
    {
        public List<Question> Questions { get; set; } = new List<Question>();
        public QuestionsService()
        {
            QuestionManager questionManager = new QuestionManager();
            Questions.AddRange(questionManager.AllQuestions);
        }
    }
}
