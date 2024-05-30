using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class QuizPresentationForUsersManagerApp
    {
        private readonly QuestionServiceApp _questionServiceApp;
        private readonly ChoicesArraysServiceApp _choicesService;

        public QuizPresentationForUsersManagerApp(QuestionServiceApp questionServiceApp, ChoicesArraysServiceApp choicesService)
        {
            _questionServiceApp = questionServiceApp;
            _choicesService = choicesService;
        }

        public void PresentQuestions()
        {
            List<Question> allQuestions = _questionServiceApp.AllQuestions.ToList();

            for (int i = 0; i < allQuestions.Count; i++)
            {
                var question = allQuestions[i];
                var choices = _choicesService.GetChoicesForQuestion(i);

                // Wyświetlanie pytania
                Console.WriteLine($"Pytanie {i + 1}: {question.QuestionContent}");

                // Wyświetlanie dostępnych wyborów w pętli
                foreach (var choice in choices)
                {
                    Console.WriteLine($"{choice.ChoiceLetter}: {choice.ChoiceContent}");
                }

                Console.WriteLine();
            }
        }           
    }
}

