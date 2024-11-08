using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    public class CorrectAnswer
    {
        /*
         Poprawna odpowiedź ma id oraz treść. Ma swój serwis a łączona jest z pytaniem id w serwisie quizu.
         */
        public int CorrectAnswerId { get; set; }  
        public string CorrectAnswerContent { get; set; }  

        public CorrectAnswer(int correctAnswerId, string correctAnswerContent)
        {
            CorrectAnswerId = correctAnswerId;
            CorrectAnswerContent = correctAnswerContent;
        }
    }
}
