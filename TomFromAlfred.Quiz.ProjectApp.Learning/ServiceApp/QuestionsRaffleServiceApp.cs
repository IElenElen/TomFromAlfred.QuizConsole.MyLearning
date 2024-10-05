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
        private static readonly ThreadLocal<Random> _random = new(() => new Random());

        public QuestionsRaffleServiceApp(QuestionServiceApp questionServiceApp)
        {
            _questionServiceApp = questionServiceApp ?? throw new ArgumentNullException(nameof(questionServiceApp));
        }

        /* Losować chcę pytania pod kątem treści, ale ważna jest aktualizacja numerów pytań,
        bo jak usunę dane pytanie to wtedy numeracja musi być odpowiednia do zmian. */

        //Podczasz losowania pytania, system losuje pytania na podstawie ich pozycji w liście, a nie na podstawie Id!!!

        public List<Question> GetRandomQuestionsWithUserNumbering()
        {
            try
            {
                var allQuestions = _questionServiceApp.GetAllQuestions().ToList();
                Console.WriteLine($"Liczba pytań dostępnych do losowania: {allQuestions.Count}");

                if (allQuestions.Count == 0)
                {
                    Console.WriteLine("Brak pytań w QuestionsRaffleServiceApp.");
                    return new List<Question>();
                }

                Console.WriteLine("Aktualizacja numerów pytań przed losowaniem.");
                _questionServiceApp.UpdateQuestionNumbers();


                Console.WriteLine("Tasowanie pytań...");
                List<Question> shuffledQuestions = allQuestions.OrderBy(q => _random.Value.Next()).ToList();
                Console.WriteLine("Pytania zostały przetasowane.");

                for (int i = 0; i < shuffledQuestions.Count; i++)
                {
                    shuffledQuestions[i].QuestionNumber = i + 1; // numeracja od 1 dla użytkownika
                    Console.WriteLine($"Pytanie {shuffledQuestions[i].QuestionContent} ma nowy numer: {shuffledQuestions[i].QuestionNumber}");
                }

                return shuffledQuestions;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania pytań: {ex.Message}");
                return new List<Question>();
            }
        }
    }
}

