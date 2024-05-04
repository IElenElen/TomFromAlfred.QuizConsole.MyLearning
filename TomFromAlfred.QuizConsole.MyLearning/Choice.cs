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
        // Statyczne instancje wyborów, oznaczone literą i numerem pytania
        public static Choice Choice0A => new Choice(0, 'a', "czerwony");
        public static Choice Choice0B => new Choice(0, 'b', "blond");
        public static Choice Choice0C => new Choice(0, 'c', "zielony");

        public static Choice Choice1A => new Choice(1, 'a', "wiosna");
        public static Choice Choice1B => new Choice(1, 'b', "jesień");
        public static Choice Choice1C => new Choice(1, 'c', "lato");

        public static Choice Choice2A => new Choice(2, 'a', "aloes");
        public static Choice Choice2B => new Choice(2, 'b', "frezja");
        public static Choice Choice2C => new Choice(2, 'c', "gojnik");
    }
}
