using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ManagerApp
{
    public class ResultsAndUsersPointsManagerApp //weryfikacja odpowiedzi i ewentualne przyznanie punktu,
                                                 //czy na pewno to jest zadanie dla managera???

       //info do użytkownika tj. wyświetlenie jaki ma rezultat - ok
    {
        private readonly AnswerVerifierServiceApp _answerVerifierServiceApp;
        private int _totalPoints;
        public ResultsAndUsersPointsManagerApp(AnswerVerifierServiceApp answerVerifierServiceApp)
        {
            _answerVerifierServiceApp = answerVerifierServiceApp;
            _totalPoints = 0;
        }

        public bool VerifyAnswer(int questionNumber, char userChoice) //to menadżer czy serwis jednak?
        {
            return _answerVerifierServiceApp.GetPointsForAnswer(questionNumber, userChoice);
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
