using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    //kolejne metody posortowanie alfabetycznie
    public class QuestionService : ICrudService<Question>
    {
        private List<Question> _questions = new List<Question>(); //lista pytań
        public int questionId = 0;

        public QuestionService() //inicjacja pytań w konstruktorze
        {
            InitializeQuestions();
        }

        private void InitializeQuestions() 
        {
            _questions.Add(new Question(1, "Co następuje po lecie?"));
            _questions.Add(new Question(2, "Co jest stolicą Polski?"));
            _questions.Add(new Question(3, "Jaki jest najwyższy szczyt świata?"));
        }

        public void Add(Question entity)
        {
            _questions.Add(entity);
            Console.WriteLine($"Dodano pytanie: {entity.QuestionId}");
        }

        public void Delete(Question entity)
        {
            _questions.Remove(entity);
            Console.WriteLine($"Usunięto pytanie: {entity.QuestionId}");
        }

        public IEnumerable<Question> GetAll()
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

        public void DisplayAllQuestions() //metoda dla wyświetlania listy pytań
        {
            Console.WriteLine("Lista wszystkich pytań:");
            foreach (var question in _questions)
            {
                Console.WriteLine($"Pytanie {question.QuestionId}: {question.QuestionContent}");
            }
        }
    }
}
