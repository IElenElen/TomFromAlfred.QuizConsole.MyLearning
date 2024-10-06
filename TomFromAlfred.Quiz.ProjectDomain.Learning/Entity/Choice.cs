using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomFromAlfred.Quiz.ProjectDomain.Learning.Entity
{
    /* Muszę pamiętać, że jeśli zmieni się kolejność pytań
       to określony wybór muszę przypisać do określonego pytania */

    public class Choice
    {
        //Id jest dla mnie, użytkownik widzi tylko numer
        public int ChoiceId { get; set; } = -1; //id unikalne dla Choice

        public int? ChoiceNumber { get; set; } //nr, jest dynamiczny podczas wyświetlania, aby odpowiadał kolejności

        public EntitySupport.OptionLetter OptionLetter { get; set; }

        public string? ChoiceContent { get; set; }

        public Choice(int choiceId, int? choiceNumber, EntitySupport.OptionLetter optionLetter, string? choiceContent = null)
        {
            if (choiceNumber == null)
            {
                Console.WriteLine($"Uwaga: Wybór o Id {choiceId} nie ma przypisanego numeru.");
            }

            if (!Enum.IsDefined(typeof(EntitySupport.OptionLetter), optionLetter))
            {
                throw new ArgumentException("Niepoprawna wartość dla Option Letter.", nameof(optionLetter));
            }

            if (string.IsNullOrEmpty(choiceContent))
            {
                Console.WriteLine($"Uwaga: Wybór o Id {choiceId} ma pustą treść.");
            }

            ChoiceId = choiceId;
            ChoiceNumber = choiceNumber;
            OptionLetter = optionLetter;
            ChoiceContent = choiceContent;

            Console.WriteLine($"Wybór z Id {ChoiceId} został utworzony. Treść wyboru: {ChoiceContent ?? "brak"}");
        }
    }
}
