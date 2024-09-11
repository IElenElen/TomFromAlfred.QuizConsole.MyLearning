using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.DataServiceApp
{
    //Klasa dla inicjalizacji danych (pytań) oraz pobierania i zapisywania do plików
    public class QuestionsDataService
    {
        private readonly QuestionServiceApp _questionServiceApp;

        public QuestionsDataService(IEnumerable<Question> allQuestions = null)
        {
            _questionServiceApp = new QuestionServiceApp(allQuestions);
            InitializeQuestions();
        }

        private void InitializeQuestions()
        {
            if (_questionServiceApp.AllQuestions.Count == 0)
            {
                _questionServiceApp.AllQuestions.AddRange(
                [
                    new (0, "Z ilu części składa się powieść Alfreda Szklarskiego?"),
                    new (1, "Jaki tytuł nosi ostatnia część o przygodach Tomka?"),
                    new (2, "Tomek przed pierwszą przygodą mieszka w: "),
                    new (3, "Na kim mści się Tomek pod koniec roku szkolnego?"),
                    new (4, "Ulubiony przedmiot Tomka to: "),
                    new (5, "Jak ma na imię ciocia Tomka?"),
                    new (6, "Pytanie specjalnie do usuwania nr 1. Niech będzie odp A."),
                    new (7, "Pytanie do usuwania nr 2. Odp c."),
                    new (8, "Pytanie też do testu nr 3. Z odp b")
                ]);
            }
        }
        public void SaveToJson(string filePath)
        {

        }

        public void LoadFromJson(string filePath)
        {

        }
    }
}
