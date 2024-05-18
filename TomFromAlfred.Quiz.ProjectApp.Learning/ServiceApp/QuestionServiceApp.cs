using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class QuestionServiceApp : BaseApp<Question> 
    {
        public virtual IEnumerable<Question> AllQuestions { get; } // właściwość reprezentująca wszystkie dostępne pytania

        public QuestionServiceApp(IEnumerable<Question> allQuestions)
        {
            AllQuestions = allQuestions;
        }
        public QuestionServiceApp()
        {
            // Inicjalizacja listy pytań
            AllQuestions = new List<Question>
            {
                new Question(0, "Z ilu części składa się powyższa powieść Alfreda Szklarskiego? "), //nowy format pytań przygotowany pod test
                new Question(1, "Jaki tytuł nosi ostatnia część o przygodach Tomka?"),
                new Question(2, "Tomek przed pierwszą przygodą mieszka w: "),
                new Question(3, "Na kim mści się Tomek pod koniec roku szkolnego?"),
                new Question(4, "Ulubiony przedmiot Tomka to: "),
                new Question(5, "Jak ma na imię ciocia Tomka?"),
                new Question(6, "Pytanie specjalnie do usuwania nr 1. Niech będzie odp A."),
                new Question(7, "Pytanie do usuwania nr 2. Odp c."),
                new Question(8, "Pytanie też do testu nr 3. Z odp b"),
            };
        }

        public Question GetQuestionByNumber(int questionNumber) //wyszukiwanie pytania na podstawie jego numeru
        {
            for (int i = 0; i < ((List<Question>)AllQuestions).Count; i++)
            {
                if (((List<Question>)AllQuestions)[i].QuestionNumber == questionNumber)
                {
                    return ((List<Question>)AllQuestions)[i];
                }
            }
            return null;
        }

        public void RemoveQuestionByNumber(int questionNumber) //metoda problematyczna do kodowania....
        {
            var questionsCopy = AllQuestions.ToList(); // kopia listy pytań

            for (int i = questionsCopy.Count() - 1; i >= 0; i--)
            {
                if (questionsCopy[i].QuestionNumber == questionNumber)
                {
                    questionsCopy.RemoveAt(i); // usuwanie pytania z kopii listy
                    Console.WriteLine("Pytanie zostało usunięte");
                    return;
                }
            }
            Console.WriteLine("Brak pytania o podanym numerze");
        }
    }
}


