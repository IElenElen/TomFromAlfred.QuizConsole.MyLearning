using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class QuestionsListServiceApp // klasa serwisowej listy zarządza listą pytań
        //czy właściwie potrzebna mi ta klasa??? mogę ją rozbudować właśnie dla losowania pytań
    {
        private readonly QuestionServiceApp _questionServiceApp;
        public List<Question> Questions { get; private set; }
        public QuestionsListServiceApp(QuestionServiceApp questionServiceApp)
        {
            _questionServiceApp = questionServiceApp;
            Questions = _questionServiceApp.AllQuestions.ToList();
        }

        public List<Question> GetRandomQuestions() //metoda losowania pytań
        {
            List<Question> randomQuestions = Questions.OrderBy(q => Guid.NewGuid()).ToList();

            for (int i = 0; i < randomQuestions.Count; i++)
            {
                randomQuestions[i].QuestionNumber = i + 1;
            }
            return randomQuestions;
        }
    }
}
