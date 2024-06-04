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
        // Właściwości tylko do odczytu
        public Choice Choice0A { get; }
        public Choice Choice0B { get; }
        public Choice Choice0C { get; }

        public Choice Choice1A { get; }
        public Choice Choice1B { get; }
        public Choice Choice1C { get; }

        public Choice Choice2A { get; }
        public Choice Choice2B { get; }
        public Choice Choice2C { get; }

        public Choice Choice3A { get; }
        public Choice Choice3B { get; }
        public Choice Choice3C { get; }

        public Choice Choice4A { get; }
        public Choice Choice4B { get; }
        public Choice Choice4C { get; }

        public Choice Choice5A { get; }
        public Choice Choice5B { get; }
        public Choice Choice5C { get; }

        public Choice Choice6A { get; }
        public Choice Choice6B { get; }
        public Choice Choice6C { get; }

        public Choice Choice7A { get; }
        public Choice Choice7B { get; }
        public Choice Choice7C { get; }

        public Choice Choice8A { get; }
        public Choice Choice8B { get; }
        public Choice Choice8C { get; }

        public Choice Choice9A { get; set; } // Tylko ta właściwość może być modyfikowana na potrzeby testów

        public ChoiceServiceApp()
        {
            // Inicjalizacja właściwości tylko do odczytu
            Choice0A = new Choice(0, 'a', "8");
            Choice0B = new Choice(0, 'b', "9");
            Choice0C = new Choice(0, 'c', "10");

            Choice1A = new Choice(1, 'a', "Tomek w grobowcach faraonów.");
            Choice1B = new Choice(1, 'b', "Tomek u źródeł Amazonki.");
            Choice1C = new Choice(1, 'c', "Tajemnicza wyprawa Tomka.");

            Choice2A = new Choice(2, 'a', "W Gdańsku.");
            Choice2B = new Choice(2, 'b', "W Krakowie.");
            Choice2C = new Choice(2, 'c', "W Warszawie.");

            Choice3A = new Choice(3, 'a', "Na koledze z klasy.");
            Choice3B = new Choice(3, 'b', "Na wrogu ze szkoły.");
            Choice3C = new Choice(3, 'c', "Na pseudo-przyjacielu ze stadniny koni.");

            Choice4A = new Choice(4, 'a', "historia");
            Choice4B = new Choice(4, 'b', "biologia");
            Choice4C = new Choice(4, 'c', "geografia");

            Choice5A = new Choice(5, 'a', "Janina");
            Choice5B = new Choice(5, 'b', "Antonina");
            Choice5C = new Choice(5, 'c', "Irena");

            Choice6A = new Choice(6, 'a', "poprawna");
            Choice6B = new Choice(6, 'b', "zła");
            Choice6C = new Choice(6, 'c', "zła 2");

            Choice7A = new Choice(7, 'a', "też nie");
            Choice7B = new Choice(7, 'b', "nie");
            Choice7C = new Choice(7, 'c', "dobra");

            Choice8A = new Choice(8, 'a', "też nie");
            Choice8B = new Choice(8, 'b', "dobra");
            Choice8C = new Choice(8, 'c', "nie");

            Choice9A = new Choice(9, 'a', "Nowy wybór"); //do testu
        }

        public void RemoveChoice(Choice choiceToRemove)
        {
            Remove(choiceToRemove);
        }
    }
}
