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
            ChoiceId = choiceId;
            ChoiceLetter = choiceLetter;
            ChoiceContent = choiceContent;
        }
    }
}
