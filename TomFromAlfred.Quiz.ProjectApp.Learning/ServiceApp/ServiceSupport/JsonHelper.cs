using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp.ServiceSupport
{
    // Wsparcie dla Correct Answer, bo entity różne od pliku jsona
    public class JsonHelper
    {
        public int QuestionNumber { get; set; } // Id pytania
        public string ?LetterCorrectAnswer { get; set; } // Litera poprawnej odpowiedzi
        public string ?ContentCorrectAnswer { get; set; } // Treść poprawnej odpowiedzi
    }
}
