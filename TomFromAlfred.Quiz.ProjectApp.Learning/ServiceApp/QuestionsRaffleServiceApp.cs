using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    //Klasa losowania treści pytań
    public class QuestionsRaffleServiceApp
    {
        private readonly QuestionServiceApp _questionServiceApp;
        private readonly Random _random = new();

        public QuestionsRaffleServiceApp(QuestionServiceApp questionServiceApp)
        {
            _questionServiceApp = questionServiceApp ?? throw new ArgumentNullException(nameof(questionServiceApp));
        }

        /* Losować chcę pytania pod kątem treści, ale ważna jest aktualizacja numerów pytań,
        bo jak usunę dane pytanie to wtedy numeracja musi być odpowiednia do zmian. */
        public List<Question> GetRandomQuestions() //pobranie zlosowanej listy pytań
        {
            try
            {
                var allQuestions = _questionServiceApp.GetAllQuestions().ToList();
                Console.WriteLine($"Liczba pytań dostępnych do losowania: {allQuestions.Count}"); // Debug

                if (allQuestions.Count == 0)
                {
                    Console.WriteLine("Brak pytań w QuestionsRaffleServiceApp."); // Debug
                    return new List<Question>(); // zwraca pustą listę, jeśli brak pytań
                }

                _questionServiceApp.UpdateQuestionNumbers();

                var allRandomQuestions = allQuestions.OrderBy(q => _random.Next()).ToList();
                return allRandomQuestions;
            }

            catch (Exception ex)
            {
                // Logowanie wyjątku i zwracanie pustej listy w przypadku błędu
                Console.WriteLine($"Błąd podczas pobierania pytań: {ex.Message}");
                return new List<Question>();
            }
        }
    }
}
