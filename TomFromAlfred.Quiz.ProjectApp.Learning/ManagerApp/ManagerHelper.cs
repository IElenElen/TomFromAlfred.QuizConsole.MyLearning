using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.Service;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class ManagerHelper // Klasa pomocnicza dla Managera
    {
        private readonly List<Question> _questions;
        private readonly QuizService _quizService;
        private int _currentIndex;

        public ManagerHelper(List<Question> questions, QuizService quizService)
        {
            _questions = questions ?? throw new ArgumentNullException(nameof(questions));
            _quizService = quizService ?? throw new ArgumentNullException(nameof(quizService));
            _currentIndex = 0;
        }

        public int GetCurrentIndex() => _currentIndex; // Na potrzeby testu

        public bool HasNext() // Czy to mi jest potrzebne???
        {
            return _currentIndex < _questions.Count - 1;
        }

        public Question GetCurrentQuestion()
        {
            if (_currentIndex >= _questions.Count) // Sprawdzam, czy indeks nie jest za duży
                throw new InvalidOperationException("Brak kolejnych pytań.");

            return _questions[_currentIndex];
        }

        public void NextQuestion()
        {
            if (_currentIndex < _questions.Count - 1) // Zabezpieczenie przed przekroczeniem listy
            {
                _currentIndex++;
            }
            else
            {
                throw new InvalidOperationException("Nie można przejść dalej, brak kolejnych pytań.");
            }
        }

        public static List<T> Shuffle<T>(List<T> list)
        {
            var random = new Random();
            return list.OrderBy(_ => random.Next()).ToList();
        }

        public void ShuffleQuestions()
        {
            _questions.Sort((a, b) => new Random().Next(-1, 2));
        }
    }
}
