using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class ManagerHelper
    {
        private readonly List<Question> _questions;
        private int _currentQuestionIndex;

        public ManagerHelper(IEnumerable<Question> questions)
        {
            _questions = questions.ToList();
            _currentQuestionIndex = 0;
        }

        public Question GetCurrentQuestion()
        {
            if (_currentQuestionIndex < _questions.Count)
            {
                return _questions[_currentQuestionIndex];
            }
            return null;
        }

        public Question NextQuestion()
        {
            _currentQuestionIndex++;
            return GetCurrentQuestion();
        }

        public bool HasNext()
        {
            return _currentQuestionIndex < _questions.Count;
        }
    }
}
