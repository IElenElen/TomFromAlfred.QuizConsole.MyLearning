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
        public int QuestionId { get; }
        public EntitySupport.OptionLetter LetterCorrectAnswer { get; }
        public string? ContentCorrectAnswer { get; }

        public ContentCorrectSet(int questionId, EntitySupport.OptionLetter letterCorrectAnswer, string? contentCorrectAnswer = null)
        {
            if (!Enum.IsDefined(typeof(EntitySupport.OptionLetter), letterCorrectAnswer))
            {
                Console.WriteLine($"Błąd: Niepoprawna wartość dla LetterCorrectAnswer: {letterCorrectAnswer}");
                throw new ArgumentException("Niepoprawna wartość dla Letter Correct Answer.", nameof(letterCorrectAnswer));
            }

            QuestionId = questionId;
            LetterCorrectAnswer = letterCorrectAnswer;
            ContentCorrectAnswer = contentCorrectAnswer;

            Console.WriteLine($"Utworzono poprawny zestaw dla pytania o id: {questionId}, z odpowiedzią: {contentCorrectAnswer ?? "brak treści"}.");
        }
    }
}
