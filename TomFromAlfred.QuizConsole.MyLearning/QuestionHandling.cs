using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    public class QuestionHandling
    {
        // Klasa QuestionHandling odpowiedzialna będzie za zarządzanie pojedynczymi pytaniami
    
        public List<Question> AllQuestions { get; } = new List<Question>(); //Lista dla wszystkich pytań

        public QuestionHandling() //w konstruktorze dodawane są pojedyncze pytania do listy pytań
        {
            AllQuestions.Add(new Question(0, "Naturalny kolor włosow to: "));
            AllQuestions.Add(new Question(1, "Pora roku następująca po zimie to: "));
            AllQuestions.Add(new Question(2, "Wskaż zioło: "));
        }
    }
}

