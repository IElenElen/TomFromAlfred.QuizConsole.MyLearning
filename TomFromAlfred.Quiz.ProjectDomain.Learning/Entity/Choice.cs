using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    public class Choice
    {
        public int ChoiceId { get; set; }
        public char ChoiceLetter { get; set; }
        public string? ChoiceContent { get; set; }

        public Choice(int choiceId, char choiceLetter, string? choiceContent)
        {
            ChoiceId = choiceId;
            ChoiceLetter = choiceLetter;
            ChoiceContent = choiceContent;
        }
        public override bool Equals([NotNullWhen(true)] object? obj) //na potrzeby testu te dwie metody
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Choice other = (Choice)obj;
            return ChoiceId == other.ChoiceId
                && ChoiceLetter == other.ChoiceLetter
                && ChoiceContent == other.ChoiceContent;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(ChoiceId, ChoiceLetter, ChoiceContent);
        }
    }
}
