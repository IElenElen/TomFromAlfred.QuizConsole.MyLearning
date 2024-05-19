using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class QuizPresentationForUsersManagerApp
    {
        // Pobranie wszystkich pytań
        List<Question> allQuestions = questionServiceApp.AllQuestions.ToList();

            // Tworzę pętlę przechodzącą przez każde pytanie w zestawie //do menagera?
            for (int i = 0; i<allQuestions.Count; i++)
            {
                var question = allQuestions[i];
        var choices = choicesService.GetChoicesForQuestion(i);

        // Wyświetlanie pytania
        Console.WriteLine($"Pytanie {question.QuestionNumber + 1}: {question.QuestionContent}");

                // Wyświetlanie dostępnych wyborów w pętli //do menagera?
                foreach (var choice in choices)
                {
                    Console.WriteLine($"{choice.ChoiceLetter}: {choice.ChoiceContent}");
                }
}
}
