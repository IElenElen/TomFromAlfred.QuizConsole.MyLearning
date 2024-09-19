using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    //Klasa poprawnych zestawów
    public class ContentCorrectSet
    {
        public string? QuestionContent { get; }
        public EntitySupport.OptionLetter LetterCorrectAnswer { get; }
        public string? ContentCorrectAnswer { get; }

        public ContentCorrectSet(EntitySupport.OptionLetter letterCorrectAnswer, string? questionContent = null, string? contentCorrectAnswer = null)
        {
            QuestionContent = questionContent; 
            LetterCorrectAnswer = letterCorrectAnswer; 
            ContentCorrectAnswer = contentCorrectAnswer; 
        }
    }
}
