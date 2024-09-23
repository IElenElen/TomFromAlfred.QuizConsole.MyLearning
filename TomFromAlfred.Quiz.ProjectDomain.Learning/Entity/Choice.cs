using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    /* Muszę pamiętać, że jeśli zmieni się kolejność pytań
       to określony wybór muszę przypisać do określonego pytania */
    public class Choice
    {
        public int? ChoiceId { get; set; }
        public EntitySupport.OptionLetter OptionLetter { get; set; }
        public string? ChoiceContent { get; set; }

        public Choice(EntitySupport.OptionLetter optionLetter, int? choiceId = null, string? choiceContent = null)
        {
            if (!Enum.IsDefined(typeof(EntitySupport.OptionLetter), optionLetter))
            {
                throw new ArgumentException("Niepoprawna wartość dla Option Letter.", nameof(optionLetter));
            }
            OptionLetter = optionLetter;
            ChoiceId = choiceId;
            ChoiceContent = choiceContent;
        }
    }
}
