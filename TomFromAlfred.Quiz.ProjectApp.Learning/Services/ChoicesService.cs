using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.Managers;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.Services
{
    public class ChoicesService //kod z poprzedniego ChoicesCollection
    {
        private ChoiceManager choiceManager;
        public ChoicesService()
        {
            choiceManager = new ChoiceManager();
        }
        public Choice[] Choice0Array => new Choice[] { choiceManager.Choice0A, choiceManager.Choice0B, choiceManager.Choice0C };
        public Choice[] Choice1Array => new Choice[] { choiceManager.Choice1A, choiceManager.Choice1B, choiceManager.Choice1C };
        public Choice[] Choice2Array => new Choice[] { choiceManager.Choice2A, choiceManager.Choice2B, choiceManager.Choice2C };
        public Choice[] Choice3Array => new Choice[] { choiceManager.Choice3A, choiceManager.Choice3B, choiceManager.Choice3C };
        public Choice[] Choice4Array => new Choice[] { choiceManager.Choice4A, choiceManager.Choice4B, choiceManager.Choice4C };
        public Choice[] Choice5Array => new Choice[] { choiceManager.Choice5A, choiceManager.Choice5B, choiceManager.Choice5C };

        public Choice[] GetChoicesForQuestion(int questionNubmer)
        {
            switch (questionNubmer)
            {
                case 0:
                    return Choice0Array;
                case 1:
                    return Choice1Array;
                case 2:
                    return Choice2Array;
                case 3:
                    return Choice3Array;
                case 4:
                    return Choice4Array;
                case 5:
                    return Choice5Array;

                default:

                    return new Choice[0]; // Domyślnie zwraca pustą tablicę
            }
        }
    }
}
