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
            if (_choices.Any(c => c.ChoiceId == choice.ChoiceId))
            {
                throw new ArgumentException("Wybór o tym Id już istnieje.");
            }
            _choices.Add(choice);
            UpdateChoiceId(choice.ChoiceId); 
            Console.WriteLine($"Wybór o numerze {choice.ChoiceId} został dodany.");
        }

        public Choice GetChoiceById(int userChoiceId)
        {
            int choiceId = userChoiceId - 1;

            return _choices.FirstOrDefault(c => c.ChoiceId == choiceId)
                   ?? throw new KeyNotFoundException($"Wybór o podanym Id {choiceId} nie został znaleziony.");
        }

        public void RemoveChoiceById(int userChoiceId)
        {
            int choiceId = userChoiceId - 1;

            var choice = GetChoiceById(choiceId);
            if (choice != null)
            {
                _choices.Remove(choice);
            }

            UpdateChoiceId(choiceId);
            Console.WriteLine($"Wybór o numerze {choiceId} został usunięty.");
        }

        public void UpdateChoiceId( int choiceId)
        {
            if (_choices.Count == 0)
            {
                Console.WriteLine("Brak wyborów do aktualizacji numerów.");
                return;
            }

            for (int i = 0; i < _choices.Count; i++)
            {
                if (_choices[i].ChoiceId == choiceId)
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

        