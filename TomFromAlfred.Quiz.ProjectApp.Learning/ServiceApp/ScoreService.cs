using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class ScoreService
    {
        private int _score; // Liczba poprawnych odpowiedzi
        private int _totalQuestions; // Liczba pytań w quizie
        private int _currentScore;

        public void StartNewQuiz(int totalQuestions)
        {
            _score = 0; // Reset punktów
            _totalQuestions = totalQuestions; // Ustawienie liczby pytań
        }

        public void IncrementScore()
        {
            _score++; // Zwiększenie liczby poprawnych odpowiedzi
        }

        public int GetScore()
        {
            return _score;
        }

        public double GetPercentage()
        {
            if (_totalQuestions == 0) return 0; // Uniknięcie dzielenia przez 0
            return ((double)_score / _totalQuestions) * 100;
        }

        public void ResetScore()
        {
            _score = 0; // Zmiana na potrzeby testu z _currentScore
        }

        public void DisplayScoreSummary() // To dla testów integracyjnych
        {
            Console.WriteLine($"Zdobyte punkty: {_score}/{_totalQuestions}");
            Console.WriteLine($"Procent poprawnych odpowiedzi: {GetPercentage():F2}%");
        }

        public int GetTotalQuestions() // Na potrzeby testu
        {
            return _totalQuestions;
        }
    }
}
