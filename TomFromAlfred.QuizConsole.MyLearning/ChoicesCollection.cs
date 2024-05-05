using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    public class ChoicesCollection
    {
        private ChoiceHandling choiceHandling; //prywatne pole
        public ChoicesCollection()
        {
            choiceHandling = new ChoiceHandling();
        }
        // Tablica wyborów dla pytania nr 0 itd
        public Choice[] Choice0Array => new Choice[] { choiceHandling.Choice0A, choiceHandling.Choice0B, choiceHandling.Choice0C };

        public Choice[] Choice1Array => new Choice[] { choiceHandling.Choice1A, choiceHandling.Choice1B, choiceHandling.Choice1C };

        public Choice[] Choice2Array => new Choice[] { choiceHandling.Choice2A, choiceHandling.Choice2B, choiceHandling.Choice2C };

        public Choice[] GetChoicesForQuestion(int questionNumber) // Metoda zwraca wybory dla konkretnego pytania
        {
            switch (questionNumber) //Wybór tablicy dla danego pytania
            {
                case 0:
                    return Choice0Array;
                case 1:
                    return Choice1Array;
                case 2:
                    return Choice2Array;

                default:
                    return new Choice[0]; // Domyślnie zwraca pustą tablicę
            }
        }
    }
}
