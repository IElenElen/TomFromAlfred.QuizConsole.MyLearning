using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service
{
    public class ChoiceService : ICrudService<Choice>
    {
        private readonly List<Choice> _choices; // Lista danych zakodowanych twardo

        public ChoiceService()
        {
            // Inicjalizacja wyborów w konstruktorze
            _choices = new List<Choice>
            {
                new Choice(11, 'A', "Jesień"),
                new Choice(11, 'B', "Zima"),
                new Choice(11, 'C', "Wiosna"),
                new Choice(12, 'A', "Kraków"),
                new Choice(12, 'B', "Warszawa"),
                new Choice(12, 'C', "Wrocław"),
                new Choice(13, 'A', "Kilimandżaro"),
                new Choice(13, 'B', "Mount Everest"),
                new Choice(13, 'C', "K2")
            };
        }

        // Dodanie nowego wyboru
        public virtual void Add(Choice entity)
        {
            if (_choices.Any(c => c.ChoiceId == entity.ChoiceId && c.ChoiceLetter == entity.ChoiceLetter))
            {
                Console.WriteLine($"Wybór o Id {entity.ChoiceId} i literze {entity.ChoiceLetter} już istnieje.");
                return;
            }

            _choices.Add(entity);
            Console.WriteLine($"Dodano wybór: Id {entity.ChoiceId}, opcja {entity.ChoiceLetter}.");
        }

        // Usunięcie wyboru
        public void Delete(Choice entity)
        {
            var choiceToRemove = _choices.FirstOrDefault(c => c.ChoiceId == entity.ChoiceId && c.ChoiceLetter == entity.ChoiceLetter);
            if (choiceToRemove != null)
            {
                _choices.Remove(choiceToRemove);
                Console.WriteLine($"Usunięto wybór: Id {entity.ChoiceId}, Opcja {entity.ChoiceLetter}.");
            }
            else
            {
                Console.WriteLine($"Wybór o Id {entity.ChoiceId} i literze {entity.ChoiceLetter} nie istnieje.");
            }
        }

        // Pobranie wszystkich wyborów
        public IEnumerable<Choice> GetAll()
        {
            return _choices.AsEnumerable(); // Zwracanie IEnumerable
        }

        public IEnumerable<Choice> GetChoicesForQuestion(int questionId) // Filtrowanie odpowiedzi na podstawie id pytania
        {
            return _choices.Where(c => c.ChoiceId == questionId);
        }

        // Aktualizacja istniejącego wyboru
        public void Update(Choice entity)
        {
            var choiceToUpdate = _choices.FirstOrDefault(c => c.ChoiceId == entity.ChoiceId && c.ChoiceLetter == entity.ChoiceLetter);
            if (choiceToUpdate != null)
            {
                choiceToUpdate.ChoiceContent = entity.ChoiceContent;
                Console.WriteLine($"Zaktualizowano wybór: Id {entity.ChoiceId}, opcja {entity.ChoiceLetter}.");
            }
            else
            {
                Console.WriteLine($"Nie znaleziono wyboru do zaktualizowania: Id {entity.ChoiceId}, opcja {entity.ChoiceLetter}.");
            }
        }
    }
}
