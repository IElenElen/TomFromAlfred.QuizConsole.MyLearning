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
        public List<Question> AllQuestions { get; set; } = new List<Question>();
        public QuestionServiceApp(IEnumerable<Question>? initialQuestions = null)
        {
            AllQuestions = (initialQuestions ?? Enumerable.Empty<Question>()).ToList();
        }

        public void AddQuestion(Question question)
        {
            if (AllQuestions.Any(q => q.QuestionContent == question.QuestionContent))
            {
                throw new InvalidOperationException("Takie pytanie już istnieje.");
            }

            int newQuestionNumber = AllQuestions.Count;
            question.QuestionNumber = newQuestionNumber;
            AllQuestions.Add(question); 
            UpdateQuestionNumbers();
        }

        public Question? GetQuestionByNumber(int userQuestionNumber)
        {
            int questionNumber = userQuestionNumber - 1;
            if (questionNumber < 0 || questionNumber >= AllQuestions.Count)
            {
                return null;
            }

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


