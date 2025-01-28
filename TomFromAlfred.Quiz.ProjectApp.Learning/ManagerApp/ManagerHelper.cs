using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class ManagerHelper // Klasa pomocnicza
    {
        private readonly List<Question> _questions;
        private int _currentIndex;

        public ManagerHelper(List<Question> questions)
        {
            _questions = questions ?? throw new ArgumentNullException(nameof(questions));
            _currentIndex = 0;
        }

        public bool HasNext()
        {
            return _currentIndex < _questions.Count;
        }

        public Question GetCurrentQuestion()
        {
            if (!HasNext())
                throw new InvalidOperationException("Brak kolejnych pytań.");

            return _questions[_currentIndex];
        }

        public void NextQuestion()
        {
            if (HasNext())
            {
                _currentIndex++;
            }
        }
        public static List<T> Shuffle<T>(List<T> list)
        {
            var random = new Random();
            return list.OrderBy(_ => random.Next()).ToList();
        }
    }
}
