using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class ChoiceService : ICrudService<Choice>
    {
        private List<Choice> _choices = new List<Choice>(); //lista wyborów
        public int choiceId = 0;

        public ChoiceService() //inicjacja wyborów w konstruktorze
        {
            InitializeChoices();
        }

        private void InitializeChoices()
        {
            _choices.Add(new Choice(1, 'A', "  opcja a"));
            _choices.Add(new Choice(1, 'B', " opcja b"));
            _choices.Add(new Choice(1, 'C', " opcja c  "));
        }
        public void Add(Choice entity)
        {
            _choices.Add(entity);
            Console.WriteLine($"Dodano wybór: {entity.ChoiceId}");
        }

        public void Delete(Choice entity)
        {
            _choices.Remove(entity);
            Console.WriteLine($"Usunięto wybór: {entity.ChoiceId}");
        }

        public IEnumerable<Choice> GetAll()
        {
            return _choices;
        }

        public void Update(Choice entity)
        {
            var choice = _choices.FirstOrDefault(ch => ch.ChoiceId == entity.ChoiceId);
            if (choice != null)
            {
                choice.ChoiceContent = entity.ChoiceContent;
                Console.WriteLine($"Zaktualizowano pytanie o Id {entity.ChoiceId}");
            }
        }

        public void DisplayAllChoices() //metoda dla wyświetlania listy wyborów
        {
            Console.WriteLine("Lista wszystkich wyborów:");
            foreach (var choice in _choices)
            {
                Console.WriteLine($"Wybór {choice.ChoiceId}, {choice.ChoiceLetter}: {choice.ChoiceContent}");

            }
        }
    }
}
