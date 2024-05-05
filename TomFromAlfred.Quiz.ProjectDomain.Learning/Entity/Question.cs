using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectDomain.Learning.CommonDomain;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    public class Question : BaseEntity //dziedziczenie selektywne
    {
        public int QuestionNumber { get; set; }
        public string? QuestionContent { get; set; }
        public Question(int questionNumber, string questionContent)
        {
            QuestionNumber = questionNumber;
            QuestionContent = questionContent;
        }
    }
}
