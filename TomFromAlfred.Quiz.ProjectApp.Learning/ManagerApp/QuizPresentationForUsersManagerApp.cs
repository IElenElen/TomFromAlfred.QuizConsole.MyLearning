using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class QuizPresentationForUsersManagerApp //klasa prezentująca quiz
    {
        private readonly QuestionsListServiceApp _questionsListService;
        private readonly ChoicesArraysServiceApp _choicesService;

        public QuizPresentationForUsersManagerApp(QuestionsListServiceApp questionsListService, ChoicesArraysServiceApp choicesService)
        {
            _questionsListService = questionsListService;
            _choicesService = choicesService;
        }

        public void PresentAQuiz()
        {
            List<Question> randomQuestions = _questionsListService.GetRandomQuestions();

            for (int i = 0; i < randomQuestions.Count; i++) 
            {
                var question = randomQuestions[i];
                var choices = _choicesService.GetChoicesForQuestion(question.QuestionNumber - 1);

                // Wyświetlanie pytania
                Console.WriteLine($"Pytanie {question.QuestionNumber}: {question.QuestionContent}");

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

