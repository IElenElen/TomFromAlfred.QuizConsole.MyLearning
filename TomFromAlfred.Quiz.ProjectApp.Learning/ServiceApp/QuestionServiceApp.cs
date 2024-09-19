using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    //Klasa serwisowa pytań odpowiada za dodawanie, zmiany i usuwanie pytań

    //potrzebuję update, jak mam też pytanie dodane... usunięte lub losowane...
    public class QuestionServiceApp : BaseApp<Question>
    {
        public virtual List<Question> AllQuestions { get; private set; }

        public QuestionServiceApp(IEnumerable<Question>? initialQuestions = null)
        {
            AllQuestions = (initialQuestions ?? Enumerable.Empty<Question>()).ToList();
        }

        public void AddQuestion(string? questionContent)
        {

            if (AllQuestions.Any(q => q.QuestionContent == questionContent))
            {
                throw new InvalidOperationException("Pytanie o tej treści już istnieje.");
            }

            int newQuestionNumber = AllQuestions.Count;
            AllQuestions.Add(new Question(newQuestionNumber, questionContent));
            UpdateQuestionNumbers();
        }

        public Question? GetQuestionByNumber(int userQuestionNumber)
        {
            int questionNumber = userQuestionNumber - 1;
            if (questionNumber < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(questionNumber), "Numer pytania nie może być ujemny.");
            }

            return AllQuestions.FirstOrDefault(q => q.QuestionNumber == questionNumber)
            ?? throw new InvalidOperationException("Brak pytania o podanym numerze.");
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
            if (AllQuestions.Count == 0)
            {
                Console.WriteLine("Brak pytań do aktualizacji numerów.");
                return;
            }

            for (int i = 0; i < AllQuestions.Count; i++)
            {
                AllQuestions[i].QuestionNumber = i;
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


