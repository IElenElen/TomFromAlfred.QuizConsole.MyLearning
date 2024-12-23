﻿using System;
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

        public void StartNewQuiz(int totalQuestions)
        {
            _score = 0; // Resetuję punkty
            _totalQuestions = totalQuestions; // Ustawiam liczbę pytań
        }

        public void IncrementScore()
        {
            _score++; // Zwiększam liczbę poprawnych odpowiedzi
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

        public void DisplayScoreSummary()
        {
            Console.WriteLine($"Zdobyte punkty: {_score}/{_totalQuestions}");
            Console.WriteLine($"Procent poprawnych odpowiedzi: {GetPercentage():F2}%");
        }
    }
}
