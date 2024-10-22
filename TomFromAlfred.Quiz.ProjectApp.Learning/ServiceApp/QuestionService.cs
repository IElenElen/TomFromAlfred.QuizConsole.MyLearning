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

        public void Add(Question entity)
        {
            _questions.Add(entity);
            Console.WriteLine($"Dodano pytanie: {entity.Content}");
        }

        public void Delete(Question entity)
        {
            _questions.Remove(entity);
            Console.WriteLine($"Usunięto pytanie o Id {entity.Id}");
        }

        public IEnumerable<Question> GetAll()
        {
            return _questions;
        }

        public void Update(Question entity)
        {
            var question = _questions.FirstOrDefault(q => q.Id == entity.Id);
            if (question != null)
            {
                question.Content = entity.Content;
                Console.WriteLine($"Zaktualizowano pytanie o Id {entity.Id}");
            }
        }
    }
}
