using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class QuizService
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

        // Pobieram wszystkie pytania
        public IEnumerable<Question> GetQuestions()
        {
            return _questionService.GetAll();
        }

        // Pobieram opcje dla danego pytania
        public IEnumerable<Choice> GetChoicesForQuestion(int questionId)
        {
            return _choiceService.GetAll().Where(choice => choice.ChoiceId == questionId);
        }

        // Pobieram poprawną odpowiedź dla danego pytania
        public string GetCorrectAnswerForQuestion(int questionId)
        {
            var correctAnswer = _correctAnswerService.GetAll()
                .FirstOrDefault(answer => answer.CorrectAnswerId == questionId);

            return correctAnswer?.CorrectAnswerContent ?? "Brak poprawnej odpowiedzi";
        }

        // Sprawdzam, czy odpowiedź użytkownika jest poprawna
        public bool CheckAnswer(int questionId, char userChoiceLetter)
        {
            var correctAnswer = _correctAnswerService.GetAll()
                .FirstOrDefault(answer => answer.CorrectAnswerId == questionId);

            var choice = _choiceService.GetAll()
                .FirstOrDefault(c => c.ChoiceId == questionId && c.ChoiceLetter == userChoiceLetter);

            return choice != null && correctAnswer != null &&
                   choice.ChoiceContent.Equals(correctAnswer.CorrectAnswerContent, StringComparison.OrdinalIgnoreCase);
        }
    }
}
