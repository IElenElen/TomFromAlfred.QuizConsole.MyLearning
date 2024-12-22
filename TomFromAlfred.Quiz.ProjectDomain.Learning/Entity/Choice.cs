using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    public class Choice
    {
        public int ChoiceId { get; set; }
        public char ChoiceLetter { get; set; } // np. A, B, C
        public string ChoiceContent { get; set; }

        public Choice (int choiceId, char choiceLetter, string choiceContent)
        {
            if (choiceLetter < 'A' || choiceLetter > 'Z') // Walidacja zakresu
                throw new ArgumentException("Litera odpowiedzi musi być w zakresie A-Z.");

            ChoiceId = choiceId;
            ChoiceLetter = choiceLetter;
            ChoiceContent = choiceContent;
        }
    }
}
