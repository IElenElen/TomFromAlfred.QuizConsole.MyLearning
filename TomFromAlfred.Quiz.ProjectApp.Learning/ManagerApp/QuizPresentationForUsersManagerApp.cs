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
        private readonly QuestionsRaffleServiceApp _questionsListService;
        private readonly ChoiceServiceApp _choicesService;
        private readonly int questionNumber;

        public QuizPresentationForUsersManagerApp(QuestionsRaffleServiceApp questionsListService, ChoiceServiceApp choicesService)
        {
            _questionsListService = questionsListService ?? throw new ArgumentNullException(nameof(questionsListService));
            _choicesService = choicesService ?? throw new ArgumentNullException(nameof(choicesService));
        }

        public void PresentAQuiz() 
        {
            List<Question> randomQuestions = _questionsListService.GetRandomQuestions();
            HashSet<int> shownQuestions = new HashSet<int>();

            int displayNumber = questionNumber + 1;

            while (shownQuestions.Count < randomQuestions.Count)
            {
                Random random = new Random();
                int nextQuestionIndex;
                do
                {
                    nextQuestionIndex = random.Next(randomQuestions.Count);
                } while (shownQuestions.Contains(nextQuestionIndex));

                shownQuestions.Add(nextQuestionIndex);

                var question = randomQuestions[nextQuestionIndex];
                var choices = _choicesService.GetChoicesForQuestion(question.QuestionNumber.ToString());
                Console.WriteLine($"Pytanie {displayNumber}: {question.QuestionContent}");

                foreach (var choice in choices)
                {
                    Console.WriteLine($"{choice.OptionLetter}: {choice.ChoiceContent}");
                }

                displayNumber++;
                Console.WriteLine();

                Console.WriteLine("Naciśnij n, jeśli chcesz przejść do kolejnego pytania.");

                var key = Console.ReadKey().Key;
                Console.WriteLine();

                if (key != ConsoleKey.N)
                {
                    Console.WriteLine("Błąd. Naciśnij n, aby kontynuować.");
                }

                Console.WriteLine("Wszystkie pytania zostały już wyświetlone.");
            }
        }
    }
}

