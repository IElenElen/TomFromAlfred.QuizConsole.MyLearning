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

    //"Nieprawidłowe Id pytania." - komunikat w konsoli 02.10.24

    public class QuestionsDataService
    {
        private readonly QuestionServiceApp _questionServiceApp;
        private QuestionServiceApp questionServiceApp;

        public QuestionsDataService(QuestionServiceApp questionServiceApp, Question question)
        {
            if (questionServiceApp == null)
            {
                throw new ArgumentNullException(nameof(questionServiceApp), "QuestionServiceApp nie może być null");
            }

            Console.WriteLine("Inicjalizacja QuestionsDataService z pytaniem.");
            _questionServiceApp = questionServiceApp;

            if (_questionServiceApp.AllQuestions == null)
            {
                Console.WriteLine("Inicjalizacja listy pytań, ponieważ AllQuestions jest null.");
                _questionServiceApp.AllQuestions = new List<Question>();
            }

            if (_questionServiceApp.AllQuestions.Any(q => q.QuestionContent == question.QuestionContent))
            {
                Console.WriteLine("Pytanie już istnieje, zgłoszono wyjątek.");
                throw new InvalidOperationException("Takie pytanie już istnieje.");
            }

            InitializeQuestions();
            Console.WriteLine("Pomyślnie zainicjalizowano pytania.");
        }

        public QuestionsDataService(QuestionServiceApp questionServiceApp)
        {
            if (questionServiceApp == null)
            {
                throw new ArgumentNullException(nameof(questionServiceApp), "QuestionServiceApp nie może być null");
            }

            Console.WriteLine("Inicjalizacja QuestionsDataService bez pytania.");
            _questionServiceApp = questionServiceApp;

            if (_questionServiceApp.AllQuestions == null)
            {
                Console.WriteLine("Inicjalizacja listy pytań, ponieważ AllQuestions jest null.");
                _questionServiceApp.AllQuestions = new List<Question>();
            }

            InitializeQuestions();
        }

        private void InitializeQuestions()
        {
            Console.WriteLine("Inicjalizacja listy pytań...");
            _questionServiceApp.AllQuestions ??= new List<Question>();

            if (!_questionServiceApp.AllQuestions.Any())
            {
                Console.WriteLine("Dodawanie początkowych pytań.");
                _questionServiceApp.AllQuestions.AddRange(new List<Question>
                {
                    new(6, 6, "Pytanie specjalnie do usuwania nr 1. Niech będzie odp A."), //id, nr, treść pytania
                    new(7, 7, "Pytanie do usuwania nr 2. Odp C."),
                    new(8, 8, "Pytanie też do testu nr 3. Z odp B.")
                });
            }
            else
            {
                Console.WriteLine("Pytania już istnieją, inicjalizacja zakończona.");
            }
        }

        public IEnumerable<Question> GetAllQuestions()
        {
            Console.WriteLine("Pobieranie wszystkich pytań...");
            var questions = _questionServiceApp.GetAllQuestions();

            if (!questions.Any())
            {
                Console.WriteLine("Brak dostępnych pytań.");
                return questions; //system zwraca pustą listę
            }

            Console.WriteLine($"Oto lista wszystkich pytań: ");

            foreach (var question in questions)
            {
                Console.WriteLine($"Nr: {question.QuestionNumber}, Treść: {question.QuestionContent}");
            }

            return questions;
        }

        public void SaveToJson(string filePath) //do pliku dopisać id dla question
        {
            Console.WriteLine($"Zapis do pliku: {filePath}");

            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Ścieżka pliku jest pusta.");
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

            Console.WriteLine($"Znaleziono {activeQuestions.Count} aktywnych pytań.");

            string json = JsonConvert.SerializeObject(activeQuestions, Formatting.Indented);

            try
            {
                string directory = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(directory))
                {
                    Console.WriteLine("Tworzenie katalogu dla pliku.");
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(filePath, json);
                Console.WriteLine("Pytania zostały zapisane do pliku.");
            }

            catch (UnauthorizedAccessException uaEx)
            {
                Console.WriteLine($"Brak uprawnień do zapisu pliku: {uaEx.Message}");
            }
            catch (DirectoryNotFoundException dnEx)
            {
                Console.WriteLine($"Nie znaleziono katalogu: {dnEx.Message}");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Błąd zapisu pliku: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }

        public void LoadFromJson(string filePath)
        {
            Console.WriteLine($"Wczytywanie pytań z pliku: {filePath}");

            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Ścieżka pliku jest pusta.");
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
                Console.WriteLine("Plik JSON został wczytany.");
                var questions = JsonConvert.DeserializeObject<List<Question>>(json);

                if (questions == null)
                {
                    Console.WriteLine("Błąd deserializacji JSON: plik JSON jest pusty lub zawiera błędy.");
                    return;
                }

                Console.WriteLine($"Liczba pytań w pliku: {questions.Count}");
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

                    Console.WriteLine($"Dodano pytanie: {question.QuestionContent}");

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
            catch (UnauthorizedAccessException uaEx)
            {
                Console.WriteLine($"Brak uprawnień do odczytu pliku: {uaEx.Message}");
            }
            catch (FileNotFoundException fnfEx)
            {
                Console.WriteLine($"Plik nie został znaleziony: {fnfEx.Message}");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Błąd odczytu pliku: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }
        }
    }
}
