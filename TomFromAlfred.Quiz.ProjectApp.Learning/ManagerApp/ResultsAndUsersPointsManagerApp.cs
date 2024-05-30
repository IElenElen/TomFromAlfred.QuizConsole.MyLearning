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
    {
        private readonly AnswerVerifierServiceApp _answerVerifierServiceApp;
        public ResultsAndUsersPointsManagerApp(AnswerVerifierServiceApp answerVerifierServiceApp)
        {
            _answerVerifierServiceApp = answerVerifierServiceApp;
        }

        public bool VerifyAnswer(int questionNumber, char userChoice)
        {
            bool result = _answerVerifierServiceApp.GetPointsForAnswer(questionNumber, userChoice);
            return result;
        }

        public void DisplayResult(bool result, ref int totalPoints)
        {
            if (result)
            {
                totalPoints++;
                Console.WriteLine("Poprawna odpowiedź. Zdobywasz 1 punkt.");
            }
            else
            {
                Console.WriteLine("Odpowiedź błędna. Brak punktu.");
            }

            Console.WriteLine();
        }
    }
}
