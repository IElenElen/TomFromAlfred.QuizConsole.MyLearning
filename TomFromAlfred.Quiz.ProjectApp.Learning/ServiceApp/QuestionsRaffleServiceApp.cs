using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    //Klasa losowania treści pytań
    public class QuestionsRaffleServiceApp(QuestionServiceApp questionServiceApp)
    {
        private readonly QuestionServiceApp _questionServiceApp = questionServiceApp ?? throw new ArgumentNullException(nameof(questionServiceApp));
        private readonly Random _random = new();

        /* Losować chcę pytania pod kątem treści, ale ważna jest aktualizacja numerów pytań,
        bo jak usunę dane pytanie to wtedy numeracja musi być odpowiednia do zmian. */
        public List<Question> GetRandomQuestions()
        {
            var questions = _questionServiceApp.AllQuestions.ToList();
            var randomQuestions = questions.OrderBy(q => _random.Next()).ToList();
            return randomQuestions;
        }
    }
}
