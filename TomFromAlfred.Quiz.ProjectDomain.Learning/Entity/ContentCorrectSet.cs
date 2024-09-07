using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    public class ContentCorrectSet
    {
        public string QuestionContent { get; }
        public EntitySupport.OptionLetter LetterCorrectAnswer { get; set; }
        public string? ContentCorrectAnswer { get; set; }

        public ContentCorrectSet(string questionContent, EntitySupport.OptionLetter letterCorrectAnswer, string contentCorrectAnswer)
        {
            QuestionContent = questionContent; 
            LetterCorrectAnswer = letterCorrectAnswer; 
            ContentCorrectAnswer = contentCorrectAnswer; 
        }
    }
}
