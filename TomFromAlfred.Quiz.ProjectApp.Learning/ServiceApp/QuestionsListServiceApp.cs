using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class QuestionsListServiceApp // klasa serwisowej listy???  zarządza listą pytań
        //czy właściwie potrzebna mi ta klasa???
    {
        public List<Question> Questions { get; set; } = new List<Question>(); // lista pytań jest reprezentowana jako lista obiektów klasy Question
        public QuestionsListServiceApp(QuestionServiceApp questionServiceApp) //konstruktor, przyjmuje jako argument obiekt klasy QuestionServiceApp, obiekt, który zawiera usługę pobierania wszystkich pytań
        {
            Questions.AddRange(questionServiceApp.AllQuestions); // dodanie wszystkich pytań z serwisu do listy pytań zarządzanej przez aplikację
        }
    }
}
