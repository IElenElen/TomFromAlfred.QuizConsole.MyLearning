using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForService;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.EntityService
{
    public class ChoiceService : ICrudService<Choice>
    {
        private readonly List<Choice> _choices; // Lista danych zakodowanych twardo tzn. dane poza plikiem json

        public ChoiceService(IEnumerable<Choice>? initialChoices = null)
        {
            _choices = initialChoices?.ToList() ?? new List<Choice> // Nie upraszczać
            {
                new Choice(11, 'A', "Jesień", true),        // -II-
                new Choice(11, 'B', "Zima", true),
                new Choice(11, 'C', "Wiosna", true),
                new Choice(12, 'A', "Kraków", true),
                new Choice(12, 'B', "Warszawa", true),
                new Choice(12, 'C', "Wrocław", true),
                new Choice(13, 'A', "Kilimandżaro", true),
                new Choice(13, 'B', "Mount Everest", true),
                new Choice(13, 'C', "K2", true)                // -II-
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
            ArgumentNullException.ThrowIfNull(choice); // Brak akcepracji nulla

            var toRemove = _choices.FirstOrDefault(c =>
                c.ChoiceId == choice.ChoiceId &&
                c.ChoiceLetter == choice.ChoiceLetter);

            if (toRemove == null)
            {
                Console.WriteLine($"Nie znaleziono pasującego wyboru do usunięcia: Id {choice.ChoiceId}, Litera {choice.ChoiceLetter}");
                return;
            }

            _choices.Remove(toRemove);
            Console.WriteLine($"Usunięto wybór: Id {toRemove.ChoiceId}, Litera {toRemove.ChoiceLetter}");
        }

        public void DeleteChoiceById(int choiceId)
        {
            var toRemove = _choices.Where(c => c.ChoiceId == choiceId).ToList();

            if (toRemove.Count == 0)
            {
                Console.WriteLine($"Nie znaleziono żadnych wyborów z Id = {choiceId}");
                return;
            }

            foreach (var choice in toRemove)
            {
                _choices.Remove(choice);
                Console.WriteLine($"Usunięto wybór: Id {choice.ChoiceId}, Litera {choice.ChoiceLetter}");
            }
        }

        // Pobranie wszystkich wyborów
        public IEnumerable<Choice> GetAllActive() 
        {
            Console.WriteLine($"Liczba wyborów w _choices: {_choices?.Count ?? 0}");

            foreach (var choice in _choices ?? new List<Choice>()) // Nie upraszczać
            {
                Console.WriteLine($"ChoiceId: {choice.ChoiceId}, IsActive: {choice.IsActive}");
            }

            return (_choices ?? new List<Choice>()) // Nie upraszczać
                   .Where(choice => choice.IsActive) // Tylko aktywne
                   .AsEnumerable();
        }

        public virtual IEnumerable<Choice> GetChoicesForQuestion(int questionId) // Filtrowanie odpowiedzi na podstawie id pytania
        {
            return _choices.Where(c => c.ChoiceId == questionId);
        }

        // Aktualizacja istniejącego wyboru - zmiana treści wyboru
        public void Update(Choice updatedChoice)
        {
            if (updatedChoice == null)
            {
                Console.WriteLine("Nie można zaktualizować pustego wyboru.");
                return;
            }

            var existingChoice = _choices.FirstOrDefault(c => c.ChoiceId == updatedChoice.ChoiceId);

            if (existingChoice == null)
            {
                Console.WriteLine($"Nie znaleziono wyboru do aktualizacji: Id {updatedChoice.ChoiceId}");
                return;
            }

            if (existingChoice.ChoiceContent == updatedChoice.ChoiceContent)
            {
                Console.WriteLine($"Brak zmian: treść dla Id {updatedChoice.ChoiceId} jest już taka sama.");
                return;
            }

            // Tworzę nową instancję z nową treścią
            var updated = new Choice
            {
                ChoiceId = existingChoice.ChoiceId,
                ChoiceLetter = existingChoice.ChoiceLetter,
                ChoiceContent = updatedChoice.ChoiceContent,
                IsActive = existingChoice.IsActive
            };

            // Wymieniam stary obiekt na nowy
            _choices.Remove(existingChoice);
            _choices.Add(updated);

            Console.WriteLine($"Zaktualizowano wybór: Id {updated.ChoiceId}, Treść: {updated.ChoiceContent}");
        }

        // Aktualizacja litery wyboru
        public void UpdateChoiceLetter(int choiceId, char newLetter) 
        {
            var existingChoice = _choices.FirstOrDefault(c => c.ChoiceId == choiceId);

            if (existingChoice == null)
            {
                Console.WriteLine($"Nie znaleziono wyboru do aktualizacji litery: Id {choiceId}");
                return;
            }

            if (!"ABC".Contains(newLetter))
            {
                Console.WriteLine($"Błąd: Niepoprawna litera '{newLetter}'. Dozwolone: A, B, C.");
                return;
            }

            // Sprawdzam, czy litera już istnieje w tej samej grupie pytań
            bool letterExists = _choices.Any(c => c.ChoiceLetter == newLetter && c.ChoiceId != choiceId);

            if (letterExists)
            {
                Console.WriteLine($"Błąd: Litera '{newLetter}' już istnieje dla innego wyboru!");
                return;
            }

            Console.WriteLine($"Zmieniam literę wyboru: Id {choiceId}, Stara litera: {existingChoice.ChoiceLetter}, Nowa litera: {newLetter}");

            existingChoice.ChoiceLetter = newLetter;
        }
    }
}
