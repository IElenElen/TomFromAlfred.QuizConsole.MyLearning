using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForService;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.EntityService
{
    // Kolejne metody posortowanie alfabetycznie

    /*
    Do pytania przypisałam jego zestaw wyboru.
    */

    public class QuestionService : ICrudService<Question>
    {
        private readonly List<Question> _questions = new List<Question>(); // Lista pytań Nie upraszczać

        public int questionId = 0;

        public QuestionService() { } // PUSTY KONSTRUKTOR - na potrzeby testu

        public QuestionService(bool loadDefaults = true)
        {
            if (loadDefaults)
            {
                InitializeQuestions(); // Wczytuje domyślne pytania TYLKO jeśli loadDefaults = true
            }
        }

        private void InitializeQuestions() // Tej metody nie testuję
        {
            _questions.Add(new Question(11, "Co następuje po lecie?"));
            _questions.Add(new Question(12, "Co jest stolicą Polski?"));
            _questions.Add(new Question(13, "Jaki jest najwyższy szczyt świata?"));
        }

        public void Add(Question newQuestion)
        {
            if (newQuestion == null)
            {
                Console.WriteLine("Attempted to add a null question. Skipping.");
                return; // Nie dodawaj null
            }

            if (_questions.Any(q => q.QuestionId == newQuestion.QuestionId))
            {
                return; // Nie dodawaj duplikatu
            }

            _questions.Add(newQuestion);
            Console.WriteLine($"Added question: {newQuestion.QuestionId} - {newQuestion.QuestionContent}");
        }

        public void Delete(Question entity)
        {
            if (entity == null)
            {
                Console.WriteLine("Nie można usunąć pustego pytania.");
                return;
            }

            var questionToRemove = _questions.FirstOrDefault(q => q.QuestionId == entity.QuestionId);

            if (questionToRemove != null)
            {
                _questions.Remove(questionToRemove);
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
            if (entity == null)
            {
                Console.WriteLine("Nie można zaktualizować pustego pytania.");
                return;
            }
            var question = _questions.FirstOrDefault(q => q.QuestionId == entity.QuestionId);

            if (question != null)
            {
                question.QuestionContent = entity.QuestionContent;
                Console.WriteLine($"Zaktualizowano pytanie o Id {entity.QuestionId}");
            }

            else
            {
                Console.WriteLine($"Nie znaleziono pytania o Id {entity.QuestionId}, aktualizacja nie powiodła się.");
            }
        }
    }
}
