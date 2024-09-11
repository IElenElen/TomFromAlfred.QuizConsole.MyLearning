﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    public class Question
    {
        /* Nr pytania. Numery pytań pojawiają się po kolei, natomiast nie są sztywno przypisane do treści pytań,
        które to będą pojawiać się losowo przy każdym kolejnym uruchomieniu konsoli. */
        public int QuestionContent { get; set; } 

        /* Treść pytania musi wiązać się z: 
           1. Z przypisanym do niej określonym wyborem 
           2. Z treścią poprawnej odpowiedzi */
        public string? QuestionContent { get; set; } 
        public Question(int questionNumber, string questionContent)
        {
            QuestionContent = questionNumber;
            QuestionContent = questionContent;
        }
    }
}
