using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class QuizService // klasa zestawu quiz'owego
    {
        private readonly QuestionService _questionService;
        private readonly ChoiceService _choiceService;
        private readonly CorrectAnswerService _correctAnswerService;
        public QuizService(QuestionService questionService, ChoiceService choiceService, CorrectAnswerService correctAnswerService)
        {
            _questionService = questionService;
            _choiceService = choiceService;
            _correctAnswerService = correctAnswerService;
        }

        public void DispalyQuiz()
        {

            var questions = _questionService.GetAll(); //pobranie wszystkich pytań jako IEnumerable
            var choices = _choiceService.GetAll(); // analogicznie pobranie wszystkich wyborów

            foreach (var question in questions)
            {
                Console.WriteLine($"Zestaw {question.QuestionId + 1}: {question.QuestionContent}");

                var choicesForQuestion = choices.Where(choice => choice.ChoiceId == question.QuestionId);
                foreach (var choice in choicesForQuestion)
                {
                    Console.WriteLine($"   {choice.ChoiceLetter}: {choice.ChoiceContent}");
                }
                Console.WriteLine();
            }
        }

        // Zwraca wszystkie pytania
        public IEnumerable<Question> GetQuestions()
        {
            return _questionService.GetAll();
        }

        // Sprawdza odpowiedź użytkownika
        public bool CheckAnswer(int questionId, string userAnswer)
        {
            var correctAnswer = _correctAnswerService.GetAll()
                .FirstOrDefault(answer => answer.CorrectAnswerId == questionId);

            return correctAnswer != null && correctAnswer.CorrectAnswerContent.Equals(userAnswer, StringComparison.OrdinalIgnoreCase);
        }
    }
}
