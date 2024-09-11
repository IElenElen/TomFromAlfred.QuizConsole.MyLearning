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

    public class ChoiceServiceApp : BaseApp<Choice> 
    {
        private readonly List<Choice> _choices;

        public ChoiceServiceApp()
        {
            _choices = [];
        }

        public void AddChoice(Choice choice)
        {
            if (_choices.Any(c => c.ChoiceId == choice.ChoiceId))
            {
                throw new ArgumentException("Wybór o tym Id już istnieje.");
            }
            _choices.Add(choice);
        }

        public Choice GetChoiceById(int choiceId)
        {
            return _choices.FirstOrDefault(c => c.ChoiceId == choiceId)
                   ?? throw new KeyNotFoundException($"Wybór o podanym Id {choiceId} nie został znaleziony.");
        }

        public List<Choice> GetChoicesForQuestion(string questionContent)
        {
            // Załóżmy, że mamy metodę, która pozwala znaleźć wybory powiązane z pytaniem!!!
            return _choices.Where(c => c.ChoiceContent == questionContent).ToList();
        }

        public void RemoveChoiceById(int choiceId)
        {
            var choice = GetChoiceById(choiceId);
            if (choice != null)
            {
                _choices.Remove(choice);
            }
        }

        public List<Choice> GetAllChoices()
        {
            return _choices;
        }
    }
}

        