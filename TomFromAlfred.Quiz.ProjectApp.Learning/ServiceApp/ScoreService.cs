using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Abstract.AbstractForService;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class ScoreService : IScoreService 
    {
        private int _score; // Liczba poprawnych odpowiedzi
        private int _newActiveQuestions; // Liczba aktywnych pytań w quizie (w domyśle - zestawy)
        private int _allActiveQuizSets; // Liczba wszystkich aktywnych zestawów Quizu

        public int Score => _score; // Publiczny getter do testów
        public int AllActiveQuizSets => _allActiveQuizSets; // Publiczny getter do testów

        public virtual void DisplayScoreSummary()
        {
            Console.WriteLine($"Zdobyte punkty: {_score}/{_allActiveQuizSets}");
            Console.WriteLine($"Procent poprawnych odpowiedzi: {GetPercentage():F2}%");
        }

        public virtual double GetPercentage()
        {
            if (_allActiveQuizSets == 0) return 0; // Uniknięcie dzielenia przez 0
            return ((double)_score / _allActiveQuizSets) * 100;
        }

        public virtual int GetScore()
        {
            Console.WriteLine($"GetScore() returns: {_score}"); // Debugowanie
            Console.WriteLine($"GetScore() called, returning: {_score}");
            return _score;
        }

        public virtual int GetTotalActiveSets()
        {
            return _allActiveQuizSets;
        }

        public virtual void IncrementScore()
        {
            _score++; // Zwiększenie liczby poprawnych odpowiedzi
            Console.WriteLine($"IncrementScore() called, new score: {_score}");
        }

        public virtual void ResetScore()
        {
            Console.WriteLine("ResetScore() called"); // Debugowanie
            _score = 0; // Zmiana na potrzeby testu z _currentScore
        }

        public virtual void RestartQuiz(int newActiveQuestions)
        {
            Console.WriteLine("RestartQuiz() called! Resetting score to 0.");
            _score = 0;
            _allActiveQuizSets = newActiveQuestions;
        }

        public virtual void StartNewQuiz(int newActiveQuestions)
        {
            if (newActiveQuestions <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newActiveQuestions), "Liczba pytań musi być większa niż 0.");
            }
            _score = 0; // Reset
            Console.WriteLine("StartNewQuiz() called!");
            _allActiveQuizSets = newActiveQuestions;
        }
    }
}
