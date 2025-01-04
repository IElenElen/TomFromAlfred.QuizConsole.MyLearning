using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service
{
    /*
     Tu zamiast inicjalizacji dodaję dane w nowej prywatnej liście do odczytu.
     */
    public class CorrectAnswerService : ICrudService<CorrectAnswer>
    {
        private readonly List<CorrectAnswer> _correctAnswers = new List<CorrectAnswer>
        {
            new CorrectAnswer(11, "Jesień"),           // Poprawna odpowiedź dla pytania 11
            new CorrectAnswer(12, "Warszawa"),
            new CorrectAnswer(13, "Mount Everest")
        };

        public void Add(CorrectAnswer entity)
        {
            _correctAnswers.Add(entity);
            Console.WriteLine($"Dodano poprawną odpowiedź: {entity.CorrectAnswerId}");
        }

        public void Delete(CorrectAnswer entity)
        {
            _correctAnswers.Remove(entity);
            Console.WriteLine($"Usunięto poprawną odpowiedź: {entity.CorrectAnswerId}");
        }

        public IEnumerable<CorrectAnswer> GetAll()
        {
            return _correctAnswers;
        }

        public void Update(CorrectAnswer entity)
        {
            var correctAnswer = _correctAnswers.FirstOrDefault(a => a.CorrectAnswerId == entity.CorrectAnswerId);
            if (correctAnswer != null)
            {
                correctAnswer.CorrectAnswerContent = entity.CorrectAnswerContent;
                Console.WriteLine($"Zaktualizowano poprawną odpowiedź o Id {entity.CorrectAnswerId}");
            }
        }
        public CorrectAnswer GetCorrectAnswerForQuestion(int questionId) // Poprawna odpowiedź dla pytania
        {
            return _correctAnswers.FirstOrDefault(ca => ca.CorrectAnswerId == questionId);
        }
    }
}
