using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.QuizConsole.MyLearning
{
    public class ChoicesCollection
    {
        // Tablica wyborów dla pytania nr 0 itd
        public static Choice[] Choice0Array => new Choice[] { Choice.Choice0A, Choice.Choice0B, Choice.Choice0C };

        public static Choice[] Choice1Array => new Choice[] { Choice.Choice1A, Choice.Choice1B, Choice.Choice1C };

        public static Choice[] Choice2Array => new Choice[] { Choice.Choice2A, Choice.Choice2B, Choice.Choice2C };
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
