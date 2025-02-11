using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    // Pojedynczy wybór posiada: Id, literę, treść, oznaczoną aktywność

    public class Choice
    {
        public int ChoiceId { get; set; }
        
        public char ChoiceLetter { get; set; }

        public string ChoiceContent { get; set; }

        public bool IsActive { get; set; } 

        public Choice(int choiceId, char choiceLetter, string choiceContent, bool isActive = true)
        {
            if (choiceLetter < 'A' || choiceLetter > 'C')
                throw new ArgumentException("Niepoprawny znak. Litera odpowiedzi musi być w zakresie A-C.");

            ChoiceId = choiceId;
            ChoiceLetter = choiceLetter;
            ChoiceContent = choiceContent;
            IsActive = isActive;
        }
    }
}
