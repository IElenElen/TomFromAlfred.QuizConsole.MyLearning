using System;
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
        public int ChoiceId { get; set; }
        public EntitySupport.OptionLetter OptionLetter { get; set; }
        public string? ChoiceContent { get; set; }

        public Choice(int choiceId, EntitySupport.OptionLetter optionLetter, string? choiceContent = null)
        {
            if (choiceId <= 0)
            {
                throw new ArgumentException("ChoiceId musi być większe niż 0.", nameof(choiceId));
            }

            ChoiceId = choiceId;
            OptionLetter = optionLetter;
            ChoiceContent = choiceContent;
        }
    }
}
