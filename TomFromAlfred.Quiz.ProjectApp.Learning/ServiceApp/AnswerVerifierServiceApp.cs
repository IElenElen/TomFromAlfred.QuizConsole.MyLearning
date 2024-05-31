using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class AnswerVerifierServiceApp 
    {
        private Dictionary<int, char> correctAnswers = new Dictionary<int, char>(); // prywatne pole słownika przechowujące poprawne odpowiedzi na pytania
        public AnswerVerifierServiceApp()  // konstruktor klasy AnswerVerifierServiceApp, który inicjalizuje słownik correctAnswers z poprawnymi odpowiedziami
        {
            // Dodanie poprawnych odpowiedzi do słownika
            correctAnswers.Add(0, 'b');
            correctAnswers.Add(1, 'a');
            correctAnswers.Add(2, 'c');
            correctAnswers.Add(3, 'a');
            correctAnswers.Add(4, 'c');
            correctAnswers.Add(5, 'a');
            correctAnswers.Add(6, 'a');
            correctAnswers.Add(7, 'c');
            correctAnswers.Add(8, 'b');
        }
        public bool GetPointsForAnswer(int questionNumber, char userChoice)
        {
            char correctAnswer;
            if (correctAnswers.TryGetValue(questionNumber, out correctAnswer)) // próba pobrania poprawnej odpowiedzi ze słownika correctAnswers dla podanego numeru pytania
            {
                return correctAnswer == userChoice; // porównanie odpowiedzi użytkownika z poprawną odpowiedzią
            }
            return false;  // jeśli numer pytania nie istnieje w słowniku lub odpowiedź użytkownika jest niepoprawna, zwróć false
        }
    }
}
