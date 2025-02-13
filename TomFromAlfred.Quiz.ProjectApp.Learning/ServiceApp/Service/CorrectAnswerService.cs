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
     Tu zamiast inicjalizacji dodaję dane w nowym prywatnym słowniku do odczytu.
     */
    public class CorrectAnswerService : ICrudService<CorrectAnswer>

    {
        private readonly ChoiceService _choiceService;

        private readonly Dictionary<int, CorrectAnswer> _correctAnswers = new Dictionary<int, CorrectAnswer>
        {
            { 11, new CorrectAnswer(11, "Jesień", true) },
            { 12, new CorrectAnswer(12, "Warszawa", true) },
            { 13, new CorrectAnswer(13, "Mount Everest", true) }
        };

        public void Add(CorrectAnswer correctAnswer)
        {
            if (correctAnswer == null)
            {
                Console.WriteLine("Nie można dodać pustej odpowiedzi.");
                return;
            }

            if (_correctAnswers.ContainsKey(correctAnswer.CorrectAnswerId))
            {
                Console.WriteLine($"Odpowiedź dla pytania {correctAnswer.CorrectAnswerId} już istnieje.");
                return;
            }

            _correctAnswers[correctAnswer.CorrectAnswerId] = correctAnswer;
        }

        public void Delete(CorrectAnswer entity)
        {
            if (entity == null || !_correctAnswers.ContainsKey(entity.CorrectAnswerId))
            {
                Console.WriteLine($"Nie można usunąć odpowiedzi, bo nie istnieje.");
                return;
            }

            _correctAnswers.Remove(entity.CorrectAnswerId);
            Console.WriteLine($"Usunięto poprawną odpowiedź: {entity.CorrectAnswerId}");
        }

        public string FindCorrectAnswerContent(int questionId, char letter)
        {
            var choice = _choiceService.GetChoicesForQuestion(questionId)
                                       .FirstOrDefault(c => c.ChoiceLetter == letter);
            return choice?.ChoiceContent ?? "Nieznana odpowiedź";
        }

        public IEnumerable<CorrectAnswer> GetAllActive()
        {
            return _correctAnswers.Values.Where(answer => answer.IsActive);
        }

        public void Update(CorrectAnswer entity)
        {
            if (entity == null || !_correctAnswers.ContainsKey(entity.CorrectAnswerId))
            {
                Console.WriteLine($"Nie można zaktualizować odpowiedzi {entity?.CorrectAnswerId ?? 0}, bo nie istnieje.");
                return;
            }

            _correctAnswers[entity.CorrectAnswerId] = entity;
            Console.WriteLine($"Zaktualizowano poprawną odpowiedź o Id {entity.CorrectAnswerId}");
        }

        public CorrectAnswer GetCorrectAnswerForQuestion(int questionId)
        {
            return _correctAnswers.TryGetValue(questionId, out var correctAnswer) ? correctAnswer : null;
        }
    }
}
