using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service
{
    //Klasa serwisowa pytań odpowiada za dodawanie, zmiany i usuwanie pytań

    public class QuestionServiceApp : BaseApp<Question>
    {
        private readonly EntitySupport _entitySupport;
        private IEnumerable<Question> _questions; 
        public List<Question> AllQuestions { get; set; } = new List<Question>();
        public QuestionServiceApp(EntitySupport entitySupport, IEnumerable<Question>? initialQuestions = null)
        {
            _entitySupport = entitySupport; // użycie istniejącej instancji
            _questions = AllQuestions; 
            AllQuestions = (initialQuestions ?? Enumerable.Empty<Question>()).ToList();
            Console.WriteLine($"Inicjalizowano QuestionServiceApp z {AllQuestions.Count} pytaniami.");
        }

        public void AddQuestion(Question question)
        {
            question.QuestionId = _entitySupport.AssignQuestionId();
            Console.WriteLine($"Dodawanie pytania: {question.QuestionContent} z Id: {question.QuestionId}");


            if (AllQuestions.Any(q => q.QuestionId == question.QuestionId))
            {
                Console.WriteLine($"Pytanie o id '{question.QuestionId}' już istnieje.");
                throw new InvalidOperationException("Takie pytanie już istnieje.");
            }

            int systemIndex = AllQuestions.Count; // zaczynamy od 0, 1, 2...
            question.QuestionNumber = systemIndex + 1; // numerujemy od 1 dla użytkownika

            question.IsActive = true;

            AllQuestions.Add(question);
            _entitySupport.Questions?.Add(question); // dodaj pytanie do EntitySupport

            Console.WriteLine($"Dodano pytanie o id: {question.QuestionId}, o numerze: {question.QuestionNumber} i o treści: {question.QuestionContent}.");
            UpdateQuestionNumbers();
        }

        public List<string> PrintIdForAllQuestions()  //metoda dla sprawdzania id
        {
            if (AllQuestions.Count == 0)
            {
                return new List<string> { "Brak pytań." };
            }

            var questions = _questions; // lista pytań

            var results = new List<string> { "Lista wszystkich pytań:" };

            Console.WriteLine("Lista wszystkich pytań:");

            foreach (var question in questions)
            {
                results.Add($"Pytanie ID: {question.QuestionId}, Numer: {question.QuestionNumber}, Treść: {question.QuestionContent}");
            }

            return results; // zwróć listę wyników
        }

        public Question? GetQuestionByNumber(int userQuestionNumber)
        {
            int questionNumber = userQuestionNumber - 1;
            if (questionNumber < 0 || questionNumber >= AllQuestions.Count)
            {
                Console.WriteLine($"Brak pytania o numerze {userQuestionNumber}.");
                return null;
            }

            Console.WriteLine($"Znaleziono pytanie o numerze {userQuestionNumber}.");
            return AllQuestions[questionNumber];
        }

        public bool RemoveQuestionByNumber(int userQuestionNumber)
        {
            try
            {
                int questionNumber = userQuestionNumber - 1;

                var question = GetQuestionByNumber(userQuestionNumber);

                if (question != null)
                {
                    AllQuestions.Remove(question);
                    UpdateQuestionNumbers();
                    Console.WriteLine($"Pytanie o numerze {questionNumber} zostało usunięte.");
                    return true;
                }

                Console.WriteLine($"Brak pytania o numerze {questionNumber}.");
                return false;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Błąd podczas usuwania pytania: {ex.Message}");
                return false;
            }
        }

        public void UpdateQuestionNumbers() //aktualizacja numerów pytań
        {
            Console.WriteLine("Aktualizowanie numerów pytań.");

            if (AllQuestions.Count == 0)
            {
                Console.WriteLine("Brak pytań do aktualizacji numerów.");
                return;
            }

            for (int i = 0; i < AllQuestions.Count; i++)
            {
                AllQuestions[i].QuestionNumber = i;
                Console.WriteLine($"Zaktualizowano numer pytania o ID {AllQuestions[i].QuestionId} na {i + 1}.");
            }

            Console.WriteLine("Numery pytań zostały zaktualizowane.");
        }

        public List<Question> GetAllQuestions()
        {
            Console.WriteLine($"Liczba pytań w QuestionServiceApp: {AllQuestions.Count}");
            return AllQuestions.ToList();
        }
    }
}


