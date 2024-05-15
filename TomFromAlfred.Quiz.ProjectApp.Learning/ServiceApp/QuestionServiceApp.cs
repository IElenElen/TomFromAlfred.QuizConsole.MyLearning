﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    public class QuestionServiceApp : BaseApp<Question> 
    {
        private int? questionNumber;
        public virtual IEnumerable<Question> AllQuestions { get; }
        public QuestionServiceApp()
        {
            // Inicjalizacja listy pytań
            AllQuestions = new List<Question>
            {
                new Question(0, "Z ilu części składa się powyższa powieść Alfreda Szklarskiego? "), //nowy format pytań przygotowany pod test
                new Question(1, "Jaki tytuł nosi ostatnia część o przygodach Tomka?"),
                new Question(2, "Tomek przed pierwszą przygodą mieszka w: "),
                new Question(3, "Na kim mści się Tomek pod koniec roku szkolnego?"),
                new Question(4, "Ulubiony przedmiot Tomka to: "),
                new Question(5, "Jak ma na imię ciocia Tomka?"),
                new Question(6, "Pytanie specjalnie do usuwania nr 1. Niech będzie odp A."),
                new Question(7, "Pytanie do usuwania nr 2. Odp c."),
                new Question(8, "Pytanie też do testu nr 3. Z odp b"),
            };
        }

        public Question GetQuestionByNumber(List<Question> AllQuestions, int questionNumber) //ta metoda z pętlą for była na początku właśnie pożądana 
        {
            for (int i = 0; i < AllQuestions.Count; i++)
            {
                if (AllQuestions[i].QuestionNumber == questionNumber)
                {
                    return AllQuestions[i];
                }
            }
            return null; // Jeśli nie znaleziono pytania o podanym numerze, zwróć null
        }

        /*public bool GetQuestionByNumber(List<Question> allQuestions, int questionNumber) //problematyczna metoda przy testowaniu
        {
            if (allQuestions == null || allQuestions.Count == 0)
            {
                Console.WriteLine("Lista pytań jest pusta.");
                return false;
            }

            foreach (var question in allQuestions)
            {
                if (question.QuestionNumber == questionNumber)
                {
                    Console.WriteLine("Pytanie znalezione.");
                    return true;
                }
            }
            Console.WriteLine("Brak pytania o podanym numerze.");
            return false;
        }*/

        public void RemoveQuestionByNumber(int questionNumber)
        {
            foreach (Question question in AllQuestions)
            {
                if (question.QuestionNumber == questionNumber)
                {
                    ((List<Question>)AllQuestions).Remove(question); // Usuwanie pytania z listy
                    Console.WriteLine("Pytanie zostało usunięte");
                    return;
                }
            }
            Console.WriteLine("Brak pytania o podanym numerze");
        }
    }
}


