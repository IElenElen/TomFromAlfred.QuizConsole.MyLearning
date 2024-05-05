using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    public class Question
    {
        public int QuestionNumber { get; set; } //numer pytania
        public string QuestionContent { get; set; } //treść pytania

        public Question(int questionNumber, string questionContent) //konstruktor i jego ciało
        {
            QuestionNumber = questionNumber;
            QuestionContent = questionContent;
        }
    }
}
