using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class ResultsAndUsersPointsManagerApp

    //info do użytkownika tj. wyświetlenie jaki ma rezultat - ok
    {
        private readonly AnswerVerifierServiceApp _answerVerifierServiceApp;
        private int _totalPoints = 0;

        public ResultsAndUsersPointsManagerApp(AnswerVerifierServiceApp answerVerifierServiceApp)
        {
            _answerVerifierServiceApp = answerVerifierServiceApp ?? throw new ArgumentNullException(nameof(answerVerifierServiceApp));
        }

        public bool VerifyAnswer(int questionId, char userChoice)
        {

            return _answerVerifierServiceApp.GetPointsForAnswer(questionId, userChoice);
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
