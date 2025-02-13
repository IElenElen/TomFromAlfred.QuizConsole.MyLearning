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
                new Choice(11, 'A', "Jesień", true),
                new Choice(11, 'B', "Zima", true),
                new Choice(11, 'C', "Wiosna", true),
                new Choice(12, 'A', "Kraków", true),
                new Choice(12, 'B', "Warszawa", true),
                new Choice(12, 'C', "Wrocław", true),
                new Choice(13, 'A', "Kilimandżaro", true),
                new Choice(13, 'B', "Mount Everest", true),
                new Choice(13, 'C', "K2", true)
            };
        }

        // Dodanie nowego wyboru
        public void Add(Choice choice)
        {
            if (choice == null)
            {
                Console.WriteLine("Nie można dodać pustej opcji.");
                return;
            }

            if (_choices.Any(c => c.ChoiceId == choice.ChoiceId && c.ChoiceLetter == choice.ChoiceLetter))
            {
                Console.WriteLine($"Opcja {choice.ChoiceLetter} dla Id {choice.ChoiceId} już istnieje.");
                return;
            }

            _choices.Add(choice);
        }

        // Usunięcie wyboru
        public void Delete(Choice choice)
        {
            if (choice == null)
            {
                Console.WriteLine("Nie można usunąć pustego wyboru.");
                return;
            }

            if (!_choices.Contains(choice))
            {
                Console.WriteLine($"Opcja {choice.ChoiceLetter} dla Id {choice.ChoiceId} nie istnieje.");
                return;
            }

            _choices.Remove(choice);
        }

        // Pobranie wszystkich wyborów
        public IEnumerable<Choice> GetAllActive() 
        {
            Console.WriteLine($"Liczba wyborów w _choices: {_choices?.Count ?? 0}");

            foreach (var choice in _choices ?? new List<Choice>())
            {
                Console.WriteLine($"ChoiceId: {choice.ChoiceId}, IsActive: {choice.IsActive}");
            }

            return (_choices ?? new List<Choice>())
                   .Where(choice => choice.IsActive) // Tylko aktywne
                   .AsEnumerable();
        }

        public IEnumerable<Choice> GetChoicesForQuestion(int questionId) // Filtrowanie odpowiedzi na podstawie id pytania
        {
            return _choices.Where(c => c.ChoiceId == questionId);
        }

        public void Clear()
        {
            _choices.Clear();
        }

        // Aktualizacja istniejącego wyboru - zmiana treści wyboru
        public void Update(Choice updatedChoice)
        {
            if (updatedChoice == null)
            {
                Console.WriteLine("Nie można zaktualizować pustego wyboru.");
                return;
            }

            var existingChoice = _choices.FirstOrDefault(c => c.ChoiceId == updatedChoice.ChoiceId && c.ChoiceLetter == 'A');
            if (existingChoice == null)
            {
                Console.WriteLine($"Nie znaleziono wyboru do aktualizacji: Id {updatedChoice.ChoiceId}");
                return;
            }

            // Aktualizuję wybór
            existingChoice.ChoiceLetter = updatedChoice.ChoiceLetter;
            existingChoice.ChoiceContent = updatedChoice.ChoiceContent;
        }
    }
}
