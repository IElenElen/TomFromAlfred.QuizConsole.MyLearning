using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.Managers
{
    public class AnswerVerifierManager
    {
        private Dictionary<int, char> correctAnswers = new Dictionary<int, char>();
        public AnswerVerifierManager()
        {
            correctAnswers.Add(0, 'b');
            correctAnswers.Add(1, 'a');
            correctAnswers.Add(2, 'c');
            correctAnswers.Add(3, 'a');
            correctAnswers.Add(4, 'c');
            correctAnswers.Add(5, 'a');
        }
        public bool GetPointsForAnswer(int questionNumber, char userChoice)
        {
            char correctAnswer;
            if (correctAnswers.TryGetValue(questionNumber, out correctAnswer))
            {
                return correctAnswer == userChoice;
            }
            return false;
        }
    }
}
