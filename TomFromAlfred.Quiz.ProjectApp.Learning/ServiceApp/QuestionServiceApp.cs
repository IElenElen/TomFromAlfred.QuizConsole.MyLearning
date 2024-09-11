using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    //Klasa serwisowa pytań odpowiada za dodawanie, zmiany i usuwanie pytań
    public class QuestionServiceApp : BaseApp<Question>
    {
        public virtual List<Question> AllQuestions { get; private set; }

        public QuestionServiceApp()
        {
            AllQuestions = [];
        }

        public QuestionServiceApp(IEnumerable<Question> initialQuestions)
        {
            AllQuestions = initialQuestions.ToList(); // inicjalizacja z istniejącymi pytaniami
        }

        public void AddQuestion(string questionContent)
        {
            int newQuestionNumber = AllQuestions.Count;
            if (AllQuestions.Any(q => q.QuestionNumber == newQuestionNumber))
            {
                throw new InvalidOperationException("Pytanie o tym numerze już istnieje.");
            }
            AllQuestions.Add(new Question(newQuestionNumber, questionContent));
        }

        public Question? GetQuestionByNumber(int questionNumber)
        {
            return AllQuestions.FirstOrDefault(q => q.QuestionNumber == questionNumber);
        }

        public void RemoveQuestionByNumber(int questionNumber)
        {
            var question = GetQuestionByNumber(questionNumber);
            if (question != null)
            {
                AllQuestions.Remove(question);
                UpdateQuestionNumbers();
                Console.WriteLine("Pytanie zostało usunięte");
            }
            else
            {
                Console.WriteLine("Brak pytania o podanym numerze");
            }
        }

        private void UpdateQuestionNumbers() //aktualizacja numerów pytań
        {
            for (int i = 0; i < AllQuestions.Count; i++)
            {
                AllQuestions[i].QuestionNumber = i;
            }
        }
    }
}


