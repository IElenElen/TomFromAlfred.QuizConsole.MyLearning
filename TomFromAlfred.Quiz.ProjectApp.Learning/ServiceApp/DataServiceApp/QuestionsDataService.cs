using Newtonsoft.Json;
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

        public QuestionsDataService(IEnumerable<Question>? allQuestions = null)
        {
            // Ustawienie pustej listy, jeśli allQuestions jest null
            _questionServiceApp = new QuestionServiceApp(allQuestions ?? Enumerable.Empty<Question>());
            InitializeQuestions();
        }

        private void InitializeQuestions()
        {
            if (!_questionServiceApp.AllQuestions.Any())
            {
                _questionServiceApp.AllQuestions.AddRange(new List<Question>
                {       
                    new(0, "Z ilu części składa się powieść Alfreda Szklarskiego?"),
                    new(1, "Jaki tytuł nosi ostatnia część o przygodach Tomka?"),
                    new(2, "Tomek przed pierwszą przygodą mieszka w: "),
                    new(3, "Na kim mści się Tomek pod koniec roku szkolnego?"),
                    new(4, "Ulubiony przedmiot Tomka to: "),
                    new(5, "Jak ma na imię ciocia Tomka?"),
                    new(6, "Pytanie specjalnie do usuwania nr 1. Niech będzie odp A."),
                    new(7, "Pytanie do usuwania nr 2. Odp c."),
                    new(8, "Pytanie też do testu nr 3. Z odp b")
                });
            }
        }

        public IEnumerable<Question> GetAllQuestions()
        {
            return _questionServiceApp.GetAllQuestions();
        }

        public void SaveToJson(string filePath)
        {
            var questionsToSave = new List<Question>
            {
                new Question(7, "Pytanie do usuwania nr 2. Odp c."),
                new Question(8, "Pytanie też do testu nr 3. Z odp b")
            };

            string json = JsonConvert.SerializeObject(questionsToSave, Formatting.Indented);

            File.WriteAllText(filePath, json);
        }

        public void LoadFromJson(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Plik JSON nie istnieje.");
                return;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                var questions = JsonConvert.DeserializeObject<List<Question>>(json);

                if (questions == null)
                {
                    Console.WriteLine("Błąd deserializacji JSON: plik JSON jest pusty lub zawiera błędy.");
                    return;
                }

                _questionServiceApp.AllQuestions.Clear();
                _questionServiceApp.AllQuestions.AddRange(questions);

                foreach (var question in questions)
                {
                    Console.WriteLine($"Numer pytania: {question.QuestionNumber}");
                    Console.WriteLine($"Treść pytania: {question.QuestionContent}");
                    Console.WriteLine();
                }
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"Błąd deserializacji JSON: {jsonEx.Message}");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Błąd odczytu pliku: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
            }
        }
    }
}
