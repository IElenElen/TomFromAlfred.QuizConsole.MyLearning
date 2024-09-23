using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    public class Question
    {
        /* Zmiana podejścia. Treść pytań przypisana do numerów pytań, 
           a losowanie to będzie stworzenie nowej listy, użytkownik zobaczy numerację pytań od 1 do X, 
           a faktycznie rzecz biorąc za każdym nowym podejściem do quizu, pyania będą losowane. */

        //Właściwości możliwe dopiero do wprowadzenia, zatem są nullable

        public int? QuestionNumber { get; set; } 

        /* Pytanie musi wiązać się z: 
           1. Z przypisanym do niego określonym wyborem 
           2. Z poprawną odpowiedzią */

        public string? QuestionContent { get; set; }

        public Question(int? questionNumber = null, string? questionContent = null)
        {
            QuestionNumber = questionNumber;
            QuestionContent = questionContent;
        }
    }
}
