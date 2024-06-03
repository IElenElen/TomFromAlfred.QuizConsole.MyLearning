using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class QuestionsListServiceApp // klasa serwisowej listy???  zarządza listą pytań
        //czy właściwie potrzebna mi ta klasa??? mogę ją rozbudować właśnie dla losowania pytań
    {
        public List<Question> Questions { get; set; } = new List<Question>(); // lista pytań jest reprezentowana jako lista obiektów klasy Question
        public QuestionsListServiceApp(QuestionServiceApp questionServiceApp) //konstruktor, przyjmuje jako argument obiekt klasy QuestionServiceApp, obiekt, który zawiera usługę pobierania wszystkich pytań
        {
            Questions.AddRange(questionServiceApp.AllQuestions); // dodanie wszystkich pytań z serwisu do listy pytań zarządzanej przez aplikację
        }

        public List<Question> GetRandomQuestionsWithUpdatedNumbers(int questionNumber) //nowa metoda losowania pytań
        {
            List<Question> randomQuestions = new List<Question>(Questions); // kopia listy pytań
            randomQuestions = randomQuestions.OrderBy(q => Guid.NewGuid()).ToList();

            for (int i = 0; i < randomQuestions.Count; i++)
            {
                randomQuestions[i].QuestionNumber = i + 1;
            }
            return randomQuestions.Take(questionNumber).ToList();
        }
    }
}
