using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    //Klasa poprawnych zestawów
    public class ContentCorrectSet(string questionContent, EntitySupport.OptionLetter letterCorrectAnswer, string contentCorrectAnswer)
    {
        public string QuestionContent { get; } = questionContent;
        public EntitySupport.OptionLetter LetterCorrectAnswer { get; set; } = letterCorrectAnswer;
        public string? ContentCorrectAnswer { get; set; } = contentCorrectAnswer;
    }
}
