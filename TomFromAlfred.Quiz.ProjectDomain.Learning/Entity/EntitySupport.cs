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

        private int _questionId = 0; //licznik (id) pytań
        private int _choiceId = 0; //licznik (id) wyborów

        public int AssignQuestionId() //przypisanie id dla entity Question
        {
            _questionId++; //inkrementacja licznika pytań
            Console.WriteLine($"Przypisano nowe QuestionId: {_questionId}"); // śledzenie inkrementacji QuestionId
            return _questionId; //zwracanie nowego QuestionId
        }

        public int AssignChoiceId() //przypisanie id dla entity Choice (analogia dla Question)
        {
            _choiceId++;
            Console.WriteLine($"Przypisano nowe ChoiceId: {_choiceId}"); // śledzenie inkrementacji ChoiceId
            return _choiceId;
        }

        public Dictionary<int, int> QuestionIdToChoiceId { get; set; } = new(); //mapowanie questionId z choiceId
    }
}
