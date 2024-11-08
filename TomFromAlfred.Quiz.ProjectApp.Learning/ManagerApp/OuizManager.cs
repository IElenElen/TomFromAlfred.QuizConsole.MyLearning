using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class OuizManager
    {
        private readonly QuizService _quizService;
        private readonly ChoiceService _choiceService;
        private readonly CorrectAnswerService _correctAnswerService;

        public OuizManager(QuizService quizService, ChoiceService choiceService, CorrectAnswerService correctAnswerService)
        {
            _quizService = quizService;
            _choiceService = choiceService;
            _correctAnswerService = correctAnswerService;
        }
        public void ConductQuiz()
        {
            // Wyświetlam pytania bez poprawnych odpowiedzi
            _quizService.DispalyQuiz();

            // Iteracja przez wszystkie pytania i przeprowadzam quiz
            foreach (var question in _quizService.GetQuestions())
            {
                Console.WriteLine($"Twoja odpowiedź dla pytania \"{question.QuestionContent}\" (Wybierz A, B lub C): ");
                char userChoiceLetter = Console.ReadLine().ToUpper()[0];

                // Pobieram opcję odpowiadającą wyborowi użytkownika
                var choiceForQuestion = _choiceService.GetAll()
                    .Where(choice => choice.ChoiceId == question.QuestionId && choice.ChoiceLetter == userChoiceLetter)
                    .FirstOrDefault();

                if (choiceForQuestion == null)
                {
                    Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                    continue;
                }

                // Sprawdzam, czy odpowiedź użytkownika jest poprawna
                bool isCorrect = _quizService.CheckAnswer(question.QuestionId, choiceForQuestion.ChoiceContent);

                if (isCorrect)
                {
                    Console.WriteLine("Poprawna odpowiedź!");
                }
                else
                {
                    Console.WriteLine("Niestety, to nie jest poprawna odpowiedź.");
                }
                Console.WriteLine();
            }
        }
    }
}
