using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class QuizPresentationForUsersManagerApp //klasa prezentująca quiz
    {
        private readonly QuestionsListServiceApp _questionsListService;
        private readonly ChoicesArraysServiceApp _choicesService;
        private readonly Random _random;

        public QuizPresentationForUsersManagerApp(QuestionsListServiceApp questionsListService, ChoicesArraysServiceApp choicesService)
        {
            _questionsListService = questionsListService;
            _choicesService = choicesService;
            _random = new Random();
        }

        public void PresentAQuiz() //część do naprawy
        {
            List<Question> randomQuestions = _questionsListService.GetRandomQuestions();
            HashSet<int> shownQuestions = new HashSet<int>();

            int displayNumber = 1;

            while (true)
            {
                if (shownQuestions.Count == randomQuestions.Count)
                {
                    Console.WriteLine("Wszystkie pytania zostały już wyświetlone.");
                    break;
                }

                int randomIndex;
                do
                {
                    randomIndex = _random.Next(randomQuestions.Count);
                }
                while (shownQuestions.Contains(randomIndex));

                shownQuestions.Add(randomIndex);
                var question = randomQuestions[randomIndex];
                var choices = _choicesService.GetChoicesForQuestion(randomIndex);

                // Wyświetlanie pytania
                Console.WriteLine($"Pytanie {displayNumber}: {question.QuestionContent}");

                // Wyświetlanie dostępnych wyborów
                foreach (var choice in choices)
                {
                    Console.WriteLine($"{choice.ChoiceLetter}: {choice.ChoiceContent}");
                }

                Console.WriteLine();
                Console.WriteLine("Naciśnij n, jeśli chcesz przejść do kolejnego pytania, jeśli chcesz zakończyć naciśniaj k");

                var key = Console.ReadKey().Key;
                Console.WriteLine(); 

                if (key == ConsoleKey.K) //czy ja to potrzebuję? Mam już menadżera dla wyjścia użytkownika
                {
                    break;
                }
                else if (key != ConsoleKey.N)
                {
                    Console.WriteLine("Błąd. Spróbuj ponownie.");
                }
            }
        }
    }
}

