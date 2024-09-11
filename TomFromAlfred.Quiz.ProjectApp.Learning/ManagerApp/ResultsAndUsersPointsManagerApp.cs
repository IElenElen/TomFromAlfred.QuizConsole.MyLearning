using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class ResultsAndUsersPointsManagerApp(AnswerVerifierServiceApp answerVerifierServiceApp)

    //info do użytkownika tj. wyświetlenie jaki ma rezultat - ok
    {
        private readonly AnswerVerifierServiceApp _answerVerifierServiceApp = answerVerifierServiceApp;
        private int _totalPoints = 0;

        public bool VerifyAnswer(string? questionContent, char userChoice)
        {
            if (questionContent == null)
            {
                Console.WriteLine("Treść pytania jest pusta.");
                return false;
            }

            return _answerVerifierServiceApp.GetPointsForAnswer(questionContent, userChoice);
        }

        public void DisplayResult(bool result)
        {
            if (result)
            {
                _totalPoints++;
                Console.WriteLine("Poprawna odpowiedź. Zdobywasz 1 punkt.");
            }
            else
            {
                Console.WriteLine("Odpowiedź błędna. Brak punktu.");
            }

            Console.WriteLine();
        }

        public void DisplayFinalScore()
        {
            Console.WriteLine($"Twój wynik końcowy: {_totalPoints} pkt.");
        }
    }
}
