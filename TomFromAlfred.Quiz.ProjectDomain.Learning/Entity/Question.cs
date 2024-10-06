using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    public class Question
    {
        /* Pytania mają swoje id, do wyświetlania zaś numery. 
           Losowanie to będzie stworzenie nowej listy, użytkownik zobaczy numerację pytań od 1 do X, 
           a faktycznie rzecz biorąc za każdym nowym podejściem do quizu, pytania będą losowane. */

        //Właściwości możliwe dopiero do wprowadzenia, zatem są nullable czyli numer i treść

        //Id jest dla mnie, użytkownik widzi tylko numer
        public int QuestionId { get; set; } = -1; //id question jest mi potrzebne, bo muszę przypisać wybór do pytania podczas losowania
                                            //kolejne id będą na stałe przypisane do pytań
                                            //-1 oznacza brak id 
        public int? QuestionNumber { get; set; } //nr, jest dynamiczny podczas wyświetlania, aby odpowiadał kolejności

        /* Pytanie musi wiązać się z: 
           1. Z przypisanym do niego określonym wyborem 
           2. Z poprawną odpowiedzią */

        public string? QuestionContent { get; set; }

        public bool IsActive { get; set; } //nie wszystkie pytania muszą być aktywne w danym momencie

        public Question(int questionId, int? questionNumber = null, string? questionContent = null)
        {
            if (string.IsNullOrEmpty(questionContent))
            {
                Console.WriteLine($"Uwaga: Pytanie o Id {questionId} ma pustą treść.");
            }

            if (questionNumber == null)
            {
                Console.WriteLine($"Uwaga: Pytanie o Id {questionId} nie ma przypisanego numeru.");
            }

            QuestionId = questionId;
            QuestionNumber = questionNumber;
            QuestionContent = questionContent;
            IsActive = true;

            Console.WriteLine($"Pytanie z Id {QuestionId} zostało utworzone. Treść pytania: {QuestionContent ?? "brak"}");
        }
    }
}
