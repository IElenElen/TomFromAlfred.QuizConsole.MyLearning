using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    public class Choice
    {
        [JsonProperty("ChoiceId")]
        public int ChoiceId { get; set; }

        [JsonProperty("OptionLetter")] 
        public char ChoiceLetter { get; set; }

        [JsonProperty("ChoiceContent")]
        public string ChoiceContent { get; set; }

        public Choice(int choiceId, char choiceLetter, string choiceContent)
        {
            if (choiceLetter < 'A' || choiceLetter > 'Z')
                throw new ArgumentException("Litera odpowiedzi musi być w zakresie A-Z.");

            ChoiceId = choiceId;
            ChoiceLetter = choiceLetter;
            ChoiceContent = choiceContent;
        }
    }
}
