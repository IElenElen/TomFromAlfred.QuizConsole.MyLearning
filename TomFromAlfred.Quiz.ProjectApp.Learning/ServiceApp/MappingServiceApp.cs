using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class MappingServiceApp
    {
        private EntitySupport _entitySupport { get; }

        public MappingServiceApp(EntitySupport entitySupport)
        {
            _entitySupport = entitySupport;
        }

        public MappingServiceApp()
        {
        }

        public void AddChoiceToQuestion(int questionId, int choiceId)
        {
            if (_entitySupport.QuestionIdToChoiceId.ContainsKey(questionId))
            {
                throw new ArgumentException($"Pytanie o Id {questionId} już ma przypisany wybór o Id {_entitySupport.QuestionIdToChoiceId[questionId]}.");
            }

            _entitySupport.QuestionIdToChoiceId[questionId] = choiceId;
        }

        public int GetChoiceForQuestion(int questionId)
        {
            if (_entitySupport.QuestionIdToChoiceId.TryGetValue(questionId, out var choiceId))
            {
                return choiceId; // zwraca znaleziony choiceId
            }

            throw new KeyNotFoundException($"Nie znaleziono wyboru dla pytania o ID {questionId}.");
        }
    }
}
