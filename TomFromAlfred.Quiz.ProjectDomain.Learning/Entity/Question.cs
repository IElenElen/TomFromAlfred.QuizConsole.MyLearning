using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    public class Question
    {
        public int QuestionNumber { get; set; } //numer ma się swobodnie losować - ale nie w tym miejscu
        public string? QuestionContent { get; set; } //treść pytania musi być powiązana z treściami wyboru oraz
                                                     //z treścią poprawnej odpowiedzi - do ogranięcia
        public Question(int questionNumber, string questionContent)
        {
            QuestionNumber = questionNumber;
            QuestionContent = questionContent;
        }
    }
}
