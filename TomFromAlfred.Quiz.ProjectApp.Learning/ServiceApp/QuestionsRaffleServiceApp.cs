using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    //Klasa losowania treści pytań
    public class QuestionsRaffleServiceApp
    {
        private readonly QuestionServiceApp _questionServiceApp;
        private readonly Random _random = new();

        public QuestionsRaffleServiceApp(QuestionServiceApp questionServiceApp)
        {
            _questionServiceApp = questionServiceApp ?? throw new ArgumentNullException(nameof(questionServiceApp));
        }

        /* Losować chcę pytania pod kątem treści, ale ważna jest aktualizacja numerów pytań,
        bo jak usunę dane pytanie to wtedy numeracja musi być odpowiednia do zmian. */
        public List<Question> GetRandomQuestions()
        {
            var allquestions = _questionServiceApp.GetAllQuestions().ToList();
            if (allquestions.Count == 0)
            {
                Console.WriteLine("Brak pytań w QuestionsRaffleServiceApp."); // Debug
            }
            var allrandomQuestions = allquestions.OrderBy(q => _random.Next()).ToList();
            return allrandomQuestions;
        }
    }
}
