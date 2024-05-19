using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class ResultsAndUsersPointsManagerApp
    {
        // Następuje weryfikacja odpowiedzi i przyznawanie punktów //to do serwisu? też raczej do managera
        bool result = answerVerifierManager.GetPointsForAnswer(question.QuestionNumber, userChoice);
        Console.WriteLine();

                // Wyświetlanie informacji o poprawności odpowiedzi //tu tutaj czy też do managera?
                if (result)
                {
                    totalPoints++;
                    Console.WriteLine("Poprawna odpowiedź. Zdobywasz 1 punkt.");
                }
                else
                {
                    Console.WriteLine("Odpowiedź błędna. Brak punktu.");
                }

    }
}
