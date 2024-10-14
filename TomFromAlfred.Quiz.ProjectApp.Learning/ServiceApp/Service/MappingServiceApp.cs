using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service
{
    public class MappingServiceApp //klasa łączenia pytania z jego wyborem id i opcjami A B C wraz z treścią

    {
        private readonly ChoiceServiceApp _choiceServiceApp;
        private EntitySupport _entitySupport { get; }
        private List<Question> _questions;
        private List<Choice> _choices;
        private readonly Dictionary<int, List<int>> _questionToChoiceMap = new Dictionary<int, List<int>>();


        public MappingServiceApp(ChoiceServiceApp choiceServiceApp, EntitySupport? entitySupport = null)
        {
            _choiceServiceApp = choiceServiceApp;
            _entitySupport = entitySupport ?? new EntitySupport();
            _questions = new List<Question>();
            _choices = new List<Choice>();
        }

        public void MapQuestionToChoices(int questionId, List<int> choiceIds)
        {
            Console.WriteLine($"Rozpoczynanie mapowania pytania o Id {questionId} z wyborami o Id {string.Join(", ", choiceIds)}...");

            Console.WriteLine($"Istniejące pytania: {string.Join(", ", _entitySupport.Questions.Select(q => q.QuestionId))}");

            var question = _entitySupport.Questions?.SingleOrDefault(q => q.QuestionId == questionId); //nullość
            if (question == null)
            {
                Console.WriteLine($"Błąd: Nieprawidłowe Id pytania {questionId}.");
                throw new ArgumentException($"Nie znaleziono pytania o Id {questionId}.");
            }

            if (!_entitySupport.QuestionIdToChoiceMap.ContainsKey(questionId))
            {
                _entitySupport.QuestionIdToChoiceMap[questionId] = new List<int>();
            }

            foreach (var choiceId in choiceIds)  // iteracja po każdym Id wyboru
            {
                var choice = _entitySupport.Choices?.SingleOrDefault(c => c.ChoiceId == choiceId);
                if (choice == null)
                {
                    Console.WriteLine($"Błąd: Nie znaleziono wyboru o Id {choiceId}.");
                    throw new ArgumentException($"Nie znaleziono wyboru o Id {choiceId}.");
                }

                if (_entitySupport.QuestionIdToChoiceMap[questionId].Contains(choiceId))
                {
                    Console.WriteLine($"Błąd: Pytanie o Id {questionId} już ma przypisany wybór o Id {choiceId}.");
                    throw new ArgumentException($"Pytanie o Id {questionId} już ma przypisany wybór o Id {choiceId}.");
                }

                _entitySupport.QuestionIdToChoiceMap[questionId].Add(choiceId);
                Console.WriteLine($"Przypisano wybór o Id {choiceId} do pytania o Id {questionId}.");
            }

            var choices = _entitySupport.QuestionIdToChoiceMap[questionId];

            var choiceLetters = choices.Select(cId => _entitySupport.Choices?.Single(c => c.ChoiceId == cId).OptionLetter.ToString());

            Console.WriteLine($"Pytanie {questionId} ma przypisane wybory: {string.Join(", ", choiceLetters)}");
        }

        public List<int> GetChoicesForQuestion(int questionId)
        {
            Console.WriteLine($"Pobieranie wyborów dla pytania o Id {questionId}...");

            if (_entitySupport.QuestionIdToChoiceMap.TryGetValue(questionId, out var choiceIds))
            {
                Console.WriteLine($"Znaleziono wybór o Id: {string.Join(", ", choiceIds)} dla pytania o Id {questionId}.");
                return choiceIds; // zwraca listę znalezionych choiceIds
            }

            Console.WriteLine($"Błąd: Nie znaleziono wyborów dla pytania o Id {questionId}.");
            throw new KeyNotFoundException($"Nie znaleziono wyborów dla pytania o Id {questionId}.");
        }
    }
}
