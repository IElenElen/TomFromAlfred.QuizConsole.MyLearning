using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    public class AnswerVerifier
    {
        private Dictionary<int, char> correctAnswers = new Dictionary<int, char>(); //numer pytania = int i poprawna odpowiedź = char 

        public AnswerVerifier()   // Konstruktor inicjalizujący poprawne odpowiedzi dla każdego pytania
        {
            correctAnswers.Add(0, 'b');
            correctAnswers.Add(1, 'a');
            correctAnswers.Add(2, 'c');
        }

        // Metoda zwraca punkty za odpowiedź użytkownika
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
