using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForService;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service
{
    /*
     Tu zamiast inicjalizacji dodaję dane w nowym prywatnym słowniku do odczytu.
     */
    public class CorrectAnswerService : ICrudService<CorrectAnswer>

    {
        private ChoiceService _choiceService;

        private Dictionary<int, CorrectAnswer> _correctAnswers = new Dictionary<int, CorrectAnswer>
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
            if (entity == null)
            {
                Console.WriteLine("Nie można usunąć pustej odpowiedzi.");
                return;
            }

            else if (_correctAnswers.Remove(entity.CorrectAnswerId))
            {
                Console.WriteLine($"Usunięto poprawną odpowiedź: {entity.CorrectAnswerId}");
            }

            else
            {
                Console.WriteLine($"Nie znaleziono odpowiedzi o Id: {entity.CorrectAnswerId}, nie można usunąć.");
            }
        }

        public string FindCorrectAnswerContent(int questionId, char letter)
        {
            if (_choiceService == null)
            {
                Console.WriteLine("Błąd: ChoiceService nie jest dostępny.");
                return "Nieznana odpowiedź";
            }

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
            if (entity == null)
            {
                Console.WriteLine("Nie można zaktualizować pustej odpowiedzi.");
                return;
            }

            if (!_correctAnswers.TryGetValue(entity.CorrectAnswerId, out var existingAnswer))
            {
                Console.WriteLine($"Nie znaleziono odpowiedzi o Id: {entity.CorrectAnswerId}, aktualizacja nie powiodła się.");
                return;
            }

            if (existingAnswer.CorrectAnswerContent == entity.CorrectAnswerContent)
            {
                Console.WriteLine($"Brak zmian: treść dla Id {entity.CorrectAnswerId} jest już taka sama.");
                return;
            }

            _correctAnswers[entity.CorrectAnswerId] = entity;
            Console.WriteLine($"Zaktualizowano poprawną odpowiedź o Id {entity.CorrectAnswerId}");
        }

        public virtual CorrectAnswer GetCorrectAnswerForQuestion(int questionId)
        {
            return _correctAnswers.TryGetValue(questionId, out var correctAnswer) ? correctAnswer : null;
        }
    }
}
