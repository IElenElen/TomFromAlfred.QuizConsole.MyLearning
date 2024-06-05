using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomFromAlfred.Quiz.ProjectApp.Learning.CommonApp;
using TomFromAlfred.Quiz.ProjectDomain.Learning.Entity;

namespace TomFromAlfred.Quiz.ProjectApp.Learning.ServiceApp
{
    //menadżery rozmawiają z klientem, serwisy odpowiadają za pamięć i dane
    //po interfejsie dziedziczy serwis a nie klasa bazowa
    public class ChoiceServiceApp : BaseApp<Choice> //ten kod to poprawny serwis 
    {
        public Choice[][] ChoicesArrays { get; private set; }

        public ChoiceServiceApp()
        {
            ChoicesArrays = new Choice[][]
            {
            new Choice[]
            {
                new Choice(0, 'a', "8"),
                new Choice(0, 'b', "9"),
                new Choice(0, 'c', "10")
            },
            new Choice[]
            {
                new Choice(1, 'a', "Tomek w grobowcach faraonów."),
                new Choice(1, 'b', "Tomek u źródeł Amazonki."),
                new Choice(1, 'c', "Tajemnicza wyprawa Tomka.")
            },
            new Choice[]
            {
                new Choice(2, 'a', "W Gdańsku."),
                new Choice(2, 'b', "W Krakowie."),
                new Choice(2, 'c', "W Warszawie.")
            },
            new Choice[]
            {
                new Choice(3, 'a', "Na koledze z klasy."),
                new Choice(3, 'b', "Na wrogu ze szkoły."),
                new Choice(3, 'c', "Na pseudo-przyjacielu ze stadniny koni.")
            },
            new Choice[]
            {
                new Choice(4, 'a', "historia"),
                new Choice(4, 'b', "biologia"),
                new Choice(4, 'c', "geografia")
            },
            new Choice[]
            {
                new Choice(5, 'a', "Janina"),
                new Choice(5, 'b', "Antonina"),
                new Choice(5, 'c', "Irena")
            },
            new Choice[]
            {
                new Choice(6, 'a', "poprawna"),
                new Choice(6, 'b', "zła"),
                new Choice(6, 'c', "zła 2")
            },
            new Choice[]
            {
                new Choice(7, 'a', "też nie"),
                new Choice(7, 'b', "nie"),
                new Choice(7, 'c', "dobra")
            },
            new Choice[]
            {
                new Choice(8, 'a', "też nie"),
                new Choice(8, 'b', "dobra"),
                new Choice(8, 'c', "nie")
            }
            };
        }

        public void AddChoice(Choice newChoice)
        {
            var newChoices = new Choice[ChoicesArrays.Length + 1][];
            for (int i = 0; i < ChoicesArrays.Length; i++)
            {
                newChoices[i] = ChoicesArrays[i];
            }
            newChoices[ChoicesArrays.Length] = new Choice[] { newChoice };
            ChoicesArrays = newChoices;
        }

        public void RemoveChoice(Choice choiceToRemove)
        {
            for (int i = 0; i < ChoicesArrays.Length; i++)
            {
                var choiceArray = ChoicesArrays[i];
                for (int j = 0; j < choiceArray.Length; j++)
                {
                    if (choiceArray[j].Equals(choiceToRemove))
                    {
                        var newChoices = new Choice[choiceArray.Length - 1];
                        for (int k = 0, l = 0; k < choiceArray.Length; k++)
                        {
                            if (k == j) continue;
                            newChoices[l++] = choiceArray[k];
                        }
                        ChoicesArrays[i] = newChoices;
                        return;
                    }
                }
            }
        }
    }
}

        