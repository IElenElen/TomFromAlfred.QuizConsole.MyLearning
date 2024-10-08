using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    public class EntitySupport //nie chcę tu statyczności
    {
        public enum OptionLetter     //litery dla wyboru: Choice

        {
            A,
            B,
            C
        }

        private int _currentQuestionId = 0; // przechowuj aktualne Id pytania
        private int _choiceId = 0; //licznik (id) wyborów

        public List<Question>? Questions { get; set; } // lub inna definicja
        public List<Choice>? Choices { get; set; } 

        public virtual int AssignQuestionId() //przypisanie id dla entity Question
        {
            _currentQuestionId++; // zwiększ Id o 1
            Console.WriteLine($"Przypisano nowe QuestionId: {_currentQuestionId}"); // śledzenie inkrementacji QuestionId
            return _currentQuestionId; 
        }

        public int AssignChoiceId() //przypisanie id dla entity Choice 
        {
            _choiceId++;
            Console.WriteLine($"Przypisano nowe ChoiceId: {_choiceId}"); // śledzenie inkrementacji ChoiceId
            return _choiceId;
        }

        public Dictionary<int, List<int>> QuestionIdToChoiceMap { get; } = new Dictionary<int, List<int>>();
    }
}
