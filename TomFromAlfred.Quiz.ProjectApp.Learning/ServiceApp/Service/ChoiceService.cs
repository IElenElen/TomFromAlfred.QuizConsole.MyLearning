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
        private readonly List<Choice> _choices; // Lista danych przechowywana wewnętrznie

        public ChoiceService()
        {
            // Inicjalizacja wyborów w konstruktorze
            _choices = new List<Choice>
            {
                new Choice(0, 'A', "Jesień"),
                new Choice(0, 'B', "Zima"),
                new Choice(0, 'C', "Wiosna"),
                new Choice(1, 'A', "Kraków"),
                new Choice(1, 'B', "Warszawa"),
                new Choice(1, 'C', "Wrocław"),
                new Choice(2, 'A', "Kilimandżaro"),
                new Choice(2, 'B', "Mount Everest"),
                new Choice(2, 'C', "K2")
            };
        }

        // Dodanie nowego wyboru
        public void Add(Choice entity)
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
                Console.WriteLine($"Usunięto wybór: ID {entity.ChoiceId}, Opcja {entity.ChoiceLetter}.");
            }
            else
            {
                Console.WriteLine($"Wybór o ID {entity.ChoiceId} i literze {entity.ChoiceLetter} nie istnieje.");
            }
        }

        // Pobranie wszystkich wyborów
        public IEnumerable<Choice> GetAll()
        {
            return _choices.AsEnumerable(); // Zwraca IEnumerable
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
