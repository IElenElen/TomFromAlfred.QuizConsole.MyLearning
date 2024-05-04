using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    public class QuestionsSet
    {
        public List<Question> Questions { get; set; } //Lista pytań

        public QuestionsSet() // Konstruktor inicjujący listę pytań
        {
            Questions = new List<Question> // Inicjalizacja listy pytań przy użyciu statycznych instancji klasy Question
            {
                Question.Question0,
                Question.Question1,
                Question.Question2,
            };
        }
    }
}
