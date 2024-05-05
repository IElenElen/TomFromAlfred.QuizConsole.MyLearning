using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    public class Choice
    {
        public int ChoiceId { get; set; } //Właściwości tu: numer wyboru
        public char ChoiceLetter { get; set; } //Litera wyboru
        public string ChoiceContent { get; set; } //Treść
        public Choice(int choiceId, char choiceLetter, string choiceContent) // Konstruktor przyjmujący parametry
        {
            ChoiceId = choiceId; //Przypisuję nr do właściwości ChoiceId
            ChoiceLetter = choiceLetter;
            ChoiceContent = choiceContent;
        }
    }
}
