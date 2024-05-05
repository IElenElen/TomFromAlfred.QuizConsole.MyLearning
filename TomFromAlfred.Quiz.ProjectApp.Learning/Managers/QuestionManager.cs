using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.Managers
{
    public class QuestionManager : BaseApp<Question> //kod z poprzedniego QuestionHandling
    {
        public List<Question> AllQuestions { get; set; } = new List<Question>();
        public QuestionManager()
        {
            AllQuestions.Add(new Question(0, "Z ilu części składa się powyższa powieść Alfreda Szklarskiego?"));
            AllQuestions.Add(new Question(1, "Jaki tytuł nosi ostatnia część o przygodach Tomka?"));
            AllQuestions.Add(new Question(2, "Tomek przed pierwszą przygodą mieszka w: "));
            AllQuestions.Add(new Question(3, "Na kim mści się Tomek pod koniec roku szkolnego?"));
            AllQuestions.Add(new Question(4, "Ulubiony przedmiot Tomka to: "));
            AllQuestions.Add(new Question(5, "Jak ma na imię ciocia Tomka?"));
        }
        public void RemoveQuestion(Question questionToRemove) //w planach
        {
            Remove(questionToRemove);
        }
    }
}
