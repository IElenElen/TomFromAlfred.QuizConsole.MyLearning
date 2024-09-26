using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    public class Question
    {
        public bool IsActive; //nie wszystkie pytania muszą być aktywne w danym momencie

        /* Zmiana podejścia. Treść pytań przypisana do numerów pytań, 
           a losowanie to będzie stworzenie nowej listy, użytkownik zobaczy numerację pytań od 1 do X, 
           a faktycznie rzecz biorąc za każdym nowym podejściem do quizu, pyania będą losowane. */

        //Właściwości możliwe dopiero do wprowadzenia, zatem są nullable

        public int QuestionId { get; set; } //id question jest mi potrzebne, bo muszę przypisać wybór do pytania podczas losowania
                                            //id jest na stałe przypisane do pytania
        public int? QuestionNumber { get; set; } //nr, jest dynamiczny

        /* Pytanie musi wiązać się z: 
           1. Z przypisanym do niego określonym wyborem 
           2. Z poprawną odpowiedzią */

        public string? QuestionContent { get; }

        public Question(int questionId, int? questionNumber = null, string? questionContent = null)
        {
            QuestionId = questionId;
            QuestionNumber = questionNumber;
            QuestionContent = questionContent;
            IsActive = true;
        }
    }
}
