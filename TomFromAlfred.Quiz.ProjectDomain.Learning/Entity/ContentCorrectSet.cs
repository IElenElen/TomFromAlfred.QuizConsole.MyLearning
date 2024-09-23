using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TomFromAlfred.Quiz.ProjectDomain.Learning.Entity.EntitySupport;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    //Klasa poprawnych zestawów (do (numeru) pytania przypisana litera i treść poprawnej odpowiedzi (opcji w woborze))
    public class ContentCorrectSet
    {
        public int? QuestionNumber { get; }
        public EntitySupport.OptionLetter LetterCorrectAnswer { get; }
        public string? ContentCorrectAnswer { get; }

        public ContentCorrectSet(EntitySupport.OptionLetter letterCorrectAnswer, int? questionNumber = null, string? contentCorrectAnswer = null)
        {
            if (!Enum.IsDefined(typeof(EntitySupport.OptionLetter), letterCorrectAnswer))
            {
                throw new ArgumentException("Niepoprawna wartość dla Letter Correct Answer.", nameof(letterCorrectAnswer));
            }

            LetterCorrectAnswer = letterCorrectAnswer;
            QuestionNumber = questionNumber; 
            ContentCorrectAnswer = contentCorrectAnswer; 
        }
    }
}
