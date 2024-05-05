using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    public class ChoiceHandling
    {
        // Instancje wyborów, nr wyboru, litery oraz treść
        public Choice Choice0A => new Choice(0, 'a', "czerwony");
        public Choice Choice0B => new Choice(0, 'b', "blond");
        public Choice Choice0C => new Choice(0, 'c', "zielony");

        public Choice Choice1A => new Choice(1, 'a', "wiosna");
        public Choice Choice1B => new Choice(1, 'b', "jesień");
        public Choice Choice1C => new Choice(1, 'c', "lato");

        public Choice Choice2A => new Choice(2, 'a', "aloes");
        public Choice Choice2B => new Choice(2, 'b', "frezja");
        public Choice Choice2C => new Choice(2, 'c', "gojnik");
    }
}
