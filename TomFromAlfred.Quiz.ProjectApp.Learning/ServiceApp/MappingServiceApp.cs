using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class MappingServiceApp //klasa łączenia pytania z jego wyborami A B C,
                                   //łączenie na podstawie id = dla weryfikacji poprawności
    {
        private EntitySupport _entitySupport { get; }

        public MappingServiceApp(EntitySupport entitySupport = null)
        {
            _entitySupport = entitySupport ?? new EntitySupport();
        }

        public void AddChoiceToQuestion(int questionId, int choiceId)
        {
            Console.WriteLine($"Rozpoczynanie mapowania pytania o Id {questionId} z wyborem o Id {choiceId}...");

            if (_entitySupport.QuestionIdToChoiceId.ContainsKey(questionId))
            {
                Console.WriteLine($"Błąd: Pytanie o Id {questionId} już ma przypisany wybór.");
                throw new ArgumentException($"Pytanie o Id {questionId} już ma przypisany wybór o Id {_entitySupport.QuestionIdToChoiceId[questionId]}.");
            }

            if (!_entitySupport.QuestionIdToChoiceId.ContainsValue(choiceId))
            {
                Console.WriteLine($"Błąd: Nie znaleziono wyboru o Id {choiceId}.");
                throw new ArgumentException($"Nie znaleziono wyboru o Id {choiceId}.");
            }

            _entitySupport.QuestionIdToChoiceId[questionId] = choiceId;
            Console.WriteLine($"Zmapowano QuestionId {questionId} z ChoiceId {choiceId}");
        }

        public int GetChoiceForQuestion(int questionId)
        {
            Console.WriteLine($"Pobieranie wyboru dla pytania o Id {questionId}...");

            if (_entitySupport.QuestionIdToChoiceId.TryGetValue(questionId, out var choiceId))
            {
                Console.WriteLine($"Znaleziono wybór o Id {choiceId} dla pytania o Id {questionId}.");
                return choiceId; // zwraca znaleziony choiceId
            }

            Console.WriteLine($"Błąd: Nie znaleziono wyboru dla pytania o Id {questionId}.");
            throw new KeyNotFoundException($"Nie znaleziono wyboru dla pytania o Id {questionId}.");
        }
    }
}
