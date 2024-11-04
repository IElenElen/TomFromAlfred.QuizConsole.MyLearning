using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    //jaką tu robię budowe???
    public class Choice
    {
        public string ChoiceContent { get; set; }

        public Choice (string choiceContent)
        {
            ChoiceContent = choiceContent;
        }
    }
}
