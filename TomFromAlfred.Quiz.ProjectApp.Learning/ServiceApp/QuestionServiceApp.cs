using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class QuestionServiceApp : BaseApp<Question> //ten kod to poprawny serwis
    {
        private int? questionNumber;

        public List<Question> AllQuestions { get; set; } = new List<Question>();
        public QuestionServiceApp()
        {
            AllQuestions.Add(new Question(0, "Z ilu części składa się powyższa powieść Alfreda Szklarskiego?"));
            AllQuestions.Add(new Question(1, "Jaki tytuł nosi ostatnia część o przygodach Tomka?"));
            AllQuestions.Add(new Question(2, "Tomek przed pierwszą przygodą mieszka w: "));
            AllQuestions.Add(new Question(3, "Na kim mści się Tomek pod koniec roku szkolnego?"));
            AllQuestions.Add(new Question(4, "Ulubiony przedmiot Tomka to: "));
            AllQuestions.Add(new Question(5, "Jak ma na imię ciocia Tomka?"));
            AllQuestions.Add(new Question(6, "Pytanie specjalnie do usuwania nr 1. Niech będzie odp A."));
            AllQuestions.Add(new Question(7, "Pytanie do usuwania nr 2. Odp c."));
            AllQuestions.Add(new Question(8, "Pytanie też do testu nr 3. Z odp b"));
        }
        public void GetQuestionByNumber(List<Question> AllQuestions, Question questionNumber)
        {
            for (int i = 0; i < AllQuestions.Count; i++)
            {
                if (AllQuestions[i].Equals(questionNumber))
                {
                    Remove(questionNumber);
                    return;
                }
            }
            Console.WriteLine("Brak pytania do wyświetlenia");
        }
    }
}
