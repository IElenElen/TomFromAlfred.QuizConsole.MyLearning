﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class EndService //Klasa dla zakończenia Quizu w każdym momencie
    {
        private readonly ScoreService _scoreService;

        public EndService(ScoreService scoreService)
        {
            _scoreService = scoreService ?? throw new ArgumentNullException(nameof(scoreService));
        }

        public bool ShouldEnd(string? userInput)
        {
            return userInput?.Trim().ToLower() == "k";
        }

        public void EndQuiz(bool quizCompleted)
        {
            if (quizCompleted)
            {
                Console.WriteLine("Quiz zakończony. Dziękujemy za udział!");
                _scoreService.DisplayScoreSummary();
            }
            else
            {
                Console.WriteLine("Quiz został przerwany przed zakończeniem. Brak punktów.");
                _scoreService.ResetScore(); // Resetuje punkty
            }

            Environment.Exit(0); // Kończy aplikację
        }
    }
}
