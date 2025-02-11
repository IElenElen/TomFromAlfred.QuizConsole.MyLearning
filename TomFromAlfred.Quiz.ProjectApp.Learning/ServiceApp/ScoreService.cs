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
        private int _newActiveQuestions; // Liczba aktywnych pytań w quizie (w domyśle - zestawy)
        private int _allActiveQuizSets; // Liczba wszystkich aktywnych zestawów Quizu

        public void StartNewQuiz(int newActiveQuestions)
        {
            if (newActiveQuestions <= 0) 
            {
                throw new ArgumentOutOfRangeException(nameof(newActiveQuestions), "Liczba pytań musi być większa niż 0.");
            }
            _score = 0; // Reset punktów
            _newActiveQuestions = newActiveQuestions; // Ustawienie liczby aktywnych pytań
        }

        public void IncrementScore()
        {
            _score++; // Zwiększenie liczby poprawnych odpowiedzi
            Console.WriteLine($"IncrementScore() called, new score: {_score}");
        }

        public int GetScore()
        {
            Console.WriteLine($"GetScore() returns: {_score}"); // Debugowanie
            Console.WriteLine($"GetScore() called, returning: {_score}");
            return _score;
        }

        public double GetPercentage()
        {
            if (_allActiveQuizSets == 0) return 0; // Uniknięcie dzielenia przez 0
            return ((double)_score / _allActiveQuizSets) * 100;
        }

        public void ResetScore()
        {
            Console.WriteLine("ResetScore() called"); // Debugowanie
            _score = 0; // Zmiana na potrzeby testu z _currentScore
        }

        // Do testów integracyjnych ???
        public void DisplayScoreSummary() 
        {
            Console.WriteLine($"Zdobyte punkty: {_score}/{_allActiveQuizSets}");
            Console.WriteLine($"Procent poprawnych odpowiedzi: {GetPercentage():F2}%");
        }

        // Na potrzeby testu
        public int GetTotalActiveQuestions() // Zwraca wszystkie aktywne pytania
        {
            return _newActiveQuestions;
        }
    }
}
