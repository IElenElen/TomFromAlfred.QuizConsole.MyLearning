using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service
{
    // Kolejne metody posortowanie alfabetycznie

    /*
    Do pytania przypisałam jego zestaw wyboru.
    */

    public class QuestionService : ICrudService<Question>
    {
        private List<Question> _questions = new List<Question>(); // Lista pytań
        public int questionId = 0;

        public QuestionService() // Inicjacja pytań w konstruktorze
        {
            InitializeQuestions();
        }

        private void InitializeQuestions() // Tej metody nie testuję
        {
            _questions.Add(new Question(11, "Co następuje po lecie?"));
            _questions.Add(new Question(12, "Co jest stolicą Polski?"));
            _questions.Add(new Question(13, "Jaki jest najwyższy szczyt świata?"));
        }

        public void Add(Question entity)
        {
            if (entity == null)
            {
                Console.WriteLine("Nie można dodać pustego pytania.");
                return;
            }

            _questions.Add(entity);
        }

        public void Delete(Question entity)
        {
            if (entity == null)
            {
                Console.WriteLine("Nie można usunąć pustego pytania.");
                return;
            }

            if (_questions.Remove(entity))
            {
                Console.WriteLine($"Usunięto pytanie: {entity.QuestionId}");
            }
            else
            {
                Console.WriteLine($"Nie znaleziono pytania o Id: {entity.QuestionId}, nie można usunąć.");
            }
        }

        public virtual IEnumerable<Question> GetAllActive() // Tej metody nie testuję, ona zawiera listę pytań
        {
            return _questions;
        }

        public void Update(Question entity)
        {
            var question = _questions.FirstOrDefault(q => q.QuestionId == entity.QuestionId);
            if (question != null)
            {
                question.QuestionContent = entity.QuestionContent;
                Console.WriteLine($"Zaktualizowano pytanie o Id {entity.QuestionId}");
            }
        }
    }
}
