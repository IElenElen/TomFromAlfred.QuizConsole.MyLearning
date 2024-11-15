using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    /*
     Pytanie to jednostka. Pojedyncze pytanie ma właściwości.
     Właściwości Question to Id - generowane przez system oraz treść. 
     Na jakiej podstawie łaczę pytanie z jego zestawem wyboru???
     */
    public class Question //Klasa publiczna, bo pytanie to podstawa w budowie Quizu
    {
        public int QuestionId { get; set; }
        public string QuestionContent { get; set; } 

        public Question(int questionId, string questionContent)
        {
            QuestionId = questionId;
            QuestionContent = questionContent;
        }
    }
}
