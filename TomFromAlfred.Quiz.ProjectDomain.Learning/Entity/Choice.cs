﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    /* Muszę pamiętać, że jeśli zmieni się kolejność treści pytań
       to treści wyboru muszę przypisać do określonego pytania */
    public class Choice
    {
        public int ChoiceId { get; set; } //nr wyboru
        public EntitySupport.OptionLetter OptionLetter { get; set; } //litery wyboru, zawsze trzy opcje A, B lub C
        public string? ChoiceContent { get; set; }

        public Choice(int choiceId, EntitySupport.OptionLetter optionLetter, string? choiceContent)
        {
            ChoiceId = choiceId;
            OptionLetter = optionLetter;
            ChoiceContent = choiceContent;
        }

        // Przesłonięta metoda Equals, sprawdzająca, czy dwa obiekty Choice są sobie równe
        // Zwraca true, jeśli obiekty są takie same; w przeciwnym razie zwraca false
        public override bool Equals([NotNullWhen(true)] object? obj) //na potrzeby testu te dwie metody
        {
            if (obj == null || GetType() != obj.GetType()) // sprawdzenie, czy obiekt jest null lub nie jest tego samego typu co obiekt Choice
            {
                return false;
            }

            Choice other = (Choice)obj;     // rzutowanie obiektu na typ Choice
            return ChoiceId == other.ChoiceId  
                && OptionLetter == other.OptionLetter // porównanie pól obiektów Choice
                && ChoiceContent == other.ChoiceContent;
        }

        public override int GetHashCode() // przesłonięta metoda GetHashCode, obliczająca skrót obiektu Choice
                                          // zwraca wartość skrótu obliczoną na podstawie pól ChoiceId, ChoiceLetter i ChoiceContent
        {
            return HashCode.Combine(ChoiceId, OptionLetter, ChoiceContent);
        }
    }
}
