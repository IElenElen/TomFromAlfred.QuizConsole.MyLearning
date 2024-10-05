using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class MappingServiceApp //klasa łączenia pytania z jego wyborami A B C,
                                   //tzn. jeden pakiet wyboru o jednym id, ale 3 opcje do wyboru
                                   //łączenie na podstawie id = dla weryfikacji poprawności
        //problem bo choice ma jedno id, ale 3 litery wyboru - nad tym popracować
    {
        private EntitySupport _entitySupport { get; }
        private List<Question> _questions;
        private List<Choice> _choices;

        public MappingServiceApp(EntitySupport entitySupport = null)
        {
            _entitySupport = entitySupport ?? new EntitySupport();
            _questions = new List<Question>(); 
            _choices = new List<Choice>();
        }

        public void AddChoiceToQuestion(int questionId, int choiceId)
        {
            Console.WriteLine($"Rozpoczynanie mapowania pytania o Id {questionId} z wyborem o Id {choiceId}...");

            var choice = _entitySupport.Choices.SingleOrDefault(c => c.ChoiceId == choiceId);
            if (choice == null)
            {
                Console.WriteLine($"Błąd: Nie znaleziono wyboru o Id {choiceId}.");
                throw new ArgumentException($"Nie znaleziono wyboru o Id {choiceId}.");
            }

            if (!_entitySupport.QuestionIdToChoiceIds.ContainsKey(questionId))
            {
                _entitySupport.QuestionIdToChoiceIds[questionId] = new List<int>();
            }
            else if (_entitySupport.QuestionIdToChoiceIds[questionId].Contains(choiceId))
            {
                Console.WriteLine($"Błąd: Pytanie o Id {questionId} już ma przypisany wybór o Id {choiceId}.");
                throw new ArgumentException($"Pytanie o Id {questionId} już ma przypisany wybór o Id {choiceId}.");
            }

            _entitySupport.QuestionIdToChoiceIds[questionId].Add(choiceId);

            var choices = _entitySupport.QuestionIdToChoiceIds[questionId];

            var choiceLetters = choices.Select(cId => _entitySupport.Choices.Single(c => c.ChoiceId == cId).OptionLetter.ToString());

            Console.WriteLine($"Pytanie {questionId} ma przypisane wybory: {string.Join(", ", choiceLetters)}");
        }

        public List<int> GetChoicesForQuestion(int questionId)
        {
            Console.WriteLine($"Pobieranie wyborów dla pytania o Id {questionId}...");

            if (_entitySupport.QuestionIdToChoiceIds.TryGetValue(questionId, out var choiceIds))
            {
                Console.WriteLine($"Znaleziono wybory o Id: {string.Join(", ", choiceIds)} dla pytania o Id {questionId}.");
                return choiceIds; // zwraca listę znalezionych choiceIds
            }

            Console.WriteLine($"Błąd: Nie znaleziono wyborów dla pytania o Id {questionId}.");
            throw new KeyNotFoundException($"Nie znaleziono wyborów dla pytania o Id {questionId}.");
        }
    }
}
