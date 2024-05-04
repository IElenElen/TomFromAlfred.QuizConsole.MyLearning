using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    public class Question
    {
        public int QuestionNumber { get; set; } //numer pytania
        public string QuestionContent { get; set; } //treść pytania

        // Statyczne instancje pytań, oznaczone numerem pytania
        public static Question Question0 { get; } = new Question { QuestionNumber = 0, QuestionContent = "Naturalny kolor włosow to: " };
        public static Question Question1 { get; } = new Question { QuestionNumber = 1, QuestionContent = "Pora roku następująca po zimie to: " };
        public static Question Question2 { get; } = new Question { QuestionNumber = 2, QuestionContent = "Wskaż zioło: " };
    }
}
