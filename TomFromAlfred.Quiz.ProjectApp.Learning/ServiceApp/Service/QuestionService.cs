using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service
{
    //kolejne metody posortowanie alfabetycznie
    /*
    08.11.24 zarówno pytania jak i wybory wyświetlają się po kolei. Zatem teraz nowa klasa QuizSerwis, która łączy te elementy.
    Metoda Dispaly usunięta w Serwisach Pytania i Wyboru - dane pobiera Serwis Quizu za pomocą IEnumerable.
    Teraz do pytania przypisałam jego zestaw wyboru.
    */
    public class QuestionService : ICrudService<Question>
    {
        private List<Question> _questions = new List<Question>(); // lista pytań
        public int questionId = 0;

        public QuestionService() // inicjacja pytań w konstruktorze
        {
            InitializeQuestions();
        }

        private void InitializeQuestions()
        {
            _questions.Add(new Question(0, "Co następuje po lecie?"));
            _questions.Add(new Question(1, "Co jest stolicą Polski?"));
            _questions.Add(new Question(2, "Jaki jest najwyższy szczyt świata?"));
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
    }
}
