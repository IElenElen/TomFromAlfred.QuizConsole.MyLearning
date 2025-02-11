using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    /*
    Poprawna odpowiedź ma id oraz treść, dodatkowo oznaczoną aktywność. 
    Ma swój serwis a łączona jest z pytaniem id w serwisie quizu.
    */

    public class CorrectAnswer
    {
        public int CorrectAnswerId { get; set; }  
        public string CorrectAnswerContent { get; set; }
        public bool IsActive { get; set; }

        public CorrectAnswer(int correctAnswerId, string correctAnswerContent, bool isActive)
        {
            CorrectAnswerId = correctAnswerId;
            CorrectAnswerContent = correctAnswerContent;
            IsActive = isActive;
        }

        // Nadpisanie metody ToString dla poprawnego wyświetlania
        public override string ToString()
        {
            return CorrectAnswerContent;
        }
    }
}
