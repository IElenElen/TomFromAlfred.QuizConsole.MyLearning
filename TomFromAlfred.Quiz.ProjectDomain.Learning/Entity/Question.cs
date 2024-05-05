using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    public class Question
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
