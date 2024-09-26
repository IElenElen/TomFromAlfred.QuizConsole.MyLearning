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
    /* W Json dane 6-8 są nieaktywne - na to miejsce mogę tutaj ręcznie wprowadzić i poćwiczyć, 
       analogiczna sprawa z choices i correct odp.
     */
    
    public class QuestionsDataService
    {
        private readonly QuestionServiceApp _questionServiceApp;
        private QuestionServiceApp questionServiceApp;

        public QuestionsDataService(QuestionServiceApp questionServiceApp, Question question)
        {
            _questionServiceApp = questionServiceApp ?? throw new ArgumentNullException(nameof(questionServiceApp));

            // Sprawdzenie, czy dane pytanie już istnieje w systemie
            if (_questionServiceApp.AllQuestions.Any(q => q.QuestionContent == question.QuestionContent))
            {
                throw new InvalidOperationException("Takie pytanie już istnieje.");
            }

            InitializeQuestions();
        }

        public QuestionsDataService(QuestionServiceApp questionServiceApp)
        {
            this.questionServiceApp = questionServiceApp;
        }

        private void InitializeQuestions()
        {
            _questionServiceApp.AllQuestions ??= new List<Question>();

            if (!_questionServiceApp.AllQuestions.Any())
            {
                _questionServiceApp.AllQuestions.AddRange(new List<Question>
                {
                    new(6, 6, "Pytanie specjalnie do usuwania nr 1. Niech będzie odp A."), //nr, id, treść pytania
                    new(7, 7, "Pytanie do usuwania nr 2. Odp C."),
                    new(8, 8, "Pytanie też do testu nr 3. Z odp B.")
                });
            }
        }

        public IEnumerable<Question> GetAllQuestions()
        {
            return _questionServiceApp.GetAllQuestions();
        }

        public void SaveToJson(string filePath) //do pliku dopisać id dla question
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Ścieżka pliku nie może być pusta.", nameof(filePath));
            }

            var activeQuestions = _questionServiceApp.AllQuestions
                                  .Where(q => q.IsActive)
                                  .ToList();

            if (!activeQuestions.Any())
            {
                Console.WriteLine("Brak aktywnych pytań do zapisania.");
                return;
            }

            string json = JsonConvert.SerializeObject(activeQuestions, Formatting.Indented);

            try
            {
                string directory = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(filePath, json);
                Console.WriteLine("Pytania zostały zapisane do pliku.");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Błąd zapisu pliku: {ioEx.Message}");
            }
        }

        public void LoadFromJson(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Ścieżka pliku nie może być pusta.", nameof(filePath));
            }

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

                foreach (var question in questions)
                {
                    if (question == null || string.IsNullOrEmpty(question.QuestionContent))
                    {
                        Console.WriteLine("Pytanie jest nieprawidłowe, pomijam.");
                        continue; 
                    }

                    if (_questionServiceApp.AllQuestions.Any(q => q.QuestionContent == question.QuestionContent))
                    {
                        Console.WriteLine("Pytanie już istnieje, pomijam.");
                        continue; 
                    }

                    question.IsActive = true;
                    _questionServiceApp.AllQuestions.Add(question);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"Numer pytania: {question.QuestionNumber}");
                    sb.AppendLine($"Treść pytania: {question.QuestionContent}");
                    Console.WriteLine(sb.ToString());
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
