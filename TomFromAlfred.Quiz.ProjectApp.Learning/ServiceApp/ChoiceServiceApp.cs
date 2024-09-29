using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    //Menadżery rozmawiają z klientem, serwisy odpowiadają za pamięć i dane.
    //Po interfejsie dziedziczy serwis a nie klasa bazowa!!!

    // Do zrobienia metoda, która pozwala znaleźć wybory powiązane z pytaniem!!!

    public class ChoiceServiceApp : BaseApp<Choice> 
    {
        private readonly List<Choice> _choices = new List<Choice>();

        public void AddChoice(Choice choice)
        {
            int choiceNumber = choice.ChoiceNumber.GetValueOrDefault(0); //jeśli ChoiceNr jest null, system poda 0 jako domyślną wartość
            if (_choices.Any(c => c.ChoiceNumber == choice.ChoiceNumber))
            {
                throw new ArgumentException("Wybór o tym Id już istnieje.");
            }
            _choices.Add(choice);
            UpdateChoiceNumber(choiceNumber); 
            Console.WriteLine($"Wybór o numerze {choice.ChoiceNumber} został dodany.");
        }

        public Choice GetChoiceById(int choiceId)
        {
            return _choices.FirstOrDefault(c => c.ChoiceId == choiceId);
        }

        public Choice GetChoiceByNumber(int userChoiceNumber)
        {
            int choiceNumber = userChoiceNumber - 1;

            return _choices.FirstOrDefault(c => c.ChoiceNumber == choiceNumber)
                   ?? throw new KeyNotFoundException($"Wybór o podanym Id {choiceNumber} nie został znaleziony.");
        }

        public void RemoveChoiceByNumber(int userChoiceNumber)
        {
            int choiceNumber = userChoiceNumber - 1;

            var choice = GetChoiceByNumber(choiceNumber);
            if (choice != null)
            {
                _choices.Remove(choice);
            }

            UpdateChoiceNumber(choiceNumber);
            Console.WriteLine($"Wybór o numerze {choiceNumber} został usunięty.");
        }

        public void UpdateChoiceNumber(int choiceNumber)
        {
            if (_choices.Count == 0)
            {
                Console.WriteLine("Brak wyborów do aktualizacji numerów.");
                return;
            }

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
            return new List<Choice>(_choices);
        }
    }
}

        