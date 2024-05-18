using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class QuestionsManagerApp // klasa QuestionsManagerApp zarządza listą pytań
    {
        public List<Question> Questions { get; set; } = new List<Question>(); // lista pytań jest reprezentowana jako lista obiektów klasy Question
        public QuestionsManagerApp(QuestionServiceApp questionServiceApp) //konstruktor, przyjmuje jako argument obiekt klasy QuestionServiceApp, obiekt, który zawiera usługę pobierania wszystkich pytań
        {
            Questions.AddRange(questionServiceApp.AllQuestions); // dodanie wszystkich pytań z serwisu do listy pytań zarządzanej przez aplikację
        }
    }
}
