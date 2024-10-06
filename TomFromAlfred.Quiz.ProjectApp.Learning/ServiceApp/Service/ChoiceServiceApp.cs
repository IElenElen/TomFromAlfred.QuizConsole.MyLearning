using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.DataServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service
{
    //Menadżery rozmawiają z klientem, serwisy odpowiadają za pamięć i dane.
    //Po interfejsie dziedziczy serwis a nie klasa bazowa!!!

    public class ChoiceServiceApp : BaseApp<Choice>
    {
        private readonly List<Choice> _choices;

        public ChoiceServiceApp(ChoicesDataService choicesDataService)
        {
            _choices = choicesDataService.GetChoices();
        }

        public void AddChoice(Choice choice)
        {
            Console.WriteLine($"Próba dodania wyboru o id: {choice.ChoiceId}.");

            if (_choices.Any(c => c.ChoiceId == choice.ChoiceId))
            {
                Console.WriteLine("Wybór o podanym id istnieje.");
                throw new ArgumentException("Wybór o tym Id już istnieje.");
            }

            if (choice.ChoiceId == -1)
            {
                Console.WriteLine("Dla nowego wyboru przypisano id.");
                var entitySupport = new EntitySupport();
                choice.ChoiceId = entitySupport.AssignChoiceId();
            }

            int systemIndex = _choices.Count; // `_choices` to lista wszystkich wyborów, system numeruje od 0
            choice.ChoiceNumber = systemIndex + 1; // numer dla użytkownika od 1

            _choices.Add(choice);

            UpdateChoiceNumber(choice.ChoiceNumber.GetValueOrDefault());

            Console.WriteLine($"Wybór o numerze {choice.ChoiceNumber} został dodany.");
        }

        public Choice? GetChoiceById(int choiceId)
        {
            Console.WriteLine($"Próba pobrania wyboru o ID: {choiceId}.");
            return _choices.FirstOrDefault(c => c.ChoiceId == choiceId);
        }

        public Choice GetChoiceByNumber(int userChoiceNumber)
        {
            int choiceNumber = userChoiceNumber - 1;
            Console.WriteLine($"Próba pobrania wyboru o numerze: {userChoiceNumber}.");

            Console.WriteLine($"Wybór o numerze {userChoiceNumber} nie został znaleziony.");
            return _choices.FirstOrDefault(c => c.ChoiceNumber == choiceNumber)
                   ?? throw new KeyNotFoundException($"Wybór o podanym Id {choiceNumber} nie został znaleziony.");
        }

        public void RemoveChoiceByNumber(int userChoiceNumber)
        {
            int choiceNumber = userChoiceNumber - 1;
            Console.WriteLine($"Próba usunięcia wyboru o numerze: {userChoiceNumber}.");

            var choice = GetChoiceByNumber(choiceNumber);
            if (choice != null)
            {
                _choices.Remove(choice);
                Console.WriteLine($"Wybór o numerze {userChoiceNumber} został usunięty.");
            }

            else
            {
                Console.WriteLine($"Nie znaleziono wyboru o numerze {userChoiceNumber}.");
            }

            UpdateChoiceNumber(choiceNumber);
        }

        public void UpdateChoiceNumber(int choiceNumber)
        {
            if (_choices.Count == 0)
            {
                Console.WriteLine("Brak wyborów do aktualizacji numerów.");
                return;
            }

            Console.WriteLine("Aktualizacja numerów wyborów...");

            for (int i = 0; i < _choices.Count; i++)
            {
                if (_choices[i].ChoiceNumber == choiceNumber)
                {
                    _choices[i].ChoiceId = i;
                }
            }

            Console.WriteLine("Numery wyborów zostały zaktualizowane.");
        }

        public List<Choice> GetAllChoices()
        {
            Console.WriteLine($"Pobieranie wszystkich wyborów, liczba wyborów: {_choices.Count}.");
            return new List<Choice>(_choices);
        }
    }
}

